using System.ComponentModel;
using System.Collections.ObjectModel;


using SipPOS.Context.Shift.Interface;
using SipPOS.Views.Login;
using SipPOS.Views.Staff;
using SipPOS.Views.Configuration;
using SipPOS.Views.Inventory;
using SipPOS.DataTransfer.Entity;
using SipPOS.Models.General;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Authentication.Implementations;

using SipPOS.Services.Accessibility.Interfaces;
using Windows.ApplicationModel.PackageExtensions;
using Microsoft.UI.Xaml.Controls;

namespace SipPOS.ViewModels.General;

/// <summary>
/// ViewModel for managing the main menu, handling data for data-binding and the logic in the MainMenuView.
/// </summary>
public class MainMenuViewModel : INotifyPropertyChanged
{
    // Data-bound properties
    private string _pageTilte;
    private string _currentStaffAuthenticationStatus;

    // Accessibility properties
    private bool _canAccessCashierMenu;
    private bool _canAccessInventoryMenu;
    private bool _canAccessStaffManagementMenu;
    private bool _canAccessProfileMenu;
    private bool _canAccessOrderHistoryMenu;
    private bool _canAccessRevenueDashboardMenu;
    private bool _canAccessSpecialOffersManagementMenu;
    private bool _canAccessConfigurationMenu;

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

        // Enforce staff accessibility status
        var pep = App.GetService<IPolicyEnforcementPoint>();
        var currentStaff = staffAuthenticationService.Context.CurrentStaff;

        _canAccessCashierMenu = pep.Enforce(currentStaff, Position.Staff);
        _canAccessInventoryMenu = pep.Enforce(currentStaff, Position.AssistantManager);
        _canAccessStaffManagementMenu = pep.Enforce(currentStaff, Position.StoreManager);
        _canAccessProfileMenu = pep.Enforce(currentStaff, Position.Staff);
        _canAccessOrderHistoryMenu = pep.Enforce(currentStaff, Position.Staff);
        _canAccessRevenueDashboardMenu = pep.Enforce(currentStaff, Position.StoreManager);
        _canAccessSpecialOffersManagementMenu = pep.Enforce(currentStaff, Position.AssistantManager);
        _canAccessConfigurationMenu = pep.Enforce(currentStaff, Position.StoreManager);

        // Check staff on-shift-state status
        // TODO: 

        // 
    }

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Handles the button click to navigate to the Cashier Menu view.
    /// </summary>
    public void HandleToCashierMenuViewButtonClick(InfoBar notificationInfoBar)
    {
        if (!CanAccessCashierMenu)
        {
            ShowAccessDeniedNotification(notificationInfoBar);
            return;
        }
    }

    /// <summary>
    /// Handles the button click to navigate to the Inventory Menu view.
    /// </summary>
    public void HandleToInventoryMenuViewClick(InfoBar notificationInfoBar)
    {
        if (!CanAccessInventoryMenu)
        {
            ShowAccessDeniedNotification(notificationInfoBar);
            return;
        }

        App.NavigateTo(typeof(InventoryMenuView));
    }

    /// <summary>
    /// Handles the button click to navigate to the Staff Management view.
    /// </summary>
    public void HandleToStaffManagementViewButtonClick(InfoBar notificationInfoBar)
    {
        if (!CanAccessStaffManagementMenu)
        {
            ShowAccessDeniedNotification(notificationInfoBar);
            return;
        }

        App.NavigateTo(typeof(StaffManagementView));
    }

    /// <summary>
    /// Handles the button click to navigate to the Profile view.
    /// </summary>
    public void HandleToProfileViewButtonClick(InfoBar notificationInfoBar)
    {
        if (!CanAccessProfileMenu)
        {
            ShowAccessDeniedNotification(notificationInfoBar);
            return;
        }
    }

    /// <summary>
    /// Handles the button click to navigate to the Revenue Dashboard view.
    /// </summary>
    public void HandleToRevenueDashboardViewButtonClick(InfoBar notificationInfoBar)
    {
        if (!CanAccessRevenueDashboardMenu)
        {
            ShowAccessDeniedNotification(notificationInfoBar);
            return;
        }
    }

    /// <summary>
    /// Handles the button click to navigate to the Order History view.
    /// </summary>
    public void HandleToOrderHistoryViewButtonClick(InfoBar notificationInfoBar)
    {
        if (!CanAccessOrderHistoryMenu)
        {
            ShowAccessDeniedNotification(notificationInfoBar);
            return;
        }
    }

    /// <summary>
    /// Handles the button click to navigate to the Special Offers Management view.
    /// </summary>
    public void HandleToSpecialOffersManagementViewButtonClick(InfoBar notificationInfoBar)
    {
        if (!CanAccessSpecialOffersManagementMenu)
        {
            ShowAccessDeniedNotification(notificationInfoBar);
            return;
        }
    }

    /// <summary>
    /// Handles the button click to navigate to the Configuration Menu view.
    /// </summary>
    public void HandleToConfigurationMenuViewButtonClick(InfoBar notificationInfoBar)
    {
        if (!CanAccessConfigurationMenu)
        {
            ShowAccessDeniedNotification(notificationInfoBar);
            return;
        }

        App.NavigateTo(typeof(ConfigurationMenuView));
    }

    /// <summary>
    /// Handles the button click to open or close the shift.
    /// </summary>
    public void HandleOpenSlashCloseShiftButtonClick()
    {

    }

    /// <summary>
    /// Handles the button click to return to the Login view.
    /// </summary>
    public void HandleReturnToLoginViewButtonClick()
    {
        App.NavigateTo(typeof(LoginView));
    }

    /// <summary>
    /// Handles the button click to change the ID (logout of staff account).
    /// </summary>
    public void HandleChangeIdButtonClick()
    {
        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
        {
            return;
        }

        staffAuthenticationService.Logout();

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

    private void ShowAccessDeniedNotification(InfoBar notificationInfoBar)
    {
        notificationInfoBar.Message = "Bạn không có quyền truy cập vào tính năng này!";
        notificationInfoBar.IsOpen = true;
    }

    /// <summary>
    /// Gets or sets the page title.
    /// </summary>
    public string PageTitle
    {
        get => _pageTilte;
        set
        {
            _pageTilte = value;
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    /// <summary>
    /// Gets or sets the current staff authentication status.
    /// </summary>
    public string CurrentStaffAuthenticationStatus
    {
        get => _currentStaffAuthenticationStatus;
        set
        {
            _currentStaffAuthenticationStatus = value;
            OnPropertyChanged(nameof(CurrentStaffAuthenticationStatus));
        }
    }

    public bool CanAccessCashierMenu
    {
        get => _canAccessCashierMenu;
        set
        {
            _canAccessCashierMenu = value;
            OnPropertyChanged(nameof(CanAccessCashierMenu));
        }
    }

    public bool CanAccessInventoryMenu
    {
        get => _canAccessInventoryMenu;
        set
        {
            _canAccessInventoryMenu = value;
            OnPropertyChanged(nameof(CanAccessInventoryMenu));
        }
    }

    public bool CanAccessStaffManagementMenu
    {
        get => _canAccessStaffManagementMenu;
        set
        {
            _canAccessStaffManagementMenu = value;
            OnPropertyChanged(nameof(CanAccessStaffManagementMenu));
        }
    }

    public bool CanAccessProfileMenu
    {
        get => _canAccessProfileMenu;
        set
        {
            _canAccessProfileMenu = value;
            OnPropertyChanged(nameof(CanAccessProfileMenu));
        }
    }

    public bool CanAccessOrderHistoryMenu
    {
        get => _canAccessOrderHistoryMenu;
        set
        {
            _canAccessOrderHistoryMenu = value;
            OnPropertyChanged(nameof(CanAccessOrderHistoryMenu));
        }
    }    

    public bool CanAccessRevenueDashboardMenu
    {
        get => _canAccessRevenueDashboardMenu;
        set
        {
            _canAccessRevenueDashboardMenu = value;
            OnPropertyChanged(nameof(CanAccessRevenueDashboardMenu));
        }
    }

    public bool CanAccessSpecialOffersManagementMenu
    {
        get => _canAccessSpecialOffersManagementMenu;
        set
        {
            _canAccessSpecialOffersManagementMenu = value;
            OnPropertyChanged(nameof(CanAccessSpecialOffersManagementMenu));
        }
    }

    public bool CanAccessConfigurationMenu
    {
        get => _canAccessConfigurationMenu;
        set
        {
            _canAccessConfigurationMenu = value;
            OnPropertyChanged(nameof(CanAccessConfigurationMenu));
        }
    }
}
