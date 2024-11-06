using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace SipPOS.ViewModels;

/// <summary>
/// ViewModel for handling staff login, managing data for data-binding and the logic in the StaffLoginView.
/// </summary>
public class StaffLoginViewModel : INotifyPropertyChanged
{
    // No need to bind these properties
    private bool _staffIdTextBoxFocused;
    private bool _staffPasswordBoxFocused;

    // Properties used for data binding -> Implement setter that fires a PropertyChanged event
    private string _staffId;
    private string _staffPassword;
    private string _selectedPrefix;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffLoginViewModel"/> class.
    /// </summary>
    public StaffLoginViewModel()
    {
        _staffIdTextBoxFocused = false;
        _staffPasswordBoxFocused = false;

        _staffId = "";
        _staffPassword = "";
        _selectedPrefix = "";
    }

    /// <summary>
    /// Handles the event when the staff ID text box gets focus.
    /// </summary>
    public void HandleStaffIdTextBoxGotFocus()
    {
        _staffIdTextBoxFocused = true;
        _staffPasswordBoxFocused = false;
    }

    /// <summary>
    /// Handles the event when the staff password box gets focus.
    /// </summary>
    public void HandleStaffPasswordBoxGotFocus()
    {
        _staffIdTextBoxFocused = false;
        _staffPasswordBoxFocused = true;
    }

    /// <summary>
    /// Handles the event when the select prefix combo box gets focus.
    /// </summary>
    public void HandleSelectPrefixComboBoxGotFocus()
    {
        _staffIdTextBoxFocused = false;
        _staffPasswordBoxFocused = false;
    }

    /// <summary>
    /// Handles the event when a numpad button is clicked.
    /// </summary>
    /// <param name="number">The number clicked on the numpad.</param>
    public void HandleNumpadButtonClick(string number)
    {
        if (_staffIdTextBoxFocused)
        {
            StaffId += number;
        }
        else if (_staffPasswordBoxFocused)
        {
            StaffPassword += number;
        }
    }

    /// <summary>
    /// Handles the event when the staff password visibility checkbox is changed.
    /// </summary>
    /// <param name="staffPasswordBox">The password box for the staff password.</param>
    public void HandleStaffPasswordVisibleCheckBoxChanged(PasswordBox staffPasswordBox)
    {
        staffPasswordBox.PasswordRevealMode = (staffPasswordBox.PasswordRevealMode == PasswordRevealMode.Hidden) ?
                                              PasswordRevealMode.Visible : PasswordRevealMode.Hidden;
    }

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets or sets the staff ID.
    /// </summary>
    public string StaffId
    {
        get => _staffId;
        set
        {
            if (_staffId == value)
            {
                return;
            }
            _staffId = value;
            OnPropertyChanged(nameof(StaffId));
        }
    }

    /// <summary>
    /// Gets or sets the staff password.
    /// </summary>
    public string StaffPassword
    {
        get => _staffPassword;
        set
        {
            if (_staffPassword == value)
            {
                return;
            }
            _staffPassword = value;
            OnPropertyChanged(nameof(StaffPassword));
        }
    }

    /// <summary>
    /// Gets or sets the selected prefix.
    /// </summary>
    public string SelectedPrefix
    {
        get => _selectedPrefix;
        set
        {
            if (_selectedPrefix == value)
            {
                return;
            }
            _selectedPrefix = value;
            OnPropertyChanged(nameof(SelectedPrefix));
        }
    }
}
