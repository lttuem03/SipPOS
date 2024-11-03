using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;
using Windows.Storage;
using SipPOS.ViewModels;
using SipPOS.Services;
using SipPOS.Services.Interfaces;

namespace SipPOS.Views
{
    public partial class StoreSettingsView : Page
    {
        public StoreSettingsView()
        {
            this.InitializeComponent();
            InitializeViewModelAsync();
        }

        private async void InitializeViewModelAsync()
        {
            var viewModel = new StoreSettingsViewModel(new StoreSettingsService());
            this.DataContext = viewModel;
            await viewModel.LoadSettingsAsync(); 
        }

        private async void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "Xác nhận",
                Content = "Bạn có chắc chắn muốn lưu không?",
                PrimaryButtonText = "Có",
                CloseButtonText = "Không",
                XamlRoot = this.XamlRoot
            };

            var result = await confirmDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var viewModel = (StoreSettingsViewModel)this.DataContext;

               
                try
                {
                    await viewModel.SaveSettingsAsync();
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Thông báo",
                        Content = viewModel.StatusMessage,
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await dialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = $"Đã xảy ra lỗi: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                }
            }
        }

        private async void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker();
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var viewModel = (StoreSettingsViewModel)this.DataContext;
                viewModel.LogoFilePath = file.Path;
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Thông báo",
                    Content = "Không có tệp nào được chọn.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // back home or previous page
        }
    }
}
