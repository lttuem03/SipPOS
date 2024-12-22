using Microsoft.UI.Xaml.Controls;
using SipPOS.Models.Entity;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

/// <summary>
/// Represents the StoreSetupSummaryPage.
/// </summary>
public sealed partial class StoreSetupSummaryPage : Page
{
    /// <summary>
    /// Gets the view model for the store setup.
    /// </summary>
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL SETUP PAGES

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreSetupSummaryPage"/> class.
    /// </summary>
    public StoreSetupSummaryPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
            return;

        if (storeAuthenticationService.Context.CurrentStore == null)
            return;

        Store currentStore = storeAuthenticationService.Context.CurrentStore;

        // Fill out the summary fields
        if (ViewModel is not null)
        {
            // Store information summary
            summaryStoreNameTextBlock.Text = currentStore.Name;
            summaryStoreAddressTextBlock.Text = currentStore.Address;
            summaryStoreEmailTextBlock.Text = currentStore.Email;
            summaryStoreTelTextBlock.Text = currentStore.Tel;
            summaryStoreUsernameTextBlock.Text = currentStore.Username;

            // Store manager account information summary
            summaryStoreManagerNameTextBlock.Text = ViewModel.StoreManagerName;
            summaryStoreManagerGenderTextBlock.Text = ViewModel.StoreManagerGender;
            summaryStoreManagerDateOfBirthTextBlock.Text = ViewModel.StoreManagerDateOfBirthString;
            summaryStoreManagerEmailTextBlock.Text = ViewModel.StoreManagerEmail;
            summaryStoreManagerTelTextBlock.Text = ViewModel.StoreManagerTel;
            summaryStoreManagerAddressTextBlock.Text = ViewModel.StoreManagerAddress;
            summaryStoreManagerEmploymentStartDateTextBlock.Text = ViewModel.StoreManagerEmploymentStartDateString;
            summaryStoreManagerCompositeUsernameTextBlock.Text = ViewModel.StoreManagerCompositeUsername;

            // Initial configuration summary
            summaryOperatingHoursTextBlock.Text = ViewModel.OperatingHoursString;
            summaryTaxCodeTextBlock.Text = ViewModel.TaxCode;
            summaryVatRateTextBlock.Text = String.Format("{0:P0}", ViewModel.VatRate);
            switch (ViewModel.VatMethod)
            {
                case "VAT_INCLUDED":
                    summaryVatMethodTextBlock.Text = "Giá bán đã bao gồm VAT";
                    break;
                case "ORDER_BASED":
                    summaryVatMethodTextBlock.Text = "Theo tổng giá trị đơn hàng";
                    break;
            }

            summaryStaffBaseSalaryTextBlock.Text = ViewModel.StaffBaseSalaryString;
            summaryStaffHourlySalaryTextBlock.Text = ViewModel.StaffHourlySalaryString;
            summaryAssistantManagerBaseSalaryTextBlock.Text = ViewModel.AssistantManagerBaseSalaryString;
            summaryAssistantManagerHourlySalaryTextBlock.Text = ViewModel.AssistantManagerHourlySalaryString;
            summaryStoreManagerBaseSalaryTextBlock.Text = ViewModel.StoreManagerBaseSalaryString;
            summaryStoreManagerHourlySalaryTextBlock.Text = ViewModel.StoreManagerHourlySalaryString;
        }
    }
}
