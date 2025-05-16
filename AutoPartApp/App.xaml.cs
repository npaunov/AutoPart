using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Globalization;
using System.Windows.Markup;
using System.Threading;
using AutoPartApp.Services;

namespace AutoPartApp
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {

            LanguageService.ChangeLanguage(AutoPartApp.Properties.Strings.BulgarianCultureCode);

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register AutoPartDbContext with connection string
                    services.AddDbContext<AutoPartDbContext>(options =>
                        options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=AutoPartsDb;Integrated Security=True;Connect Timeout=30"));

                    // Register your view models and other services
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<AutoPartsViewModel>();
                    services.AddTransient<DataImportViewModel>();
                    services.AddTransient<DbContextWrapper>();
                    // Add other services as needed
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            MainWindow = mainWindow; // Set the application's MainWindow property
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            base.OnExit(e);
        }
    }
}