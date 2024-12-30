using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Windowing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SipPOS.Views.Login;
using SipPOS.Views.Cashier;
using SipPOS.Views.Inventory;

using SipPOS.ViewModels.Cashier;
using SipPOS.ViewModels.Inventory;
using SipPOS.ViewModels.Setup;

using SipPOS.Services.Account.Interfaces;
using SipPOS.Services.Account.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.Entity.Implementations;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.DataAccess.Implementations;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Accessibility.Interfaces;
using SipPOS.Services.Accessibility.Implementations;
using SipPOS.Services.Configuration.Interfaces;
using SipPOS.Services.Configuration.Implementations;

using SipPOS.Context.Shift.Interface;
using SipPOS.Context.Shift.Implementation;

using SipPOS.Context.Configuration.Interfaces;
using SipPOS.Context.Configuration.Implementations;

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
        this.RequestedTheme = ApplicationTheme.Light; // FIXED LIGHT THEME ONLY

        // Database connection configuration
        DotNetEnv.Env.TraversePath().Load(AppContext.BaseDirectory);
        var postgres_host = DotNetEnv.Env.GetString("POSTGRES_HOST");
        var postgres_port = DotNetEnv.Env.GetInt("POSTGRES_PORT");
        var postgres_username = DotNetEnv.Env.GetString("POSTGRES_USERNAME");
        var postgres_password = DotNetEnv.Env.GetString("POSTGRES_PASSWORD");
        var postgres_database = DotNetEnv.Env.GetString("POSTGRES_DATABASE");

        // Configure culture
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("vi-VN");
        System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
        System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Contexts
            services.AddSingleton<IStaffShiftContext, StaffShiftContext>();

            // Dao
            services.AddSingleton<ISpecialOfferDao, PostgreSpecialOfferDao>();
            services.AddSingleton<IProductDao, PostgreProductDao>();
            services.AddSingleton<ICategoryDao, PostgreCategoryDao>();
            services.AddSingleton<IStoreDao, PostgreStoreDao>();
            services.AddSingleton<IStaffDao, PostgreStaffDao>();
            services.AddSingleton<IConfigurationDao, PostgreConfigurationDao>();
            services.AddSingleton<IInvoiceDao, PostgreInvoiceDao>();

            // Services
            services.AddSingleton<ISpecialOfferService, SpecialOfferService>();
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
            services.AddSingleton<IStaffAccountCreationService>(new StaffAccountCreationService());
            services.AddSingleton<IStaffAuthenticationService>(new StaffAuthenticationService());
            services.AddSingleton<IPolicyEnforcementPoint>(new PolicyEnforcementPoint());
            services.AddSingleton<IConfigurationService>(new ConfigurationService());
            services.AddSingleton<IConfigurationContext>(new ConfigurationContext());

            // Views and ViewModels
            services.AddTransient<SpecialOfferManagementViewModel>();
            services.AddTransient<CategoryManagementViewModel>();
            services.AddTransient<CategoryManagementView>();
            services.AddTransient<ProductManagementViewModel>();
            services.AddTransient<ProductManagementView>();
            services.AddTransient<CustomerPaymentViewModel>();
            services.AddTransient<CustomerPaymentView>();

            services.AddSingleton<IStoreSetupViewModel, StoreSetupViewModel>();

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
    /// Navigates to the specified page type. Show an error dialog if somehow navigation'd failed.
    /// </summary>
    /// <param name="pageType">The type of the page to navigate to.</param>
    /// <param name="parameter">The parameter to pass to the page.</param>
    /// <param name="infoOverride">The navigation transition information.</param>
    public static void NavigateTo(Type pageType, object? parameter = null, NavigationTransitionInfo? infoOverride = null)
    {
        if (App.CurrentWindow == null)
        {
            return;
        }

        var rootFrame = App.CurrentWindow.Content as Frame;

        if (rootFrame != null)
        {
            rootFrame.BackStack.Clear();
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
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // In the App startup check Window, we check for the
        // prequisite for the app to run, such as database connection

        // If everything is ready, we change the instance of _mainWindow
        // from AppStartupCheckWindow to MainWindow.

        // Otherwise, we show an error dialog and exit the app.

        _mainWindow = new AppStartupCheckWindow();

        if (_mainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.Loaded += AppStartupCheck;
        }

        // Always start app maximized and unresizable
        if (_mainWindow.AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.Maximize();
            presenter.IsResizable = false;
            presenter.IsMinimizable = false;
            presenter.SetBorderAndTitleBar(false, false);
        }

        _mainWindow.Activate();
    }

    /// <summary>
    /// Checks the application startup conditions and initializes the main window.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void AppStartupCheck(object sender, RoutedEventArgs e)
    {
        if (_mainWindow == null)
        {
            return;
        }

        if (_mainWindow.Content is FrameworkElement rootElement)
        {
            // we only want do this event once
            rootElement.Loaded -= AppStartupCheck;
        }

        // Check if the database connection service is working (if the app is using a database)
        var storeDao = App.GetService<IStoreDao>();
        var staffDao = App.GetService<IStaffDao>();
        var categoryDao = App.GetService<ICategoryDao>();
        var productDao = App.GetService<IProductDao>();
        var configurationDao = App.GetService<IConfigurationDao>();

        if (storeDao is PostgreStoreDao || 
            staffDao is PostgreStaffDao ||
            categoryDao is PostgreCategoryDao |
            productDao is PostgreProductDao ||
            configurationDao is PostgreProductDao)
        {
            try
            {
                var connectionService = App.GetService<IDatabaseConnectionService>();
                using var connection = connectionService.GetOpenConnection();
                // Connection successful, proceed with application startup
            }
            catch (Exception)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi kết nối",
                    Content = "Không thể thiết lập kết nối tới cơ sở dữ liệu",
                    CloseButtonText = "Thoát chương trình",
                    XamlRoot = _mainWindow.Content.XamlRoot
                };

                await errorDialog.ShowAsync();
                App.Current.Exit();
                return;
            }
        }

        // Everything is ready, we configure and change the instance of _mainWindow
        var mainWindow = new MainWindow();

        Frame rootFrame = new Frame();
        rootFrame.NavigationFailed += _onNavigationFailed;

        mainWindow.Content = rootFrame;
        

        // Always start app maximized and unresizable
        if (mainWindow.AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.Maximize();
            presenter.IsResizable = false;
            presenter.IsMinimizable = false;
            presenter.SetBorderAndTitleBar(false, false);
        }

        // Check if a Store's credentials is saved for authentication (clicked "Save credentials" on last authentication)
        var storeCredentialsService = App.GetService<IStoreCredentialsService>();

        (var storeUsername, var storePassword) = storeCredentialsService.LoadCredentials();

        if (storeUsername != null && storePassword != null)
        {
            var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
            var loginSuccessful = await storeAuthenticationService.LoginAsync(storeUsername, storePassword);

            if (loginSuccessful)
            {
                // Load up the configuration for the store
                var currentStoreId = storeAuthenticationService.GetCurrentStoreId();
                var storeConfigurationService = App.GetService<IConfigurationService>();

                await storeConfigurationService.LoadAsync(currentStoreId);
            }

            // Even if store authentication succeeded, we still navigate to the login page
            // and set the login tab to StaffLogin
        }

        rootFrame.Navigate(typeof(LoginView));

        // Close the AppStartupCheckWindow and change the instance of _mainWindow

        // Sometimes, the program will attempt to close the _mainWindow
        // (now AppStartupCheckWindow) when it is not fully allocated
        // and will lead to System.AccessViolationException, so doing this
        // make sure that it is fully allocated before attempting to close it

        // IGNORE THE 3 WARNINGS HERE
        if (_mainWindow.AppWindow != null &&
            _mainWindow.Bounds != null &&
            _mainWindow.Compositor != null &&
            _mainWindow.Content != null &&
            _mainWindow.CoreWindow != null &&
            _mainWindow.ExtendsContentIntoTitleBar != null &&
            _mainWindow.Title != null &&
            _mainWindow.Visible != null)
        {
            _mainWindow.Close();
        }

        _appStartupCheckWindow = _mainWindow; // to close the window if it failed to close before

        // Add an event handler so when the main window is activated,
        // we check if the AppStartupCheckWindow is still open and close it
        mainWindow.Activated += (s, args) =>
        {
            if (_appStartupCheckWindow != null)
            {
                _appStartupCheckWindow.Close();
                _appStartupCheckWindow = null;
            }
        };
        
        _mainWindow = mainWindow;
        CurrentWindow = mainWindow;
        mainWindow.Activate();
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
    private Window? _appStartupCheckWindow;
}