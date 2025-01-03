using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Inventory;
using SipPOS.Views.General;

namespace SipPOS.Views.Promotion;


public sealed partial class PromotionMenuView : Page
{

    public PromotionMenuViewModel ViewModel { get; }

    public PromotionMenuView()
    {
        this.InitializeComponent();
        ViewModel = new PromotionMenuViewModel();
        ViewModel.SelectViewUponLoad(promotionMenuNavigationView, promotionMenuNavigationFrame);
    }

    private void promotionMenuNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.HandlePromotionMenuNavigationViewSelectionChanged(promotionMenuNavigationFrame, args);
    }

    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }
}
