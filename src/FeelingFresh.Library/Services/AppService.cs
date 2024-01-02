using System.Diagnostics;
using FeelingFresh.Library.Logging;
using FeelingFresh.Library.Models;
using FeelingFresh.Library.Repositories;

namespace FeelingFresh.Library.Services;

public class AppService : IAppService
{
    private readonly IAppRepository _appRepository;
    private readonly ILoggerAdapter<AppService> _logger;

    public AppService(IAppRepository appRepository, ILoggerAdapter<AppService> logger)
    {
        _appRepository = appRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Win32App>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all apps");
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _appRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something went wrong while retrieving all apps");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("All apps retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
        }
    }

    public async Task<bool> CreateAsync(string appName)
    {
        _logger.LogInformation("Creating app with app name: {0}", appName);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _appRepository.AddAppAsync(appName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something went wrong while creating an app");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("App with app name {0} created in {1}ms", appName, stopWatch.ElapsedMilliseconds);
        }
    }

    public async Task<bool> DeleteByAppNameAsync(string appName)
    {
        _logger.LogInformation("Deleting app with name: {0}", appName);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _appRepository.DeleteByAppNameAsync(appName);
        }
        catch (Exception e)
        {
            _logger.LogInformation("Something went wrong while deleting app with name: {0}", appName);
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("App with name {0} deleted in {1}ms", stopWatch.ElapsedMilliseconds);
        }
    }

    public async Task<bool> UpdateByAppNameAsync(string oldName, string newName)
    {
        _logger.LogInformation("Updating {0} with new name: {1}", oldName, newName);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _appRepository.UpdateByAppNameAsync(oldName, newName);
        }
        catch (Exception e)
        {
            _logger.LogInformation("Something went wrong while updating app with name: {0} to new name {1}", oldName,
                newName);
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("Updated {0} to new name: {1}", oldName, newName);
        }
    }
}