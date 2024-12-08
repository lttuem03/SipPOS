using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SipPOS.Resources.Controls;

public sealed partial class EditableTextField : UserControl
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register
        (
            "Text", 
            typeof(string), 
            typeof(EditableTextField), 
            new PropertyMetadata(string.Empty)
        );

    public EditableTextField()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        TextBoxField.IsReadOnly = false;
        EditButton.Visibility = Visibility.Collapsed;
        SaveButton.Visibility = Visibility.Visible;
        CancelButton.Visibility = Visibility.Visible;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        TextBoxField.IsReadOnly = true;
        EditButton.Visibility = Visibility.Visible;
        SaveButton.Visibility = Visibility.Collapsed;
        CancelButton.Visibility = Visibility.Collapsed;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        TextBoxField.IsReadOnly = true;
        EditButton.Visibility = Visibility.Visible;
        SaveButton.Visibility = Visibility.Collapsed;
        CancelButton.Visibility = Visibility.Collapsed;
    }
}
