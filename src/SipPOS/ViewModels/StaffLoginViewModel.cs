using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace SipPOS.ViewModels;

public class StaffLoginViewModel : INotifyPropertyChanged
{
    // No need to bind these properties
    private bool _staffIdTextBoxFocused;
    private bool _staffPasswordBoxFocused;
    private bool _staffPasswordVisible;

    // Properties used for data binding -> Implement setter that fires a PropertyChanged event
    private string _staffId;
    private string _staffPassword;
    private string _selectedPrefix;
    
    public StaffLoginViewModel()
    {
        _staffIdTextBoxFocused = false;
        _staffPasswordBoxFocused = false;
        _staffPasswordVisible = false;

        _staffId = "";
        _staffPassword = "";
        _selectedPrefix = "";
    }

    public void HandleStaffIdTextBoxGotFocus()
    {
        _staffIdTextBoxFocused = true;
        _staffPasswordBoxFocused = false;
    }

    public void HandleStaffPasswordBoxGotFocus()
    {
        _staffIdTextBoxFocused = false;
        _staffPasswordBoxFocused = true;
    }

    public void HandleSelectPrefixComboBoxGotFocus()
    {
        _staffIdTextBoxFocused = false;
        _staffPasswordBoxFocused = false;
    }

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

    public void HandleStaffPasswordVisibleCheckBoxChanged(PasswordBox staffPasswordBox)
    {
        _staffPasswordVisible = !_staffPasswordVisible;

        if (_staffPasswordVisible)
        {
            staffPasswordBox.PasswordRevealMode = PasswordRevealMode.Visible;
        }
        else
        {
            staffPasswordBox.PasswordRevealMode = PasswordRevealMode.Hidden;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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