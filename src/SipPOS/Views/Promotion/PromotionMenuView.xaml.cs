using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SipPOS.ViewModels.Inventory;
using SipPOS.Views.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SipPOS.Views.Promotion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
}
