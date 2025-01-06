using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.General;
using SipPOS.Views.Staff;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Resources.Controls;

namespace SipPOS.ViewModels.Staff;

/// <summary>
/// ViewModel for managing staff-related operations.
/// </summary>
public class StaffManagementViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // Data-bound properties 
    private ObservableCollection<StaffDto> _currentPageStaffList;

    // Sort, search, filter properties
    public List<string> PositionPrefixFilter { get; private set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }

    // Pagination properties
    private long _currentPage;
    private long _totalPages;
    private long _rowsPerPage;
    private long _totalRowsCount;
    private long _fromIndex;
    private long _toIndex;
    private string _searchKeyword;

    // Edit functions properties
    public StaffDto? EditTargetStaff { get; set; }
    private string _editStaffResult;

    // Contract termination properties
    public StaffDto? TerminationTargetStaff { get; set; }
    private string _passwordForContractTermination;
    private bool _passwordVerified;
    private string _terminateStaffResult;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffManagementViewModel"/> class.
    /// </summary>
    public StaffManagementViewModel()
    {
        _currentPage = 1;
        _totalPages = 0;
        _rowsPerPage = 5;
        _totalRowsCount = 0;
        _fromIndex = 0;
        _toIndex = 0;
        _searchKeyword = "";

        _editStaffResult = "";
        _passwordForContractTermination = "";
        _terminateStaffResult = "";

        _currentPageStaffList = new();
        PositionPrefixFilter = new();
        PositionPrefixFilter.Add("SM");
        PositionPrefixFilter.Add("AM");
        PositionPrefixFilter.Add("ST");
        UpdateStaffList();
    }

    /// <summary>
    /// Updates the staff list based on the current filters and pagination settings.
    /// </summary>
    private async void UpdateStaffList()
    {
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }

        if (storeAuthenticationService.Context.CurrentStore == null)
        {
            return;
        }

        var staffDao = App.GetService<IStaffDao>();

        var (totalRowsMatched, staffDtos) = await staffDao.GetWithPaginationAsync
        (
            storeId: storeAuthenticationService.Context.CurrentStore.Id,
            page: CurrentPage,
            rowsPerPage: RowsPerPage,
            keyword: SearchKeyword,
            sortBy: SortBy,
            sortDirection: SortDirection,
            filterByPositionPrefixes: PositionPrefixFilter
        );

        if (staffDtos == null)
        {
            CurrentPageStaffList = new ObservableCollection<StaffDto>(); // empty list
            TotalRowsCount = 0;
            TotalPages = 0;
            FromIndex = 0;
            ToIndex = 0;
            return;
        }

        CurrentPageStaffList = new ObservableCollection<StaffDto>(staffDtos);
        TotalRowsCount = totalRowsMatched;
        TotalPages = (TotalRowsCount / RowsPerPage) + (TotalRowsCount % RowsPerPage == 0 ? 0 : 1);
        FromIndex = (CurrentPage - 1) * RowsPerPage + 1;
        ToIndex = Math.Min(CurrentPage * RowsPerPage, TotalRowsCount);
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
    /// Handles the Go Back button click event.
    /// </summary>
    public void HandleGoBackButtonClick()
    {
        App.NavigateTo(typeof(MainMenuView));
    }

    /// <summary>
    /// Handles the Sort By ComboBox selection change event.
    /// </summary>
    /// <param name="comboBoxIndex">The selected index of the ComboBox.</param>
    public void HandleSortByComboBoxSelectionChanged(int comboBoxIndex)
    {
        switch (comboBoxIndex)
        {
            // sort criteria must be in snake_case
            case 0:
                SortBy = "position_prefix";
                break;
            case 1:
                SortBy = "name";
                break;
        }

        UpdateStaffList();
    }

    /// <summary>
    /// Handles the Sort Direction ComboBox selection change event.
    /// </summary>
    /// <param name="comboBoxIndex">The selected index of the ComboBox.</param>
    public void HandleSortDirectionComboBoxSelectionChanged(int comboBoxIndex)
    {
        switch (comboBoxIndex)
        {
            case 0:
                SortDirection = "ASC";
                break;
            case 1:
                SortDirection = "DESC";
                break;
        }

        SortBy ??= "position_prefix";

        UpdateStaffList();
    }

    /// <summary>
    /// Handles the Position CheckBoxes change event.
    /// </summary>
    /// <param name="smChecked">Whether the SM checkbox is checked.</param>
    /// <param name="amChecked">Whether the AM checkbox is checked.</param>
    /// <param name="stChecked">Whether the ST checkbox is checked.</param>
    public void HandlePositionCheckBoxesChanged(bool? smChecked, bool? amChecked, bool? stChecked)
    {
        PositionPrefixFilter = new List<string>();

        if (smChecked == true)
        {
            PositionPrefixFilter.Add("SM");
        }

        if (amChecked == true)
        {
            PositionPrefixFilter.Add("AM");
        }

        if (stChecked == true)
        {
            PositionPrefixFilter.Add("ST");
        }

        UpdateStaffList();
    }

    /// <summary>
    /// Handles the Rows Per Page ComboBox selection change event.
    /// </summary>
    /// <param name="comboBoxIndex">The selected index of the ComboBox.</param>
    public void HandleRowsPerPageComboBoxSelectionChanged(int comboBoxIndex)
    {
        RowsPerPage = comboBoxIndex switch
        {
            0 => 5,
            1 => 10,
            2 => 15,
            3 => 20,
            _ => RowsPerPage
        };

        UpdateStaffList();
    }

    /// <summary>
    /// Handles the Previous Page button click event.
    /// </summary>
    public void HandlePreviousPageButtonClick()
    {
        if (CurrentPage - 1 >= 1)
        {
            CurrentPage--;
            UpdateStaffList();
        }
    }

    /// <summary>
    /// Handles the Next Page button click event.
    /// </summary>
    public void HandleNextPageButtonClick()
    {
        if (CurrentPage + 1 <= TotalPages)
        {
            CurrentPage++;
            UpdateStaffList();
        }
    }

    /// <summary>
    /// Handles the save click event for the staff name editable text field.
    /// </summary>
    /// <param name="editStaffNameEditableTextField">The editable text field for the staff name.</param>
    /// <param name="editStaffNameErrorMessageTeachingTip">The teaching tip to display error messages.</param>
    public void HandleEditStaffNameEditableTextFieldSaveClicked
    (
        EditableTextField editStaffNameEditableTextField,
        TeachingTip editStaffNameErrorMessageTeachingTip
    )
    {
        if (EditTargetStaff == null)
            return;

        if (string.IsNullOrEmpty(editStaffNameEditableTextField.Text))
        {
            editStaffNameErrorMessageTeachingTip.Subtitle = "Tên nhân viên không được rỗng";
            editStaffNameErrorMessageTeachingTip.IsOpen = true;
            editStaffNameEditableTextField.ResetState();
            editStaffNameEditableTextField.Text = EditTargetStaff.Name;
        }
    }

    /// <summary>
    /// Handles the save click event for the staff email editable text field.
    /// </summary>
    /// <param name="editStaffEmailEditableTextField">The editable text field for the staff email.</param>
    /// <param name="editStaffEmailErrorMessageTeachingTip">The teaching tip to display error messages.</param>
    public void HandleEditStaffEmailEditableTextFieldSaveClicked
    (
        EditableTextField editStaffEmailEditableTextField,
        TeachingTip editStaffEmailErrorMessageTeachingTip
    )
    {
        if (EditTargetStaff == null)
            return;

        if (string.IsNullOrEmpty(editStaffEmailEditableTextField.Text))
        {
            editStaffEmailErrorMessageTeachingTip.Subtitle = "Email không được rỗng";
            editStaffEmailErrorMessageTeachingTip.IsOpen = true;
            editStaffEmailEditableTextField.ResetState();
            editStaffEmailEditableTextField.Text = EditTargetStaff.Email;
        }

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(editStaffEmailEditableTextField.Text, emailPattern))
        {
            editStaffEmailErrorMessageTeachingTip.Subtitle = "Email sai định dạng";
            editStaffEmailErrorMessageTeachingTip.IsOpen = true;
            editStaffEmailEditableTextField.ResetState();
            editStaffEmailEditableTextField.Text = EditTargetStaff.Email;
        }
    }

    /// <summary>
    /// Handles the save click event for the staff telephone editable text field.
    /// </summary>
    /// <param name="editStaffTelEditableTextField">The editable text field for the staff telephone.</param>
    /// <param name="editStaffTelErrorMessageTeachingTip">The teaching tip to display error messages.</param>
    public void HandleEditStaffTelEditableTextFieldSaveClicked
    (
        EditableTextField editStaffTelEditableTextField,
        TeachingTip editStaffTelErrorMessageTeachingTip
    )
    {
        if (EditTargetStaff == null)
            return;

        if (string.IsNullOrEmpty(editStaffTelEditableTextField.Text))
        {
            editStaffTelErrorMessageTeachingTip.Subtitle = "SĐT không được rỗng";
            editStaffTelErrorMessageTeachingTip.IsOpen = true;
            editStaffTelEditableTextField.ResetState();
            editStaffTelEditableTextField.Text = EditTargetStaff.Tel;
        }

        var telPattern = @"^(0|\+84)\d{9,12}$";

        if (!Regex.IsMatch(editStaffTelEditableTextField.Text, telPattern))
        {
            editStaffTelErrorMessageTeachingTip.Subtitle = "SĐT sai định dạng";
            editStaffTelErrorMessageTeachingTip.IsOpen = true;
            editStaffTelEditableTextField.ResetState();
            editStaffTelEditableTextField.Text = EditTargetStaff.Tel;
        }
    }

    /// <summary>
    /// Handles the save click event for the staff address editable text field.
    /// </summary>
    /// <param name="editStaffAddressEditableTextField">The editable text field for the staff address.</param>
    /// <param name="editStaffAddressErrorMessageTeachingTip">The teaching tip to display error messages.</param>
    public void HandleEditStaffAddressEditableTextFieldSaveClicked
    (
        EditableTextField editStaffAddressEditableTextField,
        TeachingTip editStaffAddressErrorMessageTeachingTip
    )
    {
        if (EditTargetStaff == null)
            return;

        if (string.IsNullOrEmpty(editStaffAddressEditableTextField.Text))
        {
            editStaffAddressErrorMessageTeachingTip.Subtitle = "Địa chỉ không được rỗng";
            editStaffAddressErrorMessageTeachingTip.IsOpen = true;
            editStaffAddressEditableTextField.ResetState();
            editStaffAddressEditableTextField.Text = EditTargetStaff.Address;
        }
    }

    /// <summary>
    /// Handles the update of staff details.
    /// </summary>
    /// <param name="updatedStaffDto">The updated staff details.</param>
    /// <param name="editStaffResultContentDialog">The content dialog to show the result.</param>
    public async void HandleUpdateStaffDetails(StaffDto updatedStaffDto, ContentDialog editStaffResultContentDialog)
    {
        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
            return;

        if (staffAuthenticationService.Context.CurrentStaff == null)
            return;

        if (updatedStaffDto.Id == null)
            return;

        var staffDao = App.GetService<IStaffDao>();

        var savedStaffDto = await staffDao.UpdateByIdAsync
        (
            storeId: updatedStaffDto.StoreId,
            id: updatedStaffDto.Id.Value,
            updatedStaffDto: updatedStaffDto,
            author: staffAuthenticationService.Context.CurrentStaff
        );

        if (savedStaffDto != null)
        {
            EditStaffResult = "Lưu thông tin nhân viên thành công.";
            UpdateStaffList();
        }
        else
        {
            EditStaffResult = "Lưu thông tin nhân viên thất bại. Đã có lỗi xảy ra.";
        }

        _ = await editStaffResultContentDialog.ShowAsync();
    }

    /// <summary>
    /// Handles the termination of staff.
    /// </summary>
    /// <param name="deletedStaffDto">The staff details to be terminated.</param>
    /// <param name="terminateStaffResultContentDialog">The content dialog to show the result.</param>
    public async void HandleTerminateStaff(StaffDto deletedStaffDto, ContentDialog terminateStaffResultContentDialog)
    {
        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
            return;

        if (staffAuthenticationService.Context.CurrentStaff == null)
            return;

        if (deletedStaffDto.Id == null)
            return;

        var staffDao = App.GetService<IStaffDao>();

        var savedStaffDto = await staffDao.UpdateByIdAsync
        (
            storeId: deletedStaffDto.StoreId,
            id: deletedStaffDto.Id.Value,
            updatedStaffDto: deletedStaffDto,
            author: staffAuthenticationService.Context.CurrentStaff
        );

        if (savedStaffDto != null)
        {
            TerminateStaffResult = "Hủy hợp đồng với nhân viên thành công.";
            UpdateStaffList();
        }
        else
        {
            TerminateStaffResult = "Hủy hợp đồng với nhân viên thất bại. Đã có lỗi xảy ra.";
        }

        _ = await terminateStaffResultContentDialog.ShowAsync();
    }

    /// <summary>
    /// Handles the Register New Staff button click event.
    /// </summary>
    public void HandleRegisterNewStaffButtonClick()
    {
        App.NavigateTo(typeof(StaffRegistrationView));
    }

    /// <summary>
    /// Gets or sets the current page staff list.
    /// </summary>
    public ObservableCollection<StaffDto> CurrentPageStaffList
    {
        get => _currentPageStaffList;
        set
        {
            _currentPageStaffList = value;
            OnPropertyChanged(nameof(CurrentPageStaffList));
        }
    }

    /// <summary>
    /// Gets or sets the current page.
    /// </summary>
    public long CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }

    /// <summary>
    /// Gets or sets the total pages.
    /// </summary>
    public long TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged(nameof(TotalPages));
        }
    }

    /// <summary>
    /// Gets or sets the rows per page.
    /// </summary>
    public long RowsPerPage
    {
        get => _rowsPerPage;
        set
        {
            _rowsPerPage = value;
            OnPropertyChanged(nameof(RowsPerPage));
        }
    }

    /// <summary>
    /// Gets or sets the total rows count.
    /// </summary>
    public long TotalRowsCount
    {
        get => _totalRowsCount;
        set
        {
            _totalRowsCount = value;
            OnPropertyChanged(nameof(TotalRowsCount));
        }
    }

    /// <summary>
    /// Gets or sets the from index.
    /// </summary>
    public long FromIndex
    {
        get => _fromIndex;
        set
        {
            _fromIndex = value;
            OnPropertyChanged(nameof(FromIndex));
        }
    }

    /// <summary>
    /// Gets or sets the to index.
    /// </summary>
    public long ToIndex
    {
        get => _toIndex;
        set
        {
            _toIndex = value;
            OnPropertyChanged(nameof(ToIndex));
        }
    }

    /// <summary>
    /// Gets or sets the search keyword.
    /// </summary>
    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            _searchKeyword = value;
            OnPropertyChanged(nameof(SearchKeyword));
            UpdateStaffList();
        }
    }

    /// <summary>
    /// Gets or sets the edit staff result.
    /// </summary>
    public string EditStaffResult
    {
        get => _editStaffResult;
        set
        {
            _editStaffResult = value;
            OnPropertyChanged(nameof(EditStaffResult));
        }
    }

    /// <summary>
    /// Gets or sets the password for contract termination.
    /// </summary>
    public string PasswordForContractTermination
    {
        get => _passwordForContractTermination;
        set
        {
            _passwordForContractTermination = value;
            OnPropertyChanged(nameof(PasswordForContractTermination));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the password is verified.
    /// </summary>
    public bool PasswordVerified
    {
        get => _passwordVerified;
        set
        {
            _passwordVerified = value;
            OnPropertyChanged(nameof(PasswordVerified));
        }
    }

    /// <summary>
    /// Gets or sets the terminate staff result.
    /// </summary>
    public string TerminateStaffResult
    {
        get => _terminateStaffResult;
        set
        {
            _terminateStaffResult = value;
            OnPropertyChanged(nameof(TerminateStaffResult));
        }
    }
}
