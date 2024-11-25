using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Configuration;
using SipPOS.Views.General;

namespace SipPOS.Views.Configuration;

/// <summary>
/// Represents the Configuration Menu View.
/// </summary>
public sealed partial class ConfigurationMenuView : Page
{
    /// <summary>
    /// Gets the ViewModel for the Configuration Menu View.
    /// </summary>
    public ConfigurationMenuViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMenuView"/> class.
    /// </summary>
    public ConfigurationMenuView()
    {
        this.InitializeComponent();
        ViewModel = new ConfigurationMenuViewModel();
        ViewModel.SelectViewUponLoad(configurationMenuNavigationView, configurationMenuNavigationFrame);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the configurationMenuNavigationView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="NavigationViewSelectionChangedEventArgs"/> instance containing the event data.</param>
    private void configurationMenuNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.HandleConfigurationMenuNavigationViewSelectionChanged(configurationMenuNavigationFrame, args);
    }

    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }
}
