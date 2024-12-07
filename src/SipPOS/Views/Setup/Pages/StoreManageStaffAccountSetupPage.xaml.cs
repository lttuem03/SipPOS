using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

public sealed partial class StoreManageStaffAccountSetupPage : Page
{
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACCROSS ALL SETUP PAGES

    public StoreManageStaffAccountSetupPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;
    }

    private void managerGenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleManagerGenderComboBoxSelectionChanged(managerGenderComboBox);
    }

    private void managerDateOfBirthCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleManagerDateOfBirthCalenderDatePickerDateChanged();
    }

    private void managerEmploymentStartDateCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleManagerEmploymentStartDateCalenderDatePickerDateChanged();
    }
}