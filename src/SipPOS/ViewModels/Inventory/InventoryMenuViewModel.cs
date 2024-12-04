using Microsoft.UI.Xaml.Controls;
using SipPOS.Views.Inventory;

namespace SipPOS.ViewModels.Inventory;

public class InventoryMenuViewModel
{
    public void SelectViewUponLoad(NavigationView inventoryMenuNavigationView, Frame inventoryMenuNavigationFrame)
    {
        inventoryMenuNavigationFrame.Navigate(typeof(ProductManagementView));
    }

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