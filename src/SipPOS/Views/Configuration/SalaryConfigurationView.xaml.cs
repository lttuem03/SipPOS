using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.DataTransfer.General;
using SipPOS.ViewModels.Configuration;
using SipPOS.Resources.Helper;

namespace SipPOS.Views.Configuration;

public sealed partial class SalaryConfigurationView : Page
{
    public SalaryConfigurationViewModel ViewModel { get; }

    public SalaryConfigurationView()
    {
        this.InitializeComponent();

        ViewModel = new SalaryConfigurationViewModel();

        if (ViewModel.CurrentConfiguration == null)
            return;

        var currentConfiguration = ViewModel.CurrentConfiguration;

        // Fills the current-cycle salary configuration
        currentStaffBaseSalaryTextBlock.Text = 
            currentConfiguration.CurrentStaffBaseSalary.ToVietnamDongFormatString();
        currentStaffHourlySalaryTextBlock.Text = 
            currentConfiguration.CurrentStaffHourlySalary.ToVietnamDongFormatString();
        currentAssistantManagerBaseSalaryTextBlock.Text = 
            currentConfiguration.CurrentAssistantManagerBaseSalary.ToVietnamDongFormatString();
        currentAssistantManagerHourlySalaryTextBlock.Text = 
            currentConfiguration.CurrentAssistantManagerHourlySalary.ToVietnamDongFormatString();
        currentStoreManagerBaseSalaryTextBlock.Text = 
            currentConfiguration.CurrentStoreManagerBaseSalary.ToVietnamDongFormatString();
        currentStoreManagerHourlySalaryTextBlock.Text = 
            currentConfiguration.CurrentStoreManagerHourlySalary.ToVietnamDongFormatString();

        // Fills the next-cycle salary configuration
        nextStaffBaseSalaryTextBox.Text = $"{currentConfiguration.NextStaffBaseSalary:0N}";
        nextStaffHourlySalaryTextBox.Text = $"{currentConfiguration.NextStaffHourlySalary:0N}";
        nextAssistantManagerBaseSalaryTextBox.Text = $"{currentConfiguration.NextAssistantManagerBaseSalary:0N}";
        nextAssistantManagerHourlySalaryTextBox.Text = $"{currentConfiguration.NextAssistantManagerHourlySalary:0N}";
        nextStoreManagerBaseSalaryTextBox.Text = $"{currentConfiguration.NextStoreManagerBaseSalary:0N}";
        nextStoreManagerHourlySalaryTextBox.Text = $"{currentConfiguration.NextStoreManagerHourlySalary:0N}";
    }

    private void saveChangesOnSalaryConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSaveChangesOnSalaryConfigurationButtonClick
        (
            editSalaryConfigurationResultContentDialog,
            saveChangesOnSalaryConfigurationButton
        );

        // This is the sign that update was successful
        if (ViewModel.EditSalaryConfigurationDto.NextStaffBaseSalary == -1m)
        {
            // Reload contents of the salary TextBoxes
            if (ViewModel.CurrentConfiguration == null) // oopsies
                return;

            var currentConfiguration = ViewModel.CurrentConfiguration;

            nextStaffBaseSalaryTextBox.Text = $"{currentConfiguration.NextStaffBaseSalary:0N}";
            nextStaffHourlySalaryTextBox.Text = $"{currentConfiguration.NextStaffHourlySalary:0N}";
            nextAssistantManagerBaseSalaryTextBox.Text = $"{currentConfiguration.NextAssistantManagerBaseSalary:0N}";
            nextAssistantManagerHourlySalaryTextBox.Text = $"{currentConfiguration.NextAssistantManagerHourlySalary:0N}";
            nextStoreManagerBaseSalaryTextBox.Text = $"{currentConfiguration.NextStoreManagerBaseSalary:0N}";
            nextStoreManagerHourlySalaryTextBox.Text = $"{currentConfiguration.NextStoreManagerHourlySalary:0N}";
        }

        // Un-show warning that there are unsaved changes
        unsavedChangesWarningTextBlock.Visibility = Visibility.Collapsed;
    }

    private void cancelChangesOnSalaryConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.CurrentConfiguration == null)
            return;

        // Re-fills the next-cycle salary configuration to the original values
        nextStaffBaseSalaryTextBox.Text = $"{ViewModel.CurrentConfiguration.NextStaffBaseSalary:0N}";
        nextStaffHourlySalaryTextBox.Text = $"{ViewModel.CurrentConfiguration.NextStaffHourlySalary:0N}";
        nextAssistantManagerBaseSalaryTextBox.Text = $"{ViewModel.CurrentConfiguration.NextAssistantManagerBaseSalary:0N}";
        nextAssistantManagerHourlySalaryTextBox.Text = $"{ViewModel.CurrentConfiguration.NextAssistantManagerHourlySalary:0N}";
        nextStoreManagerBaseSalaryTextBox.Text = $"{ViewModel.CurrentConfiguration.NextStoreManagerBaseSalary:0N}";
        nextStoreManagerHourlySalaryTextBox.Text = $"{ViewModel.CurrentConfiguration.NextStoreManagerHourlySalary:0N}";

        // Reset the unsaved changes configuration DTO
        ViewModel.HandleCancelChangesOnSalaryConfigurationButtonClick();

        // Un-show warning that there are unsaved changes (if it was visible)
        unsavedChangesWarningTextBlock.Visibility = Visibility.Collapsed;

        // Disable all text boxes
        disableSalaryTextBoxes();

        // Reset buttons
        enableEditButton.Visibility = Visibility.Visible;
        saveChangesButton.Visibility = Visibility.Collapsed;
        cancelChangesButton.Visibility = Visibility.Collapsed;

        // Disable the "Save Changes" (on salary configuration) button
        saveChangesOnSalaryConfigurationButton.IsEnabled = false;
    }

    private void enableEditButton_Click(object sender, RoutedEventArgs e)
    {
        // Enables all text boxes
        enableSalaryTextBoxes();

        enableEditButton.Visibility = Visibility.Collapsed;
        saveChangesButton.Visibility = Visibility.Visible;
        cancelChangesButton.Visibility = Visibility.Visible;
    }

    private void saveChangesButton_Click(object sender, RoutedEventArgs e)
    {
        // Make sure no field is empty, if somehow the user tries to do so
        checkForEmptySalaryTextBoxes();

         var unsavedChangesConfigurationDto = new ConfigurationDto()
        {
            NextStaffBaseSalary = decimal.Parse(nextStaffBaseSalaryTextBox.Text.Replace(".", "")),
            NextStaffHourlySalary = decimal.Parse(nextStaffHourlySalaryTextBox.Text.Replace(".", "")),
            NextAssistantManagerBaseSalary = decimal.Parse(nextAssistantManagerBaseSalaryTextBox.Text.Replace(".", "")),
            NextAssistantManagerHourlySalary = decimal.Parse(nextAssistantManagerHourlySalaryTextBox.Text.Replace(".", "")),
            NextStoreManagerBaseSalary = decimal.Parse(nextStoreManagerBaseSalaryTextBox.Text.Replace(".", "")),
            NextStoreManagerHourlySalary = decimal.Parse(nextStoreManagerHourlySalaryTextBox.Text.Replace(".", ""))
        };

        // Disable all text boxes
        disableSalaryTextBoxes();

        ViewModel.HandleSaveChangesButtonClick
        (
            unsavedChangesConfigurationDto,
            unsavedChangesWarningTextBlock,
            saveChangesOnSalaryConfigurationButton
        );

        enableEditButton.Visibility = Visibility.Visible;
        saveChangesButton.Visibility = Visibility.Collapsed;
        cancelChangesButton.Visibility = Visibility.Collapsed;
    }

    private void cancelChangesButton_Click(object sender, RoutedEventArgs e)
    {
        // Make sure no field is empty, if somehow the user tries to do so
        checkForEmptySalaryTextBoxes();

        // Disable all text boxes
        disableSalaryTextBoxes();

        enableEditButton.Visibility = Visibility.Visible;
        saveChangesButton.Visibility = Visibility.Collapsed;
        cancelChangesButton.Visibility = Visibility.Collapsed;
    }

    private void salaryTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs e)
    {
        // Ensured the contents entered in the salary text boxes
        // are rendered as numeric characters only, and apply
        // formatting if needed.
        if (sender.Text.Length == 0)
            return;

        var text = sender.Text;

        // Temporarily removes thousand separator '.'
        text = text.Replace(".", "");

        // Ensuring every "last" character entered must be a digit
        var lastChar = text[^1];

        if (lastChar < '0' || lastChar > '9')
        {
            text = text.Remove(text.Length - 1);
        }

        // Ensuring '0' is not the beginning of the text.
        // If we try to enter digits but the first character(s)
        // are zero(s), then we skip the zero(s).
        if (text.Length > 1 && text.StartsWith('0'))
        {
            text = text.Remove(0, 1);
        }

        // Padding Vietnamese thousand separator '.'
        var originalLength = text.Length;
        var placementCursor = originalLength - 3;

        while (placementCursor > 0)
        {
            text = text.Insert(placementCursor, ".");
            placementCursor -= 3;
        }

        sender.Text = text;
        sender.SelectionStart = sender.Text.Length;
    }

    private void salaryTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.SelectAll();
        }
    }

    private void enableSalaryTextBoxes()
    {
        nextStaffBaseSalaryTextBox.IsEnabled = true;
        nextStaffHourlySalaryTextBox.IsEnabled = true;
        nextAssistantManagerBaseSalaryTextBox.IsEnabled = true;
        nextAssistantManagerHourlySalaryTextBox.IsEnabled = true;
        nextStoreManagerBaseSalaryTextBox.IsEnabled = true;
        nextStoreManagerHourlySalaryTextBox.IsEnabled = true;
    }

    private void disableSalaryTextBoxes()
    {
        nextStaffBaseSalaryTextBox.IsEnabled = false;
        nextStaffHourlySalaryTextBox.IsEnabled = false;
        nextAssistantManagerBaseSalaryTextBox.IsEnabled = false;
        nextAssistantManagerHourlySalaryTextBox.IsEnabled = false;
        nextStoreManagerBaseSalaryTextBox.IsEnabled = false;
        nextStoreManagerHourlySalaryTextBox.IsEnabled = false;
    }

    private void checkForEmptySalaryTextBoxes()
    {
        if (ViewModel.CurrentConfiguration == null)
            return;

        var currentConfiguration = ViewModel.CurrentConfiguration;

        // PS: I'm sorry for the mess, but it must be done this way

        // Basically, if there were "unsaved changes" (i.e. one or more  
        // properties in EditSalaryConfigurationDto is not -1m, which means
        // it was changed before), then we must revert the changes to those
        // unsaved changes, otherwise set the value to the current configuration

        if (string.IsNullOrEmpty(nextStaffBaseSalaryTextBox.Text))
        {
            nextStaffBaseSalaryTextBox.Text = (ViewModel.EditSalaryConfigurationDto.NextStaffBaseSalary == -1m) ?
                $"{currentConfiguration.NextStaffBaseSalary:0N}" : $"{ViewModel.EditSalaryConfigurationDto.NextStaffBaseSalary:0N}";
        }

        if (string.IsNullOrEmpty(nextStaffHourlySalaryTextBox.Text))
        {
            nextStaffHourlySalaryTextBox.Text = (ViewModel.EditSalaryConfigurationDto.NextStaffHourlySalary == -1m) ?
                $"{currentConfiguration.NextStaffHourlySalary:0N}" : $"{ViewModel.EditSalaryConfigurationDto.NextStaffHourlySalary:0N}";
        }

        if (string.IsNullOrEmpty(nextAssistantManagerBaseSalaryTextBox.Text))
        {
            nextAssistantManagerBaseSalaryTextBox.Text = (ViewModel.EditSalaryConfigurationDto.NextAssistantManagerBaseSalary == -1m) ?
                $"{currentConfiguration.NextAssistantManagerBaseSalary:0N}" : $"{ViewModel.EditSalaryConfigurationDto.NextAssistantManagerBaseSalary:0N}";
        }

        if (string.IsNullOrEmpty(nextAssistantManagerHourlySalaryTextBox.Text))
        {
            nextAssistantManagerHourlySalaryTextBox.Text = (ViewModel.EditSalaryConfigurationDto.NextAssistantManagerHourlySalary == -1m) ?
                $"{currentConfiguration.NextAssistantManagerHourlySalary:0N}" : $"{ViewModel.EditSalaryConfigurationDto.NextAssistantManagerHourlySalary:0N}";
        }

        if (string.IsNullOrEmpty(nextStoreManagerBaseSalaryTextBox.Text))
        {
            nextStoreManagerBaseSalaryTextBox.Text = (ViewModel.EditSalaryConfigurationDto.NextStoreManagerBaseSalary == -1m) ?
                $"{currentConfiguration.NextStoreManagerBaseSalary:0N}" : $"{ViewModel.EditSalaryConfigurationDto.NextStoreManagerBaseSalary:0N}";
        }

        if (string.IsNullOrEmpty(nextStoreManagerHourlySalaryTextBox.Text))
        {
            nextStoreManagerHourlySalaryTextBox.Text = (ViewModel.EditSalaryConfigurationDto.NextStoreManagerHourlySalary == -1m) ?
                $"{currentConfiguration.NextStoreManagerHourlySalary:0N}" : $"{ViewModel.EditSalaryConfigurationDto.NextStoreManagerHourlySalary:0N}";
        }
    }
}