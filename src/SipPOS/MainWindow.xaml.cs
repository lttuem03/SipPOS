using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;

using WinRT.Interop;

namespace SipPOS;

/// <summary>
/// Represents the main window of the application.
/// </summary>
public sealed partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Handles the Activated event of the window.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void Window_Activated(object sender, Microsoft.UI.Xaml.WindowActivatedEventArgs args)
    {

    }
}
