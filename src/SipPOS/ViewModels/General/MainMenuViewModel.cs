using System.ComponentModel;

using SipPOS.Views.Login;
using SipPOS.Views.Cashier;
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
    private string _clock;
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
        _clock = DateTime.Now.ToString("HH:mm:ss");

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

    /// <summary>
    /// Handles the event when the button to navigate to the product management view is clicked.
    /// </summary>
    public void HandleToProductManagementViewButtonClick()
    {
        App.NavigateTo(typeof(ProductManagementView));
    }

    public void HandleToInventoryMenuViewClick()
    {
        App.NavigateTo(typeof(InventoryMenuView));
    }

    public void HandleToCategoryManagementViewButtonClick()
    {
        App.NavigateTo(typeof(CategoryManagementView));
    }

    public void HandleToCustomerPaymentViewButtonClick()
    {
        IList<ProductDto> productDtos = new List<ProductDto>
        {
            new()
            {
                Id = 1,
                Name = "Cà phê sữa",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 2,
                Name = "Trà sữa",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 3,
                Name = "Nước mía",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 4,
                Name = "Nước lọc",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 5,
                Name = "Nước ngọt",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 6,
                Name = "Cà phê đen",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 7,
                Name = "Cà phê sữa đá",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 8,
                Name = "Trà sữa đá",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 9,
                Name = "Nước mía đá",
                Price = 1000,
                Quantity = 1
            },
            new()
            {
                Id = 10,
                Name = "Nước lọc đá",
                Price = 1000,
                Quantity = 1
            },
        };
        App.NavigateTo(typeof(CustomerPaymentView), productDtos);
    }

    public void HandleToConfigurationMenuViewButtonClick()
    {
        App.NavigateTo(typeof(ConfigurationMenuView));
    }

    /// <summary>
    /// Handles the event when the change ID button is clicked.
    /// </summary>
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

    public string Clock
    {
        get => _clock;
        set
        {
            _clock = value;
            OnPropertyChanged(nameof(Clock));
        }
    }
}