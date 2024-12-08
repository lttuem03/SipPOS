using System.ComponentModel;
using System.Collections.ObjectModel;

using SipPOS.Views.General;
using SipPOS.Views.Staff;
using SipPOS.DataTransfer.Entity;

using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;

namespace SipPOS.ViewModels.Staff;

public class StaffManagementViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    // Pagination properties
    public ObservableCollection<StaffDto> CurrentPageStaffList { get; private set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int RowsPerPage { get; set; }
    public int TotalRowsCount { get; set; }
    public string? Keyword { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    
    public StaffManagementViewModel()
    {
        RowsPerPage = 5;
        CurrentPage = 1;

        CurrentPageStaffList = new();
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

        var (rowsReturned, staffDtos) = await staffDao.GetWithPagination
        (
            storeId: storeAuthenticationService.Context.CurrentStore.Id,
            page: CurrentPage, 
            rowsPerPage: RowsPerPage,
            keyword: Keyword,
            sortBy: SortBy,
            sortDirection: SortDirection
        );

        if (staffDtos == null)
        {
            return;
        }

        CurrentPageStaffList = new ObservableCollection<StaffDto>(staffDtos);
        TotalRowsCount = rowsReturned;
        TotalPages = (TotalRowsCount / RowsPerPage) + (TotalRowsCount % RowsPerPage == 0 ? 0 : 1);
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void HandleGoBackButtonClick()
    {
        App.NavigateTo(typeof(MainMenuView));
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

        // Update the staff list
    }

    public void HandleRegisterNewStaffButtonClick()
    {
        App.NavigateTo(typeof(StaffRegistrationView));
    }
}