using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SipPOS.Models;
using SipPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SipPOS.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryManagementView : Page
    {
        public CategoryManagementViewModel ViewModel
        {
            get;
        }

        public CategoryManagementView()
        {
            ViewModel = App.GetService<CategoryManagementViewModel>();
            ViewModel.GetAll();
            Debug.WriteLine("CategoryManagementView");
            InitializeComponent();
        }


        public void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GetAll();
        }

        public async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            await AddCategoryDialog.ShowAsync();
        }

        public void ViewButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void PaginationControl_PageChanged(object sender, PaginationControlValueChangedEventArgs e)
        {

        }

        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            var category = (Category)checkBox.DataContext;

            if (!ViewModel.SelectedCategories.Contains(category))
            {
                ViewModel.SelectedCategories.Add(category);
            }
        }

        public void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            var category = (Category)checkBox.DataContext;

            if (ViewModel.SelectedCategories.Contains(category))
            {
                ViewModel.SelectedCategories.Remove(category);
            }
        }

        private void AddCategoryDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Get the input values from the dialog
            string CategoryName = DialogCategoryNameTextBox.Text;
            string CategoryStatus = ((ComboBoxItem)DialogCategoryStatusComboBox.SelectedItem).Content.ToString();

            if (string.IsNullOrEmpty(CategoryName))
            {
                return;
            }

            var newCategory = new Category
            {
                Id = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Name = CategoryName,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            };

            ViewModel.Insert(newCategory);

            DialogCategoryNameTextBox.Text = string.Empty;
            DialogCategoryStatusComboBox.SelectedIndex = 0;
        }

        private void AddCategoryDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            AddCategoryDialog.Hide();
        }

    }
}
