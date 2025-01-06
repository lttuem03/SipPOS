using System.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.General;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;

namespace SipPOS.ViewModels.Login;

/// <summary>
/// ViewModel for handling staff login, managing data for data-binding and the logic in the StaffLoginView.
/// </summary>
public class StaffLoginViewModel : INotifyPropertyChanged
{
    private const int MAX_LENGTH_STAFF_ID_TEXTBOX = 6; // 3 for store id, 3 for staff id

    // No need to bind these properties
    private bool _staffIdTextBoxFocused;

    // Properties used for data binding
    private string _staffId;
    private string _selectedPrefix;

    public Visibility StaffLoginFormVisibility { get; private set; }
    public Visibility StoreNotLoggedInTextBlockVisibility { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffLoginViewModel"/> class.
    /// </summary>
    public StaffLoginViewModel()
    {
        _staffIdTextBoxFocused = false;

        _staffId = "";
        _selectedPrefix = "ST";

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            StaffLoginFormVisibility = Visibility.Visible;
            StoreNotLoggedInTextBlockVisibility = Visibility.Collapsed;
            return;
        }

        StaffLoginFormVisibility = (storeAuthenticationService.Context.LoggedIn == true) ? Visibility.Visible : Visibility.Collapsed;
        StoreNotLoggedInTextBlockVisibility = (storeAuthenticationService.Context.LoggedIn == true) ? Visibility.Collapsed : Visibility.Visible;
    }

    /// <summary>
    /// Handles the event when the staff ID text box gets focus.
    /// </summary>
    public void HandleStaffIdTextBoxGotFocus()
    {
        _staffIdTextBoxFocused = true;
    }

    /// <summary>
    /// Handles the event when the select prefix combo box gets focus.
    /// </summary>
    public void HandleSelectPrefixComboBoxGotFocus()
    {
        _staffIdTextBoxFocused = false;
    }

    /// <summary>
    /// Handles the event when a numpad button is clicked.
    /// </summary>
    /// <param name="number">The number clicked on the numpad.</param>
    public void HandleNumpadButtonClick(string number)
    {
        if (_staffIdTextBoxFocused)
        {
            if (StaffId.Length + 1 > MAX_LENGTH_STAFF_ID_TEXTBOX)
            {
                return;
            }

            StaffId += number;
        }
    }

    public void HandleNumpadBackspaceButtonClick()
    {
        if (_staffIdTextBoxFocused)
        {
            if (StaffId.Length - 1 < 0)
            {
                return;
            }

            StaffId = StaffId.Substring(0, StaffId.Length - 1);
        }
    }

    public async void HandleStaffLoginButtonClick(TextBlock errorMessageTextBlock)
    {
        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
        {
            return;
        }

        var staffCompositeUsername = SelectedPrefix + StaffId;
        var loginResult = await staffAuthenticationService.LoginAsync(staffCompositeUsername);

        if (!loginResult.succeded)
        {
            errorMessageTextBlock.Text = loginResult.errorMessage ?? "Xảy ra lỗi không xác định";
            errorMessageTextBlock.Opacity = 1.0;
            return;
        }

        errorMessageTextBlock.Text = "";
        errorMessageTextBlock.Opacity = 0.0;

        App.NavigateTo(typeof(MainMenuView));
    }

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets or sets the staff ID.
    /// </summary>
    public string StaffId
    {
        get => _staffId;
        set
        {
            if (_staffId == value)
            {
                return;
            }
            _staffId = value;
            OnPropertyChanged(nameof(StaffId));
        }
    }

    /// <summary>
    /// Gets or sets the selected prefix.
    /// </summary>
    public string SelectedPrefix
    {
        get => _selectedPrefix;
        set
        {
            if (_selectedPrefix == value)
            {
                return;
            }
            _selectedPrefix = value;
            OnPropertyChanged(nameof(SelectedPrefix));
        }
    }
}
