using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

using SipPOS.Views.General;
using SipPOS.Models.Entity;
using SipPOS.ViewModels.Cashier;
using SipPOS.DataTransfer.Entity;
using Windows.Gaming.Input.ForceFeedback;

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
        currentDateTextBlock.Text = now.ToString("dddd, dd/MM/yyyy");

        // Because the timer needs to be setup in the UI thread, it is done here.
        _cashierMenuTimer.Interval = TimeSpan.FromSeconds(1);
        _cashierMenuTimer.Tick += async (sender, e) =>
        {
            // Update the current time every second
            ViewModel.CurrentTime = ViewModel.CurrentTime.AddSeconds(1);
            currentTimeTextBlock.Text = ViewModel.CurrentTime.ToString("HH:mm:ss");
            currentDateTextBlock.Text = ViewModel.CurrentTime.ToString("dddd, dd/MM/yyyy");

            // If there is a countdown for any operation, update it

            // Countdown for processing QR-Pay payment
            if (ViewModel.QrPaySecondsRemaining > 0)
            {
                ViewModel.QrPaySecondsRemaining -= 1;

                // Check if the payment has been completed
                if (await ViewModel.CheckQrPaymentCompleted())
                {
                    // proceed payment logic handled in CheckQrPaymentCompleted()

                    qrPaymentTimeoutErrorMessage.Opacity = 0.0F;
                    paymentContentDialog.Hide();
                }

                // Check if timeout
                if (ViewModel.QrPayOrderCode != 0 && ViewModel.QrPaySecondsRemaining == 0)
                {
                    ViewModel.HandleQrPaymentTimeout();

                    // update UI
                    paymentMethodRadioButtons.IsEnabled = true;
                    closePaymentDialogButton.IsEnabled = true;
                    cancelQrPayOperationButton.IsEnabled = true;
                    hideQrPaymentControls();

                    createQrPaymentCodeButton.Visibility = Visibility.Visible;

                    // show error message
                    qrPaymentTimeoutErrorMessage.Opacity = 1.0F;
                }
            }

            // Countdown for cancel QR Pay operation
            if (ViewModel.CancelQrPayOperationCountDownTimeSpan > TimeSpan.Zero)
            {
                ViewModel.CancelQrPayOperationCountDownTimeSpan = ViewModel.CancelQrPayOperationCountDownTimeSpan.Subtract(TimeSpan.FromSeconds(1));

                if (ViewModel.CancelQrPayOperationCountDownTimeSpan > TimeSpan.Zero)
                {
                    cancelQrPayOperationButtonTextBlock.Text = $"Hủy ({ViewModel.CancelQrPayOperationCountDownTimeSpan.TotalSeconds}s)";
                }
                else
                {
                    cancelQrPayOperationButtonTextBlock.Text = "Hủy";
                    cancelQrPayOperationButton.IsEnabled = true;
                }
            }
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

    private void toInvoiceHistoryButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(InvoiceHistoryView));
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

        var newInvoiceItemDto = ViewModel.HandleAddItemToOrderButtonClick(productItem, couponNotApplicableWarningTextBlock);

        orderItemListView.ScrollIntoView(newInvoiceItemDto);

        if (ViewModel.InvoiceItems.Count > 0)
        {
            openPaymentDialogButton.IsEnabled = true;
            cancelOrderButton.IsEnabled = true;
            applyCouponTextBox.IsEnabled = true;
            applyHiddenCouponButton.IsEnabled = true;
            couponSuggestionTeachingTip.IsEnabled = true;
        }

        couponSuggestionTeachingTip.IsOpen = false;
    }

    private void removeItemFromOrderButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button removeItemFromOrderButton)
            return;

        if (removeItemFromOrderButton.DataContext is not InvoiceItemDto invoiceItemDto)
            return;

        ViewModel.HandleRemoveItemFromOrderButtonClick(invoiceItemDto, couponNotApplicableWarningTextBlock);

        if (ViewModel.InvoiceItems.Count == 0)
        {
            openPaymentDialogButton.IsEnabled = false;
            cancelOrderButton.IsEnabled = false;
            applyCouponTextBox.IsEnabled = false;
            applyHiddenCouponButton.IsEnabled = false;
            couponSuggestionTeachingTip.IsEnabled = false;
            couponNotApplicableWarningTextBlock.Opacity = 0.0F;
        }

        couponSuggestionTeachingTip.IsOpen = false;
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
                hideQrPaymentControls();
                createQrPaymentCodeButton.Visibility = Visibility.Visible;
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

    private async void proceedWithPaymentButton_Click(object sender, RoutedEventArgs e)
    {
        // WITH CASH PAYMENT METHOD
        await ViewModel.ProceedWithPayment();

        paymentContentDialog.Hide();
    }

    private async void createQrPaymentCodeButton_Click(object sender, RoutedEventArgs e)
    {
        createQrPaymentCodeButton.Visibility = Visibility.Collapsed;

        await ViewModel.HandleCreateQrPaymentCodeButtonClick();

        showQrPaymentControls();
        paymentMethodRadioButtons.IsEnabled = false;
        closePaymentDialogButton.IsEnabled = false;
        cancelQrPayOperationButton.IsEnabled = false;
        ViewModel.CancelQrPayOperationCountDownTimeSpan = ViewModel.CancelQrPayOperationCountDownTimeSpan.Add(TimeSpan.FromSeconds(60));
    }

    private void cancelQrPayOperationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleQrPaymentTimeout();

        paymentMethodRadioButtons.IsEnabled = true;
        closePaymentDialogButton.IsEnabled = true;
        cancelQrPayOperationButton.IsEnabled = true;
        hideQrPaymentControls();

        createQrPaymentCodeButton.Visibility = Visibility.Visible;
    }

    private void showQrPaymentControls()
    {
        qrPayInstructionTextBlock.Visibility = Visibility.Visible;
        qrPayCodeBorder.Visibility = Visibility.Visible;
        qrPayMetadata.Visibility = Visibility.Visible;
        qrPayPrintCodeButton.Visibility = Visibility.Visible;
        qrPayAboutTextBlockAndCancelButton.Visibility = Visibility.Visible;
    }

    private void hideQrPaymentControls()
    {
        qrPayInstructionTextBlock.Visibility = Visibility.Collapsed;
        qrPayCodeBorder.Visibility = Visibility.Collapsed;
        qrPayMetadata.Visibility = Visibility.Collapsed;
        qrPayPrintCodeButton.Visibility = Visibility.Collapsed;
        qrPayAboutTextBlockAndCancelButton.Visibility = Visibility.Collapsed;
    }


    private void applyCouponButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button applyCouponButton)
            return;

        if (applyCouponButton.DataContext is not SpecialOffer specialOffer)
            return;

        applyCouponTextBox.Text = specialOffer.Code;

        var isCouponApplicable = ViewModel.HandleApplyCouponButtonClick(specialOffer);

        couponSuggestionTeachingTip.IsOpen = false;

        if (!isCouponApplicable)
        {
            couponNotApplicableWarningTextBlock.Opacity = 1.0F;
        }
        else
            couponNotApplicableWarningTextBlock.Opacity = 0.0F;
    }

    private void applyCouponTextBox_GettingFocus(UIElement sender, GettingFocusEventArgs args)
    {
        couponSuggestionTeachingTip.IsOpen = true;
    }

    private void applyCouponTextBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
    {
        var doesCouponExist = ViewModel.HandleApplyHiddenCoupon(applyCouponTextBox.Text);

        if (!doesCouponExist)
        {
            couponNotApplicableWarningTextBlock.Opacity = 1.0F;
        }
        else
        {
            couponNotApplicableWarningTextBlock.Opacity = 0.0F;
        }
    }

    private void applyHiddenCouponButton_Click(object sender, RoutedEventArgs e)
    {
        var doesCouponExist = ViewModel.HandleApplyHiddenCoupon(applyCouponTextBox.Text);

        if (!doesCouponExist)
        {
            couponNotApplicableWarningTextBlock.Opacity = 1.0F;
        }
        else
        {
            couponNotApplicableWarningTextBlock.Opacity = 0.0F;
        }
    }

    private void changeStaffIdButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleChangeStaffIdButtonClick();
    }
}