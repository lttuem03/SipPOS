using SipPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SipPOS.Services.Interfaces
{
    public interface IStoreSettingsService
    {
        Task<StoreSettings> GetStoreSettingsAsync();
        Task<bool> UpdateStoreSettingsAsync(StoreSettings settings);

        // Individual property accessors
        string GetStoreName();
        void SetStoreName(string value);
        string GetAddress();
        void SetAddress(string value);
        string GetCity();
        void SetCity(string value);
        string GetPhone();
        void SetPhone(string value);
        string GetBranchPhone();
        void SetBranchPhone(string value);
        string GetUrl();
        void SetUrl(string value);
        string GetNote();
        void SetNote(string value);
        string GetLogoUrl();
        void SetLogoUrl(string value);
        string GetTaxCode();
        void SetTaxCode(string value);
        List<string> GetPaymentMethods();
        void SetPaymentMethods(List<string> value);
        string GetCurrency();
        void SetCurrency(string value);
        decimal GetExchangeRate();
        void SetExchangeRate(decimal value);
        List<string> GetMeasurementUnits();
        void SetMeasurementUnits(List<string> value);
        bool GetHighlightFeaturedProducts();
        void SetHighlightFeaturedProducts(bool value);
        bool GetHighlightDiscountedProducts();
        void SetHighlightDiscountedProducts(bool value);
        decimal GetTaxRate();
        void SetTaxRate(decimal value);
        decimal GetServiceFee();
        void SetServiceFee(decimal value);
        decimal GetShippingFee();
        void SetShippingFee(decimal value);
        TimeSpan GetOpeningTime();
        void SetOpeningTime(TimeSpan value);
        TimeSpan GetClosingTime();
        void SetClosingTime(TimeSpan value);
        bool GetAllowDiscounts();
        void SetAllowDiscounts(bool value);
        List<string> GetPromotions();
        void SetPromotions(List<string> value);
        List<string> GetCoupons();
        void SetCoupons(List<string> value);
        List<string> GetRewardPoints();
        void SetRewardPoints(List<string> value);
        string GetLayout();
        void SetLayout(string value);
        string GetThemeColor();
        void SetThemeColor(string value);
        string GetLanguage();
        void SetLanguage(string value);
        bool GetEnableSalesReport();
        void SetEnableSalesReport(bool value);
        bool GetEnableInventoryReport();
        void SetEnableInventoryReport(bool value);
        bool GetEnableMonthlyReport();
        void SetEnableMonthlyReport(bool value);
        List<string> GetReportParameters();
        void SetReportParameters(List<string> value);
    }
}


