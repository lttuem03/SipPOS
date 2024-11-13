using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using CommunityToolkit.WinUI.UI.Controls;

using SipPOS.DataTransfer;
using SipPOS.Models;
using SipPOS.ViewModels;
using SipPOS.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SipPOS.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerPaymentView : Page
    {
        public CustomerPaymentViewModel ViewModel { get; }

        public CustomerPaymentView()
        {
            ViewModel = App.GetService<CustomerPaymentViewModel>();
            InitializeComponent();
            VietQRLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Vietqr-Logo.png"));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Products.Clear();
            if (e.Parameter is IList<ProductDto> productList)
            {
                foreach (var item in productList)
                {
                    ViewModel.Products.Add(item);     
                }
                ViewModel.CalculateTotalPrice();
            } else
            {
                //handle error or navigate back
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _ = ViewModel.CancelPayment();
        }
    }
}
