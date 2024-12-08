using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Staff;

namespace SipPOS.Views.Staff;

public sealed partial class StaffManagementView : Page
{
    public StaffManagementViewModel ViewModel { get; }

    public StaffManagementView()
    {
        this.InitializeComponent();
        ViewModel = new StaffManagementViewModel();
    }

    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleGoBackButtonClick();
    }

    private void rowsPerPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandleRowsPerPageComboBoxSelectionChanged(rowsPerPageComboBox.SelectedIndex);
    }

    private void registerNewStaffButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleRegisterNewStaffButtonClick();
    }
}