using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using DotNetEnv;

using SipPOS.ViewModels;
using SipPOS.Views;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.Entity.Implementations;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.DataAccess.Implementations;

namespace SipPOS;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Gets the current window of the application.
    /// </summary>
    public static Window? CurrentWindow { get; private set; }

    /// <summary>
    /// Gets the host for dependency injection.
    /// </summary>
    public IHost Host { get; }

    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();

        // Database connection configuration
        DotNetEnv.Env.TraversePath().Load(AppContext.BaseDirectory);
        var postgres_host = DotNetEnv.Env.GetString("POSTGRES_HOST");
        var postgres_port = DotNetEnv.Env.GetInt("POSTGRES_PORT");
        var postgres_username = DotNetEnv.Env.GetString("POSTGRES_USERNAME");
        var postgres_password = DotNetEnv.Env.GetString("POSTGRES_PASSWORD");
        var postgres_database = DotNetEnv.Env.GetString("POSTGRES_DATABASE");

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Dao
            services.AddSingleton<IProductDao, MockProductDao>();
            services.AddSingleton<ICategoryDao, MockCategoryDao>();
            services.AddSingleton<IStoreDao, PostgreStoreDao>();

            // Services
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IDatabaseConnectionService>(new PostgreSqlConnectionService(
                host: postgres_host,
                port: postgres_port,
                username: postgres_username,
                password: postgres_password,
                database: postgres_database
            )); // please use arcordingly with the DAOs using the database connections
            services.AddSingleton<IPasswordEncryptionService>(new PasswordEncryptionService());
            services.AddSingleton<IStoreAccountCreationService>(new StoreAccountCreationService());
            services.AddSingleton<IStoreAuthenticationService>(new StoreAuthenticationService());


            // Views and ViewModels
            services.AddTransient<CategoryManagementViewModel>();
            services.AddTransient<CategoryManagementView>();
            services.AddTransient<ProductManagementViewModel>();
            services.AddTransient<ProductManagementView>();
            services.AddTransient<CustomerPaymentViewModel>();
            services.AddTransient<CustomerPaymentView>();

            //Add AutoMapper
            services.AddAutoMapper(typeof(App).Assembly);
        }).
        Build();
    }

    /// <summary>
    /// Gets a service of the specified type from the host's service provider.
    /// </summary>
    /// <typeparam name="T">The type of service to get.</typeparam>
    /// <returns>The service of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the service is not registered.</exception>
    public static T GetService<T>()
        where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _mainWindow = new MainWindow();
        App.CurrentWindow = _mainWindow;

        Frame rootFrame = new Frame();
        rootFrame.NavigationFailed += _onNavigationFailed;

        rootFrame.Navigate(typeof(MainMenuView));

        _mainWindow.Content = rootFrame;
        _mainWindow.Activate();
    }

    /// <summary>
    /// Navigates to the specified page type. Show an error dialog if somehow navigation'd failed.
    /// </summary>
    /// <param name="pageType">The type of the page to navigate to.</param>
    /// <param name="parameter">The parameter to pass to the page.</param>
    /// <param name="infoOverride">The navigation transition information.</param>
    public static void NavigateTo(Type pageType, object? parameter=null, NavigationTransitionInfo? infoOverride=null)
    {
        if (App.CurrentWindow == null)
            return;

        var rootFrame = App.CurrentWindow.Content as Frame;

        if (rootFrame != null)
        {
            rootFrame.Navigate(pageType, parameter);
        }
        else
        {
            var errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = "Navigation frame is null.",
                CloseButtonText = "Close"
            };

            _ = errorDialog.ShowAsync();
        }
    }

    /// <summary>
    /// Handles navigation failures.
    /// </summary>
    /// <param name="sender">The source of the navigation failure.</param>
    /// <param name="e">Details about the navigation failure.</param>
    private async void _onNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Navigation Error",
            Content = $"Failed to load View: {e.SourcePageType.FullName}",
            CloseButtonText = "OK"
        };

        if (App.CurrentWindow != null)
        {
            dialog.XamlRoot = App.CurrentWindow.Content.XamlRoot;
            await dialog.ShowAsync();
        }

        return;
    }

    private Window? _mainWindow;
}
