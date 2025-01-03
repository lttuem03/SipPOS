using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Inventory;
using SipPOS.Views.General;

namespace SipPOS.Views.Inventory;

/// <summary>
/// Represents the Inventory Menu View.
/// </summary>
public sealed partial class InventoryMenuView : Page
{
    /// <summary>
    /// Gets the ViewModel for the Inventory Menu View.
    /// </summary>
    public InventoryMenuViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryMenuView"/> class.
    /// </summary>
    public InventoryMenuView()
    {
        this.InitializeComponent();
        ViewModel = new InventoryMenuViewModel();
        ViewModel.SelectViewUponLoad(inventoryMenuNavigationView, inventoryMenuNavigationFrame);
    }

    /// <summary>
    /// Handles the SelectionChanged event of the inventoryMenuNavigationView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="NavigationViewSelectionChangedEventArgs"/> instance containing the event data.</param>
    private void inventoryMenuNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.HandleInventoryMenuNavigationViewSelectionChanged(inventoryMenuNavigationFrame, args);
    }

    /// <summary>
    /// Handles the Click event of the goBackButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }
}
