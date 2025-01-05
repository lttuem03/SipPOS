using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.General;
using SipPOS.ViewModels.Cashier;

namespace SipPOS.Views.Cashier;

public sealed partial class InvoiceHistoryView : Page
{
    public InvoiceHistoryViewModel ViewModel { get; }

    public InvoiceHistoryView()
    {
        this.InitializeComponent();
        ViewModel = new();

        var openingTime = ViewModel.CurrentConfiguration.OpeningTime;
        var closingTime = ViewModel.CurrentConfiguration.ClosingTime;

        dateCalendarDatePicker.Date = new DateTimeOffset(DateTime.Now);
        fromTimePicker.Time = new TimeSpan(openingTime.Hour, openingTime.Minute, openingTime.Second);
        toTimePicker.Time = new TimeSpan(closingTime.Hour, closingTime.Minute, closingTime.Second);
    }

    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }

    private void toCashierMenuViewButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(CashierMenuView));
    }

    private async void dateCalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        if (dateCalendarDatePicker.Date == null)
            return;

        await ViewModel.HandleDateCalendarDatePickerDateChanged(dateCalendarDatePicker.Date.Value.Date);

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    private async void fromTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        await ViewModel.HandleFromTimePickerTimeChanged(fromTimePicker.Time);

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    private async void toTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        await ViewModel.HandleToTimePickerTimeChanged(toTimePicker.Time);

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    private async void previousPageButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.HandlePreviousPageButtonClick();

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    private async void nextPageButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.HandleNextPageButtonClick();

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    private async void rowsPerPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        await ViewModel.HandleRowsPerPageComboBoxSelectionChanged(rowsPerPageComboBox.SelectedIndex);

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    private void invoiceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        if (sender is not ListView invoiceListView)
            return;

        ViewModel.HandleInvoiceListViewSelectionChanged(invoiceListView);
    }
}
