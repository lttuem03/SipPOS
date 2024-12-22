using System.ComponentModel;
using System.Text.RegularExpressions;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.General;
using SipPOS.Views.Setup.Pages;
using SipPOS.DataTransfer.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.Account.Interfaces;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Configuration.Interfaces;
using Microsoft.Extensions.Logging.Console;

namespace SipPOS.ViewModels.Setup;

/// <summary>
/// ViewModel for managing the store setup process.
/// </summary>
public partial class StoreSetupViewModel : INotifyPropertyChanged, IStoreSetupViewModel
{
    // IMPORTANT: This ViewModel is used by ALL the pages in the
    // setup view. So it will be registered as a Singleton.
    // I'm sorry for whoever has to review this.

    private readonly Dictionary<int, Type> _pages;
    private int _currentPageIndex;
    private readonly int _totalPageCount;

    public Frame? StoreSetupNavigationFrame { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    // General
    private string _pageNumberStatus;
    private string _nextButtonText;

    // Page - Store Manager Account Setup
    private readonly StaffDto _storeManagerRawInfo = new(); // all the fields use the same backup property
    private string _storeManagerConfirmPassword = string.Empty;
    private float _storeManagerNameErrorMessageOpacity = 0.0F;
    private string _storeManagerNameErrorMessageText = string.Empty;
    private float _storeManagerGenderErrorMessageOpacity = 0.0F;
    private string _storeManagerGenderErrorMessageText = string.Empty;
    private float _storeManagerEmailErrorMessageOpacity = 0.0F;
    private string _storeManagerEmailErrorMessageText = string.Empty;
    private float _storeManagerTelErrorMessageOpacity = 0.0F;
    private string _storeManagerTelErrorMessageText = string.Empty;
    private float _storeManagerAddressErrorMessageOpacity = 0.0F;
    private string _storeManagerAddressErrorMessageText = string.Empty;
    private float _storeManagerPasswordErrorMessageOpacity = 0.0F;
    private string _storeManagerPasswordErrorMessageText = string.Empty;
    private float _storeManagerConfirmPasswordErrorMessageOpacity = 0.0F;
    private string _storeManagerConfirmPasswordErrorMessageText = string.Empty;

    // Pages - Initial configurations
    private readonly ConfigurationDto _initialConfigurationDto = new();

    private string _operatingHoursString = string.Empty;
    private float _operatingHoursErrorMessageOpacity = 0.0F;
    private string _operatingHoursErrorMessageText = string.Empty;

    private float _taxConfigurationErrorMessageOpacity = 0.0F;
    private string _taxConfigurationErrorMessageText = string.Empty;

    private bool _staffBaseSalaryCheckBoxChecked = false;
    private bool _staffHourlySalaryCheckBoxChecked = false;
    private bool _assistantManagerBaseSalaryCheckBoxChecked = false;
    private bool _assistantManagerHourlySalaryCheckBoxChecked = false;
    private bool _storeManagerBaseSalaryCheckBoxChecked = false;
    private bool _storeManagerHourlySalaryCheckBoxChecked = false;

    private string _staffBaseSalaryString = string.Empty;
    private string _staffHourlySalaryString = string.Empty;
    private string _assistantManagerBaseSalaryString = string.Empty;
    private string _assistantManagerHourlySalaryString = string.Empty;
    private string _storeManagerBaseSalaryString = string.Empty;
    private string _storeManagerHourlySalaryString = string.Empty;

    private float _salaryErrorMessageOpacity = 0.0F;
    private string _salaryErrorMessageText = string.Empty;

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
            { 0, typeof(StoreManagerStaffAccountSetupPage) },
            { 1, typeof(StoreConfigurationInitialSetupPage) },
            { 2, typeof(TaxConfigurationInitialSetupPage) },
            { 3, typeof(SalaryConfigurationInitialSetupPage) },
            { 4, typeof(StoreSetupSummaryPage) }
        };

        _currentPageIndex = 0;
        _totalPageCount = _pages.Count;
        _nextButtonText = "Tiếp theo";
        _pageNumberStatus = $"THIẾT LẬP CỬA HÀNG ({_currentPageIndex + 1}/{_pages.Count})";

        StoreManagerCompositeUsername = "";
        VatRate = 0.0m;
        VatMethod = "VAT_INCLUDED";
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
        if (StoreSetupNavigationFrame.CurrentSourcePageType == typeof(StoreManagerStaffAccountSetupPage))
        {
            if (validateStoreManagerInformation() == false)
                return;
        }

        // Do the validations if the current page is the StoreConfigurationInitialSetupPage
        if (StoreSetupNavigationFrame.CurrentSourcePageType == typeof(StoreConfigurationInitialSetupPage))
        {
            if (validateStoreConfigurationInformation() == false)
                return;
        }

        // Do the validations if the current page is the TaxConfigurationInitialSetupPage
        if (StoreSetupNavigationFrame.CurrentSourcePageType == typeof(TaxConfigurationInitialSetupPage))
        {
            if (validateTaxConfigurationInformation() == false)
                return;
        }

        // Do the validation if the current page is the SalaryConfigurationInitialSetupPage
        if (StoreSetupNavigationFrame.CurrentSourcePageType == typeof(SalaryConfigurationInitialSetupPage))
        {
            if (validateSalaryConfigurationInformation() == false)
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
            // Just pray that this will not happen
            return;
        }

        // Create the configuration model for the store
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var configurationService = App.GetService<IConfigurationService>();
        var createResult = await configurationService.CreateAsync(storeAuthenticationService.GetCurrentStoreId(), _initialConfigurationDto);

        if (!createResult)
        {
            // Just pray that this will not happen
            return;
        }
    
        // Load the configuration
        await configurationService.LoadAsync(storeAuthenticationService.GetCurrentStoreId());

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

    public void HandleSelectOpeningHourTimePickerSelectedTimeChanged(TimePicker selectOpeningHourTimePicker)
    {
        OpeningTime = TimeOnly.FromTimeSpan(selectOpeningHourTimePicker.Time);

        OperatingHoursString = $"{OpeningTime.ToString("HH:mm")} đến {ClosingTime.ToString("HH:mm")}";
    }

    public void HandleSelectClosingHourTimePickerSelectedTimeChanged(TimePicker selectClosingHourTimePicker)
    {
        ClosingTime = TimeOnly.FromTimeSpan(selectClosingHourTimePicker.Time);

        OperatingHoursString = $"{OpeningTime.ToString("HH:mm")} đến {ClosingTime.ToString("HH:mm")}";
    }

    public void HandleGeneralVatRateComboBoxSelectionChanged(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                VatRate = 0.00m;
                break;
            case 1:
                VatRate = 0.01m;
                break;
            case 2:
                VatRate = 0.03m;
                break;
            case 3:
                VatRate = 0.05m;
                break;
            case 4:
                VatRate = 0.08m;
                break;
            case 5:
                VatRate = 0.10m;
                break;
        }
    }

    public void HandleSelectVatMethodComboBoxSelectionChanged(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                VatMethod = "VAT_INCLUDED";
                break;
            case 1:
                VatMethod = "ORDER_BASED";
                break;
        }
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

    private bool validateStoreConfigurationInformation()
    {
        var allFieldsValid = true;

        if (OpeningTime == TimeOnly.MinValue && ClosingTime == TimeOnly.MinValue)
        {
            OperatingHoursErrorMessageText = "Vui lòng chọn giờ mở cửa và giờ đóng cửa";
            OperatingHoursErrorMessageOpacity = 1.0F;

            allFieldsValid = false;

            return allFieldsValid;
        }

        if (OpeningTime != TimeOnly.MinValue && ClosingTime == TimeOnly.MinValue)
        {
            OperatingHoursErrorMessageText = "Vui lòng chọn giờ đóng cửa";
            OperatingHoursErrorMessageOpacity = 1.0F;

            allFieldsValid = false;

            return allFieldsValid;
        }

        if (OpeningTime == TimeOnly.MinValue && ClosingTime != TimeOnly.MinValue)
        {
            OperatingHoursErrorMessageText = "Vui lòng chọn giờ mở cửa";
            OperatingHoursErrorMessageOpacity = 1.0F;

            allFieldsValid = false;

            return allFieldsValid;
        }

        if (OpeningTime.CompareTo(ClosingTime) >= 0)
        {
            OperatingHoursErrorMessageText = "Giờ mở cửa phải trước giờ đóng cửa";
            OperatingHoursErrorMessageOpacity = 1.0F;

            allFieldsValid = false;

            return allFieldsValid;
        }

        if (allFieldsValid)
            OperatingHoursErrorMessageOpacity = 0.0F;

        return allFieldsValid;
    }

    private bool validateTaxConfigurationInformation()
    {
        var allFieldsValid = true;

        if (string.IsNullOrWhiteSpace(TaxCode))
        {
            TaxConfigurationErrorMessageText = "Vui lòng nhập mã số thuế";
            TaxConfigurationErrorMessageOpacity = 1.0F;

            allFieldsValid = false;

            return allFieldsValid;
        }

        var taxCodePattern = @"^\d{8}[1-9]\d(-\d\d[1-9])?$";

        if (!Regex.IsMatch(TaxCode, taxCodePattern))
        {
            TaxConfigurationErrorMessageText = @"
                                  Mã số thuế sai định dạng.
            Đối với định dạng MST 13 chữ số phải có dấu gạch ngang '-' ở sau chữ số thứ 10.
            ";
            TaxConfigurationErrorMessageOpacity = 1.0F;

            allFieldsValid = false;

            return allFieldsValid;
        }

        if (allFieldsValid)
            TaxConfigurationErrorMessageOpacity = 0.0F;

        return allFieldsValid;
    }

    private bool validateSalaryConfigurationInformation()
    {
        var allFieldsValid = true;

        if (!StaffBaseSalaryCheckBoxChecked && !StaffHourlySalaryCheckBoxChecked)
        {
            SalaryErrorMessageText = "Vui lòng chọn ít nhất một loại lương mỗi bậc nhân viên";
            SalaryErrorMessageOpacity = 1.0F;

            return false;
        }

        if (!AssistantManagerBaseSalaryCheckBoxChecked && !AssistantManagerHourlySalaryCheckBoxChecked)
        {
            SalaryErrorMessageText = "Vui lòng chọn ít nhất một loại lương mỗi bậc nhân viên";
            SalaryErrorMessageOpacity = 1.0F;

            return false;
        }

        if (!StoreManagerBaseSalaryCheckBoxChecked && !StoreManagerHourlySalaryCheckBoxChecked)
        {
            SalaryErrorMessageText = "Vui lòng chọn ít nhất một loại lương mỗi bậc nhân viên";
            SalaryErrorMessageOpacity = 1.0F;

            return false;
        }

        // Convert the strings to decimal values
        // We made sure that the values entered in the salary TextBoxes are numeric only
        // in the TextChanging event handler for all the salary TextBoxes

        var staffBaseSalaryString = StaffBaseSalaryString.Replace(",", "");
        var staffHourlySalaryString = StaffHourlySalaryString.Replace(",", "");
        var assistantManagerBaseSalaryString = AssistantManagerBaseSalaryString.Replace(",", "");
        var assistantManagerHourlySalaryString = AssistantManagerHourlySalaryString.Replace(",", "");
        var storeManagerBaseSalaryString = StoreManagerBaseSalaryString.Replace(",", "");
        var storeManagerHourlySalaryString = StoreManagerHourlySalaryString.Replace(",", "");

        StaffBaseSalary = Decimal.TryParse(staffBaseSalaryString, out var staffBaseSalary) ? staffBaseSalary : 0m;
        StaffHourlySalary = Decimal.TryParse(staffHourlySalaryString, out var staffHourlySalary) ? staffHourlySalary : 0m;
        AssistantManagerBaseSalary = Decimal.TryParse(assistantManagerBaseSalaryString, out var assistantManagerBaseSalary) ? assistantManagerBaseSalary : 0m;
        AssistantManagerHourlySalary = Decimal.TryParse(assistantManagerHourlySalaryString, out var assistantManagerHourlySalary) ? assistantManagerHourlySalary : 0m;
        StoreManagerBaseSalary = Decimal.TryParse(storeManagerBaseSalaryString, out var storeManagerBaseSalary) ? storeManagerBaseSalary : 0m;
        StoreManagerHourlySalary = Decimal.TryParse(storeManagerHourlySalaryString, out var storeManagerHourlySalary) ? storeManagerHourlySalary : 0m;

        if (StaffBaseSalary % 500 != 0 ||
            StaffHourlySalary % 500 != 0 ||
            AssistantManagerBaseSalary % 500 != 0 ||
            AssistantManagerHourlySalary % 500 != 0 ||
            StoreManagerBaseSalary % 500 != 0 ||
            StoreManagerHourlySalary % 500 != 0)
        {
            SalaryErrorMessageText = "Giá trị lương phải chia hết cho 500 VNĐ.";
            SalaryErrorMessageOpacity = 1.0F;

            return false;
        }

        if (allFieldsValid)
            SalaryErrorMessageOpacity = 0.0F;

        return allFieldsValid;
    }
}