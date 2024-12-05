using System.ComponentModel;

using SipPOS.Views.Login;
using SipPOS.Views.Cashier;
using SipPOS.Views.Staff;
using SipPOS.Views.Configuration;
using SipPOS.Views.Inventory;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using System.Collections.ObjectModel;
using SipPOS.Context.Shift.Interface;

namespace SipPOS.ViewModels.General;

/// <summary>
/// ViewModel for managing the main menu, handling data for data-binding and the logic in the MainMenuView.
/// </summary>
public class MainMenuViewModel : INotifyPropertyChanged
{
    // Data-bound properties
    private string _pageTilte;
    private string _currentStaffAuthenticationStatus;

    public ObservableCollection<StaffDto> OnShiftStaffs { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenuViewModel"/> class.
    /// </summary>
    public MainMenuViewModel()
    {
        var staffShiftContext = App.GetService<IStaffShiftContext>();

        OnShiftStaffs = staffShiftContext.OnShiftStaffs;

        _pageTilte = "Cửa hàng: CHƯA ĐĂNG NHẬP";
        _currentStaffAuthenticationStatus = "Nhân viên: CHƯA MỞ CA";

        // Check store authentication status
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }

        if (storeAuthenticationService.Context.CurrentStore == null)
        {
            return;
        }
        
        _pageTilte = $"Cửa hàng: {storeAuthenticationService.Context.CurrentStore.Name}";

        // Check staff authentication status
        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
        {
            return;
        }

        if (staffAuthenticationService.Context.CurrentStaff == null)
        {
            return;
        }

        _currentStaffAuthenticationStatus = $"Nhân viên: {staffAuthenticationService.Context.CurrentStaff.Name} (CHƯA MỞ CA)";

        // Check staff on-shift-state status
        // TODO: 
    }

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    public void HandleToCashierMenuViewButtonClick()
    {
        
    }

    public void HandleToInventoryMenuViewClick()
    {
        App.NavigateTo(typeof(InventoryMenuView));
    }

    public void HandleToStaffManagementViewButtonClick()
    {
        App.NavigateTo(typeof(StaffManagementView));
    }

    public void HandleToProfileViewButtonClick()
    {
        
    }

    public void HandleToRevenueDashboardViewButtonClick()
    {
    
    }

    public void HandleToSpecialOffersManagementViewButtonClick()
    {
    
    }

    public void HandleToConfigurationMenuViewButtonClick()
    {
        App.NavigateTo(typeof(ConfigurationMenuView));
    }

    public void HandleOpenSlashCloseShiftButtonClick()
    {
    
    }

    public void HandleReturnToLoginViewButtonClick()
    {
        App.NavigateTo(typeof(LoginView));
    }

    public void HandleChangeIdButtonClick()
    {
        // Store is logged in, so return to LoginView
        // will change the login tab to StaffLogin

        App.NavigateTo(typeof(LoginView));
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string PageTitle
    {
        get => _pageTilte;
        set
        {
            _pageTilte = value;
            OnPropertyChanged(nameof(PageTitle));
        }
    }
    
    public string CurrentStaffAuthenticationStatus
    {
        get => _currentStaffAuthenticationStatus;
        set
        {
            _currentStaffAuthenticationStatus = value;
            OnPropertyChanged(nameof(CurrentStaffAuthenticationStatus));
        }
    }
}