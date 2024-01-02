using FeelingFresh.Library.Models;

namespace FeelingFresh.Library.Repositories;

public interface IAppRepository
{
    Task<IEnumerable<Win32App>> GetAllAsync();
    Task<bool> AddAppAsync(string appName);
    Task<bool> DeleteByAppNameAsync(string appName);
    Task<bool> UpdateByAppNameAsync(string oldName, string newName);
}