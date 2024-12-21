using Microsoft.UI.Xaml.Controls;
using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

public sealed partial class TaxConfigurationInitialSetupPage : Page
{
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL SETUP PAGES

    public TaxConfigurationInitialSetupPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;
    }

    private void generalVatRateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleGeneralVatRateComboBoxSelectionChanged(generalVatRateComboBox.SelectedIndex);
    }

    private void selectVatMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleSelectVatMethodComboBoxSelectionChanged(selectVatMethodComboBox.SelectedIndex);
    }
}
