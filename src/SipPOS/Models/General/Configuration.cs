using SipPOS.DataTransfer.General;

namespace SipPOS.Models.General;

public class Configuration
{
    public long StoreId { get; }

    public TimeOnly OpeningTime { get; }

    public TimeOnly ClosingTime { get; }

    public string TaxCode { get; }

    public decimal VatRate { get; }

    public string VatMethod { get; }

    public decimal CurrentStaffBaseSalary { get; }

    public decimal CurrentStaffHourlySalary { get; }

    public decimal CurrentAssistantManagerBaseSalary { get; }

    public decimal CurrentAssistantManagerHourlySalary { get; }

    public decimal CurrentStoreManagerBaseSalary { get; }

    public decimal CurrentStoreManagerHourlySalary { get; }

    public decimal NextStaffBaseSalary { get; }
                   
    public decimal NextStaffHourlySalary { get; }
                   
    public decimal NextAssistantManagerBaseSalary { get; }
                   
    public decimal NextAssistantManagerHourlySalary { get; }
                   
    public decimal NextStoreManagerBaseSalary { get; }
                  
    public decimal NextStoreManagerHourlySalary { get; }

    public Configuration(long id, ConfigurationDto dto)
    {
        StoreId = id;
        OpeningTime = dto.OpeningTime;
        ClosingTime = dto.ClosingTime;

        TaxCode = dto.TaxCode;
        VatRate = dto.VatRate;
        VatMethod = dto.VatMethod;

        CurrentStaffBaseSalary = dto.CurrentStaffBaseSalary;
        CurrentStaffHourlySalary = dto.CurrentStaffHourlySalary;
        CurrentAssistantManagerBaseSalary = dto.CurrentAssistantManagerBaseSalary;
        CurrentAssistantManagerHourlySalary = dto.CurrentAssistantManagerHourlySalary;
        CurrentStoreManagerBaseSalary = dto.CurrentStoreManagerBaseSalary;
        CurrentStoreManagerHourlySalary = dto.CurrentStoreManagerHourlySalary;

        NextStaffBaseSalary = dto.NextStaffBaseSalary;
        NextStaffHourlySalary = dto.NextStaffHourlySalary;
        NextAssistantManagerBaseSalary = dto.NextAssistantManagerBaseSalary;
        NextAssistantManagerHourlySalary = dto.NextAssistantManagerHourlySalary;
        NextStoreManagerBaseSalary = dto.NextStoreManagerBaseSalary;
        NextStoreManagerHourlySalary = dto.NextStoreManagerHourlySalary;
    }
}