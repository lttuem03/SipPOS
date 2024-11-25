using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Windowing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SipPOS.Views.Login;
using SipPOS.Views.Cashier;
using SipPOS.Views.General;
using SipPOS.Views.Management;

using SipPOS.ViewModels.Cashier;
using SipPOS.ViewModels.Management;

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
            services.AddSingleton<IStoreDao, MockStoreDao>();

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
            services.AddSingleton<IStoreCredentialsService>(new StoreCredentialsService());

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
    protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _mainWindow = new MainWindow();
        App.CurrentWindow = _mainWindow;

        // Always start app maximized
        if (_mainWindow.AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.Maximize();
        }

        Frame rootFrame = new Frame();
        rootFrame.NavigationFailed += _onNavigationFailed;

        // Check if a Store's credentials is saved for authentication (clicked "Save credentials" on last authentication)
        var storeCredentialsService = App.GetService<IStoreCredentialsService>();

        (var storeUsername, var storePassword) = storeCredentialsService.LoadCredentials();

        if (storeUsername != null && storePassword != null)
        {
            var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();

            var loginSuccessful = await storeAuthenticationService.LoginAsync(storeUsername, storePassword);

            if (loginSuccessful)
            {
                rootFrame.Navigate(typeof(MainMenuView));
                _mainWindow.Content = rootFrame;
                _mainWindow.Activate();

                return;
            }
        }
         
        rootFrame.Navigate(typeof(LoginView));
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
        {
            return;
        }

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
