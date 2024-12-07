using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup;

public sealed partial class StoreSetupView : Page
{
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACCROSS ALL PAGES

    public StoreSetupView()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
        {
            viewModel.StoreSetupNavigationFrame = storeSetupNavigationFrame;
            ViewModel = viewModel;
            ViewModel.ToFirstPage();
        }
    }

    private void toPreviousStepButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleToPreviousStepButtonClick();
    }

    private void toNextStepButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        if (ViewModel.CurrentPageIndex + 1 < ViewModel.TotalPageCount)
        {
            ViewModel.HandleToNextStepButtonClick();
        }
        else
        {
            ViewModel.HandleCompleteSetupButtonClick(setupCompleteContentDialog);
        }
    }
}
