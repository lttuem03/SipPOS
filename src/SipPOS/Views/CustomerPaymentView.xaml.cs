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
using Microsoft.UI.Xaml.Media.Imaging;

using SipPOS.DataTransfer.Entity;
using SipPOS.ViewModels;

namespace SipPOS.Views;

/// <summary>
/// A page that handles customer payments.
/// </summary>
public sealed partial class CustomerPaymentView : Page
{
    /// <summary>
    /// Gets the ViewModel for the customer payment.
    /// </summary>
    public CustomerPaymentViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerPaymentView"/> class.
    /// </summary>
    public CustomerPaymentView()
    {
        ViewModel = App.GetService<CustomerPaymentViewModel>();
        InitializeComponent();
        VietQRLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Vietqr-Logo.png"));
    }

    /// <summary>
    /// Called when the page is navigated to.
    /// </summary>
    /// <param name="e">The event data.</param>
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
        }
        else
        {
            //handle error or navigate back
        }
    }

    /// <summary>
    /// Called when the page is navigated from.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        _ = ViewModel.CancelPayment();
    }
}
