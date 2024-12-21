using System.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using SipPOS.DataTransfer.Entity;
using SipPOS.Views.Staff;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Account.Interfaces;

namespace SipPOS.ViewModels.Staff;

/// <summary>
/// ViewModel for staff registration.
/// </summary>
public class StaffRegistrationViewModel : INotifyPropertyChanged
{
    // Data-bound properties
    private readonly StaffDto _staffRawInfo;
    private long _staffId;
    private string _staffCompositeUsername;
    private string _staffDateOfBirthString;
    private string _staffEmploymentStartDateString;

    // Validation properties
    private float _staffNameErrorMessageOpacity;
    private string _staffNameErrorMessageText;

    private float _staffGenderErrorMessageOpacity;
    private string _staffGenderErrorMessageText;

    private float _staffEmailErrorMessageOpacity;
    private string _staffEmailErrorMessageText;

    private float _staffTelErrorMessageOpacity;
    private string _staffTelErrorMessageText;

    private float _staffAddressErrorMessageOpacity;
    private string _staffAddressErrorMessageText;

    private float _staffPositionErrorMessageOpacity;
    private string _staffPositionErrorMessageText;

    private string _accountCreationResult;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffRegistrationViewModel"/> class.
    /// </summary>
    public StaffRegistrationViewModel()
    {
        _staffNameErrorMessageOpacity = 0.0F;
        _staffNameErrorMessageText = string.Empty;

        _staffGenderErrorMessageOpacity = 0.0F;
        _staffGenderErrorMessageText = string.Empty;

        _staffEmailErrorMessageOpacity = 0.0F;
        _staffEmailErrorMessageText = string.Empty;

        _staffTelErrorMessageOpacity = 0.0F;
        _staffTelErrorMessageText = string.Empty;

        _staffAddressErrorMessageOpacity = 0.0F;
        _staffAddressErrorMessageText = string.Empty;

        _staffPositionErrorMessageOpacity = 0.0F;
        _staffPositionErrorMessageText = string.Empty;

        _accountCreationResult = "";

        _staffRawInfo = new();
        _staffRawInfo.StoreId = -1;
        _staffRawInfo.PasswordHash = "123456"; // default
        _staffCompositeUsername = "";
        _staffDateOfBirthString = "";
        _staffEmploymentStartDateString = "";
        _staffId = -1;
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">Name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Handles the selection change of the staff gender ComboBox.
    /// </summary>
    /// <param name="comboBoxIndex">Index of the selected item.</param>
    public void HandleStaffGenderComboBoxSelectionChanged(int comboBoxIndex)
    {
        switch (comboBoxIndex)
        {
            case 0:
                StaffGender = "Nam";
                break;
            case 1:
                StaffGender = "Nữ";
                break;
        }
    }

    /// <summary>
    /// Handles the date change of the staff date of birth CalendarDatePicker.
    /// </summary>
    public void HandleStaffDateOfBirthCalenderDatePickerDateChanged()
    {
        StaffDateOfBirthString = StaffDateOfBirth.ToString("dd/MM/yyyy");
    }

    /// <summary>
    /// Handles the date change of the staff employment start date CalendarDatePicker.
    /// </summary>
    public void HandleStaffEmploymentStartDateCalenderDatePickerDateChanged()
    {
        StaffEmploymentStartDateString = StaffEmploymentStartDate.ToString("dd/MM/yyyy");
    }

    /// <summary>
    /// Handles the selection change of the staff position ComboBox.
    /// </summary>
    /// <param name="comboBoxIndex">Index of the selected item.</param>
    public async void HandleStaffPositionComboBoxSelectionChanged(int comboBoxIndex)
    {
        // Still hasn't retrieve yet
        if (_staffRawInfo.StoreId == -1 || _staffId == -1)
        {
            if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
                return;

            if (storeAuthenticationService.Context.CurrentStore == null)
                return;

            _staffRawInfo.StoreId = storeAuthenticationService.Context.CurrentStore.Id;

            var staffDao = App.GetService<IStaffDao>();
            var allStaffRecords = await staffDao.GetAllAsync(storeAuthenticationService.Context.CurrentStore.Id);

            if (allStaffRecords == null)
                return;

            _staffId = allStaffRecords.Count; // this is not used when saving to database, but it is used let the user know their username
        }

        switch (comboBoxIndex)
        {
            case 0:
                StaffPositionPrefix = "ST";
                break;
            case 1:
                StaffPositionPrefix = "AM";
                break;
        }
    }

    /// <summary>
    /// Handles the click event of the register staff button.
    /// </summary>
    /// <param name="confirmStaffInformationContentDialog">Content dialog to confirm staff information.</param>
    /// <param name="accountCreationResultContentDialog">Content dialog to show account creation result.</param>
    public async void HandleRegisterStaffButtonClick(ContentDialog confirmStaffInformationContentDialog,
                                                     ContentDialog accountCreationResultContentDialog)
    {
        AccountCreationResult = "";

        if (validateStaffInformation() == false)
            return;

        ContentDialogResult confirmResult = await confirmStaffInformationContentDialog.ShowAsync();

        if (confirmResult != ContentDialogResult.Primary)
            return;

        // Create the store manager account
        var staffAccountCreationService = App.GetService<IStaffAccountCreationService>();
        var storeManagerStaffDto = await staffAccountCreationService.CreateAccountAsync(_staffRawInfo);

        if (storeManagerStaffDto == null)
        {
            // Hopefully this doesn't happen
            AccountCreationResult = "Đã xảy ra lỗi khi tạo tài khoản nhân viên. Vui lòng thử lại sau.";
        }
        else
        {
            AccountCreationResult = "Tạo tài khoản nhân viên thành công.";
        }

        _ = await accountCreationResultContentDialog.ShowAsync();

        App.NavigateTo(typeof(StaffManagementView));
    }

    /// <summary>
    /// Validates the staff information.
    /// </summary>
    /// <returns>True if all fields are valid, otherwise false.</returns>
    private bool validateStaffInformation()
    {
        var allFieldsValid = true;

        var staffAccountCreationService = App.GetService<IStaffAccountCreationService>();
        var fieldValidationResults = staffAccountCreationService.ValidateFields(_staffRawInfo);

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
                    StaffNameErrorMessageText = validationResult;
                    StaffNameErrorMessageOpacity = opacity;
                    break;
                case "gender":
                    StaffGenderErrorMessageText = validationResult;
                    StaffGenderErrorMessageOpacity = opacity;
                    break;
                case "email":
                    StaffEmailErrorMessageText = validationResult;
                    StaffEmailErrorMessageOpacity = opacity;
                    break;
                case "tel":
                    StaffTelErrorMessageText = validationResult;
                    StaffTelErrorMessageOpacity = opacity;
                    break;
                case "address":
                    StaffAddressErrorMessageText = validationResult;
                    StaffAddressErrorMessageOpacity = opacity;
                    break;
            }
        }

        if (_staffCompositeUsername == "")
        {
            StaffPositionErrorMessageText = "Vui lòng chọn chức vụ cho nhân viên";
            StaffPositionErrorMessageOpacity = 1.0F;
        }
        else
        {
            StaffPositionErrorMessageOpacity = 0.0F;
        }

        return allFieldsValid;
    }

    /// <summary>
    /// Gets or sets the staff name.
    /// </summary>
    public string StaffName
    {
        get => _staffRawInfo.Name;
        set
        {
            _staffRawInfo.Name = value;
            OnPropertyChanged(nameof(StaffName));
        }
    }

    /// <summary>
    /// Gets or sets the staff gender.
    /// </summary>
    public string StaffGender
    {
        get => _staffRawInfo.Gender;
        set
        {
            _staffRawInfo.Gender = value;
            OnPropertyChanged(nameof(StaffGender));
        }
    }

    /// <summary>
    /// Gets or sets the staff date of birth.
    /// </summary>
    public DateTimeOffset StaffDateOfBirth
    {
        // the time returned by CalendarDatePicker is DateTimeOffset, so we have to do casting here
        get => (DateTimeOffset)_staffRawInfo.DateOfBirth.ToDateTime(TimeOnly.MinValue);
        set
        {
            _staffRawInfo.DateOfBirth = DateOnly.FromDateTime(value.DateTime);
            OnPropertyChanged(nameof(StaffDateOfBirth));
        }
    }

    /// <summary>
    /// Gets or sets the staff date of birth string.
    /// </summary>
    public string StaffDateOfBirthString
    {
        get => _staffDateOfBirthString;
        set
        {
            _staffDateOfBirthString = value;
            OnPropertyChanged(nameof(StaffDateOfBirthString));
        }
    }

    /// <summary>
    /// Gets or sets the staff email.
    /// </summary>
    public string StaffEmail
    {
        get => _staffRawInfo.Email;
        set
        {
            _staffRawInfo.Email = value;
            OnPropertyChanged(nameof(StaffEmail));
        }
    }

    /// <summary>
    /// Gets or sets the staff telephone number.
    /// </summary>
    public string StaffTel
    {
        get => _staffRawInfo.Tel;
        set
        {
            _staffRawInfo.Tel = value;
            OnPropertyChanged(nameof(StaffTel));
        }
    }

    /// <summary>
    /// Gets or sets the staff employment start date.
    /// </summary>
    public DateTimeOffset StaffEmploymentStartDate
    {
        // the time returned by CalendarDatePicker is DateTimeOffset, so we have to do casting here
        get => (DateTimeOffset)_staffRawInfo.EmploymentStartDate.ToDateTime(TimeOnly.MinValue);
        set
        {
            _staffRawInfo.EmploymentStartDate = DateOnly.FromDateTime(value.DateTime);
            OnPropertyChanged(nameof(StaffEmploymentStartDate));
        }
    }

    /// <summary>
    /// Gets or sets the staff employment start date string.
    /// </summary>
    public string StaffEmploymentStartDateString
    {
        get => _staffEmploymentStartDateString;
        set
        {
            _staffEmploymentStartDateString = value;
            OnPropertyChanged(nameof(StaffEmploymentStartDateString));
        }
    }

    /// <summary>
    /// Gets or sets the staff address.
    /// </summary>
    public string StaffAddress
    {
        get => _staffRawInfo.Address;
        set
        {
            _staffRawInfo.Address = value;
            OnPropertyChanged(nameof(StaffAddress));
        }
    }

    /// <summary>
    /// Gets or sets the staff position prefix.
    /// </summary>
    public string StaffPositionPrefix
    {
        get => _staffRawInfo.PositionPrefix;
        set
        {
            _staffRawInfo.PositionPrefix = value;
            StaffCompositeUsername = Models.Entity.Staff.GetCompositeUsername(_staffRawInfo.PositionPrefix, _staffRawInfo.StoreId, _staffId);
            OnPropertyChanged(nameof(StaffPositionPrefix));
        }
    }

    /// <summary>
    /// Gets or sets the staff composite username.
    /// </summary>
    public string StaffCompositeUsername
    {
        get => _staffCompositeUsername;
        set
        {
            _staffCompositeUsername = value;
            OnPropertyChanged(nameof(StaffCompositeUsername));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the staff name error message.
    /// </summary>
    public float StaffNameErrorMessageOpacity
    {
        get => _staffNameErrorMessageOpacity;
        set
        {
            _staffNameErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffNameErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the staff name error message text.
    /// </summary>
    public string StaffNameErrorMessageText
    {
        get => _staffNameErrorMessageText;
        set
        {
            _staffNameErrorMessageText = value;
            OnPropertyChanged(nameof(StaffNameErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the staff gender error message.
    /// </summary>
    public float StaffGenderErrorMessageOpacity
    {
        get => _staffGenderErrorMessageOpacity;
        set
        {
            _staffGenderErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffGenderErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the staff gender error message text.
    /// </summary>
    public string StaffGenderErrorMessageText
    {
        get => _staffGenderErrorMessageText;
        set
        {
            _staffGenderErrorMessageText = value;
            OnPropertyChanged(nameof(StaffGenderErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the staff email error message.
    /// </summary>
    public float StaffEmailErrorMessageOpacity
    {
        get => _staffEmailErrorMessageOpacity;
        set
        {
            _staffEmailErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffEmailErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the staff email error message text.
    /// </summary>
    public string StaffEmailErrorMessageText
    {
        get => _staffEmailErrorMessageText;
        set
        {
            _staffEmailErrorMessageText = value;
            OnPropertyChanged(nameof(StaffEmailErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the staff telephone error message.
    /// </summary>
    public float StaffTelErrorMessageOpacity
    {
        get => _staffTelErrorMessageOpacity;
        set
        {
            _staffTelErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffTelErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the staff telephone error message text.
    /// </summary>
    public string StaffTelErrorMessageText
    {
        get => _staffTelErrorMessageText;
        set
        {
            _staffTelErrorMessageText = value;
            OnPropertyChanged(nameof(StaffTelErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the staff address error message.
    /// </summary>
    public float StaffAddressErrorMessageOpacity
    {
        get => _staffAddressErrorMessageOpacity;
        set
        {
            _staffAddressErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffAddressErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the staff address error message text.
    /// </summary>
    public string StaffAddressErrorMessageText
    {
        get => _staffAddressErrorMessageText;
        set
        {
            _staffAddressErrorMessageText = value;
            OnPropertyChanged(nameof(StaffAddressErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the staff position error message.
    /// </summary>
    public float StaffPositionErrorMessageOpacity
    {
        get => _staffPositionErrorMessageOpacity;
        set
        {
            _staffPositionErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffPositionErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the staff position error message text.
    /// </summary>
    public string StaffPositionErrorMessageText
    {
        get => _staffPositionErrorMessageText;
        set
        {
            _staffPositionErrorMessageText = value;
            OnPropertyChanged(nameof(StaffPositionErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the account creation result.
    /// </summary>
    public string AccountCreationResult
    {
        get => _accountCreationResult;
        set
        {
            _accountCreationResult = value;
            OnPropertyChanged(nameof(AccountCreationResult));
        }
    }
}
