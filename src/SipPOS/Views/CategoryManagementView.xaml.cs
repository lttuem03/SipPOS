using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using CommunityToolkit.WinUI.UI.Controls;

using SipPOS.DataTransfer;
using SipPOS.Models;
using SipPOS.ViewModels;
using SipPOS.Controls;

namespace SipPOS.Views;

public sealed partial class CategoryManagementView : Page
{
    public CategoryManagementViewModel ViewModel
    {
        get;
    }

    public CategoryManagementView()
    {
        ViewModel = App.GetService<CategoryManagementViewModel>();
        ViewModel.Search();
        InitializeComponent();
    }

    public void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Search();
    }

    public async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectedCategory = new CategoryDto();
        ViewModel.ImageUrls.Clear();
        Dialog.Title = "THÊM DANH MỤC";
        Dialog.IsPrimaryButtonEnabled = true;
        ViewModel.ActionType = "ADD";
        await Dialog.ShowAsync();
    }

    private void ShowNotification(string message)
    {
        InAppToast.Show("Thông báo", message, 5000);
    }

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

    public void DeleteConfirmationDialog_YesClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        ViewModel.DeleteByIds();
    }

    public void DeleteConfirmationDialog_NoClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        DeleteConfirmationDialog.Hide();
    }

    public void PaginationControl_PageChanged(object sender, PaginationControlValueChangedEventArgs e)
    {
        ViewModel.Search();
    }

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

    private void Dialog_NoButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Dialog.Hide();
    }

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


    private void DialogClose(object sender, ContentDialogClosedEventArgs e)
    {
        //reset input values
    }

    private void ContentGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Handle selection changes if needed
    }

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

    private void DialogOpen(ContentDialog sender, ContentDialogOpenedEventArgs args)
    {

    }
}
