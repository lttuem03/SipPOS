using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup;

/// <summary>
/// Represents the StoreSetupView.
/// </summary>
public sealed partial class StoreSetupView : Page
{
    /// <summary>
    /// Gets the view model for the store setup.
    /// </summary>
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL PAGES

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreSetupView"/> class.
    /// </summary>
    public StoreSetupView()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
        {
            viewModel.StoreSetupNavigationFrame = storeSetupNavigationFrame;
            ViewModel = viewModel;
            ViewModel.ToFirstPage();
        }
    }

    /// <summary>
    /// Handles the Click event of the toPreviousStepButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void toPreviousStepButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleToPreviousStepButtonClick();
    }

    /// <summary>
    /// Handles the Click event of the toNextStepButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void toNextStepButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        if (ViewModel.CurrentPageIndex + 1 < ViewModel.TotalPageCount)
        {
            ViewModel.HandleToNextStepButtonClick();
        }
        else
        {
            ViewModel.HandleCompleteSetupButtonClick(setupCompleteContentDialog);
        }
    }
}
