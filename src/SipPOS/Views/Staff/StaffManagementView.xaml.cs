using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.DataTransfer.Entity;
using SipPOS.ViewModels.Staff;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Authentication.Interfaces;

namespace SipPOS.Views.Staff;

/// <summary>
/// Represents the StaffManagementView.
/// </summary>
public sealed partial class StaffManagementView : Page
{
    /// <summary>
    /// Gets the view model for the staff management.
    /// </summary>
    public StaffManagementViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffManagementView"/> class.
    /// </summary>
    public StaffManagementView()
    {
        this.InitializeComponent();
        ViewModel = new StaffManagementViewModel();
    }

    /// <summary>
    /// Handles the Click event of the goBackButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleGoBackButtonClick();
    }

    /// <summary>
    /// Handles the SelectionChanged event of the rowsPerPageComboBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void rowsPerPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandleRowsPerPageComboBoxSelectionChanged(rowsPerPageComboBox.SelectedIndex);
    }

    /// <summary>
    /// Handles the Click event of the registerNewStaffButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void registerNewStaffButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleRegisterNewStaffButtonClick();
    }

    /// <summary>
    /// Handles the SelectionChanged event of the sortByComboBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void sortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandleSortByComboBoxSelectionChanged(sortByComboBox.SelectedIndex);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the sortDirectionComboBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void sortDirectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandleSortDirectionComboBoxSelectionChanged(sortDirectionComboBox.SelectedIndex);
    }

    /// <summary>
    /// Handles the Changed event of the position check boxes.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void positionCheckBoxes_Changed(object sender, RoutedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandlePositionCheckBoxesChanged(smPositionCheckBox.IsChecked,
                                                  amPositionCheckBox.IsChecked,
                                                  stPositionCheckBox.IsChecked);
    }

    /// <summary>
    /// Handles the Click event of the previousPageButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void previousPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandlePreviousPageButtonClick();
    }

    /// <summary>
    /// Handles the Click event of the nextPageButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void nextPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandleNextPageButtonClick();
    }

    /// <summary>
    /// Handles the Click event of the viewStaffDetailsButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void viewStaffDetailsButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.CommandParameter is not StaffDto staffDto)
            return;

        staffDetailPositionTextBlock.Text = staffDto.Position.Name;
        staffDetailNameTextBlock.Text = staffDto.Name;
        staffDetailGenderTextBlock.Text = staffDto.Gender;
        staffDetailDateOfBirthTextBlock.Text = staffDto.DateOfBirth.ToString("dd/MM/yyyy");
        staffDetailEmailTextBlock.Text = staffDto.Email;
        staffDetailTelTextBlock.Text = staffDto.Tel;
        staffDetailAddressTextBlock.Text = staffDto.Address;
        staffDetailEmploymentStartDateTextBlock.Text = staffDto.EmploymentStartDate.ToString("dd/MM/yyyy");
        staffDetailCompositeUsernameTextBlock.Text = staffDto.CompositeUsername;

        _ = await staffDetailsContentDialog.ShowAsync();
    }

    /// <summary>
    /// Handles the Click event of the editStaffButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void editStaffButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.CommandParameter is not StaffDto staffDto)
            return;

        if (staffDto == null)
            return;

        // If trying to edit store manager's info, don't show the position combo box
        if (staffDto.PositionPrefix == "SM")
        {
            editStaffPositionComboBox.Visibility = Visibility.Collapsed;
            absolutionStaffPositionTextBlock.Visibility = Visibility.Visible;
        }
        else
        {
            editStaffPositionComboBox.Visibility = Visibility.Visible;
            absolutionStaffPositionTextBlock.Visibility = Visibility.Collapsed;
        }

        switch (staffDto.PositionPrefix)
        {
            case "ST":
                editStaffPositionComboBox.SelectedIndex = 0;
                break;
            case "AM":
                editStaffPositionComboBox.SelectedIndex = 1;
                break;
        }

        switch (staffDto.Gender)
        {
            case "Nam":
                editStaffGenderComboBox.SelectedIndex = 0;
                break;
            case "Nữ":
                editStaffGenderComboBox.SelectedIndex = 1;
                break;
        }

        editStaffNameEditableTextField.Text = staffDto.Name;
        editStaffDateOfBirthCalenderDatePicker.Date = (DateTimeOffset)staffDto.DateOfBirth.ToDateTime(TimeOnly.MinValue);
        editStaffEmailEditableTextField.Text = staffDto.Email;
        editStaffTelEditableTextField.Text = staffDto.Tel;
        editStaffAddressEditableTextField.Text = staffDto.Address;

        ViewModel.EditTargetStaff = staffDto;

        ContentDialogResult result = await editStaffDetailsContentDialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {
            editStaffDetailsContentDialog.IsPrimaryButtonEnabled = false;
            ViewModel.EditTargetStaff = null;
            return;
        }

        // Update new staff details

        switch (editStaffPositionComboBox.SelectedIndex)
        {
            case 0:
                staffDto.PositionPrefix = "ST";
                break;
            case 1:
                staffDto.PositionPrefix = "AM";
                break;
        }
        
        switch (editStaffGenderComboBox.SelectedIndex)
        {
            case 0:
                staffDto.Gender = "Nam";
                break;
            case 1:
                staffDto.Gender = "Nữ";
                break;
        }

        staffDto.Name = editStaffNameEditableTextField.Text;
        staffDto.DateOfBirth = DateOnly.FromDateTime(editStaffDateOfBirthCalenderDatePicker.Date.Value.DateTime);
        staffDto.Email = editStaffEmailEditableTextField.Text;
        staffDto.Tel = editStaffTelEditableTextField.Text;
        staffDto.Address = editStaffAddressEditableTextField.Text;

        ViewModel.HandleUpdateStaffDetails(staffDto, editStaffResultContentDialog);
    }

    /// <summary>
    /// Handles the Click event of the terminateContractButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void terminateContractButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.CommandParameter is not StaffDto staffDto)
            return;

        if (staffDto == null)
            return;

        ViewModel.PasswordForContractTermination = "";
        ViewModel.PasswordVerified = false;
        ViewModel.TerminationTargetStaff = staffDto;

        ContentDialogResult result = await terminateContractContentDialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {
            ViewModel.TerminationTargetStaff = null;
            return;
        }

        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
        {
            return;
        }

        if (staffAuthenticationService.Context.CurrentStaff == null)
        {
            return;
        }

        // Mark staff as "deleted" and "OutOfEmployment"
        staffDto.EmploymentEndDate = DateOnly.FromDateTime(DateTime.Now);
        staffDto.EmploymentStatus = "OutOfEmployment";
        staffDto.DeletedAt = DateTime.Now;
        staffDto.DeletedBy = staffAuthenticationService.Context.CurrentStaff.CompositeUsername;

        ViewModel.HandleTerminateStaff(staffDto, terminateStaffResultContentDialog);
    }

    /// <summary>
    /// Handles the DateChanged event of the editStaffDateOfBirthCalenderDatePicker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void editStaffDateOfBirthCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel.EditTargetStaff == null)
            return;

        if (editStaffDateOfBirthCalenderDatePicker.Date != (DateTimeOffset)ViewModel.EditTargetStaff.DateOfBirth.ToDateTime(TimeOnly.MinValue))
            editStaffDetailsContentDialog.IsPrimaryButtonEnabled = true;
    }

    /// <summary>
    /// Handles the SelectionChanged event of the editStaffPositionComboBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editStaffPositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // This event handler only functions to enable the primary button of the content dialog
        // if the new selection is different from the current in the ViewModel.EditStaffTarget.PositionPrefix

        // It does not update the ViewModel.EditStaffTarget.PositionPrefix, this is done directly in the 
        // editStaffButton_Click() method

        if (ViewModel is not StaffManagementViewModel)
            return;

        if (ViewModel.EditTargetStaff == null)
            return;

        var newPositionSelection = "";

        switch (editStaffPositionComboBox.SelectedIndex)
        {
            case 0:
                newPositionSelection = "ST";
                break;
            case 1:
                newPositionSelection = "AM";
                break;
        }

        if (newPositionSelection != ViewModel.EditTargetStaff.PositionPrefix)
            editStaffDetailsContentDialog.IsPrimaryButtonEnabled = true;
    }

    /// <summary>
    /// Handles the SelectionChanged event of the editStaffGenderComboBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editStaffGenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // This event handler only functions to enable the primary button of the content dialog
        // if the new selection is different from the current in the ViewModel.EditStaffTarget.Gender

        // It does not update the ViewModel.EditStaffTarget.Gender, this is done directly in the 
        // editStaffButton_Click() method

        if (ViewModel is not StaffManagementViewModel)
            return;

        if (ViewModel.EditTargetStaff == null)
            return;

        var newGenderSelection = "";

        switch (editStaffGenderComboBox.SelectedIndex)
        {
            case 0:
                newGenderSelection = "Nam";
                break;
            case 1:
                newGenderSelection = "Nữ";
                break;
        }

        if (newGenderSelection != ViewModel.EditTargetStaff.Gender)
            editStaffDetailsContentDialog.IsPrimaryButtonEnabled = true;
    }

    /// <summary>
    /// Handles the SaveClicked event of the editStaffNameEditableTextField control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editStaffNameEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleEditStaffNameEditableTextFieldSaveClicked
        (
            editStaffNameEditableTextField,
            editStaffNameErrorMessageTeachingTip
        );
    }

    /// <summary>
    /// Handles the SaveClicked event of the editStaffEmailEditableTextField control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editStaffEmailEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleEditStaffEmailEditableTextFieldSaveClicked
        (
            editStaffEmailEditableTextField,
            editStaffEmailErrorMessageTeachingTip
        );
    }

    /// <summary>
    /// Handles the SaveClicked event of the editStaffTelEditableTextField control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editStaffTelEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleEditStaffTelEditableTextFieldSaveClicked
        (
            editStaffTelEditableTextField,
            editStaffTelErrorMessageTeachingTip
        );
    }

    /// <summary>
    /// Handles the SaveClicked event of the editStaffAddressEditableTextField control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editStaffAddressEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleEditStaffAddressEditableTextFieldSaveClicked
        (
            editStaffAddressEditableTextField,
            editStaffAddressErrorMessageTeachingTip
        );
    }

    /// <summary>
    /// Handles the TextModified event of the editStaffDetails control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editStaffDetails_TextModified(object sender, EventArgs e)
    {
        editStaffDetailsContentDialog.IsPrimaryButtonEnabled = true;
    }

    /// <summary>
    /// Handles the Closed event of the editStaffDetailsContentDialog control.
    /// Resets the state of all EditableTextField controls to ensure they are not in editing mode.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void editStaffDetailsContentDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
    {
        editStaffNameEditableTextField.ResetState();
        editStaffEmailEditableTextField.ResetState();
        editStaffTelEditableTextField.ResetState();
        editStaffAddressEditableTextField.ResetState();
    }

    /// <summary>
    /// Handles the PasswordChanged event of the contractTerminationPasswordBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void contractTerminationPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (ViewModel.TerminationTargetStaff != null)
        {
            if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
                return;

            var verifyResult = await staffAuthenticationService.VerifyPasswordAsync
            (
                compositeUsername: ViewModel.TerminationTargetStaff.CompositeUsername,
                password: ViewModel.PasswordForContractTermination
            );

            if (verifyResult.succeded)
            {
                ViewModel.PasswordVerified = true;
            }
            else
            {
                ViewModel.PasswordVerified = false;
            }
        }
    }

}