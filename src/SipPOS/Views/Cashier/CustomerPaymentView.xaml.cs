using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;

using SipPOS.DataTransfer.Entity;
using SipPOS.ViewModels.Cashier;

namespace SipPOS.Views.Cashier;

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
        this.InitializeComponent();
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
