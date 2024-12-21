namespace SipPOS.DataTransfer.General;

public class ConfigurationDto
{
    public long? StoreId { get; set; }

    public TimeOnly OpeningTime { get; set; }

    public TimeOnly ClosingTime { get; set; }

    public string TaxCode { get; set; }

    public decimal VatRate { get; set; }

    public string VatMethod { get; set; }

    public decimal StaffBaseSalary { get; set; }

    public decimal StaffHourlySalary { get; set; }

    public decimal AssistantManagerBaseSalary { get; set; }

    public decimal AssistantManagerHourlySalary { get; set; }

    public decimal StoreManagerBaseSalary { get; set; }

    public decimal StoreManagerHourlySalary { get; set; }

    public ConfigurationDto()
    {
        StoreId = null;
        OpeningTime = TimeOnly.MinValue;
        ClosingTime = TimeOnly.MinValue;

        TaxCode = string.Empty;
        VatRate = -1.0m;
        VatMethod = string.Empty;

        StaffBaseSalary = -1m;
        StaffHourlySalary = -1m;
        AssistantManagerBaseSalary = -1m;
        AssistantManagerHourlySalary = -1m;
        StoreManagerBaseSalary = -1m;
        StoreManagerHourlySalary = -1m;
    }
}