using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;
using System.Data;

using SipPOS.Services.Interfaces;

namespace SipPOS.Services.Implementations;

public class PostgreSqlConnectionService : IDatabaseConnectionService
{
    private readonly NpgsqlDataSource _dataSource;
    private const int MIN_POOL_SIZE = 5;
    private const int MAX_POOL_SIZE = 20;

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

    public PostgreSqlConnectionService(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        // Connection pooling configuration
        dataSourceBuilder.ConnectionStringBuilder.MinPoolSize = MIN_POOL_SIZE;
        dataSourceBuilder.ConnectionStringBuilder.MaxPoolSize = MAX_POOL_SIZE;

        _dataSource = dataSourceBuilder.Build();
    }

    public IDbConnection GetOpenConnection()
    {
        var connection = _dataSource.OpenConnection();

        if (connection.State != ConnectionState.Open)
        {
            throw new InvalidOperationException("Failed to open a connection to the database.");
        }

        return connection;
    }

    public void CloseConnection(IDbConnection connection)
    {
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }
}