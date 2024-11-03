using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SipPOS.Models;
using SipPOS.Services.Interfaces;

namespace SipPOS.ViewModels
{
    public partial class StoreSettingsViewModel : ObservableObject 
    {
        [ObservableProperty]
        private string storeName;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private string city;

        [ObservableProperty]
        private string phone;

        [ObservableProperty]
        private string branchPhone;

        [ObservableProperty]
        private string url;

        [ObservableProperty]
        private string note;

        [ObservableProperty]
        private string taxCode;

        [ObservableProperty]
        private string paymentMethods;

        [ObservableProperty]
        private string currency;

        [ObservableProperty]
        private double exchangeRate;

        [ObservableProperty]
        private string measurementUnits;

        [ObservableProperty]
        private bool highlightFeaturedProducts;

        [ObservableProperty]
        private bool highlightDiscountedProducts;

        [ObservableProperty]
        private double taxRate;

        [ObservableProperty]
        private double serviceFee;

        [ObservableProperty]
        private double shippingFee;

        [ObservableProperty]
        private TimeSpan openingTime;

        [ObservableProperty]
        private TimeSpan closingTime;

        [ObservableProperty]
        private bool allowDiscounts;

        [ObservableProperty]
        private string promotions;

        [ObservableProperty]
        private string coupons;

        [ObservableProperty]
        private int rewardPoints;

        [ObservableProperty]
        private string layout;

        [ObservableProperty]
        private string themeColor;

        [ObservableProperty]
        private string language;

        [ObservableProperty]
        private bool enableSalesReport;

        [ObservableProperty]
        private bool enableInventoryReport;

        [ObservableProperty]
        private bool enableMonthlyReport;

        [ObservableProperty]
        private string reportParameters;
 
        private readonly IStoreSettingsService _storeSettingsService;
        private StoreSettings _storeSettings;

        public StoreSettings StoreSettings
        {
            get => _storeSettings;
            set => SetProperty(ref _storeSettings, value);
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public StoreSettingsViewModel(IStoreSettingsService storeSettingsService)
        {
            _storeSettingsService = storeSettingsService;
            LoadSettingsAsync(); 
        }

        public async Task LoadSettingsAsync()
        {
            StoreSettings = await _storeSettingsService.GetStoreSettingsAsync();
        }

        public async Task SaveSettingsAsync()
        {
            try
            {
                bool isSaved = await _storeSettingsService.UpdateStoreSettingsAsync(StoreSettings);
                if (isSaved)
                {
                    StatusMessage = "Thông tin đã được lưu thành công";
                }
                else
                {
                    StatusMessage = "lỗi khi lưu.";
                }
            }
            catch (Exception ex)
            {
           
                StatusMessage = $"Error saving settings: {ex.Message}";
            }
        }

        // Optional: Logo file path
        public string LogoFilePath { get; set; }
    }
}


