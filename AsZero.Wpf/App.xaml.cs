using AsZero.Core.Services;
using AsZero.Core.Services.Auth;
using AsZero.DbContexts;
using AsZero.Wpf.Views;
using FutureTech.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsZero.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this._host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureLogging((ctx, logging) => {
                    if (ctx.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddDebug();
                    }
                    logging.AddLog4Net();
                })
                .ConfigureServices(HostStartup.ConfigureServices)
                .Build();
            this.ServiceProvider = this._host.Services;
        }

        public IHost _host { get; private set; }
        public IServiceProvider ServiceProvider { get; internal set; }

        private CancellationTokenSource cts = new CancellationTokenSource();

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                using (var scope = this.ServiceProvider.CreateScope())
                {
                    var sp = scope.ServiceProvider;
                    var env = sp.GetRequiredService<IHostEnvironment>();
                    var config = sp.GetRequiredService<IConfiguration>();
                    var db = sp.GetRequiredService<AsZeroDbContext>();
                    var logger = sp.GetRequiredService<ILogger<App>>();
                    await db.Database.EnsureCreatedAsync();
                    logger.LogInformation($"数据库确认初始化成功！当前环境:env={env.EnvironmentName}||Root={env.ContentRootPath}");
                }
                var loginWindow = this.ServiceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();
                _ = Task.Run(async () =>
                {
                    await _host.RunAsync(cts.Token);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                var lieftime = _host.Services.GetRequiredService<IHostApplicationLifetime>();
                lieftime.StopApplication();
            }
            base.OnExit(e);
        }
    }

}
