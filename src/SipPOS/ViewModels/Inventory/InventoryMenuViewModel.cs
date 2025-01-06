using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.Inventory;

namespace SipPOS.ViewModels.Inventory;

/// <summary>
/// ViewModel for managing the inventory menu navigation.
/// </summary>
public class InventoryMenuViewModel
{
    /// <summary>
    /// Selects the default view upon loading the inventory menu.
    /// </summary>
    /// <param name="inventoryMenuNavigationView">The navigation view of the inventory menu.</param>
    /// <param name="inventoryMenuNavigationFrame">The frame to navigate within the inventory menu.</param>
    public void SelectViewUponLoad(NavigationView inventoryMenuNavigationView, Frame inventoryMenuNavigationFrame)
    {
        inventoryMenuNavigationView.SelectedItem = inventoryMenuNavigationView.MenuItems[0];
    }

    /// <summary>
    /// Handles the selection change in the inventory menu navigation view.
    /// </summary>
    /// <param name="inventoryMenuNavigationFrame">The frame to navigate within the inventory menu.</param>
    /// <param name="args">The event arguments for the selection change.</param>
    public void HandleInventoryMenuNavigationViewSelectionChanged(Frame inventoryMenuNavigationFrame, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer != null)
        {
            var selectedItemTag = args.SelectedItemContainer.Tag.ToString();

            switch (selectedItemTag)
            {
                case "ProductManagement":
                    inventoryMenuNavigationFrame.Navigate(typeof(ProductManagementView));
                    break;
                case "CategoryManagement":
                    inventoryMenuNavigationFrame.Navigate(typeof(CategoryManagementView));
                    break;
            }
        }
    }
}
