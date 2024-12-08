using System.ComponentModel;
using System.Collections.ObjectModel;

using SipPOS.Views.General;
using SipPOS.Views.Staff;
using SipPOS.DataTransfer.Entity;

using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using WinRT.SipPOSVtableClasses;

namespace SipPOS.ViewModels.Staff;

public class StaffManagementViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // Data-bound properties 
    private ObservableCollection<StaffDto> _currentPageStaffList;

    // Sort, search, filter properties
    public List<string> PositionPrefixFilter { get; private set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }

    // Pagination properties
    private long _currentPage;
    private long _totalPages;
    private long _rowsPerPage;
    private long _totalRowsCount;
    private long _fromIndex;
    private long _toIndex;
    private string _searchKeyword;
    
    public StaffManagementViewModel()
    {
        _currentPage = 1;
        _totalPages = 0;
        _rowsPerPage = 5;
        _totalRowsCount = 0;
        _fromIndex = 0;
        _toIndex = 0;
        _searchKeyword = "";
        
        _currentPageStaffList = new();
        PositionPrefixFilter = new();
        PositionPrefixFilter.Add("SM");
        PositionPrefixFilter.Add("AM");
        PositionPrefixFilter.Add("ST");
        UpdateStaffList();
    }

    private async void UpdateStaffList()
    {
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }

        if (storeAuthenticationService.Context.CurrentStore == null)
        {
            return;
        }

        var staffDao = App.GetService<IStaffDao>();

        var (totalRowsMatched, staffDtos) = await staffDao.GetWithPagination
        (
            storeId: storeAuthenticationService.Context.CurrentStore.Id,
            page: CurrentPage, 
            rowsPerPage: RowsPerPage,
            keyword: SearchKeyword,
            sortBy: SortBy,
            sortDirection: SortDirection,
            filterByPositionPrefixes: PositionPrefixFilter
        );

        if (staffDtos == null)
        {
            CurrentPageStaffList = new ObservableCollection<StaffDto>(); // empty list
            TotalRowsCount = 0;
            TotalPages = 0;
            FromIndex = 0;
            ToIndex = 0;
            return;
        }

        CurrentPageStaffList = new ObservableCollection<StaffDto>(staffDtos);
        TotalRowsCount = totalRowsMatched;
        TotalPages = (TotalRowsCount / RowsPerPage) + (TotalRowsCount % RowsPerPage == 0 ? 0 : 1);
        FromIndex = (CurrentPage - 1) * RowsPerPage + 1;
        ToIndex = Math.Min(CurrentPage * RowsPerPage, TotalRowsCount);
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void HandleGoBackButtonClick()
    {
        App.NavigateTo(typeof(MainMenuView));
    }

    public void HandleSortByComboBoxSelectionChanged(int comboBoxIndex)
    {
        switch (comboBoxIndex)
        {
            // sort criteria must be in snake_case
            case 0:
                SortBy = "position_prefix";
                break;
            case 1:
                SortBy = "name";
                break;
        }

        UpdateStaffList();
    }

    public void HandleSortDirectionComboBoxSelectionChanged(int comboBoxIndex)
    {
        switch (comboBoxIndex)
        {
            case 0:
                SortDirection = "ASC";
                break;
            case 1:
                SortDirection = "DESC";
                break;
        }

        SortBy ??= "position_prefix";

        UpdateStaffList();
    }

    public void HandlePositionCheckBoxesChanged(bool? smChecked, bool? amChecked, bool? stChecked)
    {
        PositionPrefixFilter = new List<string>();

        if (smChecked == true)
        {
            PositionPrefixFilter.Add("SM");
        }

        if (amChecked == true)
        {
            PositionPrefixFilter.Add("AM");
        }

        if (stChecked == true)
        {
            PositionPrefixFilter.Add("ST");
        }

        UpdateStaffList();
    }

    public void HandleRowsPerPageComboBoxSelectionChanged(int comboBoxIndex)
    {
        switch (comboBoxIndex)
        {
            case 0:
                RowsPerPage = 5;
                break;
            case 1:
                RowsPerPage = 10;
                break;
            case 2:
                RowsPerPage = 15;
                break;
            case 3:
                RowsPerPage = 20;
                break;
        }

        UpdateStaffList();
    }

    public void HandlePreviousPageButtonClick()
    {
        if (CurrentPage - 1 >= 1)
        {
            CurrentPage--;
            UpdateStaffList();
        }
    }

    public void HandleNextPageButtonClick()
    {
        if (CurrentPage + 1 <= TotalPages)
        {
            CurrentPage++;
            UpdateStaffList();
        }
    }

    public void HandleRegisterNewStaffButtonClick()
    {
        App.NavigateTo(typeof(StaffRegistrationView));
    }

    public ObservableCollection<StaffDto> CurrentPageStaffList
    {
        get => _currentPageStaffList;
        set
        {
            _currentPageStaffList = value;
            OnPropertyChanged(nameof(CurrentPageStaffList));
        }
    }

    public long CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }

    public long TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged(nameof(TotalPages));
        }
    }

    public long RowsPerPage
    {
        get => _rowsPerPage;
        set
        {
            _rowsPerPage = value;
            OnPropertyChanged(nameof(RowsPerPage));
        }
    }
    
    public long TotalRowsCount
    {
        get => _totalRowsCount;
        set
        {
            _totalRowsCount = value;
            OnPropertyChanged(nameof(TotalRowsCount));
        }
    }

    public long FromIndex
    {
        get => _fromIndex;
        set
        {
            _fromIndex = value;
            OnPropertyChanged(nameof(FromIndex));
        }
    }

    public long ToIndex
    {
        get => _toIndex;
        set
        {
            _toIndex = value;
            OnPropertyChanged(nameof(ToIndex));
        }
    }

    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            _searchKeyword = value;
            OnPropertyChanged(nameof(SearchKeyword));
            UpdateStaffList();
        }
    }
}