using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

public sealed partial class SalaryConfigurationInitialSetupPage : Page
{
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACROSS ALL SETUP PAGES

    public SalaryConfigurationInitialSetupPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;

        if (ViewModel != null)
        {
            // Doing it like this to avoid stackoverflow cuz TwoWay binding is a double-edged sword

            // CheckBoxes' IsChecked values
            staffBaseSalaryCheckBox.IsChecked = ViewModel.StaffBaseSalaryCheckBoxChecked;
            staffHourlySalaryCheckBox.IsChecked = ViewModel.StaffHourlySalaryCheckBoxChecked;
            assistantManagerBaseSalaryCheckBox.IsChecked = ViewModel.AssistantManagerBaseSalaryCheckBoxChecked;
            assistantManagerHourlySalaryCheckBox.IsChecked = ViewModel.AssistantManagerHourlySalaryCheckBoxChecked;
            storeManagerBaseSalaryCheckBox.IsChecked = ViewModel.StoreManagerBaseSalaryCheckBoxChecked;
            storeManagerHourlySalaryCheckBox.IsChecked = ViewModel.StoreManagerHourlySalaryCheckBoxChecked;

            // TextBoxes' IsEnabled values
            staffBaseSalaryTextBox.IsEnabled = ViewModel.StaffBaseSalaryCheckBoxChecked;
            staffHourlySalaryTextBox.IsEnabled = ViewModel.StaffHourlySalaryCheckBoxChecked;
            assistantManagerBaseSalaryTextBox.IsEnabled = ViewModel.AssistantManagerBaseSalaryCheckBoxChecked;
            assistantManagerHourlySalaryTextBox.IsEnabled = ViewModel.AssistantManagerHourlySalaryCheckBoxChecked;
            storeManagerBaseSalaryTextBox.IsEnabled = ViewModel.StoreManagerBaseSalaryCheckBoxChecked;
            storeManagerHourlySalaryTextBox.IsEnabled = ViewModel.StoreManagerHourlySalaryCheckBoxChecked;
        }
    }

    private void staffBaseSalaryCheckBox_Changed(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.StaffBaseSalaryCheckBoxChecked = staffBaseSalaryCheckBox.IsChecked ?? false;
        staffBaseSalaryTextBox.IsEnabled = ViewModel.StaffBaseSalaryCheckBoxChecked;

        // Auto-fill '0' if the user checked the checkbox
        if (staffBaseSalaryTextBox.IsEnabled && ViewModel.StaffBaseSalaryString == "")
        {
            ViewModel.StaffBaseSalaryString = "0";
        }

        // If user unchecked the checkbox, clear the values
        if (!staffBaseSalaryTextBox.IsEnabled && ViewModel.StaffBaseSalaryString != "")
        {
            ViewModel.StaffBaseSalaryString = "";
        }
    }

    private void staffHourlySalaryCheckBox_Changed(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.StaffHourlySalaryCheckBoxChecked = staffHourlySalaryCheckBox.IsChecked ?? false;
        staffHourlySalaryTextBox.IsEnabled = ViewModel.StaffHourlySalaryCheckBoxChecked;

        // Auto-fill '0' if the user checked the checkbox
        if (staffHourlySalaryTextBox.IsEnabled && ViewModel.StaffHourlySalaryString == "")
        {
            ViewModel.StaffHourlySalaryString = "0";
        }

        // If user unchecked the checkbox, clear the values
        if (!staffHourlySalaryTextBox.IsEnabled && ViewModel.StaffHourlySalaryString != "")
        {
            ViewModel.StaffHourlySalaryString = "";
        }
    }

    private void assistantManagerBaseSalaryCheckBox_Changed(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.AssistantManagerBaseSalaryCheckBoxChecked = assistantManagerBaseSalaryCheckBox.IsChecked ?? false;
        assistantManagerBaseSalaryTextBox.IsEnabled = ViewModel.AssistantManagerBaseSalaryCheckBoxChecked;

        // Auto-fill '0' if the user checked the checkbox
        if (assistantManagerBaseSalaryTextBox.IsEnabled && ViewModel.AssistantManagerBaseSalaryString == "")
        {
            ViewModel.AssistantManagerBaseSalaryString = "0";
        }

        // If user unchecked the checkbox, clear the values
        if (!assistantManagerBaseSalaryTextBox.IsEnabled && ViewModel.AssistantManagerBaseSalaryString != "")
        {
            ViewModel.AssistantManagerBaseSalaryString = "";
        }
    }

    private void assistantManagerHourlySalaryCheckBox_Changed(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.AssistantManagerHourlySalaryCheckBoxChecked = assistantManagerHourlySalaryCheckBox.IsChecked ?? false;
        assistantManagerHourlySalaryTextBox.IsEnabled = ViewModel.AssistantManagerHourlySalaryCheckBoxChecked;

        // Auto-fill '0' if the user checked the checkbox
        if (assistantManagerHourlySalaryTextBox.IsEnabled && ViewModel.AssistantManagerHourlySalaryString == "")
        {
            ViewModel.AssistantManagerHourlySalaryString = "0";
        }

        // If user unchecked the checkbox, clear the values
        if (!assistantManagerHourlySalaryTextBox.IsEnabled && ViewModel.AssistantManagerHourlySalaryString != "")
        {
            ViewModel.AssistantManagerHourlySalaryString = "";
        }
    }

    private void storeManagerBaseSalaryCheckBox_Changed(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.StoreManagerBaseSalaryCheckBoxChecked = storeManagerBaseSalaryCheckBox.IsChecked ?? false;
        storeManagerBaseSalaryTextBox.IsEnabled = ViewModel.StoreManagerBaseSalaryCheckBoxChecked;

        // Auto-fill '0' if the user checked the checkbox
        if (storeManagerBaseSalaryTextBox.IsEnabled && ViewModel.StoreManagerBaseSalaryString == "")
        {
            ViewModel.StoreManagerBaseSalaryString = "0";
        }

        // If user unchecked the checkbox, clear the values
        if (!storeManagerBaseSalaryTextBox.IsEnabled && ViewModel.StoreManagerBaseSalaryString != "")
        {
            ViewModel.StoreManagerBaseSalaryString = "";
        }
    }

    private void storeManagerHourlySalaryCheckBox_Changed(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (ViewModel == null)
            return;

        ViewModel.StoreManagerHourlySalaryCheckBoxChecked = storeManagerHourlySalaryCheckBox.IsChecked ?? false;
        storeManagerHourlySalaryTextBox.IsEnabled = ViewModel.StoreManagerHourlySalaryCheckBoxChecked;

        // Auto-fill '0' if the user checked the checkbox
        if (storeManagerHourlySalaryTextBox.IsEnabled && ViewModel.StoreManagerHourlySalaryString == "")
        {
            ViewModel.StoreManagerHourlySalaryString = "0";
        }

        // If user unchecked the checkbox, clear the values
        if (!storeManagerHourlySalaryTextBox.IsEnabled && ViewModel.StoreManagerHourlySalaryString != "")
        {
            ViewModel.StoreManagerHourlySalaryString = "";
        }
    }

    private void salaryTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        // Ensured the contents entered in the salary text boxes
        // are rendered as numeric characters only, and apply
        // formatting if needed.
        if (sender.Text.Length == 0)
            return;

        var text = sender.Text;

        // Temporarily removes thousand separator ','
        text = text.Replace(",", "");

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

        // Padding Vietnamese thousand separator ','
        var originalLength = text.Length;
        var placementCursor = originalLength - 3;

        while (placementCursor > 0)
        {
            text = text.Insert(placementCursor, ",");
            placementCursor -= 3;
        }

        sender.Text = text;
        sender.SelectionStart = sender.Text.Length;
        //sender.SelectionStart = cursorPosition + (text.Length - originalLength);
    }
}