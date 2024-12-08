using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SipPOS.Resources.Controls;

/// <summary>
/// A user control that provides an editable text field with save and cancel functionality.
/// </summary>
public sealed partial class EditableTextField : UserControl
{
    /// <summary>
    /// Identifies the Text dependency property.
    /// </summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(EditableTextField),
            new PropertyMetadata(string.Empty)
        );

    /// <summary>
    /// Identifies the PlaceholderText dependency property.
    /// </summary>
    public static readonly DependencyProperty PlaceholderTextProperty =
        DependencyProperty.Register(
            "PlaceholderText",
            typeof(string),
            typeof(EditableTextField),
            new PropertyMetadata(string.Empty)
        );

    private string _originalText;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditableTextField"/> class.
    /// </summary>
    public EditableTextField()
    {
        this.InitializeComponent();
        this.DataContext = this;
        _originalText = "";
    }

    /// <summary>
    /// Gets or sets the text of the editable text field.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the placeholder text of the editable text field.
    /// </summary>
    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    /// <summary>
    /// Occurs when the text is modified.
    /// </summary>
    public event EventHandler? TextModified;

    /// <summary>
    /// Handles the click event of the Edit button.
    /// Enables editing of the text field.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        _originalText = Text;
        TextBoxField.IsReadOnly = false;
        EditButton.Visibility = Visibility.Collapsed;
        SaveButton.Visibility = Visibility.Visible;
        CancelButton.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Handles the click event of the Save button.
    /// Saves the edited text and disables editing.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        TextBoxField.IsReadOnly = true;
        EditButton.Visibility = Visibility.Visible;
        SaveButton.Visibility = Visibility.Collapsed;
        CancelButton.Visibility = Visibility.Collapsed;
        OnTextModified();
    }

    /// <summary>
    /// Handles the click event of the Cancel button.
    /// Cancels the editing and reverts the text to the original value.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Text = _originalText;
        TextBoxField.IsReadOnly = true;
        EditButton.Visibility = Visibility.Visible;
        SaveButton.Visibility = Visibility.Collapsed;
        CancelButton.Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Raises the <see cref="TextModified"/> event.
    /// </summary>
    private void OnTextModified()
    {
        TextModified?.Invoke(this, EventArgs.Empty);
    }
}
