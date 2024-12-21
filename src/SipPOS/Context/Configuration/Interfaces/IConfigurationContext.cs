using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Context.Configuration.Interfaces;

public interface IConfigurationContext
{
    Models.General.Configuration? GetConfiguration();
    bool IsSet();
    void SetConfiguration(Models.General.Configuration configuration);
    void ClearConfiguration();
}