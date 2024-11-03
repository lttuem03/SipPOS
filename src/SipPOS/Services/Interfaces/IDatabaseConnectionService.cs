using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Services.Interfaces;

public interface IDatabaseConnectionService
{
    IDbConnection GetOpenConnection();
    void CloseConnection(IDbConnection connection);
}
