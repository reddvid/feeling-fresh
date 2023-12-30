using Dapper;

namespace FeelingFresh.Library.Data;

public class DatabaseInitializer
{
    private readonly ISqlDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(ISqlDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        SqlMapper.AddTypeHandler(new SqlGuidTypeHandler());
        SqlMapper.RemoveTypeMap(typeof(Guid));
        SqlMapper.RemoveTypeMap(typeof(Guid?));

        using var connection = await _connectionFactory.CreateDbConnectionAsync();
        await connection.ExecuteAsync(
            "CREATE TABLE IF NOT EXISTS AppsList (Id INT IDENTITY(0,1) PRIMARY KEY), AppName VARCHAR(255) NOT NULL");
    }
}