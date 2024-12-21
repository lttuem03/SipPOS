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

    private void saveChangesOnStoreConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSaveChangesOnStoreConfigurationButtonClick();
    }

    private void cancelChangesOnStoreConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleCancelChangesOnStoreConfigurationButtonClick(saveChangesOnStoreConfigurationButton);

        storeNameEditableTextField.ResetState();
        storeAddressEditableTextField.ResetState();
        storeEmailEditableTextField.ResetState();
        storeTelEditableTextField.ResetState();
    }

    private void editableTextField_TextModified(object sender, EventArgs e)
    {
        ViewModel.HandleEditableTextFieldTextModified(saveChangesOnStoreConfigurationButton);
    }

    private void storeNameEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleStoreNameEditableTextFieldSaveClicked
        (
            storeNameErrorMessageTeachingTip,
            storeNameEditableTextField
        );
    }

    private void storeAddressEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleStoreAddressEditableTextFieldSaveClicked
        (
            storeAddressErrorMessageTeachingTip,
            storeAddressEditableTextField
        );
    }

    private void storeEmailEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleStoreEmailEditableTextFieldSaveClicked
        (
            storeEmailErrorMessageTeachingTip,
            storeEmailEditableTextField
        );
    }

    private void storeTelEditableTextField_SaveClicked(object sender, EventArgs e)
    {
        ViewModel.HandleStoreTelEditableTextFieldSaveClicked
        (
            storeTelErrorMessageTeachingTip,
            storeTelEditableTextField
        );
    }

    private void editOpeningHourTimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
    {
        //ViewModel.HandleEditOpeningHourTimePickerSelectedTimeChanged()
    }

    private void editClosingHourTimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
    {

    }
}
