using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SipPOS.Models;
using SipPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SipPOS.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductView : Page
    {
        public ProductViewModel ViewModel
        {
            get;
        }

        public ProductView()
        {
            ViewModel = App.GetService<ProductViewModel>();
            ViewModel.Get();
            Debug.WriteLine("ProductView");
            InitializeComponent();
        }

        public void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Get();
        }

        public async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            await AddProductDialog.ShowAsync();
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

            var product = (Product)checkBox.DataContext;

            if (!ViewModel.SelectedProducts.Contains(product))
            {
                ViewModel.SelectedProducts.Add(product);
            }
        }

        public void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            var product = (Product)checkBox.DataContext;

            if (ViewModel.SelectedProducts.Contains(product))
            {
                ViewModel.SelectedProducts.Remove(product);
            }
        }

        private void AddProductDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Get the input values from the dialog
            string productName = DialogProductNameTextBox.Text;
            int productCategory = DialogProductCategoryComboBox.SelectedIndex; // Fixed categories
            string productStatus = ((ComboBoxItem)DialogProductStatusComboBox.SelectedItem).Content.ToString();

            if (string.IsNullOrEmpty(productName) || productCategory <= 0)
            {
                return;
            }

            var newProduct = new Product
            {
                Id = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Name = productName,
                CategoryId = productCategory,
                Status = productStatus,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            };

            ViewModel.Add(newProduct);

            DialogProductNameTextBox.Text = string.Empty;
            DialogProductCategoryComboBox.SelectedIndex = -1;
            DialogProductStatusComboBox.SelectedIndex = 0;
        }

        private void AddProductDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            AddProductDialog.Hide();
        }

        private List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Status = "Hoạt động", CreatedAt = DateTime.Now, CreatedBy = "Admin" },
                new Product { Id = 2, Name = "Product B", Status = "Không hoạt động", CreatedAt = DateTime.Now, CreatedBy = "User" }
            };
        }
    }
}
