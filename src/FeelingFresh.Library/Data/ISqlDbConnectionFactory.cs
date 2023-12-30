using System.Data;

namespace FeelingFresh.Library.Data;

public interface ISqlDbConnectionFactory
{
    Task<IDbConnection> CreateDbConnectionAsync();
}