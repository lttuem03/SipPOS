using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.Configuration;

namespace SipPOS.ViewModels.Configuration;

/// <summary>
/// ViewModel for the Configuration Menu.
/// </summary>
public class ConfigurationMenuViewModel
{
    /// <summary>
    /// Selects the default view upon loading the configuration menu.
    /// </summary>
    /// <param name="configurationMenuNavigationView">The navigation view of the configuration menu.</param>
    /// <param name="configurationMenuNavigationFrame">The frame to navigate within the configuration menu.</param>
    public void SelectViewUponLoad(NavigationView configurationMenuNavigationView, Frame configurationMenuNavigationFrame)
    {
        configurationMenuNavigationFrame.Navigate(typeof(StoreConfigurationView));
    }

    /// <summary>
    /// Handles the selection changed event of the configuration menu navigation view.
    /// </summary>
    /// <param name="configurationMenuNavigationFrame">The frame to navigate within the configuration menu.</param>
    /// <param name="args">The event arguments containing the selection details.</param>
    public void HandleConfigurationMenuNavigationViewSelectionChanged(Frame configurationMenuNavigationFrame, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer != null)
        {
            var selectedItemTag = args.SelectedItemContainer.Tag.ToString();

            switch (selectedItemTag)
            {
                case "StoreConfiguration":
                    configurationMenuNavigationFrame.Navigate(typeof(StoreConfigurationView));
                    break;
                case "TaxConfiguration":
                    configurationMenuNavigationFrame.Navigate(typeof(TaxConfigurationView));
                    break;
                case "SalaryConfiguration":
                    configurationMenuNavigationFrame.Navigate(typeof(SalaryConfigurationView));
                    break;
                case "QrPayConfiguration":
                    configurationMenuNavigationFrame.Navigate(typeof(QrPayConfigurationView));
                    break;
            }
        }
    }
}
