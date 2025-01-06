using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.DataTransfer;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using CommunityToolkit.WinUI.UI.Controls;

using SipPOS.ViewModels.Inventory;
using SipPOS.Resources.Controls;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using System.Diagnostics;

namespace SipPOS.Views.Inventory;

/// <summary>
/// Represents the ProductManagementView.
/// </summary>
public sealed partial class ProductManagementView : Page
{
    /// <summary>
    /// Gets the view model for the product management view.
    /// </summary>
    public ProductManagementViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductManagementView"/> class.
    /// </summary>
    public ProductManagementView()
    {
        ViewModel = App.GetService<ProductManagementViewModel>();
        ViewModel.UpdateProductList();
        ViewModel.GetAllCategory();
        InitializeComponent();
        SizeChanged += OnPageSizeChanged;
    }

    private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
    {
        ViewModel.TableHeight = (int)e.NewSize.Height - 260;
    }

    /// <summary>
    /// Handles the click event of the refresh button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.UpdateProductList();
    }

    private void EmptyButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ProductFilterDto = new ProductFilterDto();
        ViewModel.UpdateProductList();
    }

    /// <summary>
    /// Handles the click event of the add button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        ProductDto newProductDto = new ProductDto();
        newProductDto.Status = "Available";
        ViewModel.SelectedProduct = newProductDto;
        ViewModel.ProductOptions.Clear();
        ViewModel.ProductOptions.Add(new()
        {
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
        });
        ViewModel.ImageUrls.Clear();
        Dialog.Title = "THÊM SẢN PHẨM";
        Dialog.IsPrimaryButtonEnabled = true;
        ViewModel.ActionType = "ADD";
        await Dialog.ShowAsync();
    }

    /// <summary>
    /// Shows a notification with the specified message.
    /// </summary>
    /// <param name="message">The message to display.</param>
    private void ShowNotification(string message)
    {
        InAppToast.Show("Thông báo", message, 5000);
    }

    /// <summary>
    /// Handles the click event of the view button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void ViewButton_Click(object sender, RoutedEventArgs e)
    {
        IList<ProductDto> selectedProducts = ViewModel.Products.Where(x => x.IsSelected).ToList();
        if (selectedProducts.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một sản phẩm để xem.");
            return;
        }
        if (selectedProducts.Count > 1)
        {
            ShowNotification("Vui lòng chỉ chọn một sản phẩm để xem.");
            return;
        }
        ViewModel.SelectedProduct = selectedProducts[0];
        ViewModel.ProductOptions.Clear();
        for (int i = 0; i < ViewModel.SelectedProduct.ProductOptions.Count; i++)
        {
            ViewModel.ProductOptions.Add(new ProductOptionDto()
            {
                Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Option = ViewModel.SelectedProduct.ProductOptions[i].option,
                Price = ViewModel.SelectedProduct.ProductOptions[i].price
            });
        }
        Dialog.Title = "XEM SẢN PHẨM";
        Dialog.IsPrimaryButtonEnabled = false;
        ViewModel.ActionType = "VIEW";
        ViewModel.ImageUrls.Clear();
        foreach (var item in ViewModel.SelectedProduct.ImageUris)
        {
            ViewModel.ImageUrls.Add(item);
        }
        await Dialog.ShowAsync();
    }

    /// <summary>
    /// Handles the click event of the edit button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        IList<ProductDto> selectedProducts = ViewModel.Products.Where(x => x.IsSelected).ToList();
        if (selectedProducts.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một sản phẩm để chỉnh sửa.");
            return;
        }
        if (selectedProducts.Count > 1)
        {
            ShowNotification("Vui lòng chỉ chọn một sản phẩm để chỉnh sửa.");
            return;
        }
        ViewModel.SelectedProduct = selectedProducts[0];
        ViewModel.ProductOptions.Clear();
        for (int i = 0; i < ViewModel.SelectedProduct.ProductOptions.Count; i++)
        {
            ViewModel.ProductOptions.Add(new ProductOptionDto()
            {
                Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Option = ViewModel.SelectedProduct.ProductOptions[i].option,
                Price = ViewModel.SelectedProduct.ProductOptions[i].price
            });
        }
        Dialog.Title = "CHỈNH SỬA SẢN PHẨM";
        Dialog.IsPrimaryButtonEnabled = true;
        ViewModel.ActionType = "EDIT";
        ViewModel.ImageUrls.Clear();
        foreach (var item in ViewModel.SelectedProduct.ImageUris)
        {
            ViewModel.ImageUrls.Add(item);
        }
        await Dialog.ShowAsync();
    }

    /// <summary>
    /// Handles the click event of the delete button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        IList<ProductDto> selectedProducts = ViewModel.Products.Where(x => x.IsSelected).ToList();
        if (selectedProducts.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một sản phẩm để xóa.");
            return;
        }
        await DeleteConfirmationDialog.ShowAsync();
    }

    /// <summary>
    /// Handles the click event of the "Yes" button in the delete confirmation dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    public void DeleteConfirmationDialog_YesClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        ViewModel.DeleteByIds();
    }

    /// <summary>
    /// Handles the click event of the "No" button in the delete confirmation dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    public void DeleteConfirmationDialog_NoClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        DeleteConfirmationDialog.Hide();
    }

    /// <summary>
    /// Handles the page changed event of the pagination control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void PaginationControl_PageChanged(object sender, PaginationControlValueChangedEventArgs e)
    {
        ViewModel.UpdateProductList();
    }

    /// <summary>
    /// Handles the sorting event of the data grid.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        ViewModel.SortDto.SortBy = e.Column.Tag.ToString();
        if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
        {
            ViewModel.SortDto.SortType = "ASC";
            e.Column.SortDirection = DataGridSortDirection.Ascending;
        }
        else
        {
            ViewModel.SortDto.SortType = "DESC";
            e.Column.SortDirection = DataGridSortDirection.Descending;
        }
        ViewModel.UpdateProductList();
        foreach (var dgColumn in dg.Columns)
        {
            if (null != dgColumn.Tag && dgColumn.Tag.ToString() != e.Column.Tag.ToString())
            {
                dgColumn.SortDirection = null;
            }
        }
    }

    /// <summary>
    /// Handles the click event of the "Yes" button in the dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private async void Dialog_YesClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (ViewModel.SelectedProduct == null)
        {
            return;
        }
        var isOk = false;
        switch (ViewModel.ActionType)
        {
            case "ADD":
                isOk = null != await ViewModel.Insert();
                break;
            case "EDIT":
                isOk = null != await ViewModel.UpdateById();
                break;
            default:
                break;
        }
        if (!isOk)
        {
            args.Cancel = true;
        }
    }

    /// <summary>
    /// Handles the click event of the "No" button in the dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void Dialog_NoButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Dialog.Hide();
    }

    /// <summary>
    /// Handles the click event of the add image button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void AddImageButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedProduct == null)
        {
            return;
        }

        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        var window = App.CurrentWindow;

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.ViewMode = PickerViewMode.List;
        openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        openPicker.FileTypeFilter.Add(".jpg");
        openPicker.FileTypeFilter.Add(".jpeg");
        openPicker.FileTypeFilter.Add(".png");

        IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
        if (files.Count > 0)
        {
            foreach (StorageFile file in files)
            {
                var localFolder = ApplicationData.Current.LocalFolder;

                // copy images to the local folder (make uri stable)
                var newFile = await file.CopyAsync(localFolder, file.Name, NameCollisionOption.ReplaceExisting);

                ViewModel.SelectedProduct.ImageUris.Add(newFile.Path);

                // if the image was using the default image before, remove it

                if (ViewModel.SelectedProduct.ImageUris.Count > 1 && ViewModel.SelectedProduct.ImageUris[0] == "ms-appx:///Assets/default_product_image.png")
                {
                    ViewModel.SelectedProduct.ImageUris.RemoveAt(0);
                }

                ViewModel.ImageUrls.Add(newFile.Path);

                if (ViewModel.ImageUrls.Count > 1 && ViewModel.ImageUrls[0] == "ms-appx:///Assets/default_product_image.png")
                {
                    ViewModel.ImageUrls.RemoveAt(0);
                }
            }
        }
        else
        {
            // The user didn't pick any files
        }
    }

    /// <summary>
    /// Handles the drag over event of the image list view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ImageListView_DragOver(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            e.AcceptedOperation = DataPackageOperation.Move; // Allow move operation
        }
        else
        {
            e.AcceptedOperation = DataPackageOperation.None; // Indicate that drop is not allowed
        }
    }

    /// <summary>
    /// Handles the closed event of the dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void DialogClose(object sender, ContentDialogClosedEventArgs e)
    {
        //reset input values
    }

    /// <summary>
    /// Handles the selection changed event of the content grid view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ContentGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Handle selection changes if needed
    }

    /// <summary>
    /// Handles the item click event of the content grid view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ContentGridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        // Handle item click

        // deletes when implemented this method
        //var dialog = new ContentDialog
        //{
        //    Title = "Not implemented",
        //    Content = $"Not implemented",
        //    CloseButtonText = "Ok"
        //};
        //
        //await dialog.ShowAsync();
    }

    /// <summary>
    /// Handles the opened event of the dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void DialogOpen(ContentDialog sender, ContentDialogOpenedEventArgs args)
    {

    }

    /// <summary>
    /// Handles the text changed event of the product name text box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ProductName_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.ProductNameRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the text changed event of the product description text box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ProductDesc_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.ProductDescRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the value changed event of the product price control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void ProductPrice_ValueChanged(object sender, object args)
    {
        ViewModel.ProductPriceRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the selection changed event of the product category combo box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ProductCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.ProductCategoryRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the click event of the delete price option button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void DeletePriceOption_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedProduct == null)
        {
            return;
        }
        if (ViewModel.ProductOptions.Count() == 1)
        {
            ViewModel.ProductPriceRequireMessage = "Phải có ít nhất một giá cho sản phẩm";
            return;
        }
        string? id = (sender as Button)?.Tag.ToString();
        if (id != null)
        {
            int index = -1;

            foreach (var item in ViewModel.ProductOptions)
            {
                if (item.Id.ToString() == id)
                {
                    index = ViewModel.ProductOptions.IndexOf(item);
                    break;
                }
            }
            if (index != -1)
            {
                ViewModel.ProductOptions.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// Handles the click event of the add price option button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void AddPriceOption_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedProduct == null)
        {
            return;
        }
        string? id = (sender as Button)?.Tag.ToString();
        if (id != null)
        {
            int index = -1;

            foreach (var item in ViewModel.ProductOptions)
            {
                if (item.Id.ToString() == id)
                {
                    index = ViewModel.ProductOptions.IndexOf(item);
                    break;
                }
            }
            if (index != -1)
            {
                ViewModel.ProductOptions.Insert(index + 1, new()
                {
                    Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                });
            }
        }
    }

}
