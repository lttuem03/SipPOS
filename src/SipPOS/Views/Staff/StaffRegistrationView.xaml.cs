using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Staff;

namespace SipPOS.Views.Staff;

public sealed partial class StaffRegistrationView : Page
{
    public StaffRegistrationViewModel ViewModel { get; }

    public StaffRegistrationView()
    {
        this.InitializeComponent();
        ViewModel = new StaffRegistrationViewModel();
    }

    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(StaffManagementView));
    }

    private void staffPositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffRegistrationViewModel)
            return;

        ViewModel.HandleStaffPositionComboBoxSelectionChanged(staffPositionComboBox.SelectedIndex);
    }

    private void staffGenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffRegistrationViewModel)
            return;

        ViewModel.HandleStaffGenderComboBoxSelectionChanged(staffGenderComboBox.SelectedIndex);
    }

    private void cancelStaffRegistrationButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(StaffManagementView));
    }

    private void registerStaffButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleRegisterStaffButtonClick
        (
            confirmStaffInformationContentDialog,
            accountCreationResultContentDialog
        );
    }

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

    private void staffDateOfBirthCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleStaffDateOfBirthCalenderDatePickerDateChanged();
    }

    private void staffEmploymentStartDateCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleStaffEmploymentStartDateCalenderDatePickerDateChanged();
    }
}