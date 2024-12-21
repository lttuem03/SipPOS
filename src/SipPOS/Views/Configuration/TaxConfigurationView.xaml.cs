using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Configuration;

namespace SipPOS.Views.Configuration;

public sealed partial class TaxConfigurationView : Page
{
    public TaxConfigurationViewModel ViewModel { get; }

    public TaxConfigurationView()
    {
        this.InitializeComponent();

        ViewModel = new TaxConfigurationViewModel();
    }

    private void saveChangesOnTaxConfigurationButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void cancelChangesOnTaxConfigurationButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void generalVatRateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not TaxConfigurationViewModel)
        {
            return;
        }
    }

    private void selectVatMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not TaxConfigurationViewModel)
        {
            return;
        }
    }
}
