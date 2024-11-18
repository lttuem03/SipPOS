using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.DataTransfer.Entity;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Views;

namespace SipPOS.ViewModels;

/// <summary>
/// ViewModel for managing the main menu, handling data for data-binding and the logic in the MainMenuView.
/// </summary>
public class MainMenuViewModel : INotifyPropertyChanged
{
    private string _currentStoreAuthenticationStatus;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenuViewModel"/> class.
    /// </summary>
    public MainMenuViewModel()
    {
        _currentStoreAuthenticationStatus = "Đang kiểm tra trạng thái đăng nhập...";

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }

        if (storeAuthenticationService.Context.CurrentStore == null)
        {
            CurrentStoreAuthenticationStatus = $"Bạn chưa đăng nhập";
        }
        else
        {
            CurrentStoreAuthenticationStatus = $"Đã đăng nhập bằng tài khoản: {storeAuthenticationService.Context.CurrentStore.Username}";
        }
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

    /// <summary>
    /// Handles the event when the change ID button is clicked.
    /// </summary>
    public void HandleChangeIdButtonClick()
    {
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

    /// <summary>
    /// Gets or sets the current store authentication status.
    /// </summary>
    public string CurrentStoreAuthenticationStatus
    {
        get => _currentStoreAuthenticationStatus;
        set
        {
            _currentStoreAuthenticationStatus = value;
            OnPropertyChanged(nameof(CurrentStoreAuthenticationStatus));
        }
    }
}
