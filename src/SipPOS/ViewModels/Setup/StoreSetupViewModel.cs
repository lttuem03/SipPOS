using System.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Views.Setup.Pages;
using SipPOS.Views.General;

using SipPOS.Models.Entity;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.Account.Interfaces;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Authentication.Implementations;

namespace SipPOS.ViewModels.Setup;

/// <summary>
/// ViewModel for managing the store setup process.
/// </summary>
public class StoreSetupViewModel : INotifyPropertyChanged, IStoreSetupViewModel
{
    // IMPORTANT: This ViewModel is used by ALL the pages in the
    // setup view. So it will be registered as a Singleton

    private readonly Dictionary<int, Type> _pages;
    private int _currentPageIndex;
    private readonly int _totalPageCount;

    public Frame? StoreSetupNavigationFrame { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    // Data-bound properties:

    // General
    private string _pageNumberStatus;
    private string _nextButtonText;

    // Page - Store Manager Account Setup
    private readonly StaffDto _storeManagerRawInfo; // all the fields use the same backup property
    private string _storeManagerConfirmPassword = string.Empty;

    private float _storeManagerNameErrorMessageOpacity;
    private string _storeManagerNameErrorMessageText;

    private float _storeManagerGenderErrorMessageOpacity;
    private string _storeManagerGenderErrorMessageText;

    private float _storeManagerEmailErrorMessageOpacity;
    private string _storeManagerEmailErrorMessageText;

    private float _storeManagerTelErrorMessageOpacity;
    private string _storeManagerTelErrorMessageText;

    private float _storeManagerAddressErrorMessageOpacity;
    private string _storeManagerAddressErrorMessageText;

    private float _storeManagerPasswordErrorMessageOpacity;
    private string _storeManagerPasswordErrorMessageText;

    private float _storeManagerConfirmPasswordErrorMessageOpacity;
    private string _storeManagerConfirmPasswordErrorMessageText;

    // Page - Summary
    private string _storeManagerDateOfBirthString = string.Empty;
    private string _storeManagerEmploymentStartDateString = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreSetupViewModel"/> class.
    /// </summary>
    public StoreSetupViewModel()
    {
        _pages = new Dictionary<int, Type>
        {
            { 0, typeof(StoreManageStaffAccountSetupPage) },
            { 1, typeof(AddInitialStaffsPage) },
            { 2, typeof(PaymentConfigurationInitalSetupPage) },
            { 3, typeof(StoreSetupSummaryPage) }
        };

        _currentPageIndex = 0;
        _totalPageCount = _pages.Count;
        _nextButtonText = "Tiếp theo";
        _pageNumberStatus = $"THIẾT LẬP CỬA HÀNG ({_currentPageIndex + 1}/{_pages.Count})";

        StoreManagerCompositeUsername = "";
        _storeManagerRawInfo = new();

        _storeManagerNameErrorMessageOpacity = 0.0F;
        _storeManagerNameErrorMessageText = string.Empty;

        _storeManagerGenderErrorMessageOpacity = 0.0F;
        _storeManagerGenderErrorMessageText = string.Empty;

        _storeManagerEmailErrorMessageOpacity = 0.0F;
        _storeManagerEmailErrorMessageText = string.Empty;

        _storeManagerTelErrorMessageOpacity = 0.0F;
        _storeManagerTelErrorMessageText = string.Empty;

        _storeManagerAddressErrorMessageOpacity = 0.0F;
        _storeManagerAddressErrorMessageText = string.Empty;

        _storeManagerPasswordErrorMessageOpacity = 0.0F;
        _storeManagerPasswordErrorMessageText = string.Empty;

        _storeManagerConfirmPasswordErrorMessageOpacity = 0.0F;
        _storeManagerConfirmPasswordErrorMessageText = string.Empty;

        StoreSetupNavigationFrame = null; // please assign this externally before calling any navigation

        if (App.GetService<IStoreAuthenticationService>() is StoreAuthenticationService storeAuthenticationService)
        {
            if (storeAuthenticationService.Context.CurrentStore != null)
            {
                _storeManagerRawInfo.Id = 0;
                _storeManagerRawInfo.StoreId = storeAuthenticationService.Context.CurrentStore.Id;
                _storeManagerRawInfo.PositionPrefix = "SM";
            }
        }
    }

    /// <summary>
    /// Navigates to the first page of the setup process.
    /// </summary>
    public void ToFirstPage()
    {
        if (StoreSetupNavigationFrame == null)
            return;
        
        StoreSetupNavigationFrame.Navigate(_pages[0]);
    }

    /// <summary>
    /// Handles the click event to navigate to the previous step in the setup process.
    /// </summary>
    public void HandleToPreviousStepButtonClick()
    {
        if (StoreSetupNavigationFrame == null)
            return;

        if (_currentPageIndex > 0)
        {
            _currentPageIndex--;
            StoreSetupNavigationFrame.Navigate(_pages[_currentPageIndex]);
            PageNumberStatus = $"THIẾT LẬP CỬA HÀNG ({_currentPageIndex + 1}/{_pages.Count})";

            NextButtonText = "Tiếp theo";
        }
    }

    /// <summary>
    /// Handles the click event to navigate to the next step in the setup process.
    /// </summary>
    public void HandleToNextStepButtonClick()
    {
        if (StoreSetupNavigationFrame == null)
            return;

        // Do the validations if the current page is the StoreManagerAccountSetupPage
        if (_currentPageIndex == 0)
        {
            if (validateStoreManagerInformation() == false)
                return;
        }

        // If the current page is the last page, user presses "Complete setup" 
        // button (formerly "Next" button)

        // This is handled differently in the HandleCompleteSetupButtonClick()

        if (_currentPageIndex < _pages.Count - 1)
        {
            _currentPageIndex++;
            StoreSetupNavigationFrame.Navigate(_pages[_currentPageIndex]);
            PageNumberStatus = $"THIẾT LẬP CỬA HÀNG ({_currentPageIndex + 1}/{_pages.Count})";

            // If reached the last page
            if (_currentPageIndex == _pages.Count - 1)
            {
                NextButtonText = "Hoàn thành thiết lập";
            }
        }
    }

    /// <summary>
    /// Handles the click event to complete the setup process.
    /// </summary>
    /// <param name="setupCompleteContentDialog">The content dialog to show upon setup completion.</param>
    public async void HandleCompleteSetupButtonClick(ContentDialog setupCompleteContentDialog)
    {
        ContentDialogResult result = await setupCompleteContentDialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
            return;

        // Create the store manager account
        var staffAccountCreationService = App.GetService<IStaffAccountCreationService>();
        var storeManagerStaffDto = await staffAccountCreationService.CreateAccountAsync(_storeManagerRawInfo);

        if (storeManagerStaffDto == null)
        {
            // TODO: Handle this
            // Hopefully this doesn't happen
            return;
        }

        // Logs the store manager in
        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
        {
            return;
        }

        var loginResult = await staffAuthenticationService.LoginAsync(storeManagerStaffDto.CompositeUsername);

        if (loginResult.succeded == false)
        {
            return;
        }

        // Login successful !
        App.NavigateTo(typeof(MainMenuView));
    }

    /// <summary>
    /// Handles the selection change event for the manager gender ComboBox.
    /// </summary>
    /// <param name="gendermanagerGenderComboBox">The ComboBox for selecting the manager's gender.</param>
    public void HandleManagerGenderComboBoxSelectionChanged(ComboBox gendermanagerGenderComboBox)
    {
        switch (gendermanagerGenderComboBox.SelectedIndex)
        {
            case -1:
                StoreManagerGender = "";
                break;
            case 0:
                StoreManagerGender = "Nam";
                break;
            case 1:
                StoreManagerGender = "Nữ";
                break;
        }
    }

    /// <summary>
    /// Handles the date changed event for the manager's date of birth CalendarDatePicker.
    /// </summary>
    public void HandleManagerDateOfBirthCalenderDatePickerDateChanged()
    {
        StoreManagerDateOfBirthString = StoreManagerDateOfBirth.ToString("dd/MM/yyyy");
    }

    /// <summary>
    /// Handles the date changed event for the manager's employment start date CalendarDatePicker.
    /// </summary>    
    public void HandleManagerEmploymentStartDateCalenderDatePickerDateChanged()
    {
        StoreManagerEmploymentStartDateString = StoreManagerEmploymentStartDate.ToString("dd/MM/yyyy");
    }

    /// <summary>
    /// Validates the store manager's information.
    /// </summary>
    /// <returns>True if all fields are valid, otherwise false.</returns>
    private bool validateStoreManagerInformation()
    {
        var allFieldsValid = true;

        var staffAccountCreationService = App.GetService<IStaffAccountCreationService>();
        var fieldValidationResults = staffAccountCreationService.ValidateFields(_storeManagerRawInfo);

        foreach (var fieldValidationResult in fieldValidationResults)
        {
            var field = fieldValidationResult.Key;
            var validationResult = fieldValidationResult.Value;

            var opacity = 0.0F;

            if (validationResult != "OK")
            {
                opacity = 1.0F;
                allFieldsValid = false;
            }

            switch (field)
            {
                case "name":
                    StoreManagerNameErrorMessageText = validationResult;
                    StoreManagerNameErrorMessageOpacity = opacity;
                    break;
                case "gender":
                    StoreManagerGenderErrorMessageText = validationResult;
                    StoreManagerGenderErrorMessageOpacity = opacity;
                    break;
                case "email":
                    StoreManagerEmailErrorMessageText = validationResult;
                    StoreManagerEmailErrorMessageOpacity = opacity;
                    break;
                case "tel":
                    StoreManagerTelErrorMessageText = validationResult;
                    StoreManagerTelErrorMessageOpacity = opacity;
                    break;
                case "address":
                    StoreManagerAddressErrorMessageText = validationResult;
                    StoreManagerAddressErrorMessageOpacity = opacity;
                    break;
                case "password":
                    StoreManagerPasswordErrorMessageText = validationResult;
                    StoreManagerPasswordErrorMessageOpacity = opacity;
                    break;
            }
        }

        if (_storeManagerRawInfo.PasswordHash != _storeManagerConfirmPassword)
        {
            allFieldsValid = false;
            StoreManagerConfirmPasswordErrorMessageText = "Mật khẩu xác nhận không khớp";
            StoreManagerConfirmPasswordErrorMessageOpacity = 1.0F;
        }

        return allFieldsValid;
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets or sets the page number status.
    /// </summary>
    public string PageNumberStatus
    {
        get => _pageNumberStatus;
        set
        {
            _pageNumberStatus = value;
            OnPropertyChanged(nameof(PageNumberStatus));
        }
    }

    /// <summary>
    /// Gets or sets the text for the next button.
    /// </summary>
    public string NextButtonText
    {
        get => _nextButtonText;
        set
        {
            _nextButtonText = value;
            OnPropertyChanged(nameof(NextButtonText));
        }
    }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPageCount
    {
        get => _totalPageCount;
        private set
        {
            // Nothing here because you can't set it
        }
    }

    /// <summary>
    /// Gets the current page index.
    /// </summary>
    public int CurrentPageIndex
    {
        get => _currentPageIndex;
        private set
        {
            // Nothing here because you can't set it
        }
    }

    /// <summary>
    /// Gets or sets the store manager's name.
    /// </summary>
    public string StoreManagerName
    {
        get => _storeManagerRawInfo.Name;
        set
        {
            _storeManagerRawInfo.Name = value;
            OnPropertyChanged(nameof(StoreManagerName));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's gender.
    /// </summary>
    public string StoreManagerGender
    {
        get => _storeManagerRawInfo.Gender;
        set
        {
            _storeManagerRawInfo.Gender = value;
            OnPropertyChanged(nameof(StoreManagerGender));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's date of birth.
    /// </summary>
    public DateTimeOffset StoreManagerDateOfBirth
    {
        // the time returned by CalendarDatePicker is DateTimeOffset, so we have to do casting here
        get => (DateTimeOffset)_storeManagerRawInfo.DateOfBirth.ToDateTime(TimeOnly.MinValue);
        set
        {
            _storeManagerRawInfo.DateOfBirth = DateOnly.FromDateTime(value.DateTime);
            OnPropertyChanged(nameof(StoreManagerDateOfBirth));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's email.
    /// </summary>
    public string StoreManagerEmail
    {
        get => _storeManagerRawInfo.Email;
        set
        {
            _storeManagerRawInfo.Email = value;
            OnPropertyChanged(nameof(StoreManagerEmail));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's telephone number.
    /// </summary>
    public string StoreManagerTel
    {
        get => _storeManagerRawInfo.Tel;
        set
        {
            _storeManagerRawInfo.Tel = value;
            OnPropertyChanged(nameof(StoreManagerTel));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's employment start date.
    /// </summary>
    public DateTimeOffset StoreManagerEmploymentStartDate
    {
        // the time returned by CalendarDatePicker is DateTimeOffset, so we have to do casting here
        get => (DateTimeOffset)_storeManagerRawInfo.EmploymentStartDate.ToDateTime(TimeOnly.MinValue);
        set
        {
            _storeManagerRawInfo.EmploymentStartDate = DateOnly.FromDateTime(value.DateTime);
            OnPropertyChanged(nameof(StoreManagerEmploymentStartDate));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's address.
    /// </summary>
    public string StoreManagerAddress
    {
        get => _storeManagerRawInfo.Address;
        set
        {
            _storeManagerRawInfo.Address = value;
            OnPropertyChanged(nameof(StoreManagerAddress));
        }
    }

    /// <summary>
    /// Gets the store manager's composite username.
    /// </summary>
    public string StoreManagerCompositeUsername
    {
        get => _storeManagerRawInfo.CompositeUsername;
        private set
        {
            // Nothing here because you can't set it here
        }
    }

    /// <summary>
    /// Gets or sets the store manager's password.
    /// </summary>
    public string StoreManagerPassword
    {
        get => _storeManagerRawInfo.PasswordHash; // in this page, password is un-hashed
        set
        {
            _storeManagerRawInfo.PasswordHash = value;
            OnPropertyChanged(nameof(StoreManagerPassword));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's confirm password.
    /// </summary>
    public string StoreManagerConfirmPassword
    {
        get => _storeManagerConfirmPassword;
        set
        {
            _storeManagerConfirmPassword = value;
            OnPropertyChanged(nameof(StoreManagerConfirmPassword));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's date of birth as a string.
    /// </summary>
    public string StoreManagerDateOfBirthString
    {
        get => _storeManagerDateOfBirthString;
        set
        {
            _storeManagerDateOfBirthString = value;
            OnPropertyChanged(nameof(StoreManagerDateOfBirthString));
        }
    }

    /// <summary>
    /// Gets or sets the store manager's employment start date as a string.
    /// </summary>
    public string StoreManagerEmploymentStartDateString
    {
        get => _storeManagerEmploymentStartDateString;
        set
        {
            _storeManagerEmploymentStartDateString = value;
            OnPropertyChanged(nameof(StoreManagerEmploymentStartDateString));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store manager's name error message.
    /// </summary>
    public float StoreManagerNameErrorMessageOpacity
    {
        get => _storeManagerNameErrorMessageOpacity;
        set
        {
            _storeManagerNameErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreManagerNameErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store manager's name error message.
    /// </summary>
    public string StoreManagerNameErrorMessageText
    {
        get => _storeManagerNameErrorMessageText;
        set
        {
            _storeManagerNameErrorMessageText = value;
            OnPropertyChanged(nameof(StoreManagerNameErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store manager's gender error message.
    /// </summary>
    public float StoreManagerGenderErrorMessageOpacity
    {
        get => _storeManagerGenderErrorMessageOpacity;
        set
        {
            _storeManagerGenderErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreManagerGenderErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store manager's gender error message.
    /// </summary>
    public string StoreManagerGenderErrorMessageText
    {
        get => _storeManagerGenderErrorMessageText;
        set
        {
            _storeManagerGenderErrorMessageText = value;
            OnPropertyChanged(nameof(StoreManagerGenderErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store manager's email error message.
    /// </summary>
    public float StoreManagerEmailErrorMessageOpacity
    {
        get => _storeManagerEmailErrorMessageOpacity;
        set
        {
            _storeManagerEmailErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreManagerEmailErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store manager's email error message.
    /// </summary>
    public string StoreManagerEmailErrorMessageText
    {
        get => _storeManagerEmailErrorMessageText;
        set
        {
            _storeManagerEmailErrorMessageText = value;
            OnPropertyChanged(nameof(StoreManagerEmailErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store manager's telephone error message.
    /// </summary>
    public float StoreManagerTelErrorMessageOpacity
    {
        get => _storeManagerTelErrorMessageOpacity;
        set
        {
            _storeManagerTelErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreManagerTelErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store manager's telephone error message.
    /// </summary>
    public string StoreManagerTelErrorMessageText
    {
        get => _storeManagerTelErrorMessageText;
        set
        {
            _storeManagerTelErrorMessageText = value;
            OnPropertyChanged(nameof(StoreManagerTelErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store manager's address error message.
    /// </summary>
    public float StoreManagerAddressErrorMessageOpacity
    {
        get => _storeManagerAddressErrorMessageOpacity;
        set
        {
            _storeManagerAddressErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreManagerAddressErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store manager's address error message.
    /// </summary>
    public string StoreManagerAddressErrorMessageText
    {
        get => _storeManagerAddressErrorMessageText;
        set
        {
            _storeManagerAddressErrorMessageText = value;
            OnPropertyChanged(nameof(StoreManagerAddressErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store manager's password error message.
    /// </summary>
    public float StoreManagerPasswordErrorMessageOpacity
    {
        get => _storeManagerPasswordErrorMessageOpacity;
        set
        {
            _storeManagerPasswordErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreManagerPasswordErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store manager's password error message.
    /// </summary>
    public string StoreManagerPasswordErrorMessageText
    {
        get => _storeManagerPasswordErrorMessageText;
        set
        {
            _storeManagerPasswordErrorMessageText = value;
            OnPropertyChanged(nameof(StoreManagerPasswordErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store manager's confirm password error message.
    /// </summary>
    public float StoreManagerConfirmPasswordErrorMessageOpacity
    {
        get => _storeManagerConfirmPasswordErrorMessageOpacity;
        set
        {
            _storeManagerConfirmPasswordErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreManagerConfirmPasswordErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store manager's confirm password error message.
    /// </summary>
    public string StoreManagerConfirmPasswordErrorMessageText
    {
        get => _storeManagerConfirmPasswordErrorMessageText;
        set
        {
            _storeManagerConfirmPasswordErrorMessageText = value;
            OnPropertyChanged(nameof(StoreManagerConfirmPasswordErrorMessageText));
        }
    }
}