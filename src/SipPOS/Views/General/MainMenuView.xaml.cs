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

    private void toCashierMenuViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToCashierMenuViewButtonClick();
    }

    private void toInventoryMenuView_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToInventoryMenuViewClick();
    }

    private void toStaffManagementViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToStaffManagementViewButtonClick();
    }

    private void toProfileViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToProfileViewButtonClick();
    }

    private void toRevenueDashboardViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToRevenueDashboardViewButtonClick();
    }

    private void toSpecialOffersManagementViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToSpecialOffersManagementViewButtonClick();
    }

    private void toConfigurationMenuViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToConfigurationMenuViewButtonClick();
    }

    private void openSlashCloseShiftButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleOpenSlashCloseShiftButtonClick();
    }

    private void returnToLoginViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleReturnToLoginViewButtonClick();
    }

    private void changeIdButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleChangeIdButtonClick();
    }

    private void exitProgramButton_Click(object sender, RoutedEventArgs e)
    {
        App.Current.Exit();
    }
}
