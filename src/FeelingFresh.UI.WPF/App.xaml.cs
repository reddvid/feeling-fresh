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
using FeelingFresh.Library.Repositories;
using FeelingFresh.Library.Services;
using FeelingFresh.UI.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FeelingFresh.UI.WPF;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddTransient<IAppRepository, AppRepository>();
                services.AddTransient<IAppService, AppService>();
                services.AddTransient<ISqlDbConnectionFactory, SqlDbConnectionFactory>();
                services.AddTransient<ILoggerAdapter<object>, LoggerAdapter<object>>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}