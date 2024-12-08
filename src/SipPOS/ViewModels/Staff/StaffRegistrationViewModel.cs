using System.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using SipPOS.DataTransfer.Entity;
using SipPOS.Views.Staff;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Account.Interfaces;

namespace SipPOS.ViewModels.Staff;

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

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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

    public void HandleStaffDateOfBirthCalenderDatePickerDateChanged()
    {
        StaffDateOfBirthString = StaffDateOfBirth.ToString("dd/MM/yyyy");
    }

    public void HandleStaffEmploymentStartDateCalenderDatePickerDateChanged()
    {
        StaffEmploymentStartDateString = StaffEmploymentStartDate.ToString("dd/MM/yyyy");
    }

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

    public string StaffName
    {
        get => _staffRawInfo.Name;
        set
        {
            _staffRawInfo.Name = value;
            OnPropertyChanged(nameof(StaffName));
        }
    }

    public string StaffGender
    {
        get => _staffRawInfo.Gender;
        set
        {
            _staffRawInfo.Gender = value;
            OnPropertyChanged(nameof(StaffGender));
        }
    }

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

    public string StaffDateOfBirthString
    {
        get => _staffDateOfBirthString;
        set
        {
            _staffDateOfBirthString = value;
            OnPropertyChanged(nameof(StaffDateOfBirthString));
        }
    }

    public string StaffEmail
    {
        get => _staffRawInfo.Email;
        set
        {
            _staffRawInfo.Email = value;
            OnPropertyChanged(nameof(StaffEmail));
        }
    }

    public string StaffTel
    {
        get => _staffRawInfo.Tel;
        set
        {
            _staffRawInfo.Tel = value;
            OnPropertyChanged(nameof(StaffTel));
        }
    }

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

    public string StaffEmploymentStartDateString
    {
        get => _staffEmploymentStartDateString;
        set
        {
            _staffEmploymentStartDateString = value;
            OnPropertyChanged(nameof(StaffEmploymentStartDateString));
        }
    }

    public string StaffAddress
    {
        get => _staffRawInfo.Address;
        set
        {
            _staffRawInfo.Address = value;
            OnPropertyChanged(nameof(StaffAddress));
        }
    }

    public string StaffPositionPrefix
    {
        get => _staffRawInfo.PositionPrefix;
        set
        {
            _staffRawInfo.PositionPrefix = value;
            StaffCompositeUsername = SipPOS.Models.Entity.Staff.GetCompositeUsername(_staffRawInfo.PositionPrefix, _staffRawInfo.StoreId, _staffId);
            OnPropertyChanged(nameof(StaffPositionPrefix));
        }
    }

    public string StaffCompositeUsername
    {
        get => _staffCompositeUsername;
        set
        {
            _staffCompositeUsername = value;
            OnPropertyChanged(nameof(StaffCompositeUsername));
        }
    }

    public float StaffNameErrorMessageOpacity
    {
        get => _staffNameErrorMessageOpacity;
        set
        {
            _staffNameErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffNameErrorMessageOpacity));
        }
    }

    public string StaffNameErrorMessageText
    {
        get => _staffNameErrorMessageText;
        set
        {
            _staffNameErrorMessageText = value;
            OnPropertyChanged(nameof(StaffNameErrorMessageText));
        }
    }

    public float StaffGenderErrorMessageOpacity
    {
        get => _staffGenderErrorMessageOpacity;
        set
        {
            _staffGenderErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffGenderErrorMessageOpacity));
        }
    }

    public string StaffGenderErrorMessageText
    {
        get => _staffGenderErrorMessageText;
        set
        {
            _staffGenderErrorMessageText = value;
            OnPropertyChanged(nameof(StaffGenderErrorMessageText));
        }
    }

    public float StaffEmailErrorMessageOpacity
    {
        get => _staffEmailErrorMessageOpacity;
        set
        {
            _staffEmailErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffEmailErrorMessageOpacity));
        }
    }

    public string StaffEmailErrorMessageText
    {
        get => _staffEmailErrorMessageText;
        set
        {
            _staffEmailErrorMessageText = value;
            OnPropertyChanged(nameof(StaffEmailErrorMessageText));
        }
    }

    public float StaffTelErrorMessageOpacity
    {
        get => _staffTelErrorMessageOpacity;
        set
        {
            _staffTelErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffTelErrorMessageOpacity));
        }
    }

    public string StaffTelErrorMessageText
    {
        get => _staffTelErrorMessageText;
        set
        {
            _staffTelErrorMessageText = value;
            OnPropertyChanged(nameof(StaffTelErrorMessageText));
        }
    }

    public float StaffAddressErrorMessageOpacity
    {
        get => _staffAddressErrorMessageOpacity;
        set
        {
            _staffAddressErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffAddressErrorMessageOpacity));
        }
    }

    public string StaffAddressErrorMessageText
    {
        get => _staffAddressErrorMessageText;
        set
        {
            _staffAddressErrorMessageText = value;
            OnPropertyChanged(nameof(StaffAddressErrorMessageText));
        }
    }

    public float StaffPositionErrorMessageOpacity
    {
        get => _staffPositionErrorMessageOpacity;
        set
        {
            _staffPositionErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StaffPositionErrorMessageOpacity));
        }
    }

    public string StaffPositionErrorMessageText
    {
        get => _staffPositionErrorMessageText;
        set
        {
            _staffPositionErrorMessageText = value;
            OnPropertyChanged(nameof(StaffPositionErrorMessageText));
        }
    }

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
