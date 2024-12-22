using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

public sealed partial class StoreConfigurationInitialSetupPage : Page
{
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL SETUP PAGES

    public StoreConfigurationInitialSetupPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;
    }

    private void selectOpeningHourTimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleSelectOpeningHourTimePickerSelectedTimeChanged(selectOpeningHourTimePicker);
    }

    private void selectClosingHourTimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
    {
        if (ViewModel == null)
            return;

        ViewModel.HandleSelectClosingHourTimePickerSelectedTimeChanged(selectClosingHourTimePicker);
    }
}
