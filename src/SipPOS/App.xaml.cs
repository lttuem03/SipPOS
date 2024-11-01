using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
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
using SipPOS.Services.Interfaces;
using SipPOS.Services.Implementations;
using SipPOS.DataAccess.Interfaces;
using SipPOS.DataAccess.Implementations;

namespace SipPOS;

public partial class App : Application
{
    public static Window? CurrentWindow { get; private set; }
    public IHost Host { get; }

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();

        // Database connection configuration
        DotNetEnv.Env.TraversePath().Load();
        var host = DotNetEnv.Env.GetString("POSTGRES_HOST");
        var port = DotNetEnv.Env.GetInt("POSTGRES_PORT");
        var username = DotNetEnv.Env.GetString("POSTGRES_USERNAME");
        var password = DotNetEnv.Env.GetString("POSTGRES_PASSWORD");
        var database = DotNetEnv.Env.GetString("POSTGRES_DATABASE");

        Host = Microsoft.Extensions.Hosting.Host.
               CreateDefaultBuilder().
               UseContentRoot(AppContext.BaseDirectory).
               ConfigureServices((context, services) =>
               {
                   // Dao
                   services.AddSingleton<IProductDao, MockProductDao>();
                   services.AddSingleton<ICategoryDao, MockCategoryDao>();
               
                   // Services
                   services.AddSingleton<IProductService, ProductService>();
                   services.AddSingleton<ICategoryService, CategoryService>();
               
                   // Views and ViewModels
                   services.AddTransient<CategoryManagementViewModel>();
                   services.AddTransient<CategoryManagementView>();
                   services.AddTransient<ProductManagementViewModel>();
                   services.AddTransient<ProductManagementView>();
               
                   //Add AutoMapper
                   services.AddAutoMapper(typeof(App).Assembly);
               }).
               Build();
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
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
        rootFrame.NavigationFailed += OnNavigationFailed;

        rootFrame.Navigate(typeof(MainMenuView));

        _mainWindow.Content = rootFrame;
        _mainWindow.Activate();
    }

    private async void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
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
