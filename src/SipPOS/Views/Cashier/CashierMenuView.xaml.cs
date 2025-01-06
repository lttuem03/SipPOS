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

/// <summary>
/// Represents the view for the cashier menu.
/// </summary>
public sealed partial class CashierMenuView : Page
{
    private bool isDraggingCategoryList = false;
    private Windows.Foundation.Point lastPointerPosition;

    /// <summary>
    /// Gets the view model for the cashier menu.
    /// </summary>
    public CashierMenuViewModel ViewModel { get; }

    private static readonly DispatcherTimer _cashierMenuTimer = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CashierMenuView"/> class.
    /// </summary>
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

    /// <summary>
    /// Handles the click event for the close pane button.
    /// Moves the navigation pane canvas to the left, hiding it from view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void closePaneButton_Click(object sender, RoutedEventArgs e)
    {
        Canvas.SetLeft(navigationPaneCanvas, -400);
    }

    /// <summary>
    /// Handles the click event for the open pane button.
    /// Moves the navigation pane canvas to the right, making it visible.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void openPaneButton_Click(object sender, RoutedEventArgs e)
    {
        Canvas.SetLeft(navigationPaneCanvas, 0);
    }

    /// <summary>
    /// Handles the click event for the button that navigates to the main menu.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void toMainMenuButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }

    /// <summary>
    /// Handles the click event for the button that navigates to the invoice history.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void toInvoiceHistoryButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(InvoiceHistoryView));
    }

    /// <summary>
    /// Handles the selection changed event for the category browsing GridView.
    /// Updates the ViewModel with the selected category.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void categoryBrowsingGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not GridView categoryBrowsingGridView)
            return;

        if (categoryBrowsingGridView.SelectedItem is not Category selectedCategory)
            return;

        ViewModel.HandleCategoryBrowsingGridViewSelectionChanged(selectedCategory);
    }

    /// <summary>
    /// Handles the text changed event for the item search text box.
    /// Updates the ViewModel with the new search text.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void itemSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox itemSearchTextBox)
            return;

        ViewModel.HandleItemSearchTextBoxTextChanged(itemSearchTextBox.Text);
    }

    /// <summary>
    /// Handles the click event for the add item to order button.
    /// Adds the selected product to the order and updates the UI accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the click event for the remove item from order button.
    /// Removes the selected item from the order and updates the UI accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the click event for the note suggestion button.
    /// Appends the suggestion to the note of the currently edited order item.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void noteSuggestionButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button noteSuggestionButton)
            return;

        if (ViewModel.EditNoteTarget == null)
            return;

        editNoteSuggestionTeachingTip.IsOpen = false;

        ViewModel.EditNoteTarget.Note += noteSuggestionButton.Content.ToString();
    }

    /// <summary>
    /// Handles the got focus event for the edit note text box.
    /// Opens the suggestion teaching tip and sets the target for note editing.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void editNoteTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox editNoteTextBox)
            return;

        if (editNoteTextBox.DataContext is not InvoiceItemDto orderItemDto)
            return;

        editNoteSuggestionTeachingTip.IsOpen = true;

        ViewModel.EditNoteTarget = orderItemDto;
    }

    /// <summary>
    /// Handles the pointer wheel changed event for the category browsing scroll view.
    /// Adjusts the horizontal scroll position based on the mouse wheel delta.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the pointer pressed event for the category browsing touch screen.
    /// Initiates the dragging operation for the category list.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void categoryBrowsingTouchScreen_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        isDraggingCategoryList = true;
        lastPointerPosition = e.GetCurrentPoint(categoryBrowsingScrollView).Position;
    }

    /// <summary>
    /// Handles the pointer moved event for the category browsing touch screen.
    /// Adjusts the horizontal scroll position based on the pointer movement.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the pointer released event for the category browsing touch screen.
    /// Ends the dragging operation for the category list.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void categoryBrowsingTouchScreen_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        isDraggingCategoryList = false;
    }

    /// <summary>
    /// Handles the selection changed event for the product options combo box.
    /// Updates the selected option for the product.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the click event for the cancel order button.
    /// Cancels the current order and updates the UI accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void cancelOrderButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleCancelOrderButtonClick(cancelOrderConfimationContentDialog);

        openPaymentDialogButton.IsEnabled = false;
        cancelOrderButton.IsEnabled = false;
    }

    /// <summary>
    /// Handles the click event for the open payment dialog button.
    /// Displays the payment content dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void openPaymentDialogButton_Click(object sender, RoutedEventArgs e)
    {
        _ = await paymentContentDialog.ShowAsync();
    }

    /// <summary>
    /// Handles the click event for the close payment dialog button.
    /// Resets the payment monetary details and hides the payment content dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void closePaymentDialogButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ResetPaymentMonetaryDetails();
        paymentContentDialog.Hide();
    }

    /// <summary>
    /// Handles the selection changed event for the payment method radio buttons.
    /// Updates the ViewModel with the selected payment method and adjusts the UI accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the click event for the numpad add amount button.
    /// Adds the specified amount to the payment and updates the validation text block.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

        proceedWithPaymentButton.IsEnabled = string.IsNullOrEmpty(amountPaidValidationTextBlock.Text);
    }

    /// <summary>
    /// Handles the click event for the numpad number button.
    /// Adds the specified number to the payment and updates the validation text block.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void numpadNumberButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button numberButton)
            return;

        if (numberButton.Content is not TextBlock numberTextBlock)
            return;

        var numberText = numberTextBlock.Text;

        ViewModel.HandleNumpadNumberButtonClick(numberText);

        amountPaidValidationTextBlock.Text = ViewModel.ValidatePaymentMonetaryDetails();

        proceedWithPaymentButton.IsEnabled = string.IsNullOrEmpty(amountPaidValidationTextBlock.Text);
    }

    /// <summary>
    /// Handles the click event for the numpad clear amount button.
    /// Clears the payment amount and updates the validation text block.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void numpadClearAmountButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleNumpadClearAmountButtonClick();

        amountPaidValidationTextBlock.Text = ViewModel.ValidatePaymentMonetaryDetails();

        proceedWithPaymentButton.IsEnabled = string.IsNullOrEmpty(amountPaidValidationTextBlock.Text);
    }

    /// <summary>
    /// Handles the click event for the numpad backspace button.
    /// Removes the last digit from the payment amount and updates the validation text block.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void numpadBackspaceButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleNumpadBackspaceButtonClick();

        amountPaidValidationTextBlock.Text = ViewModel.ValidatePaymentMonetaryDetails();

        proceedWithPaymentButton.IsEnabled = string.IsNullOrEmpty(amountPaidValidationTextBlock.Text);
    }

    /// <summary>
    /// Handles the click event for the proceed with payment button.
    /// Proceeds with the payment using the selected payment method and hides the payment content dialog.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void proceedWithPaymentButton_Click(object sender, RoutedEventArgs e)
    {
        // WITH CASH PAYMENT METHOD
        await ViewModel.ProceedWithPayment();
        paymentContentDialog.Hide();
    }

    /// <summary>
    /// Handles the click event for the create QR payment code button.
    /// Generates a QR payment code and updates the UI to show QR payment controls.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the click event for the cancel QR pay operation button.
    /// Cancels the QR payment operation and resets the UI to its initial state.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void cancelQrPayOperationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleQrPaymentTimeout();
        paymentMethodRadioButtons.IsEnabled = true;
        closePaymentDialogButton.IsEnabled = true;
        cancelQrPayOperationButton.IsEnabled = true;
        hideQrPaymentControls();
        createQrPaymentCodeButton.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Shows the QR payment controls in the UI.
    /// </summary>
    private void showQrPaymentControls()
    {
        qrPayInstructionTextBlock.Visibility = Visibility.Visible;
        qrPayCodeBorder.Visibility = Visibility.Visible;
        qrPayMetadata.Visibility = Visibility.Visible;
        qrPayPrintCodeButton.Visibility = Visibility.Visible;
        qrPayAboutTextBlockAndCancelButton.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Hides the QR payment controls in the UI.
    /// </summary>
    private void hideQrPaymentControls()
    {
        qrPayInstructionTextBlock.Visibility = Visibility.Collapsed;
        qrPayCodeBorder.Visibility = Visibility.Collapsed;
        qrPayMetadata.Visibility = Visibility.Collapsed;
        qrPayPrintCodeButton.Visibility = Visibility.Collapsed;
        qrPayAboutTextBlockAndCancelButton.Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Handles the click event for the apply coupon button.
    /// Applies the coupon code from the button's data context and updates the UI accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the getting focus event for the apply coupon text box.
    /// Opens the coupon suggestion teaching tip.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void applyCouponTextBox_GettingFocus(UIElement sender, GettingFocusEventArgs args)
    {
        couponSuggestionTeachingTip.IsOpen = true;
    }

    /// <summary>
    /// Handles the losing focus event for the apply coupon text box.
    /// Validates the coupon code and updates the UI accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void applyCouponTextBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
    {
        //var doesCouponExist = ViewModel.HandleApplyHiddenCoupon(applyCouponTextBox.Text);
        //
        //if (!doesCouponExist)
        //{
        //    couponNotApplicableWarningTextBlock.Opacity = 1.0F;
        //}
        //else
        //{
        //    couponNotApplicableWarningTextBlock.Opacity = 0.0F;
        //}
    }

    /// <summary>
    /// Handles the click event for the apply hidden coupon button.
    /// Validates the hidden coupon code and updates the UI accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the click event for the change staff ID button.
    /// Triggers the ViewModel to handle the staff ID change.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void changeStaffIdButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleChangeStaffIdButtonClick();
    }
}