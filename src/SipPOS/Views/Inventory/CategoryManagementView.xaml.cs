using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.DataTransfer;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using CommunityToolkit.WinUI.UI.Controls;

using SipPOS.ViewModels.Inventory;
using SipPOS.Resources.Controls;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Views.Inventory;

/// <summary>
/// Represents the CategoryManagementView.
/// </summary>
public sealed partial class CategoryManagementView : Page
{
    /// <summary>
    /// Gets the view model for the CategoryManagementView.
    /// </summary>
    public CategoryManagementViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryManagementView"/> class.
    /// </summary>
    public CategoryManagementView()
    {
        ViewModel = App.GetService<CategoryManagementViewModel>();
        ViewModel.Search();
        InitializeComponent();
        SizeChanged += OnPageSizeChanged;
    }

    private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
    {
       ViewModel.TableHeight = (int)e.NewSize.Height - 400;
    }

    /// <summary>
    /// Handles the click event of the refresh button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Search();
    }

    /// <summary>
    /// Handles the click event of the add button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectedCategory = new CategoryDto();
        ViewModel.ImageUrls.Clear();
        Dialog.Title = "THÊM DANH MỤC";
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
        IList<CategoryDto> selectedCategories = ViewModel.Categories.Where(x => x.IsSeteled).ToList();
        if (selectedCategories.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một danh mục để xem.");
            return;
        }
        if (selectedCategories.Count > 1)
        {
            ShowNotification("Vui lòng chỉ chọn một danh mục để xem.");
            return;
        }
        ViewModel.SelectedCategory = selectedCategories[0];
        Dialog.Title = "XEM DANH MỤC";
        Dialog.IsPrimaryButtonEnabled = false;
        ViewModel.ActionType = "VIEW";
        ViewModel.ImageUrls.Clear();
        foreach (var item in ViewModel.SelectedCategory.ImageUrls)
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
        IList<CategoryDto> selectedCategories = ViewModel.Categories.Where(x => x.IsSeteled).ToList();
        if (selectedCategories.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một danh mục để chỉnh sửa.");
            return;
        }
        if (selectedCategories.Count > 1)
        {
            ShowNotification("Vui lòng chỉ chọn một danh mục để chỉnh sửa.");
            return;
        }
        ViewModel.SelectedCategory = selectedCategories[0];
        Dialog.Title = "CHỈNH SỬA DANH MỤC";
        Dialog.IsPrimaryButtonEnabled = true;
        ViewModel.ActionType = "EDIT";
        ViewModel.ImageUrls.Clear();
        foreach (var item in ViewModel.SelectedCategory.ImageUrls)
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
        IList<CategoryDto> selectedCategories = ViewModel.Categories.Where(x => x.IsSeteled).ToList();
        if (selectedCategories.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một danh mục để xóa.");
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
        ViewModel.Search();
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
        ViewModel.Search();
        foreach (var dgColumn in dg.Columns)
        {
            if (null != dgColumn.Tag && dgColumn.Tag.ToString() != e.Column.Tag.ToString())
            {
                dgColumn.SortDirection = null;
            }
        }
    }

    /// <summary>
    /// Handles the click event of the "Yes" button in the product dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void ProductDialog_YesClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (ViewModel.SelectedCategory == null)
        {
            args.Cancel = true;
            ShowNotification("Vui lòng chọn danh mục cần thêm sản phẩm.");
            return;
        }

        if (string.IsNullOrEmpty(ViewModel.SelectedCategory.Name))
        {
            args.Cancel = true;
            ShowNotification("Vui lòng nhập tên danh mục.");
            return;
        }
        switch (ViewModel.ActionType)
        {
            case "ADD":
                ViewModel.Insert();
                break;
            case "EDIT":
                ViewModel.UpdateById();
                break;
            default:
                break;
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
        if (ViewModel.SelectedCategory == null)
        {
            ShowNotification("Vui lòng chọn danh mục cần thêm ảnh.");
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
                ViewModel.SelectedCategory.ImageUrls.Add(file.Path);
                ViewModel.ImageUrls.Add(file.Path);
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
    private async void ContentGridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        // Handle item click

        // deletes when implemented this method
        var dialog = new ContentDialog
        {
            Title = "Not implemented",
            Content = $"Not implemented",
            CloseButtonText = "Ok"
        };

        await dialog.ShowAsync();
    }

    /// <summary>
    /// Handles the opened event of the dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void DialogOpen(ContentDialog sender, ContentDialogOpenedEventArgs args)
    {

    }
}
