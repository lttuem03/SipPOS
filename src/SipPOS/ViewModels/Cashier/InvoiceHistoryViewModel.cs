using System.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.Context.Configuration.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using System.Collections.ObjectModel;
using SipPOS.Models.Entity;
using SipPOS.Resources.Helper;
using SipPOS.DataTransfer.Entity;
using System.Security.Cryptography;
using Microsoft.UI.Xaml.Media;

namespace SipPOS.ViewModels.Cashier;

public class InvoiceHistoryViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // Contextual properties
    public Models.Entity.Store CurrentStore { get; private set; }
    public Models.General.Configuration CurrentConfiguration { get; private set; }

    // Pagination properties
    private long _currentPage = 1;
    private long _totalPages = 0;
    private long _rowsPerPage = 5;
    private long _totalRowsCount = 0;
    private long _fromIndex = 0;
    private long _toIndex = 0;
    private DateOnly _dateFilter = DateOnly.MinValue;
    private TimeOnly _fromTimeFilter = TimeOnly.MinValue;
    private TimeOnly _toTimeFilter = TimeOnly.MinValue;

    // Data-bound properties
    private readonly Invoice _noInvoiceSelected = new(new InvoiceDto());
    private Invoice _currentInvoice;
    private TrulyObservableCollection<Invoice> _currentPageInvoiceList = new();
    private TrulyObservableCollection<InvoiceItem> _currentInvoiceItemList = new();
    private string _vatRateString = string.Empty;
    private string _vatMessageString = string.Empty;

    private long _currentInvoiceId = -1;
    private DateTime _currentInvoiceCreatedAt = DateTime.MinValue;
    private string _currentInvoiceStaffName = string.Empty;
    private long _currentInvoiceItemCount = 0;
    private decimal _currentInvoiceSubTotal = 0m;
    private decimal _currentInvoiceTotalDiscount = 0m;
    private decimal _currentInvoiceInvoiceBasedVAT = 0m;
    private decimal _currentInvoiceTotal = 0m;
    private decimal _currentInvoicePaid = 0m;
    private decimal _currentInvoiceChange = 0m;

    public InvoiceHistoryViewModel()
    {
        var currentConfiguration = App.GetService<IConfigurationContext>().GetConfiguration();

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
            throw new InvalidOperationException("Store authentication service is not registered.");

        if (currentConfiguration == null)
            throw new InvalidOperationException("Configuration is not loaded.");

        if (storeAuthenticationService.Context.CurrentStore == null)
            throw new InvalidOperationException("Store is not logged in.");

        CurrentStore = storeAuthenticationService.Context.CurrentStore;
        CurrentConfiguration = currentConfiguration;

        DateFilter = DateOnly.FromDateTime(DateTime.Now);
        FromTimeFilter = CurrentConfiguration.OpeningTime;
        ToTimeFilter = CurrentConfiguration.ClosingTime;

        // Load tax configuration
        VatRateString = CurrentConfiguration.VatRate.ToString("P0");
        VatMessageString = CurrentConfiguration.VatMethod switch
        {
            "VAT_INCLUDED" => "(Giá bán sản phẩm đã bao gồm thuế VAT)",
            "ORDER_BASED" => "(Thuế VAT sẽ tính dựa trên tổng giá trị đơn hàng)",
            _ => "Phương thức VAT chưa xác định"
        };

        _currentInvoice = _noInvoiceSelected;

        _ = UpdateInvoiceList();
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task UpdateInvoiceList()
    {
        var invoiceDao = App.GetService<IInvoiceDao>();
        
        var totalCount = await invoiceDao.GetTotalCountWithDateTimeFilterAsync
        (
            storeId: CurrentStore.Id,
            date: DateFilter,
            fromTime: FromTimeFilter,
            toTime: ToTimeFilter
        );

        if (totalCount == 0)
        {
            CurrentPageInvoiceList.Clear();
            TotalRowsCount = 0;
            TotalPages = 0;
            FromIndex = 0;
            ToIndex = 0;
            return;
        }

        var invoices = await invoiceDao.GetWithPaginationAsync
        (
            storeId: CurrentStore.Id,
            page: CurrentPage,
            rowsPerPage: RowsPerPage,
            date: DateFilter,
            fromTime: FromTimeFilter,
            toTime: ToTimeFilter
        );

        if (invoices == null)
        {
            CurrentPageInvoiceList.Clear();
            TotalRowsCount = 0;
            TotalPages = 0;
            FromIndex = 0;
            ToIndex = 0;
            return;
        }

        CurrentPageInvoiceList.Clear();

        foreach (var invoice in invoices)
            CurrentPageInvoiceList.Add(invoice);

        TotalRowsCount = totalCount;
        TotalPages = (TotalRowsCount / RowsPerPage) + (TotalRowsCount % RowsPerPage == 0 ? 0 : 1);
        FromIndex = (CurrentPage - 1) * RowsPerPage + 1;
        ToIndex = Math.Min(CurrentPage * RowsPerPage, TotalRowsCount);

        if (CurrentPageInvoiceList.Count > 0)
            CurrentInvoice = CurrentPageInvoiceList[0];
        else
            CurrentInvoice = _noInvoiceSelected;
    }

    public void HandleInvoiceListViewSelectionChanged(ListView invoiceListView)
    {
        CurrentInvoice = CurrentPageInvoiceList[invoiceListView.SelectedIndex];
    }

    public async Task HandleDateCalendarDatePickerDateChanged(DateTime newDate)
    {
        DateFilter = DateOnly.FromDateTime(newDate);
        await UpdateInvoiceList();
    }

    public async Task HandleFromTimePickerTimeChanged(TimeSpan newFromTime)
    {
        FromTimeFilter = TimeOnly.FromTimeSpan(newFromTime);
        await UpdateInvoiceList();
    }

    public async Task HandleToTimePickerTimeChanged(TimeSpan newToTime)
    {
        ToTimeFilter = TimeOnly.FromTimeSpan(newToTime);
        await UpdateInvoiceList();
    }

    public async Task HandlePreviousPageButtonClick()
    {
        if (CurrentPage - 1 >= 1)
        {
            CurrentPage--;
            await UpdateInvoiceList();
        }
    }

    public async Task HandleNextPageButtonClick()
    {
        if (CurrentPage + 1 <= TotalPages)
        {
            CurrentPage++;
            await UpdateInvoiceList();
        }
    }

    public async Task HandleRowsPerPageComboBoxSelectionChanged(int comboBoxIndex)
    {
        RowsPerPage = comboBoxIndex switch
        {
            0 => 5,
            1 => 10,
            2 => 15,
            3 => 20,
            _ => RowsPerPage
        };

        await UpdateInvoiceList();
    }

    public TrulyObservableCollection<Invoice> CurrentPageInvoiceList
    {
        get => _currentPageInvoiceList;
        set
        {
            _currentPageInvoiceList = value;
            OnPropertyChanged(nameof(CurrentPageInvoiceList));
        }
    }

    /// <summary>
    /// Gets or sets the current page.
    /// </summary>
    public long CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }

    /// <summary>
    /// Gets or sets the total pages.
    /// </summary>
    public long TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged(nameof(TotalPages));
        }
    }

    /// <summary>
    /// Gets or sets the rows per page.
    /// </summary>
    public long RowsPerPage
    {
        get => _rowsPerPage;
        set
        {
            _rowsPerPage = value;
            OnPropertyChanged(nameof(RowsPerPage));
        }
    }

    /// <summary>
    /// Gets or sets the total rows count.
    /// </summary>
    public long TotalRowsCount
    {
        get => _totalRowsCount;
        set
        {
            _totalRowsCount = value;
            OnPropertyChanged(nameof(TotalRowsCount));
        }
    }

    /// <summary>
    /// Gets or sets the from index.
    /// </summary>
    public long FromIndex
    {
        get => _fromIndex;
        set
        {
            _fromIndex = value;
            OnPropertyChanged(nameof(FromIndex));
        }
    }

    /// <summary>
    /// Gets or sets the to index.
    /// </summary>
    public long ToIndex
    {
        get => _toIndex;
        set
        {
            _toIndex = value;
            OnPropertyChanged(nameof(ToIndex));
        }
    }

    public DateOnly DateFilter
    {
        get => _dateFilter;
        set
        {
            _dateFilter = value;
            OnPropertyChanged(nameof(DateFilter));
        }
    }

    public TimeOnly FromTimeFilter
    {
        get => _fromTimeFilter;
        set
        {
            _fromTimeFilter = value;
            OnPropertyChanged(nameof(FromTimeFilter));
        }
    }

    public TimeOnly ToTimeFilter
    {
        get => _toTimeFilter;
        set
        {
            _toTimeFilter = value;
            OnPropertyChanged(nameof(ToTimeFilter));
        }
    }

    public Invoice CurrentInvoice
    {
        get => _currentInvoice;
        set
        {
            _currentInvoice = value;
            CurrentInvoiceItemList.Clear();


            foreach (var invoiceItem in value.InvoiceItems)
            {
                CurrentInvoiceItemList.Add(invoiceItem);
            }

            CurrentInvoiceId = value.Id;
            CurrentInvoiceCreatedAt = value.CreatedAt;
            CurrentInvoiceStaffName = value.StaffName;
            CurrentInvoiceItemCount = value.ItemCount;
            CurrentInvoiceSubTotal = value.SubTotal;
            CurrentInvoiceTotalDiscount = value.TotalDiscount;
            CurrentInvoiceInvoiceBasedVAT = value.InvoiceBasedVAT;
            CurrentInvoiceTotal = value.Total;
            CurrentInvoicePaid = value.Paid;
            CurrentInvoiceChange = value.Change;

            //OnPropertyChanged(nameof(CurrentInvoiceId));
            //OnPropertyChanged(nameof(CurrentInvoiceCreatedAt));
            //OnPropertyChanged(nameof(CurrentInvoiceStaffName));
            //OnPropertyChanged(nameof(CurrentInvoiceItemCount));
            //OnPropertyChanged(nameof(CurrentInvoiceSubTotal));
            //OnPropertyChanged(nameof(CurrentInvoiceTotalDiscount));
            //OnPropertyChanged(nameof(CurrentInvoiceInvoiceBasedVAT));
            //OnPropertyChanged(nameof(CurrentInvoiceTotal));

            OnPropertyChanged(nameof(CurrentInvoice));
        }
    }

    public long CurrentInvoiceId
    {
        get => _currentInvoiceId;
        set
        {
            _currentInvoiceId = value;
            OnPropertyChanged(nameof(CurrentInvoiceId));
        }
    }

    public DateTime CurrentInvoiceCreatedAt
    {
        get => _currentInvoiceCreatedAt;
        set
        {
            _currentInvoiceCreatedAt = value;
            OnPropertyChanged(nameof(CurrentInvoiceCreatedAt));
        }
    }

    public string CurrentInvoiceStaffName
    {
        get => _currentInvoiceStaffName;
        set
        {
            _currentInvoiceStaffName = value;
            OnPropertyChanged(nameof(CurrentInvoiceStaffName));
        }
    }

    public long CurrentInvoiceItemCount
    {
        get => _currentInvoiceItemCount;
        set
        {
            _currentInvoiceItemCount = value;
            OnPropertyChanged(nameof(CurrentInvoiceItemCount));
        }
    }

    public decimal CurrentInvoiceSubTotal
    {
        get => _currentInvoiceSubTotal;
        set
        {
            _currentInvoiceSubTotal = value;
            OnPropertyChanged(nameof(CurrentInvoiceSubTotal));
        }
    }

    public decimal CurrentInvoiceTotalDiscount
    {
        get => _currentInvoiceTotalDiscount;
        set
        {
            _currentInvoiceTotalDiscount = value;
            OnPropertyChanged(nameof(CurrentInvoiceTotalDiscount));
        }
    }

    public decimal CurrentInvoiceInvoiceBasedVAT
    {
        get => _currentInvoiceInvoiceBasedVAT;
        set
        {
            _currentInvoiceInvoiceBasedVAT = value;
            OnPropertyChanged(nameof(CurrentInvoiceInvoiceBasedVAT));
        }
    }

    public decimal CurrentInvoiceTotal
    {
        get => _currentInvoiceTotal;
        set
        {
            _currentInvoiceTotal = value;
            OnPropertyChanged(nameof(CurrentInvoiceTotal));
        }
    }

    public decimal CurrentInvoicePaid
    {
        get => _currentInvoicePaid;
        set
        {
            _currentInvoicePaid = value;
            OnPropertyChanged(nameof(CurrentInvoicePaid));
        }
    }

    public decimal CurrentInvoiceChange
    {
        get => _currentInvoiceChange;
        set
        {
            _currentInvoiceChange = value;
            OnPropertyChanged(nameof(CurrentInvoiceChange));
        }
    }

    public TrulyObservableCollection<InvoiceItem> CurrentInvoiceItemList
    {
        get => _currentInvoiceItemList;
        set
        {
            _currentInvoiceItemList = value;
            OnPropertyChanged(nameof(CurrentInvoiceItemList));
        }
    }

    public string VatRateString
    {
        get => _vatRateString;
        set
        {
            _vatRateString = value;
            OnPropertyChanged(nameof(VatRateString));
        }
    }

    public string VatMessageString
    {
        get => _vatMessageString;
        set
        {
            _vatMessageString = value;
            OnPropertyChanged(nameof(VatMessageString));
        }
    }

    
}