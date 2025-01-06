using Microsoft.UI.Xaml.Controls;
using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

/// <summary>
/// Represents the initial setup page for tax configuration.
/// </summary>
public sealed partial class TaxConfigurationInitialSetupPage : Page
{
    /// <summary>
    /// Gets the ViewModel for the Store Setup.
    /// </summary>
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL SETUP PAGES

    /// <summary>
    /// Initializes a new instance of the <see cref="TaxConfigurationInitialSetupPage"/> class.
    /// </summary>
    public TaxConfigurationInitialSetupPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;
    }

    /// <summary>
    /// Handles the SelectionChanged event of the general VAT rate ComboBox.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void generalVatRateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleGeneralVatRateComboBoxSelectionChanged(generalVatRateComboBox.SelectedIndex);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the VAT method ComboBox.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void selectVatMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleSelectVatMethodComboBoxSelectionChanged(selectVatMethodComboBox.SelectedIndex);
    }
}
