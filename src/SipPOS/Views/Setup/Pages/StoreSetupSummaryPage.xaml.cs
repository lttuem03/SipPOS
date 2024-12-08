using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

/// <summary>
/// Represents the StoreSetupSummaryPage.
/// </summary>
public sealed partial class StoreSetupSummaryPage : Page
{
    /// <summary>
    /// Gets the view model for the store setup.
    /// </summary>
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL SETUP PAGES

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreSetupSummaryPage"/> class.
    /// </summary>
    public StoreSetupSummaryPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;
    }
}
