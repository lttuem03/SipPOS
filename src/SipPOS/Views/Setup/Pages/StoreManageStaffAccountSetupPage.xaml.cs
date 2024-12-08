using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

/// <summary>
/// Represents the StoreManageStaffAccountSetupPage.
/// </summary>
public sealed partial class StoreManageStaffAccountSetupPage : Page
{
    /// <summary>
    /// Gets the view model for the store setup.
    /// </summary>
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL SETUP PAGES

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreManageStaffAccountSetupPage"/> class.
    /// </summary>
    public StoreManageStaffAccountSetupPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;
    }

    /// <summary>
    /// Handles the SelectionChanged event of the manager gender combo box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void managerGenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleManagerGenderComboBoxSelectionChanged(managerGenderComboBox);
    }

    /// <summary>
    /// Handles the DateChanged event of the manager date of birth calendar date picker.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void managerDateOfBirthCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleManagerDateOfBirthCalenderDatePickerDateChanged();
    }

    /// <summary>
    /// Handles the DateChanged event of the manager employment start date calendar date picker.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void managerEmploymentStartDateCalenderDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleManagerEmploymentStartDateCalenderDatePickerDateChanged();
    }
}
