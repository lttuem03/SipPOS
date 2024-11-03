namespace SipPOS.Models
{
    public class StoreSettingsDto
    {
        // Basic Information
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string BranchPhone { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public string LogoUrl { get; set; }

        // Financial Information
        public string TaxCode { get; set; }
        public List<string> PaymentMethods { get; set; }
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }

        // Product and Category Configuration
        public List<string> MeasurementUnits { get; set; }
        public bool HighlightFeaturedProducts { get; set; }
        public bool HighlightDiscountedProducts { get; set; }

        // Tax and Fee Settings
        public decimal TaxRate { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal ShippingFee { get; set; }

        // Operating Hours
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
    

        // Promotion and Discount Settings
        public bool AllowDiscounts { get; set; }
        public List<string> Promotions { get; set; }
        public List<string> Coupons { get; set; }
        public List<string> RewardPoints { get; set; }

        // UI Configuration
        public string Layout { get; set; }
        public string ThemeColor { get; set; }
        public string Language { get; set; }

        // Reporting and Data Analysis
        public bool EnableSalesReport { get; set; }
        public bool EnableInventoryReport { get; set; }
        public bool EnableMonthlyReport { get; set; }
        public List<string> ReportParameters { get; set; }
    }
}
