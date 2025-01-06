using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Configuration;

namespace SipPOS.Views.Configuration;

/// <summary>
/// Represents the view for tax configuration.
/// </summary>
public sealed partial class TaxConfigurationView : Page
{
    /// <summary>
    /// Gets the view model for the tax configuration.
    /// </summary>
    public TaxConfigurationViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TaxConfigurationView"/> class.
    /// </summary>
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

    /// <summary>
    /// Handles the click event for the save changes button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void saveChangesOnTaxConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSaveChangesOnTaxConfigurationButtonClick(editTaxConfigurationResultContentDialog, saveChangesOnTaxConfigurationButton);
    }

    /// <summary>
    /// Handles the click event for the cancel changes button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the selection changed event for the general VAT rate combo box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void generalVatRateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not TaxConfigurationViewModel)
        {
            return;
        }

        ViewModel.HandleGeneralVatRateComboBoxSelectionChanged(generalVatRateComboBox.SelectedIndex, saveChangesOnTaxConfigurationButton);
    }

    /// <summary>
    /// Handles the selection changed event for the VAT method combo box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void selectVatMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not TaxConfigurationViewModel)
        {
            return;
        }

        ViewModel.HandleSelectVatMethodComboBoxSelectionChanged(selectVatMethodComboBox.SelectedIndex, saveChangesOnTaxConfigurationButton);
    }

    /// <summary>
    /// Handles the text modified event for the tax code editable text field.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void taxCodeEditableTextField_TextModified(object sender, EventArgs e)
    {
        ViewModel.HandleTaxCodeEditableTextFieldTextModified(saveChangesOnTaxConfigurationButton);
    }

    /// <summary>
    /// Handles the save clicked event for the tax code editable text field.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void taxCodeEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleTaxCodeEditableTextField_SaveClicked
        (
            taxCodeErrorMessageTeachingTip,
            taxCodeEditableTextField
        );
    }
}
