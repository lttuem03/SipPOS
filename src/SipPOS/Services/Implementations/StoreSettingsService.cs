using Newtonsoft.Json;
using SipPOS.Models;
using SipPOS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace SipPOS.Services
{
    public class StoreSettingsService : IStoreSettingsService
    {
        private StoreSettings _storeSettings;

        public StoreSettingsService()
        {
            // Initialize StoreSettings with default values
            _storeSettings = new StoreSettings
            {
                StoreName = "Default Store",
                Address = "123 Default St.",
                City = "Default City",
                Phone = "123-456-7890",
                BranchPhone = "098-765-4321",
                Url = "http://defaultstore.com",
                Note = "Default Note",
                LogoUrl = "http://defaultstore.com/logo.png",
                TaxCode = "123456789",
                PaymentMethods = new List<string> { "Cash", "Credit Card" },
                Currency = "USD",
                ExchangeRate = 1.0m,
                MeasurementUnits = new List<string> { "kg", "g", "l" },
                HighlightFeaturedProducts = true,
                HighlightDiscountedProducts = false,
                TaxRate = 0.1m,
                ServiceFee = 5.0m,
                ShippingFee = 10.0m,
                OpeningTime = new TimeSpan(9, 0, 0), 
                ClosingTime = new TimeSpan(21, 0, 0), 
                AllowDiscounts = true,
                Promotions = new List<string>(),
                Coupons = new List<string>(),
                RewardPoints = new List<string>(),
                Layout = "Default Layout",
                ThemeColor = "Blue",
                Language = "en-US",
                EnableSalesReport = true,
                EnableInventoryReport = true,
                EnableMonthlyReport = true,
                ReportParameters = new List<string>()
            };
        }

        public async Task<StoreSettings> GetStoreSettingsAsync()
        {
          
            return await Task.FromResult(_storeSettings);
        }

        public async Task<bool> UpdateStoreSettingsAsync(StoreSettings settings)
        {
            // Simulate updating store configuration
            _storeSettings = settings;
            await SaveStoreSettingsAsync();
            return true; 
        }

        public async Task SaveStoreSettingsAsync()
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // Serialize StoreSettings to JSON
            var json = JsonConvert.SerializeObject(_storeSettings);

            // Save to LocalSettings
            localSettings.Values["StoreSettings"] = json;
        }

        public async Task LoadStoreSettingsAsync()
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // Check if StoreSettings exists in LocalSettings
            if (localSettings.Values.ContainsKey("StoreSettings"))
            {
                // Get value from LocalSettings and deserialize back to StoreSettings object
                var json = localSettings.Values["StoreSettings"] as string;
                _storeSettings = JsonConvert.DeserializeObject<StoreSettings>(json);
            }
        }

        // Implementations for individual property accessors
        public string GetStoreName() => _storeSettings.StoreName;
        public void SetStoreName(string value) => _storeSettings.StoreName = value;

        public string GetAddress() => _storeSettings.Address;
        public void SetAddress(string value) => _storeSettings.Address = value;

        public string GetCity() => _storeSettings.City;
        public void SetCity(string value) => _storeSettings.City = value;

        public string GetPhone() => _storeSettings.Phone;
        public void SetPhone(string value) => _storeSettings.Phone = value;

        public string GetBranchPhone() => _storeSettings.BranchPhone;
        public void SetBranchPhone(string value) => _storeSettings.BranchPhone = value;

        public string GetUrl() => _storeSettings.Url;
        public void SetUrl(string value) => _storeSettings.Url = value;

        public string GetNote() => _storeSettings.Note;
        public void SetNote(string value) => _storeSettings.Note = value;

        public string GetLogoUrl() => _storeSettings.LogoUrl;
        public void SetLogoUrl(string value) => _storeSettings.LogoUrl = value;

        public string GetTaxCode() => _storeSettings.TaxCode;
        public void SetTaxCode(string value) => _storeSettings.TaxCode = value;

        public List<string> GetPaymentMethods() => _storeSettings.PaymentMethods;
        public void SetPaymentMethods(List<string> value) => _storeSettings.PaymentMethods = value;

        public string GetCurrency() => _storeSettings.Currency;
        public void SetCurrency(string value) => _storeSettings.Currency = value;

        public decimal GetExchangeRate() => _storeSettings.ExchangeRate;
        public void SetExchangeRate(decimal value) => _storeSettings.ExchangeRate = value;

        public List<string> GetMeasurementUnits() => _storeSettings.MeasurementUnits;
        public void SetMeasurementUnits(List<string> value) => _storeSettings.MeasurementUnits = value;

        public bool GetHighlightFeaturedProducts() => _storeSettings.HighlightFeaturedProducts;
        public void SetHighlightFeaturedProducts(bool value) => _storeSettings.HighlightFeaturedProducts = value;

        public bool GetHighlightDiscountedProducts() => _storeSettings.HighlightDiscountedProducts;
        public void SetHighlightDiscountedProducts(bool value) => _storeSettings.HighlightDiscountedProducts = value;

        public decimal GetTaxRate() => _storeSettings.TaxRate;
        public void SetTaxRate(decimal value) => _storeSettings.TaxRate = value;

        public decimal GetServiceFee() => _storeSettings.ServiceFee;
        public void SetServiceFee(decimal value) => _storeSettings.ServiceFee = value;

        public decimal GetShippingFee() => _storeSettings.ShippingFee;
        public void SetShippingFee(decimal value) => _storeSettings.ShippingFee = value;

        public TimeSpan GetOpeningTime() => _storeSettings.OpeningTime;
        public void SetOpeningTime(TimeSpan value) => _storeSettings.OpeningTime = value;

        public TimeSpan GetClosingTime() => _storeSettings.ClosingTime;
        public void SetClosingTime(TimeSpan value) => _storeSettings.ClosingTime = value;

        public bool GetAllowDiscounts() => _storeSettings.AllowDiscounts;
        public void SetAllowDiscounts(bool value) => _storeSettings.AllowDiscounts = value;

        public List<string> GetPromotions() => _storeSettings.Promotions;
        public void SetPromotions(List<string> value) => _storeSettings.Promotions = value;

        public List<string> GetCoupons() => _storeSettings.Coupons;
        public void SetCoupons(List<string> value) => _storeSettings.Coupons = value;

        public List<string> GetRewardPoints() => _storeSettings.RewardPoints;
        public void SetRewardPoints(List<string> value) => _storeSettings.RewardPoints = value;

        public string GetLayout() => _storeSettings.Layout;
        public void SetLayout(string value) => _storeSettings.Layout = value;

        public string GetThemeColor() => _storeSettings.ThemeColor;
        public void SetThemeColor(string value) => _storeSettings.ThemeColor = value;

        public string GetLanguage() => _storeSettings.Language;
        public void SetLanguage(string value) => _storeSettings.Language = value;

        public bool GetEnableSalesReport() => _storeSettings.EnableSalesReport;
        public void SetEnableSalesReport(bool value) => _storeSettings.EnableSalesReport = value;

        public bool GetEnableInventoryReport() => _storeSettings.EnableInventoryReport;
        public void SetEnableInventoryReport(bool value) => _storeSettings.EnableInventoryReport = value;

        public bool GetEnableMonthlyReport() => _storeSettings.EnableMonthlyReport;
        public void SetEnableMonthlyReport(bool value) => _storeSettings.EnableMonthlyReport = value;

        public List<string> GetReportParameters() => _storeSettings.ReportParameters;
        public void SetReportParameters(List<string> value) => _storeSettings.ReportParameters = value;
    }
}
