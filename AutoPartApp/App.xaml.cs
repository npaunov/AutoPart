using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using AutoPart.DataAccess;
using AutoPart.Utilities;
using AutoPartApp.DIServices.Messengers;
using AutoPartApp.DIServices.Services.Interfaces;
using AutoPartApp.DIServices.Services;
using AutoPartApp.ViewModels;
using AutoPartApp.Views;
using Microsoft.Extensions.Configuration;

namespace AutoPartApp
{
    /// <summary>
    /// Interaction logic for the WPF Application.
    /// Handles application startup, shutdown, dependency injection, and global message registration.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The application's dependency injection host.
        /// </summary>
        public static IHost AppHost { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// Sets up language handling, dependency injection, and message registration.
        /// </summary>
        public App()
        {
            RegisterLanguageMessenger();
            LanguageUtil.ChangeLanguage(AutoPartApp.Properties.Strings.BulgarianCultureCode);

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<AutoPartDbContext>(options =>
                        options.UseSqlServer(connectionString));
                    // Register view models and other services
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<IDialogService, DialogService>();
                    services.AddScoped<OrdersViewModel>();
                    services.AddScoped<StoresViewModel>();
                    services.AddScoped<WarehouseViewModel>();
                    services.AddScoped<SalesHistoryViewModel>();
                    services.AddScoped<DataImportViewModel>();
                })
                .Build();
        }

        /// <summary>
        /// Handles application startup logic, including starting the DI host and showing the main window.
        /// </summary>
        /// <param name="e">Startup event arguments.</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            MainWindow = mainWindow; // Set the application's MainWindow property
            mainWindow.Show();
            base.OnStartup(e);
        }

        /// <summary>
        /// Handles application exit logic, including stopping the DI host.
        /// </summary>
        /// <param name="e">Exit event arguments.</param>
        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            base.OnExit(e);
        }

        /// <summary>
        /// Registers a messenger handler for language change messages.
        /// Updates the UI language when a <see cref="LanguageChangedMessage"/> is received.
        /// </summary>
        private void RegisterLanguageMessenger()
        {
            WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
            {
                if (Application.Current.MainWindow != null)
                {
                    var culture = System.Globalization.CultureInfo.CurrentUICulture;
                    Application.Current.MainWindow.Language = System.Windows.Markup.XmlLanguage.GetLanguage(culture.IetfLanguageTag);
                }
            });
        }
    }
}
