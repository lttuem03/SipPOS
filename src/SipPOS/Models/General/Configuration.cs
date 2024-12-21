using SipPOS.DataTransfer.General;

namespace SipPOS.Models.General;

public class Configuration
{
    public long StoreId
    {
        get;
    }

    public TimeOnly OpeningTime
    {
        get;
    }

    public TimeOnly ClosingTime
    {
        get;
    }

    public string TaxCode
    {
        get;
    }

    public decimal VatRate
    {
        get;
    }

    public string VatMethod
    {
        get;
    }

    public decimal StaffBaseSalary
    {
        get;
    }

    public decimal StaffHourlySalary
    {
        get;
    }

    public decimal AssistantManagerBaseSalary
    {
        get;
    }

    public decimal AssistantManagerHourlySalary
    {
        get;
    }

    public decimal StoreManagerBaseSalary
    {
        get;
    }

    public decimal StoreManagerHourlySalary
    {
        get;
    }

    public Configuration(long id, ConfigurationDto dto)
    {
        StoreId = id;
        OpeningTime = dto.OpeningTime;
        ClosingTime = dto.ClosingTime;

        TaxCode = dto.TaxCode;
        VatRate = dto.VatRate;
        VatMethod = dto.VatMethod;

        StaffBaseSalary = dto.StaffBaseSalary;
        StaffHourlySalary = dto.StaffHourlySalary;
        AssistantManagerBaseSalary = dto.AssistantManagerBaseSalary;
        AssistantManagerHourlySalary = dto.AssistantManagerHourlySalary;
        StoreManagerBaseSalary = dto.StoreManagerBaseSalary;
        StoreManagerHourlySalary = dto.StoreManagerHourlySalary;
    }
}