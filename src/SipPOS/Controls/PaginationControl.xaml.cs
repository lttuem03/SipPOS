using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SipPOS.Controls;

/// <summary>
/// A control for handling pagination in the SipPOS application.
/// </summary>
public sealed partial class PaginationControl : UserControl
{
    public static readonly DependencyProperty MinPageProperty = DependencyProperty.Register(
        nameof(MinPage),
        typeof(int),
        typeof(PaginationControl),
        new PropertyMetadata(1, (d, e) => (d as PaginationControl)?.UpdateButtons()));

    public static readonly DependencyProperty TotalPageProperty = DependencyProperty.Register(
        nameof(TotalPage),
        typeof(int),
        typeof(PaginationControl),
        new PropertyMetadata(1, (d, e) => (d as PaginationControl)?.UpdateButtons()));

    public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
        nameof(CurrentPage),
        typeof(int),
        typeof(PaginationControl),
        new PropertyMetadata(1, (d, e) => (d as PaginationControl)?.UpdateButtons()));

    public static readonly DependencyProperty PerPageProperty = DependencyProperty.Register(
        nameof(PerPage),
        typeof(int),
        typeof(PaginationControl),
        new PropertyMetadata(5, (d, e) => (d as PaginationControl)?.UpdateButtons()));

    public static readonly DependencyProperty TotalRecordProperty = DependencyProperty.Register(
        nameof(TotalRecord),
        typeof(long),
        typeof(PaginationControl),
        new PropertyMetadata(0L, (d, e) => (d as PaginationControl)?.UpdateButtons()));

    public static readonly DependencyProperty NotifyProperty = DependencyProperty.Register(
        nameof(Notify),
        typeof(string),
        typeof(PaginationControl),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IsPreviousPageButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsPreviousPageButtonEnabled),
        typeof(bool),
        typeof(PaginationControl),
        new PropertyMetadata(true));

    public static readonly DependencyProperty IsNextPageButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsNextPageButtonEnabled),
        typeof(bool),
        typeof(PaginationControl),
        new PropertyMetadata(true));

    private bool _isInitialized = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationControl"/> class.
    /// </summary>
    public PaginationControl()
    {
        this.InitializeComponent();
        this.Loaded += PaginationControl_Loaded;
        UpdateButtons();
    }

    /// <summary>
    /// Handles the Loaded event of the PaginationControl.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void PaginationControl_Loaded(object sender, RoutedEventArgs e)
    {
        _isInitialized = true;
    }

    /// <summary>
    /// Occurs when the page changes.
    /// </summary>
    public event TypedEventHandler<PaginationControl, PaginationControlValueChangedEventArgs>? PageChanged;

    /// <summary>
    /// Gets or sets a value indicating whether the previous page button is enabled.
    /// </summary>
    public bool IsPreviousPageButtonEnabled
    {
        get => (bool)GetValue(IsPreviousPageButtonEnabledProperty);
        set => SetValue(IsPreviousPageButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the next page button is enabled.
    /// </summary>
    public bool IsNextPageButtonEnabled
    {
        get => (bool)GetValue(IsNextPageButtonEnabledProperty);
        set => SetValue(IsNextPageButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the current page.
    /// </summary>
    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum page.
    /// </summary>
    public int MinPage
    {
        get => (int)GetValue(MinPageProperty);
        set => SetValue(MinPageProperty, value);
    }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPage
    {
        get => (int)GetValue(TotalPageProperty);
        set => SetValue(TotalPageProperty, value);
    }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PerPage
    {
        get => (int)GetValue(PerPageProperty);
        set => SetValue(PerPageProperty, value);
    }

    /// <summary>
    /// Gets or sets the total number of records.
    /// </summary>
    public long TotalRecord
    {
        get => (long)GetValue(TotalRecordProperty);
        set => SetValue(TotalRecordProperty, value);
    }

    /// <summary>
    /// Gets or sets the per page options as a comma-separated string.
    /// </summary>
    public string PerPages { get; set; } = "5,10,20,50";

    private List<int> _PerPages => PerPages.Split(',').Select(int.Parse).ToList();

    private string Notify
    {
        get => (string)GetValue(NotifyProperty);
        set => SetValue(NotifyProperty, value);
    }

    /// <summary>
    /// Updates the state of the pagination buttons.
    /// </summary>
    private void UpdateButtons()
    {
        IsNextPageButtonEnabled = CurrentPage < TotalPage;
        IsPreviousPageButtonEnabled = CurrentPage > MinPage;
        var maxRecordShow = CurrentPage * PerPage > TotalRecord ? TotalRecord : CurrentPage * PerPage;
        Notify = $"{CurrentPage * PerPage - PerPage + 1} - {maxRecordShow} of {TotalRecord}";
    }

    /// <summary>
    /// Handles the click event of the previous page button.
    /// </summary>
    private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
    {
        SetValue(CurrentPageProperty, Math.Max(CurrentPage - 1, MinPage));
    }

    /// <summary>
    /// Handles the click event of the next page button.
    /// </summary>
    private void NextPageButton_Click(object sender, RoutedEventArgs e)
    {
        SetValue(CurrentPageProperty, Math.Min(CurrentPage + 1, TotalPage));
    }

    /// <summary>
    /// Handles the value changed event of the current page number box.
    /// </summary>
    private void CurrentPageNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (!_isInitialized) return;
        SetValue(CurrentPageProperty, (int)args.NewValue);
        PageChanged?.Invoke(this, new PaginationControlValueChangedEventArgs(PerPage, (int)args.NewValue));
        UpdateButtons();
    }

    /// <summary>
    /// Handles the click event of the first page button.
    /// </summary>
    private void FistPageButton_Click(object sender, RoutedEventArgs e)
    {
        SetValue(CurrentPageProperty, MinPage);
    }

    /// <summary>
    /// Handles the click event of the last page button.
    /// </summary>
    private void LastPageButton_Click(object sender, RoutedEventArgs e)
    {
        SetValue(CurrentPageProperty, TotalPage);
    }

    /// <summary>
    /// Handles the selection changed event of the per page combo box.
    /// </summary>
    private void PerPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_isInitialized) return;
        if (PerPageComboBox.SelectedItem is int selectedPerPage)
        {
            SetValue(PerPageProperty, selectedPerPage);
            PageChanged?.Invoke(this, new PaginationControlValueChangedEventArgs(PerPage, CurrentPage));
            UpdateButtons();
        }
    }
}

/// <summary>
/// Provides data for the PageChanged event.
/// </summary>
public sealed class PaginationControlValueChangedEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationControlValueChangedEventArgs"/> class.
    /// </summary>
    /// <param name="CurrentPage">The current page.</param>
    /// <param name="PerPage">The number of items per page.</param>
    public PaginationControlValueChangedEventArgs(int CurrentPage, int PerPage)
    {
        this.CurrentPage = CurrentPage;
        this.PerPage = PerPage;
    }

    /// <summary>
    /// Gets the current page.
    /// </summary>
    public int CurrentPage { get; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PerPage { get; }
}
