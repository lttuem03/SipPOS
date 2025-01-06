using SipPOS.DataTransfer.General;

namespace SipPOS.Models.General;

/// <summary>
/// Represents the configuration settings for a store.
/// </summary>
public class Configuration
{
    /// <summary>
    /// Gets the store ID.
    /// </summary>
    public long StoreId { get; }

    /// <summary>
    /// Gets the opening time of the store.
    /// </summary>
    public TimeOnly OpeningTime { get; }

    /// <summary>
    /// Gets the closing time of the store.
    /// </summary>
    public TimeOnly ClosingTime { get; }

    /// <summary>
    /// Gets the tax code of the store.
    /// </summary>
    public string TaxCode { get; }

    /// <summary>
    /// Gets the VAT rate.
    /// </summary>
    public decimal VatRate { get; }

    /// <summary>
    /// Gets the VAT method.
    /// </summary>
    public string VatMethod { get; }

    /// <summary>
    /// Gets the current base salary for staff.
    /// </summary>
    public decimal CurrentStaffBaseSalary { get; }

    /// <summary>
    /// Gets the current hourly salary for staff.
    /// </summary>
    public decimal CurrentStaffHourlySalary { get; }

    /// <summary>
    /// Gets the current base salary for assistant managers.
    /// </summary>
    public decimal CurrentAssistantManagerBaseSalary { get; }

    /// <summary>
    /// Gets the current hourly salary for assistant managers.
    /// </summary>
    public decimal CurrentAssistantManagerHourlySalary { get; }

    /// <summary>
    /// Gets the current base salary for store managers.
    /// </summary>
    public decimal CurrentStoreManagerBaseSalary { get; }

    /// <summary>
    /// Gets the current hourly salary for store managers.
    /// </summary>
    public decimal CurrentStoreManagerHourlySalary { get; }

    /// <summary>
    /// Gets the next base salary for staff.
    /// </summary>
    public decimal NextStaffBaseSalary { get; }

    /// <summary>
    /// Gets the next hourly salary for staff.
    /// </summary>
    public decimal NextStaffHourlySalary { get; }

    /// <summary>
    /// Gets the next base salary for assistant managers.
    /// </summary>
    public decimal NextAssistantManagerBaseSalary { get; }

    /// <summary>
    /// Gets the next hourly salary for assistant managers.
    /// </summary>
    public decimal NextAssistantManagerHourlySalary { get; }

    /// <summary>
    /// Gets the next base salary for store managers.
    /// </summary>
    public decimal NextStoreManagerBaseSalary { get; }

    /// <summary>
    /// Gets the next hourly salary for store managers.
    /// </summary>
    public decimal NextStoreManagerHourlySalary { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Configuration"/> class.
    /// </summary>
    /// <param name="id">The store ID.</param>
    /// <param name="dto">The data transfer object to initialize from.</param>
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
