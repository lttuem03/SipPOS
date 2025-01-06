using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

/// <summary>
/// Represents the initial setup page for store configuration.
/// </summary>
public sealed partial class StoreConfigurationInitialSetupPage : Page
{
    /// <summary>
    /// Gets the ViewModel for the Store Setup.
    /// </summary>
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL SETUP PAGES

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreConfigurationInitialSetupPage"/> class.
    /// </summary>
    public StoreConfigurationInitialSetupPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;
    }

    /// <summary>
    /// Handles the SelectedTimeChanged event of the opening hour TimePicker.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void selectOpeningHourTimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleSelectOpeningHourTimePickerSelectedTimeChanged(selectOpeningHourTimePicker);
    }

    /// <summary>
    /// Handles the SelectedTimeChanged event of the closing hour TimePicker.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void selectClosingHourTimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleSelectClosingHourTimePickerSelectedTimeChanged(selectClosingHourTimePicker);
    }
}
