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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SipPOS.Controls;

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

    public PaginationControl()
    {
        this.InitializeComponent();
        this.Loaded += PaginationControl_Loaded;
        UpdateButtons();
    }

    private void PaginationControl_Loaded(object sender, RoutedEventArgs e)
    {
        _isInitialized = true;
    }

    public event TypedEventHandler<PaginationControl, PaginationControlValueChangedEventArgs>? PageChanged;

    public bool IsPreviousPageButtonEnabled
    {
        get => (bool)GetValue(IsPreviousPageButtonEnabledProperty);
        set => SetValue(IsPreviousPageButtonEnabledProperty, value);
    }

    public bool IsNextPageButtonEnabled
    {
        get => (bool)GetValue(IsNextPageButtonEnabledProperty);
        set => SetValue(IsNextPageButtonEnabledProperty, value);
    }

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public int MinPage
    {
        get => (int)GetValue(MinPageProperty);
        set => SetValue(MinPageProperty, value);
    }

    public int TotalPage
    {
        get => (int)GetValue(TotalPageProperty);
        set => SetValue(TotalPageProperty, value);
    }

    public int PerPage
    {
        get => (int)GetValue(PerPageProperty);
        set => SetValue(PerPageProperty, value);
    }

    public long TotalRecord
    {
        get => (long)GetValue(TotalRecordProperty);
        set => SetValue(TotalRecordProperty, value);
    }

    public string PerPages { get; set; } = "5,10,20,50";

    private List<int> _PerPages => PerPages.Split(',').Select(int.Parse).ToList();

    private string Notify
    {
        get => (string)GetValue(NotifyProperty);
        set => SetValue(NotifyProperty, value);
    }

    private void UpdateButtons()
    {
        IsNextPageButtonEnabled = CurrentPage < TotalPage;
        IsPreviousPageButtonEnabled = CurrentPage > MinPage;
        var maxRecordShow = CurrentPage * PerPage > TotalRecord ? TotalRecord : CurrentPage * PerPage;
        Notify = $"{CurrentPage * PerPage - PerPage + 1} - {maxRecordShow} của {TotalRecord}";
    }

    private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
    {
        SetValue(CurrentPageProperty, Math.Max(CurrentPage - 1, MinPage));
    }

    private void NextPageButton_Click(object sender, RoutedEventArgs e)
    {
        SetValue(CurrentPageProperty, Math.Min(CurrentPage + 1, TotalPage));
    }

    private void CurrentPageNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (!_isInitialized) return;
        SetValue(CurrentPageProperty, (int)args.NewValue);
        PageChanged?.Invoke(this, new PaginationControlValueChangedEventArgs(PerPage, (int)args.NewValue));
        UpdateButtons();
    }

    private void FistPageButton_Click(object sender, RoutedEventArgs e)
    {
        SetValue(CurrentPageProperty, MinPage);
    }

    private void LastPageButton_Click(object sender, RoutedEventArgs e)
    {
        SetValue(CurrentPageProperty, TotalPage);
    }

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

public sealed class PaginationControlValueChangedEventArgs
{
public PaginationControlValueChangedEventArgs(int CurrentPage, int PerPage)
{
    this.CurrentPage = CurrentPage;
    this.PerPage = PerPage;
}

public int CurrentPage { get; }

public int PerPage { get; }
}