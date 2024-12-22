using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Configuration;

namespace SipPOS.Views.Configuration;

public sealed partial class TaxConfigurationView : Page
{
    public TaxConfigurationViewModel ViewModel { get; }

    public TaxConfigurationView()
    {
        this.InitializeComponent();

        ViewModel = new TaxConfigurationViewModel();

        switch (ViewModel.EditVatRate)
        {
            case 0.00m:
                generalVatRateComboBox.SelectedIndex = 0;
                break;
            case 0.01m:
                generalVatRateComboBox.SelectedIndex = 1;
                break;
            case 0.03m:
                generalVatRateComboBox.SelectedIndex = 2;
                break;
            case 0.05m:
                generalVatRateComboBox.SelectedIndex = 3;
                break;
            case 0.08m:
                generalVatRateComboBox.SelectedIndex = 4;
                break;
            case 0.10m:
                generalVatRateComboBox.SelectedIndex = 5;
                break;
        }

        switch (ViewModel.EditVatMethod)
        {
            case "VAT_INCLUDED":
                selectVatMethodComboBox.SelectedIndex = 0;
                break;
            case "ORDER_BASED":
                selectVatMethodComboBox.SelectedIndex = 1;
                break;
        }
    }

    private void saveChangesOnTaxConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSaveChangesOnTaxConfigurationButtonClick(editTaxConfigurationResultContentDialog, saveChangesOnTaxConfigurationButton);
    }

    private void cancelChangesOnTaxConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleCancelChangesOnTaxConfigurationButtonClick(saveChangesOnTaxConfigurationButton);

        taxCodeEditableTextField.ResetState();

        switch (ViewModel.EditVatRate)
        {
            case 0.00m:
                generalVatRateComboBox.SelectedIndex = 0;
                break;
            case 0.01m:
                generalVatRateComboBox.SelectedIndex = 1;
                break;
            case 0.03m:
                generalVatRateComboBox.SelectedIndex = 2;
                break;
            case 0.05m:
                generalVatRateComboBox.SelectedIndex = 3;
                break;
            case 0.08m:
                generalVatRateComboBox.SelectedIndex = 4;
                break;
            case 0.10m:
                generalVatRateComboBox.SelectedIndex = 5;
                break;
        }

        switch (ViewModel.EditVatMethod)
        {
            case "VAT_INCLUDED":
                selectVatMethodComboBox.SelectedIndex = 0;
                break;
            case "ORDER_BASED":
                selectVatMethodComboBox.SelectedIndex = 1;
                break;
        }
    }

    private void generalVatRateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not TaxConfigurationViewModel)
        {
            return;
        }

        ViewModel.HandleGeneralVatRateComboBoxSelectionChanged(generalVatRateComboBox.SelectedIndex, saveChangesOnTaxConfigurationButton);
    }

    private void selectVatMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not TaxConfigurationViewModel)
        {
            return;
        }

        ViewModel.HandleSelectVatMethodComboBoxSelectionChanged(selectVatMethodComboBox.SelectedIndex, saveChangesOnTaxConfigurationButton);
    }

    private void taxCodeEditableTextField_TextModified(object sender, EventArgs e)
    {
        ViewModel.HandleTaxCodeEditableTextFieldTextModified(saveChangesOnTaxConfigurationButton);
    }

    private void taxCodeEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleTaxCodeEditableTextField_SaveClicked
        (
            taxCodeErrorMessageTeachingTip,
            taxCodeEditableTextField
        );
    }
}
