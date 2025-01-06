using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.General;
using SipPOS.ViewModels.Inventory;

namespace SipPOS.Views.Promotion;


/// <summary>
/// Represents the PromotionMenuView.
/// </summary>
public sealed partial class PromotionMenuView : Page
{
    /// <summary>
    /// Gets the view model for the promotion menu view.
    /// </summary>
    public PromotionMenuViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PromotionMenuView"/> class.
    /// </summary>
    public PromotionMenuView()
    {
        this.InitializeComponent();
        ViewModel = new PromotionMenuViewModel();
        ViewModel.SelectViewUponLoad(promotionMenuNavigationView, promotionMenuNavigationFrame);
    }

    /// <summary>
    /// Handles the selection changed event of the promotion menu navigation view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void promotionMenuNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.HandlePromotionMenuNavigationViewSelectionChanged(promotionMenuNavigationFrame, args);
    }

    /// <summary>
    /// Handles the click event of the go back button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }
}
