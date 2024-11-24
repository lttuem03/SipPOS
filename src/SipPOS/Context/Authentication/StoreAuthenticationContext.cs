using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SipPOS.Models.Entity;

namespace SipPOS.Context.Authentication;

/// <summary>
/// Represents the context for store authentication within the SipPOS application.
/// </summary>
public class StoreAuthenticationContext
{
    /// <summary>
    /// Gets a value indicating whether a store is logged in.
    /// </summary>
    public bool LoggedIn { get; private set; }

    /// <summary>
    /// Gets the login time of the current store.
    /// </summary>
    public DateTime? LoginTime { get; private set; }

    /// <summary>
    /// The current authenticated store.
    /// </summary>
    private Store? _currentStore;

    /// <summary>
    /// Gets or sets the current authenticated store.
    /// </summary>
    public Store? CurrentStore
    {
        get => _currentStore;
        set
        {
            if (value != null)
            {
                LoggedIn = true;
                LoginTime = DateTime.Now;
                _currentStore = value;
            }
            else
            {
                LoggedIn = false;
                LoginTime = null;
                _currentStore = null;
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreAuthenticationContext"/> class.
    /// </summary>
    public StoreAuthenticationContext()
    {
        CurrentStore = null;
    }

    /// <summary>
    /// Sets the authenticated store as the current store.
    /// </summary>
    /// <param name="authenticatedStore">The store to authenticate.</param>
    public void SetStore(Store authenticatedStore)
    {
        CurrentStore = authenticatedStore;
    }

    /// <summary>
    /// Clears the current authenticated store.
    /// </summary>
    public void ClearStore()
    {
        CurrentStore = null;
    }
}
