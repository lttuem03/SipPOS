using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;

namespace SipPOS.Authentication;

public class StoreAuthenticationContext
{
    public bool LoggedIn
    {
        get; private set;
    }
    public DateTime? LoginTime
    {
        get; private set;
    }
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

    public StoreAuthenticationContext()
    {
        LoggedIn = false;
        LoginTime = null;
        CurrentStore = null;
    }

    public void SetStore(Store authenticatedStore)
    {
        CurrentStore = authenticatedStore;
    }

    public void ClearStore()
    {
        CurrentStore = null;
    }

    // backup fields
    private Store? _currentStore;
}
