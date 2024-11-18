using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;
using System.Data;
using SipPOS.Services.General.Interfaces;

namespace SipPOS.Services.General.Implementations;

/// <summary>
/// Service for managing PostgreSQL database connections.
/// </summary>
public class PostgreSqlConnectionService : IDatabaseConnectionService
{
    private readonly NpgsqlDataSource _dataSource;
    private const int MIN_POOL_SIZE = 5;
    private const int MAX_POOL_SIZE = 20;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgreSqlConnectionService"/> class with specified connection parameters.
    /// </summary>
    /// <param name="host">The database host.</param>
    /// <param name="port">The database port.</param>
    /// <param name="username">The database username.</param>
    /// <param name="password">The database password.</param>
    /// <param name="database">The database name.</param>
    public PostgreSqlConnectionService(string host, int port, string username, string password, string database)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder();

        // Connection credentials
        dataSourceBuilder.ConnectionStringBuilder.Host = host;
        dataSourceBuilder.ConnectionStringBuilder.Port = port;
        dataSourceBuilder.ConnectionStringBuilder.Username = username;
        dataSourceBuilder.ConnectionStringBuilder.Password = password;
        dataSourceBuilder.ConnectionStringBuilder.Database = database;

        // Connection pooling configuration
        dataSourceBuilder.ConnectionStringBuilder.Pooling = true;
        dataSourceBuilder.ConnectionStringBuilder.MinPoolSize = MIN_POOL_SIZE;
        dataSourceBuilder.ConnectionStringBuilder.MaxPoolSize = MAX_POOL_SIZE;

        // Connection other configuration

        _dataSource = dataSourceBuilder.Build();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgreSqlConnectionService"/> class with a connection string.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    public PostgreSqlConnectionService(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        // Connection pooling configuration
        dataSourceBuilder.ConnectionStringBuilder.Pooling = true;
        dataSourceBuilder.ConnectionStringBuilder.MinPoolSize = MIN_POOL_SIZE;
        dataSourceBuilder.ConnectionStringBuilder.MaxPoolSize = MAX_POOL_SIZE;

        _dataSource = dataSourceBuilder.Build();
    }

    /// <summary>
    /// Gets an open database connection.
    /// </summary>
    /// <returns>An open database connection.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the connection cannot be opened.</exception>
    public IDbConnection GetOpenConnection()
    {
        var connection = _dataSource.OpenConnection();

        if (connection.State != ConnectionState.Open)
        {
            throw new InvalidOperationException("Failed to open a connection to the database.");
        }

        return connection;
    }

    /// <summary>
    /// Closes the specified database connection.
    /// </summary>
    /// <param name="connection">The database connection to close.</param>
    public void CloseConnection(IDbConnection connection)
    {
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
