using System.ComponentModel;

using SipPOS.Views.General;

namespace SipPOS.ViewModels.Staff;

public class StaffManagementViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void HandleGoBackButtonClick()
    {
        App.NavigateTo(typeof(MainMenuView));
    }
}