using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Setup;

namespace SipPOS.Views.Setup.Pages;

public sealed partial class StoreSetupSummaryPage : Page
{
    public StoreSetupViewModel? ViewModel { get; } // SINGLETON, USED ACCROSS ALL SETUP PAGES

    public StoreSetupSummaryPage()
    {
        this.InitializeComponent();

        if (App.GetService<IStoreSetupViewModel>() is StoreSetupViewModel viewModel)
            ViewModel = viewModel;
    }
}
