using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Inventory;
using SipPOS.Views.General;

namespace SipPOS.Views.Inventory;

public sealed partial class InventoryMenuView : Page
{
    public InventoryMenuViewModel ViewModel { get; }

    public InventoryMenuView()
    {
        this.InitializeComponent();
        ViewModel = new InventoryMenuViewModel();
        ViewModel.SelectViewUponLoad(inventoryMenuNavigationView, inventoryMenuNavigationFrame);
    }
    private void inventoryMenuNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.HandleInventoryMenuNavigationViewSelectionChanged(inventoryMenuNavigationFrame, args);
    }

    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }
}
