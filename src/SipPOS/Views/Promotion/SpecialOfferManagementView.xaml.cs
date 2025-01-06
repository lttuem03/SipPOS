using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Resources.Controls;
using SipPOS.ViewModels.Inventory;

namespace SipPOS.Views.Promotion;

/// <summary>
/// Represents the view for managing special offers.
/// </summary>
public sealed partial class SpecialOffersManagementView : Page
{
    /// <summary>
    /// Gets the ViewModel for the SpecialOffersManagementView.
    /// </summary>
    public SpecialOfferManagementViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecialOffersManagementView"/> class.
    /// </summary>
    public SpecialOffersManagementView()
    {
        ViewModel = App.GetService<SpecialOfferManagementViewModel>();
        ViewModel.UpdateSpecialOfferList();
        ViewModel.GetAllCategory();
        ViewModel.GetAllProduct();
        InitializeComponent();
        SizeChanged += OnPageSizeChanged;
    }

    /// <summary>
    /// Handles the SizeChanged event of the page.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
    {
        ViewModel.TableHeight = (int)e.NewSize.Height - 260;
    }

    /// <summary>
    /// Handles the click event of the Refresh button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.UpdateSpecialOfferList();
    }

    /// <summary>
    /// Handles the click event of the Empty button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void EmptyButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SpecialOffersFilterDto = new SpecialOfferFilterDto();
        ViewModel.UpdateSpecialOfferList();
    }

    /// <summary>
    /// Handles the click event of the Add button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        SpecialOfferDto newSpecialOfferDto = new SpecialOfferDto();
        newSpecialOfferDto.Status = "Inactive";
        newSpecialOfferDto.Type = "InvoicePromotion";
        newSpecialOfferDto.PriceType = "Original";
        newSpecialOfferDto.StartDate = DateTime.Now;
        newSpecialOfferDto.EndDate = DateTime.Now;
        ViewModel.SelectedSpecialOffer = newSpecialOfferDto;
        Dialog.Title = "THÊM KHUYẾN MÃI";
        Dialog.IsPrimaryButtonEnabled = true;
        ViewModel.ActionType = "ADD";
        await Dialog.ShowAsync();
    }

    /// <summary>
    /// Shows a notification with the specified message.
    /// </summary>
    /// <param name="message">The message to display in the notification.</param>
    private void ShowNotification(string message)
    {
        InAppToast.Show("Thông báo", message, 5000);
    }

    /// <summary>
    /// Handles the click event of the View button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void ViewButton_Click(object sender, RoutedEventArgs e)
    {
        IList<SpecialOfferDto> selectedSpecialOffers = ViewModel.SpecialOffers.Where(x => x.IsSelected).ToList();
        if (selectedSpecialOffers.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một khuyến mãi để xem.");
            return;
        }
        if (selectedSpecialOffers.Count > 1)
        {
            ShowNotification("Vui lòng chỉ chọn một khuyến mãi để xem.");
            return;
        }
        ViewModel.SelectedSpecialOffer = selectedSpecialOffers[0];
        UpdateConditionUI();
        Dialog.Title = "XEM KHUYẾN MÃI";
        Dialog.IsPrimaryButtonEnabled = false;
        ViewModel.ActionType = "VIEW";
        await Dialog.ShowAsync();
    }

    /// <summary>
    /// Handles the click event of the Edit button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        IList<SpecialOfferDto> selectedSpecialOffers = ViewModel.SpecialOffers.Where(x => x.IsSelected).ToList();
        if (selectedSpecialOffers.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một khuyến mãi để chỉnh sửa.");
            return;
        }
        if (selectedSpecialOffers.Count > 1)
        {
            ShowNotification("Vui lòng chỉ chọn một khuyến mãi để chỉnh sửa.");
            return;
        }
        ViewModel.SelectedSpecialOffer = selectedSpecialOffers[0];
        UpdateConditionUI();
        Dialog.Title = "CHỈNH SỬA KHUYẾN MẠI";
        Dialog.IsPrimaryButtonEnabled = true;
        ViewModel.ActionType = "EDIT";
        await Dialog.ShowAsync();
    }

    /// <summary>
    /// Handles the click event of the Delete button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        IList<SpecialOfferDto> selectedSpecialOffers = ViewModel.SpecialOffers.Where(x => x.IsSelected).ToList();
        if (selectedSpecialOffers.Count == 0)
        {
            ShowNotification("Vui lòng chọn ít nhất một khuyến mãi để xóa.");
            return;
        }
        await DeleteConfirmationDialog.ShowAsync();
    }

    /// <summary>
    /// Handles the Yes click event of the delete confirmation dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    public void DeleteConfirmationDialog_YesClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        ViewModel.DeleteByIds();
    }

    /// <summary>
    /// Handles the No click event of the delete confirmation dialog.
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
        ViewModel.UpdateSpecialOfferList();
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
        ViewModel.UpdateSpecialOfferList();
        foreach (var dgColumn in dg.Columns)
        {
            if (null != dgColumn.Tag && dgColumn.Tag.ToString() != e.Column.Tag.ToString())
            {
                dgColumn.SortDirection = null;
            }
        }
    }

    /// <summary>
    /// Handles the Yes click event of the dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private async void Dialog_YesClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (ViewModel.SelectedSpecialOffer == null)
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
    /// Handles the click event of the No button in the dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void Dialog_NoButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Dialog.Hide();
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

    /// <summary>
    /// Handles the text changed event of the special offer code text box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void SpecialOfferCode_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.SpecialOfferCodeRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the text changed event of the special offer name text box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void SpecialOfferName_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.SpecialOfferNameRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the date changed event of the special offer date picker.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void SpecialOfferDate_DateChanged(object sender, object args)
    {
        ViewModel.SpecialOfferStartDateRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the value changed event of the special offer max items control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void SpecialOfferMaxItems_ValueChanged(object sender, object args)
    {
        ViewModel.SpecialOfferMaxItemsRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the value changed event of the special offer price control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void SpecialOfferPrice_ValueChanged(object sender, object args)
    {
        ViewModel.SpecialOfferPriceRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the text changed event of the special offer description text box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void SpecialOfferDesc_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.SpecialOfferDescRequireMessage = string.Empty;
    }

    /// <summary>
    /// Handles the selection changed event of the price type combo box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void PriceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateConditionUI();
        ViewModel.SpecialOfferPriceRequireMessage = string.Empty;
    }

    /// <summary>
    /// Updates the condition UI based on the selected special offer.
    /// </summary>
    private void UpdateConditionUI()
    {
        if (ViewModel.SelectedSpecialOffer == null)
        {
            return;
        }
        if (ViewModel.SelectedSpecialOffer.PriceType == "Original")
        {
            ViewModel.DiscountPriceVisibility = Visibility.Visible;
            ViewModel.DiscountPecentageVisibility = Visibility.Collapsed;
        }
        else if (ViewModel.SelectedSpecialOffer.PriceType == "Percentage")
        {
            ViewModel.DiscountPriceVisibility = Visibility.Collapsed;
            ViewModel.DiscountPecentageVisibility = Visibility.Visible;
        }

        if (ViewModel.SelectedSpecialOffer == null)
        {
            return;
        }
        if (ViewModel.SelectedSpecialOffer.Type == "InvoicePromotion")
        {
            ViewModel.CategoryPromotionVisibility = Visibility.Collapsed;
            ViewModel.ProductPromotionVisibility = Visibility.Collapsed;
        }
        else if (ViewModel.SelectedSpecialOffer.Type == "CategoryPromotion")
        {
            ViewModel.CategoryPromotionVisibility = Visibility.Visible;
            ViewModel.ProductPromotionVisibility = Visibility.Collapsed;
        }
        else if (ViewModel.SelectedSpecialOffer.Type == "ProductPromotion")
        {
            ViewModel.CategoryPromotionVisibility = Visibility.Collapsed;
            ViewModel.ProductPromotionVisibility = Visibility.Visible;
        }
    }

    /// <summary>
    /// Handles the selection changed event of the special offer type combo box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateConditionUI();
        ViewModel.SpecialOfferTypeRequireMessage = string.Empty;
    }

}
