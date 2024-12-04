using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.General;

namespace SipPOS.Views.General;

/// <summary>
/// Represents the MainMenuView.
/// </summary>
public sealed partial class MainMenuView : Page
{
    /// <summary>
    /// Gets the view model for the main menu view.
    /// </summary>
    public MainMenuViewModel ViewModel { get; }

    private readonly DispatcherTimer _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenuView"/> class.
    /// </summary>
    public MainMenuView()
    {
        this.InitializeComponent();
        ViewModel = new MainMenuViewModel();
        clockTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");

        // Because the timer needs to be instantiated in the UI thread, it is done here.
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (sender, e) =>
        {
            clockTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
        };
        _timer.Start();
    }

    /// <summary>
    /// Handles the click event of the button to navigate to the inventory view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void toProductManagementViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToProductManagementViewButtonClick();
    }

    private void toCategoryManagementViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToCategoryManagementViewButtonClick();
    }

    private void toCustomerPaymentViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToCustomerPaymentViewButtonClick();
    }

    /// <summary>
    /// Handles the click event of the button to change the ID.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void changeIdButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleChangeIdButtonClick();
    }

    private void toConfigurationMenuViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToConfigurationMenuViewButtonClick();
    }
    private void toInventoryMenuView_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToInventoryMenuViewClick();
    }

    private void returnToLoginViewButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void exitProgramButton_Click(object sender, RoutedEventArgs e)
    {
        App.Current.Exit();
    }

    private void toProfileViewMangement_Click(object sender, RoutedEventArgs e)
    {

    }

    private void toProfileViewButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void toSpecialOffersManagementView_Click(object sender, RoutedEventArgs e)
    {

    }

    private void openSlashCloseShiftButton_Click(object sender, RoutedEventArgs e)
    {

    }
}
