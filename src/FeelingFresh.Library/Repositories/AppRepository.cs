using Dapper;
using FeelingFresh.Library.Data;
using FeelingFresh.Library.Models;

namespace FeelingFresh.Library.Repositories;

public class AppRepository : IAppRepository
{
    private readonly SqlDbConnectionFactory _connectionFactory;

    public AppRepository(SqlDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Win32App>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateDbConnectionAsync();
        return await connection.QueryAsync<Win32App>("SELECT * FROM AppsList");
    }

    public async Task<bool> CreateAsync(string appName)
    {
        const string query = "INSERT INTO AppsList (AppName) VALUES (@AppName)";
        using var connection = await _connectionFactory.CreateDbConnectionAsync();
        var result = await connection.ExecuteAsync(query, new { AppName = appName });
        return result > 0;
    }

    public async Task<bool> DeleteByAppNameAsync(string appName)
    {
        const string query = "DELETE FROM AppsList WHERE AppName = @AppName";
        using var connection = await _connectionFactory.CreateDbConnectionAsync();
        var result = await connection.ExecuteAsync(query, new { AppName = appName });
        return result > 0;
    }

    public async Task<bool> UpdateByAppNameAsync(string oldName, string newName)
    {
        const string query = "UPDATE AppsList SET AppName=(@NewAppName) WHERE AppName=(@OldAppName)";
        using var connection = await _connectionFactory.CreateDbConnectionAsync();
        var result = await connection.ExecuteAsync(query, new { NewAppName = newName, OldAppName = oldName });
        return result > 0;
    }
}