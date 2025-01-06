using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Configuration;

namespace SipPOS.Views.Configuration;

/// <summary>
/// Represents the view for store configuration.
/// </summary>
public sealed partial class StoreConfigurationView : Page
{
    /// <summary>
    /// Gets the view model for the store configuration.
    /// </summary>
    public StoreConfigurationViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreConfigurationView"/> class.
    /// </summary>
    public StoreConfigurationView()
    {
        this.InitializeComponent();
        ViewModel = new StoreConfigurationViewModel();
    }

    /// <summary>
    /// Handles the click event for the save changes button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void saveChangesOnStoreConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSaveChangesOnStoreConfigurationButtonClick
        (
            editStoreConfigurationResultContentDialog,
            saveChangesOnStoreConfigurationButton
        );
    }

    /// <summary>
    /// Handles the click event for the cancel changes button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void cancelChangesOnStoreConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleCancelChangesOnStoreConfigurationButtonClick(saveChangesOnStoreConfigurationButton);

        storeNameEditableTextField.ResetState();
        storeAddressEditableTextField.ResetState();
        storeEmailEditableTextField.ResetState();
        storeTelEditableTextField.ResetState();

        editOpeningHourTimePicker.Time = ViewModel.EditOpeningTime;
        editClosingHourTimePicker.Time = ViewModel.EditClosingTime;
    }

    /// <summary>
    /// Handles the text modified event for editable text fields.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editableTextField_TextModified(object sender, EventArgs e)
    {
        ViewModel.HandleEditableTextFieldTextModified(saveChangesOnStoreConfigurationButton);
    }

    /// <summary>
    /// Handles the save clicked event for the store name editable text field.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void storeNameEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleStoreNameEditableTextFieldSaveClicked
        (
            storeNameErrorMessageTeachingTip,
            storeNameEditableTextField
        );
    }

    /// <summary>
    /// Handles the save clicked event for the store address editable text field.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void storeAddressEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleStoreAddressEditableTextFieldSaveClicked
        (
            storeAddressErrorMessageTeachingTip,
            storeAddressEditableTextField
        );
    }

    /// <summary>
    /// Handles the save clicked event for the store email editable text field.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void storeEmailEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleStoreEmailEditableTextFieldSaveClicked
        (
            storeEmailErrorMessageTeachingTip,
            storeEmailEditableTextField
        );
    }

    /// <summary>
    /// Handles the save clicked event for the store telephone editable text field.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void storeTelEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleStoreTelEditableTextFieldSaveClicked
        (
            storeTelErrorMessageTeachingTip,
            storeTelEditableTextField
        );
    }

    /// <summary>
    /// Handles the selected time changed event for the opening hour time picker.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void editOpeningHourTimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
    {
        ViewModel.HandleEditOpeningHourTimePickerSelectedTimeChanged(sender.Time, saveChangesOnStoreConfigurationButton);
    }

    /// <summary>
    /// Handles the selected time changed event for the closing hour time picker.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void editClosingHourTimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
    {
        ViewModel.HandleEditClosingHourTimePickerSelectedTimeChanged(sender.Time, saveChangesOnStoreConfigurationButton);
    }
}