using System.ComponentModel;

using SipPOS.Views.General;
using SipPOS.DataTransfer.Entity;
using System.Collections.ObjectModel;

namespace SipPOS.ViewModels.Staff;

public class StaffManagementViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<StaffDto> CurrentPageStaffList { get; private set; }

    public StaffManagementViewModel()
    {
        CurrentPageStaffList = new ObservableCollection<StaffDto>();
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void HandleGoBackButtonClick()
    {
        App.NavigateTo(typeof(MainMenuView));
    }
}