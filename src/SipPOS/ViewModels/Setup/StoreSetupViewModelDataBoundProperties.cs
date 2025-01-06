using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace SipPOS.ViewModels.Setup;

public partial class StoreSetupViewModel : INotifyPropertyChanged, IStoreSetupViewModel
{
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

    /// <summary>
    /// Gets or sets the operating hours string.
    /// </summary>
    public string OperatingHoursString
    {
        get => _operatingHoursString;
        set
        {
            _operatingHoursString = value;
            OnPropertyChanged(nameof(OperatingHoursString));
        }
    }

    /// <summary>
    /// Gets or sets the opening time.
    /// </summary>
    public TimeOnly OpeningTime
    {
        get => _initialConfigurationDto.OpeningTime;
        set
        {
            _initialConfigurationDto.OpeningTime = value;
            OnPropertyChanged(nameof(OpeningTime));
        }
    }

    /// <summary>
    /// Gets or sets the closing time.
    /// </summary>
    public TimeOnly ClosingTime
    {
        get => _initialConfigurationDto.ClosingTime;
        set
        {
            _initialConfigurationDto.ClosingTime = value;
            OnPropertyChanged(nameof(ClosingTime));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the operating hours error message.
    /// </summary>
    public float OperatingHoursErrorMessageOpacity
    {
        get => _operatingHoursErrorMessageOpacity;
        set
        {
            _operatingHoursErrorMessageOpacity = value;
            OnPropertyChanged(nameof(OperatingHoursErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the operating hours error message.
    /// </summary>
    public string OperatingHoursErrorMessageText
    {
        get => _operatingHoursErrorMessageText;
        set
        {
            _operatingHoursErrorMessageText = value;
            OnPropertyChanged(nameof(OperatingHoursErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the tax code.
    /// </summary>
    public string TaxCode
    {
        get => _initialConfigurationDto.TaxCode;
        set
        {
            _initialConfigurationDto.TaxCode = value;
            OnPropertyChanged(nameof(TaxCode));
        }
    }

    /// <summary>
    /// Gets or sets the VAT rate.
    /// </summary>
    public decimal VatRate
    {
        get => _initialConfigurationDto.VatRate;
        set
        {
            _initialConfigurationDto.VatRate = value;
            OnPropertyChanged(nameof(VatRate));
        }
    }

    /// <summary>
    /// Gets or sets the VAT method.
    /// </summary>
    public string VatMethod
    {
        get => _initialConfigurationDto.VatMethod;
        set
        {
            _initialConfigurationDto.VatMethod = value;
            OnPropertyChanged(nameof(VatMethod));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the tax configuration error message.
    /// </summary>
    public float TaxConfigurationErrorMessageOpacity
    {
        get => _taxConfigurationErrorMessageOpacity;
        set
        {
            _taxConfigurationErrorMessageOpacity = value;
            OnPropertyChanged(nameof(TaxConfigurationErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the tax configuration error message.
    /// </summary>
    public string TaxConfigurationErrorMessageText
    {
        get => _taxConfigurationErrorMessageText;
        set
        {
            _taxConfigurationErrorMessageText = value;
            OnPropertyChanged(nameof(TaxConfigurationErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the base salary for staff.
    /// </summary>
    public decimal StaffBaseSalary
    {
        get => _initialConfigurationDto.CurrentStaffBaseSalary;
        set
        {
            _initialConfigurationDto.CurrentStaffBaseSalary = value;
            _initialConfigurationDto.NextStaffBaseSalary = value;
            OnPropertyChanged(nameof(StaffBaseSalary));
        }
    }

    /// <summary>
    /// Gets or sets the hourly salary for staff.
    /// </summary>
    public decimal StaffHourlySalary
    {
        get => _initialConfigurationDto.CurrentStaffHourlySalary;
        set
        {
            _initialConfigurationDto.CurrentStaffHourlySalary = value;
            _initialConfigurationDto.NextStaffHourlySalary = value;
            OnPropertyChanged(nameof(StaffHourlySalary));
        }
    }

    /// <summary>
    /// Gets or sets the base salary for assistant managers.
    /// </summary>
    public decimal AssistantManagerBaseSalary
    {
        get => _initialConfigurationDto.CurrentAssistantManagerBaseSalary;
        set
        {
            _initialConfigurationDto.CurrentAssistantManagerBaseSalary = value;
            _initialConfigurationDto.NextAssistantManagerBaseSalary = value;
            OnPropertyChanged(nameof(AssistantManagerBaseSalary));
        }
    }

    /// <summary>
    /// Gets or sets the hourly salary for assistant managers.
    /// </summary>
    public decimal AssistantManagerHourlySalary
    {
        get => _initialConfigurationDto.CurrentAssistantManagerHourlySalary;
        set
        {
            _initialConfigurationDto.CurrentAssistantManagerHourlySalary = value;
            _initialConfigurationDto.NextAssistantManagerHourlySalary = value;
            OnPropertyChanged(nameof(AssistantManagerHourlySalary));
        }
    }

    /// <summary>
    /// Gets or sets the base salary for store managers.
    /// </summary>
    public decimal StoreManagerBaseSalary
    {
        get => _initialConfigurationDto.CurrentStoreManagerBaseSalary;
        set
        {
            _initialConfigurationDto.CurrentStoreManagerBaseSalary = value;
            _initialConfigurationDto.NextStoreManagerBaseSalary = value;
            OnPropertyChanged(nameof(StoreManagerBaseSalary));
        }
    }

    /// <summary>
    /// Gets or sets the hourly salary for store managers.
    /// </summary>
    public decimal StoreManagerHourlySalary
    {
        get => _initialConfigurationDto.CurrentStoreManagerHourlySalary;
        set
        {
            _initialConfigurationDto.CurrentStoreManagerHourlySalary = value;
            _initialConfigurationDto.NextStoreManagerHourlySalary = value;
            OnPropertyChanged(nameof(StoreManagerHourlySalary));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the staff base salary checkbox is checked.
    /// </summary>
    public bool StaffBaseSalaryCheckBoxChecked
    {
        get => _staffBaseSalaryCheckBoxChecked;
        set
        {
            _staffBaseSalaryCheckBoxChecked = value;
            OnPropertyChanged(nameof(StaffBaseSalaryCheckBoxChecked));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the staff hourly salary checkbox is checked.
    /// </summary>
    public bool StaffHourlySalaryCheckBoxChecked
    {
        get => _staffHourlySalaryCheckBoxChecked;
        set
        {
            _staffHourlySalaryCheckBoxChecked = value;
            OnPropertyChanged(nameof(StaffHourlySalaryCheckBoxChecked));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the assistant manager base salary checkbox is checked.
    /// </summary>
    public bool AssistantManagerBaseSalaryCheckBoxChecked
    {
        get => _assistantManagerBaseSalaryCheckBoxChecked;
        set
        {
            _assistantManagerBaseSalaryCheckBoxChecked = value;
            OnPropertyChanged(nameof(AssistantManagerBaseSalaryCheckBoxChecked));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the assistant manager hourly salary checkbox is checked.
    /// </summary>
    public bool AssistantManagerHourlySalaryCheckBoxChecked
    {
        get => _assistantManagerHourlySalaryCheckBoxChecked;
        set
        {
            _assistantManagerHourlySalaryCheckBoxChecked = value;
            OnPropertyChanged(nameof(AssistantManagerHourlySalaryCheckBoxChecked));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the store manager base salary checkbox is checked.
    /// </summary>
    public bool StoreManagerBaseSalaryCheckBoxChecked
    {
        get => _storeManagerBaseSalaryCheckBoxChecked;
        set
        {
            _storeManagerBaseSalaryCheckBoxChecked = value;
            OnPropertyChanged(nameof(StoreManagerBaseSalaryCheckBoxChecked));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the store manager hourly salary checkbox is checked.
    /// </summary>
    public bool StoreManagerHourlySalaryCheckBoxChecked
    {
        get => _storeManagerHourlySalaryCheckBoxChecked;
        set
        {
            _storeManagerHourlySalaryCheckBoxChecked = value;
            OnPropertyChanged(nameof(StoreManagerHourlySalaryCheckBoxChecked));
        }
    }

    /// <summary>
    /// Gets or sets the string representation of the staff base salary.
    /// </summary>
    public string StaffBaseSalaryString
    {
        get => _staffBaseSalaryString;
        set
        {
            _staffBaseSalaryString = value;
            OnPropertyChanged(nameof(StaffBaseSalaryString));
        }
    }

    /// <summary>
    /// Gets or sets the string representation of the staff hourly salary.
    /// </summary>
    public string StaffHourlySalaryString
    {
        get => _staffHourlySalaryString;
        set
        {
            _staffHourlySalaryString = value;
            OnPropertyChanged(nameof(StaffHourlySalaryString));
        }
    }

    /// <summary>
    /// Gets or sets the string representation of the assistant manager base salary.
    /// </summary>
    public string AssistantManagerBaseSalaryString
    {
        get => _assistantManagerBaseSalaryString;
        set
        {
            _assistantManagerBaseSalaryString = value;
            OnPropertyChanged(nameof(AssistantManagerBaseSalaryString));
        }
    }

    /// <summary>
    /// Gets or sets the string representation of the assistant manager hourly salary.
    /// </summary>
    public string AssistantManagerHourlySalaryString
    {
        get => _assistantManagerHourlySalaryString;
        set
        {
            _assistantManagerHourlySalaryString = value;
            OnPropertyChanged(nameof(AssistantManagerHourlySalaryString));
        }
    }

    /// <summary>
    /// Gets or sets the string representation of the store manager base salary.
    /// </summary>
    public string StoreManagerBaseSalaryString
    {
        get => _storeManagerBaseSalaryString;
        set
        {
            _storeManagerBaseSalaryString = value;
            OnPropertyChanged(nameof(StoreManagerBaseSalaryString));
        }
    }

    /// <summary>
    /// Gets or sets the string representation of the store manager hourly salary.
    /// </summary>
    public string StoreManagerHourlySalaryString
    {
        get => _storeManagerHourlySalaryString;
        set
        {
            _storeManagerHourlySalaryString = value;
            OnPropertyChanged(nameof(StoreManagerHourlySalaryString));
        }
    }

    /// <summary>
    /// Gets or sets the text of the salary error message.
    /// </summary>
    public string SalaryErrorMessageText
    {
        get => _salaryErrorMessageText;
        set
        {
            _salaryErrorMessageText = value;
            OnPropertyChanged(nameof(SalaryErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the salary error message.
    /// </summary>
    public float SalaryErrorMessageOpacity
    {
        get => _salaryErrorMessageOpacity;
        set
        {
            _salaryErrorMessageOpacity = value;
            OnPropertyChanged(nameof(SalaryErrorMessageOpacity));
        }
    }
}