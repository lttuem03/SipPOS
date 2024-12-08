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

    

    private void sortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandleSortByComboBoxSelectionChanged(sortByComboBox.SelectedIndex);
    }

    private void sortDirectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandleSortDirectionComboBoxSelectionChanged(sortDirectionComboBox.SelectedIndex);
    }

    private void positionCheckBoxes_Changed(object sender, RoutedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandlePositionCheckBoxesChanged(smPositionCheckBox.IsChecked,
                                                  amPositionCheckBox.IsChecked,
                                                  stPositionCheckBox.IsChecked);
    }

    private void previousPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandlePreviousPageButtonClick();
    }

    private void nextPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel is not StaffManagementViewModel)
            return;

        ViewModel.HandleNextPageButtonClick();
    }
}