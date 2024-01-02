using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FeelingFresh.Library;
using FeelingFresh.Library.Data;
using FeelingFresh.Library.Logging;
using FeelingFresh.Library.Options;
using FeelingFresh.Library.Repositories;
using FeelingFresh.Library.Services;
using FeelingFresh.UI.WPF.Controls;
using FeelingFresh.UI.WPF.ViewModels;
using FeelingFresh.UI.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FeelingFresh.UI.WPF;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddScoped<IAppRepository, AppRepository>();
                services.AddScoped<ILoggerAdapter<MainViewModel>, LoggerAdapter<MainViewModel>>();
                services.AddScoped<IAppService, AppService>();
                services.AddScoped<SqlDbConnectionFactory>();
                services.AddScoped<MainViewModel>();
                services.AddScoped<EditAppViewModel>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<EditAppDialog>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        await AppHost!.StartAsync();
        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}