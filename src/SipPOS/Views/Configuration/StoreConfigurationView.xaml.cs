using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Configuration;

namespace SipPOS.Views.Configuration;

/// <summary>
/// Represents the view for store configuration.
/// </summary>
public sealed partial class StoreConfigurationView : Page
{
    /// <summary>
    /// Gets the view model for the store configuration.
    /// </summary>
    public StoreConfigurationViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreConfigurationView"/> class.
    /// </summary>
    public StoreConfigurationView()
    {
        this.InitializeComponent();
        ViewModel = new StoreConfigurationViewModel();
    }
}
