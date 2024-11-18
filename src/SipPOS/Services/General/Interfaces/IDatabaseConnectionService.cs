using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Services.General.Interfaces;

/// <summary>
/// Service interface for managing database connections.
/// </summary>
public interface IDatabaseConnectionService
{
    /// <summary>
    /// Gets an open database connection.
    /// </summary>
    /// <returns>An open database connection.</returns>
    IDbConnection GetOpenConnection();

    /// <summary>
    /// Closes the specified database connection.
    /// </summary>
    /// <param name="connection">The database connection to close.</param>
    void CloseConnection(IDbConnection connection);
}
