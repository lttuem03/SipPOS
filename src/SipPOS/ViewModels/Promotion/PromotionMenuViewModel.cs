using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.Promotion;

namespace SipPOS.ViewModels.Inventory;

/// <summary>
/// ViewModel for managing the inventory menu navigation.
/// </summary>
public class PromotionMenuViewModel
{
    /// <summary>
    /// Selects the initial view upon loading.
    /// </summary>
    /// <param name="promotionMenuNavigationView">The navigation view to select the item in.</param>
    /// <param name="promotionMenuNavigationFrame">The frame to navigate within.</param>
    public void SelectViewUponLoad(NavigationView promotionMenuNavigationView, Frame promotionMenuNavigationFrame)
    {
        promotionMenuNavigationView.SelectedItem = promotionMenuNavigationView.MenuItems[0];
    }

    /// <summary>
    /// Handles the selection change in the promotion menu navigation view.
    /// </summary>
    /// <param name="promotionMenuNavigationFrame">The frame to navigate within.</param>
    /// <param name="args">The event arguments containing the selected item.</param>
    public void HandlePromotionMenuNavigationViewSelectionChanged(Frame promotionMenuNavigationFrame, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer != null)
        {
            var selectedItemTag = args.SelectedItemContainer.Tag.ToString();

            switch (selectedItemTag)
            {
                case "PromotionManagement":
                    promotionMenuNavigationFrame.Navigate(typeof(SpecialOffersManagementView));
                    break;
            }
        }
    }
}
