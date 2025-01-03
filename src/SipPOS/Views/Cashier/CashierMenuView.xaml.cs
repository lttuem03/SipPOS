using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

using SipPOS.Views.General;
using SipPOS.Models.Entity;
using SipPOS.ViewModels.Cashier;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Views.Cashier;

public sealed partial class CashierMenuView : Page
{
    private bool isDraggingCategoryList = false;
    private Windows.Foundation.Point lastPointerPosition;

    public CashierMenuViewModel ViewModel { get; }

    private static readonly DispatcherTimer _cashierMenuTimer = new();

    public CashierMenuView()
    {
        this.InitializeComponent();
        ViewModel = new();

        var now = DateTime.Now;

        currentTimeTextBlock.Text = now.ToString("HH:mm:ss");
        currentDateTextBlock.Text = now.ToString("dd/MM/yyyy");

        // Because the timer needs to be setup in the UI thread, it is done here.
        _cashierMenuTimer.Interval = TimeSpan.FromSeconds(1);
        _cashierMenuTimer.Tick += (sender, e) =>
        {
            ViewModel.CurrentTime = ViewModel.CurrentTime.AddSeconds(1);
            currentTimeTextBlock.Text = ViewModel.CurrentTime.ToString("HH:mm:ss");
            currentDateTextBlock.Text = ViewModel.CurrentTime.ToString("dd/MM/yyyy");
        };
        _cashierMenuTimer.Start();
    }

    private void closePaneButton_Click(object sender, RoutedEventArgs e)
    {
        Canvas.SetLeft(navigationPaneCanvas, -400);
    }

    private void openPaneButton_Click(object sender, RoutedEventArgs e)
    {
        Canvas.SetLeft(navigationPaneCanvas, 0);
    }

    private void toMainMenuButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }

    private void toOrderHistoryButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void categoryBrowsingGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not GridView categoryBrowsingGridView)
            return;

        if (categoryBrowsingGridView.SelectedItem is not Category selectedCategory)
            return;

        ViewModel.HandleCategoryBrowsingGridViewSelectionChanged(selectedCategory);
    }

    private void itemSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox itemSearchTextBox)
            return;

        ViewModel.HandleItemSearchTextBoxTextChanged(itemSearchTextBox.Text);
    }

    private void addItemToOrderButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button addItemToOrderButton)
            return;

        if (addItemToOrderButton.DataContext is not Product productItem)
            return;

        var newInvoiceItemDto = ViewModel.HandleAddItemToOrderButtonClick(productItem);

        orderItemListView.ScrollIntoView(newInvoiceItemDto);

        if (ViewModel.InvoiceItems.Count > 0)
        {
            openPaymentDialogButton.IsEnabled = true;
            cancelOrderButton.IsEnabled = true;
        }
    }

    private void removeItemFromOrderButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button removeItemFromOrderButton)
            return;

        if (removeItemFromOrderButton.DataContext is not InvoiceItemDto invoiceItemDto)
            return;

        ViewModel.HandleRemoveItemFromOrderButtonClick(invoiceItemDto);

        if (ViewModel.InvoiceItems.Count == 0)
        {
            openPaymentDialogButton.IsEnabled = false;
            cancelOrderButton.IsEnabled = false;
        }
    }

    private void noteSuggestionButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button noteSuggestionButton)
            return;

        if (ViewModel.EditNoteTarget == null)
            return;

        editNoteSuggestionTeachingTip.IsOpen = false;

        ViewModel.EditNoteTarget.Note += noteSuggestionButton.Content.ToString();
    }

    private void editNoteTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox editNoteTextBox)
            return;

        if (editNoteTextBox.DataContext is not InvoiceItemDto orderItemDto)
            return;

        editNoteSuggestionTeachingTip.IsOpen = true;

        ViewModel.EditNoteTarget = orderItemDto;
    }

    private void categoryBrowsingScrollView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not ScrollView scrollView)
            return;

        var pointer = e.GetCurrentPoint(scrollView);
        var delta = pointer.Properties.MouseWheelDelta;

        // Adjust the horizontal scroll position
        scrollView.ScrollBy(-delta, 0);

        // Mark the event as handled to prevent vertical scrolling
        e.Handled = true;
    }

    private void categoryBrowsingTouchScreen_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        isDraggingCategoryList = true;
        lastPointerPosition = e.GetCurrentPoint(categoryBrowsingScrollView).Position;
    }

    private void categoryBrowsingTouchScreen_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!isDraggingCategoryList)
            return;

        var currentPointerPosition = e.GetCurrentPoint(categoryBrowsingScrollView).Position;
        var deltaX = currentPointerPosition.X - lastPointerPosition.X;

        // Adjust the horizontal scroll position
        categoryBrowsingScrollView.ScrollBy(-deltaX, 0);

        lastPointerPosition = currentPointerPosition;
    }

    private void categoryBrowsingTouchScreen_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        isDraggingCategoryList = false;
    }

    private void productOptionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox productOptionsComboBox)
            return;

        if (productOptionsComboBox.SelectedItem is (string name, decimal price))
        {
            if (productOptionsComboBox.DataContext is not Product product)
                return;

            product.SelectedOption = (name, price);
        }
    }

    private void cancelOrderButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleCancelOrderButtonClick(cancelOrderConfimationContentDialog);

        openPaymentDialogButton.IsEnabled = false;
        cancelOrderButton.IsEnabled = false;
    }

    private async void openPaymentDialogButton_Click(object sender, RoutedEventArgs e)
    {
        _ = await paymentContentDialog.ShowAsync();
    }

    private void closePaymentDialogButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ResetPaymentMonetaryDetails();

        paymentContentDialog.Hide();
    }

    private void paymentMethodRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
            return;

        if (sender is not RadioButtons radioButtons)
            return;

        ViewModel.HandlePaymentMethodRadioButtonsSelectionChanged(radioButtons.SelectedIndex);

        switch (radioButtons.SelectedIndex)
        {
            case 0:
                cashPaymentMethodGrid.Visibility = Visibility.Visible;
                qrPaymentMethodGrid.Visibility = Visibility.Collapsed;
                break;

            case 1:
                qrPaymentMethodGrid.Visibility = Visibility.Visible;
                cashPaymentMethodGrid.Visibility = Visibility.Collapsed;
                break;
        }
    }

    private void numpadAddAmountButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button addAmountButton)
            return;

        if (addAmountButton.Content is not TextBlock amountTextBlock)
            return;

        var amountText = amountTextBlock.Text;

        amountText = amountText.Replace("+", string.Empty);
        amountText = amountText.Replace("K", string.Empty);

        var amountDecimal = decimal.Parse(amountText);

        amountDecimal *= 1000;

        ViewModel.HandleNumpadAddAmountButtonClick(amountDecimal);

        amountPaidValidationTextBlock.Text = ViewModel.ValidatePaymentMonetaryDetails();

        if (amountPaidValidationTextBlock.Text == "")
            proceedWithPaymentButton.IsEnabled = true;
        else
            proceedWithPaymentButton.IsEnabled = false;
    }

    private void numpadNumberButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button numberButton)
            return;

        if (numberButton.Content is not TextBlock numberTextBlock)
            return;

        var numberText = numberTextBlock.Text;

        ViewModel.HandleNumpadNumberButtonClick(numberText);

        amountPaidValidationTextBlock.Text = ViewModel.ValidatePaymentMonetaryDetails();

        if (amountPaidValidationTextBlock.Text == "")
            proceedWithPaymentButton.IsEnabled = true;
        else
            proceedWithPaymentButton.IsEnabled = false;
    }

    private void numpadClearAmountButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleNumpadClearAmountButtonClick();

        amountPaidValidationTextBlock.Text = ViewModel.ValidatePaymentMonetaryDetails();

        if (amountPaidValidationTextBlock.Text == "")
            proceedWithPaymentButton.IsEnabled = true;
        else
            proceedWithPaymentButton.IsEnabled = false;
    }

    private void numpadBackspaceButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleNumpadBackspaceButtonClick();

        amountPaidValidationTextBlock.Text = ViewModel.ValidatePaymentMonetaryDetails();

        if (amountPaidValidationTextBlock.Text == "")
            proceedWithPaymentButton.IsEnabled = true;
        else
            proceedWithPaymentButton.IsEnabled = false;
    }

    private void proceedWithPaymentButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleProceedWithPaymentButtonClick();

        paymentContentDialog.Hide();
    }
}