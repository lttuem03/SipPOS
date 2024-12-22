namespace SipPOS.DataTransfer.General;

/// <summary>
/// Data transfer object for configuration settings.
/// </summary>
public class ConfigurationDto
{
    /// <summary>
    /// Gets or sets the store ID.
    /// </summary>
    public long? StoreId { get; set; }

    /// <summary>
    /// Gets or sets the opening time.
    /// </summary>
    public TimeOnly OpeningTime { get; set; }

    /// <summary>
    /// Gets or sets the closing time.
    /// </summary>
    public TimeOnly ClosingTime { get; set; }

    /// <summary>
    /// Gets or sets the tax code.
    /// </summary>
    public string TaxCode { get; set; }

    /// <summary>
    /// Gets or sets the VAT rate.
    /// </summary>
    public decimal VatRate { get; set; }

    /// <summary>
    /// Gets or sets the VAT method.
    /// </summary>
    public string VatMethod { get; set; }

    /// <summary>
    /// Gets or sets the current staff base salary.
    /// </summary>
    public decimal CurrentStaffBaseSalary { get; set; }

    /// <summary>
    /// Gets or sets the current staff hourly salary.
    /// </summary>
    public decimal CurrentStaffHourlySalary { get; set; }

    /// <summary>
    /// Gets or sets the current assistant manager base salary.
    /// </summary>
    public decimal CurrentAssistantManagerBaseSalary { get; set; }

    /// <summary>
    /// Gets or sets the current assistant manager hourly salary.
    /// </summary>
    public decimal CurrentAssistantManagerHourlySalary { get; set; }

    /// <summary>
    /// Gets or sets the current store manager base salary.
    /// </summary>
    public decimal CurrentStoreManagerBaseSalary { get; set; }

    /// <summary>
    /// Gets or sets the current store manager hourly salary.
    /// </summary>
    public decimal CurrentStoreManagerHourlySalary { get; set; }

    /// <summary>
    /// Gets or sets the next staff base salary.
    /// </summary>
    public decimal NextStaffBaseSalary { get; set; }

    /// <summary>
    /// Gets or sets the next staff hourly salary.
    /// </summary>
    public decimal NextStaffHourlySalary { get; set; }

    /// <summary>
    /// Gets or sets the next assistant manager base salary.
    /// </summary>
    public decimal NextAssistantManagerBaseSalary { get; set; }

    /// <summary>
    /// Gets or sets the next assistant manager hourly salary.
    /// </summary>
    public decimal NextAssistantManagerHourlySalary { get; set; }

    /// <summary>
    /// Gets or sets the next store manager base salary.
    /// </summary>
    public decimal NextStoreManagerBaseSalary { get; set; }

    /// <summary>
    /// Gets or sets the next store manager hourly salary.
    /// </summary>
    public decimal NextStoreManagerHourlySalary { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationDto"/> class.
    /// </summary>
    public ConfigurationDto()
    {
        StoreId = null;
        OpeningTime = TimeOnly.MinValue;
        ClosingTime = TimeOnly.MinValue;

        TaxCode = string.Empty;
        VatRate = -1.0m;
        VatMethod = string.Empty;

        CurrentStaffBaseSalary = -1m;
        CurrentStaffHourlySalary = -1m;
        CurrentAssistantManagerBaseSalary = -1m;
        CurrentAssistantManagerHourlySalary = -1m;
        CurrentStoreManagerBaseSalary = -1m;
        CurrentStoreManagerHourlySalary = -1m;

        NextStaffBaseSalary = -1m;
        NextStaffHourlySalary = -1m;
        NextAssistantManagerBaseSalary = -1m;
        NextAssistantManagerHourlySalary = -1m;
        NextStoreManagerBaseSalary = -1m;
        NextStoreManagerHourlySalary = -1m;
    }
}
