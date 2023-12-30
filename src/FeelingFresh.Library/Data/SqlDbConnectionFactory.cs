using System.Data;
using System.Data.SqlClient;
using FeelingFresh.Library.Options;
using Microsoft.Extensions.Options;

namespace FeelingFresh.Library.Data;

public class SqlDbConnectionFactory : ISqlDbConnectionFactory
{
    private readonly DbConnectionOptions _connectionOptions;

    public SqlDbConnectionFactory(IOptions<DbConnectionOptions> connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

    public async Task<IDbConnection> CreateDbConnectionAsync()
    {
        var connection = new SqlConnection(_connectionOptions.ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}