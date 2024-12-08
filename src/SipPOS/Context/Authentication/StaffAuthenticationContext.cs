using SipPOS.Models.Entity;

namespace SipPOS.Context.Authentication;

/// <summary>
/// Represents the context for staff authentication within the SipPOS application.
/// </summary>
public class StaffAuthenticationContext
{
    /// <summary>
    /// Gets a value indicating whether a staff member is logged in.
    /// </summary>
    public bool LoggedIn
    {
        get; private set;
    }

    /// <summary>
    /// Gets the login time of the current staff member.
    /// </summary>
    public DateTime? LoginTime
    {
        get; private set;
    }

    private Staff? _currentStaff;

    /// <summary>
    /// Gets or sets the current authenticated staff member.
    /// </summary>
    public Staff? CurrentStaff
    {
        get => _currentStaff;
        set
        {
            if (value != null)
            {
                LoggedIn = true;
                LoginTime = DateTime.Now;
                _currentStaff = value;
            }
            else
            {
                LoggedIn = false;
                LoginTime = null;
                _currentStaff = null;
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffAuthenticationContext"/> class.
    /// </summary>
    public StaffAuthenticationContext()
    {
        _currentStaff = null;
        CurrentStaff = null;
    }

    /// <summary>
    /// Sets the authenticated staff member.
    /// </summary>
    /// <param name="authenticatedStaff">The authenticated staff member.</param>
    public void SetStaff(Staff authenticatedStaff)
    {
        CurrentStaff = authenticatedStaff;
    }

    /// <summary>
    /// Clears the authenticated staff member.
    /// </summary>
    public void ClearStaff()
    {
        CurrentStaff = null;
    }
}
