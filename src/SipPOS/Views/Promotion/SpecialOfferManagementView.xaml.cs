using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Resources.Controls;
using SipPOS.ViewModels.Inventory;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SipPOS.Views.Promotion
{
    public sealed partial class SpecialOffersManagementView : Page
    {

        public SpecialOfferManagementViewModel ViewModel { get; }

        public SpecialOffersManagementView()
        {
            ViewModel = App.GetService<SpecialOfferManagementViewModel>();
            ViewModel.UpdateSpecialOfferList();
            ViewModel.GetAllCategory();
            ViewModel.GetAllProduct();
            InitializeComponent();
            SizeChanged += OnPageSizeChanged;
        }

        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.TableHeight = (int)e.NewSize.Height - 260;
        }

        public void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateSpecialOfferList();
        }

        private void EmptyButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SpecialOffersFilterDto = new SpecialOfferFilterDto();
            ViewModel.UpdateSpecialOfferList();
        }

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

        private void ShowNotification(string message)
        {
            InAppToast.Show("Thông báo", message, 5000);
        }

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
            ViewModel.UpdateSpecialOfferList();
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
            ViewModel.UpdateSpecialOfferList();
            foreach (var dgColumn in dg.Columns)
            {
                if (null != dgColumn.Tag && dgColumn.Tag.ToString() != e.Column.Tag.ToString())
                {
                    dgColumn.SortDirection = null;
                }
            }
        }

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

        private void Dialog_NoButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Dialog.Hide();
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

        /// <summary>
        /// Handles the opened event of the dialog.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event data.</param>
        private void DialogOpen(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {

        }
        private void SpecialOfferCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SpecialOfferCodeRequireMessage = string.Empty;
        }

        private void SpecialOfferName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SpecialOfferNameRequireMessage = string.Empty;
        }

        private void SpecialOfferDate_DateChanged(object sender, object args)
        {
            ViewModel.SpecialOfferStartDateRequireMessage = string.Empty;
        }

        private void SpecialOfferMaxItems_ValueChanged(object sender, object args)
        {
            ViewModel.SpecialOfferMaxItemsRequireMessage = string.Empty;
        }

        private void SpecialOfferPrice_ValueChanged(object sender, object args)
        {
            ViewModel.SpecialOfferPriceRequireMessage = string.Empty;
        }

        private void SpecialOfferDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SpecialOfferDescRequireMessage = string.Empty;
        }


        public void PriceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateConditionUI();
            ViewModel.SpecialOfferPriceRequireMessage = string.Empty;
        }

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

        public void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateConditionUI();
            ViewModel.SpecialOfferTypeRequireMessage = string.Empty;
        }

    }
}
