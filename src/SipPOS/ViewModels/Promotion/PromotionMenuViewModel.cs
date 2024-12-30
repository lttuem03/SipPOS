using Microsoft.UI.Xaml.Controls;
using SipPOS.Views.Inventory;
using SipPOS.Views.Promotion;

namespace SipPOS.ViewModels.Inventory;

/// <summary>
/// ViewModel for managing the inventory menu navigation.
/// </summary>
public class PromotionMenuViewModel
{

    public void SelectViewUponLoad(NavigationView promotionMenuNavigationView, Frame promotionMenuNavigationFrame)
    {
        promotionMenuNavigationView.SelectedItem = promotionMenuNavigationView.MenuItems[0];
    }

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
