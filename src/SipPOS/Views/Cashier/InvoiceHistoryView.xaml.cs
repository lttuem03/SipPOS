using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.General;
using SipPOS.ViewModels.Cashier;

namespace SipPOS.Views.Cashier;

/// <summary>
/// Represents the view for displaying invoice history.
/// </summary>
public sealed partial class InvoiceHistoryView : Page
{
    public InvoiceHistoryViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceHistoryView"/> class.
    /// Sets up the ViewModel and initializes the date and time pickers with the current configuration.
    /// </summary>
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

    /// <summary>  
    /// Handles the click event for the go back button.  
    /// Navigates to the main menu view.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">The event data.</param>  
    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }

    /// <summary>  
    /// Handles the click event for the to cashier menu view button.  
    /// Navigates to the cashier menu view.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">The event data.</param>  
    private void toCashierMenuViewButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(CashierMenuView));
    }

    /// <summary>  
    /// Handles the date changed event for the date calendar date picker.  
    /// Updates the ViewModel with the selected date and refreshes the order item list view.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="args">The event data.</param>  
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

    /// <summary>  
    /// Handles the time changed event for the from time picker.  
    /// Updates the ViewModel with the selected time and refreshes the order item list view.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">The event data.</param>  
    private async void fromTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        await ViewModel.HandleFromTimePickerTimeChanged(fromTimePicker.Time);

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    /// <summary>  
    /// Handles the time changed event for the to time picker.  
    /// Updates the ViewModel with the selected time and refreshes the order item list view.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">The event data.</param>  
    private async void toTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        await ViewModel.HandleToTimePickerTimeChanged(toTimePicker.Time);

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    /// <summary>  
    /// Handles the click event for the previous page button.  
    /// Navigates to the previous page of invoices and refreshes the order item list view.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">The event data.</param>  
    private async void previousPageButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.HandlePreviousPageButtonClick();

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    /// <summary>  
    /// Handles the click event for the next page button.  
    /// Navigates to the next page of invoices and refreshes the order item list view.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">The event data.</param>  
    private async void nextPageButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.HandleNextPageButtonClick();

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    /// <summary>  
    /// Handles the selection changed event for the rows per page combo box.  
    /// Updates the ViewModel with the selected number of rows per page and refreshes the order item list view.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">The event data.</param>  
    private async void rowsPerPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        await ViewModel.HandleRowsPerPageComboBoxSelectionChanged(rowsPerPageComboBox.SelectedIndex);

        if (ViewModel.TotalRowsCount != 0)
            orderItemListView.SelectedIndex = 0;
    }

    /// <summary>  
    /// Handles the selection changed event for the invoice list view.  
    /// Updates the ViewModel with the selected invoice.  
    /// </summary>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">The event data.</param>  
    private void invoiceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel is not InvoiceHistoryViewModel)
            return;

        if (sender is not ListView invoiceListView)
            return;

        ViewModel.HandleInvoiceListViewSelectionChanged(invoiceListView);
    }
}
