using FeelingFresh.Library.Models;

namespace FeelingFresh.Library.Services;

public interface IAppService
{
    Task<IEnumerable<Win32App>> GetAllAsync();
    Task<bool> CreateAsync(string appName);
    Task<bool> DeleteByAppNameAsync(string appName);
    Task<bool> UpdateByAppNameAsync(string oldName, string newName);
}