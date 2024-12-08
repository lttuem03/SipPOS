using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Staff;

namespace SipPOS.Views.Staff;

/// <summary>
/// Represents the StaffRegistrationView.
/// </summary>
public sealed partial class StaffRegistrationView : Page
{
    /// <summary>
    /// Gets the view model for the staff registration.
    /// </summary>
    public StaffRegistrationViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffRegistrationView"/> class.
    /// </summary>
    public StaffRegistrationView()
    {
        this.InitializeComponent();
        ViewModel = new StaffRegistrationViewModel();
    }

    /// <summary>
    /// Handles the Click event of the goBackButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(StaffManagementView));
    }

    /// <summary>
    /// Handles the SelectionChanged event of the staffPositionComboBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void staffPositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffRegistrationViewModel)
            return;

        ViewModel.HandleStaffPositionComboBoxSelectionChanged(staffPositionComboBox.SelectedIndex);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the staffGenderComboBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void staffGenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffRegistrationViewModel)
            return;

        ViewModel.HandleStaffGenderComboBoxSelectionChanged(staffGenderComboBox.SelectedIndex);
    }

    /// <summary>
    /// Handles the Click event of the cancelStaffRegistrationButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void cancelStaffRegistrationButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(StaffManagementView));
    }

    /// <summary>
    /// Handles the Click event of the registerStaffButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void registerStaffButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleRegisterStaffButtonClick
        (
            confirmStaffInformationContentDialog,
            accountCreationResultContentDialog
        );
    }

    /// <summary>
    /// Handles the Opened event of the confirmStaffInformationContentDialog control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void confirmStaffInformationContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
    {
        confirmStaffNameTextBlock.Text = ViewModel.StaffName;
        confirmStaffGenderTextBlock.Text = ViewModel.StaffGender;
        confirmStaffDateOfBirthTextBlock.Text = ViewModel.StaffDateOfBirthString;
        confirmStaffEmailTextBlock.Text = ViewModel.StaffEmail;
        confirmStaffTelTextBlock.Text = ViewModel.StaffTel;
        confirmStaffAddressTextBlock.Text = ViewModel.StaffAddress;
        confirmStaffEmploymentStartDateTextBlock.Text = ViewModel.StaffEmploymentStartDateString;
        confirmStaffCompositeUsernameTextBlock.Text = ViewModel.StaffCompositeUsername;
    }

    /// <summary>
    /// Handles the DateChanged event of the staffDateOfBirthCalenderDatePicker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void staffDateOfBirthCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleStaffDateOfBirthCalenderDatePickerDateChanged();
    }

    /// <summary>
    /// Handles the DateChanged event of the staffEmploymentStartDateCalenderDatePicker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void staffEmploymentStartDateCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleStaffEmploymentStartDateCalenderDatePickerDateChanged();
    }
}
