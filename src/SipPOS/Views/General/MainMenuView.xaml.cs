using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.General;
using SipPOS.Views.Cashier;

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

    private readonly DispatcherTimer _mainMenuTimer;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenuView"/> class.
    /// </summary>
    public MainMenuView()
    {
        this.InitializeComponent();
        ViewModel = new MainMenuViewModel();

        clockTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");

        // Because the timer needs to be instantiated in the UI thread, it is done here.
        _mainMenuTimer = new DispatcherTimer();
        _mainMenuTimer.Interval = TimeSpan.FromSeconds(1);
        _mainMenuTimer.Tick += (sender, e) =>
        {
            clockTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
        };
        _mainMenuTimer.Start();
    }

    /// <summary>
    /// Handles the Click event of the toCashierMenuViewButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void toCashierMenuViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToCashierMenuViewButtonClick(mainMenuNotificationInfoBar);
    }

    /// <summary>
    /// Handles the Click event of the toInventoryMenuView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void toInventoryMenuView_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToInventoryMenuViewClick(mainMenuNotificationInfoBar);
    }

    /// <summary>
    /// Handles the Click event of the toStaffManagementViewButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void toStaffManagementViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToStaffManagementViewButtonClick(mainMenuNotificationInfoBar);
    }

    /// <summary>
    /// Handles the Click event of the toProfileViewButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void toProfileViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToProfileViewButtonClick(mainMenuNotificationInfoBar);
    }

    
    private void toInvoiceHistoryViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToInvoiceHistoryViewButtonClick(mainMenuNotificationInfoBar);
    }

    /// <summary>
    /// Handles the Click event of the toRevenueDashboardViewButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void toRevenueDashboardViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToRevenueDashboardViewButtonClick(mainMenuNotificationInfoBar);
    }

    /// <summary>
    /// Handles the Click event of the toSpecialOffersManagementViewButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void toSpecialOffersManagementViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToSpecialOffersManagementViewButtonClick(mainMenuNotificationInfoBar);
    }

    /// <summary>
    /// Handles the Click event of the toConfigurationMenuViewButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void toConfigurationMenuViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToConfigurationMenuViewButtonClick(mainMenuNotificationInfoBar);
    }

    /// <summary>
    /// Handles the Click event of the openSlashCloseShiftButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void openSlashCloseShiftButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleOpenSlashCloseShiftButtonClick();
    }

    /// <summary>
    /// Handles the Click event of the returnToLoginViewButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void returnToLoginViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleReturnToLoginViewButtonClick();
    }

    /// <summary>
    /// Handles the Click event of the changeIdButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void changeIdButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleChangeIdButtonClick();
    }

    /// <summary>
    /// Handles the Click event of the exitProgramButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void exitProgramButton_Click(object sender, RoutedEventArgs e)
    {
        App.Current.Exit();
    }

}
