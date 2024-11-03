using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Services.Implementations;
using SipPOS.Services.Interfaces;

namespace SipPOS.ViewModels;
public class MainMenuViewModel : INotifyPropertyChanged
{
    private string _currentStoreAuthenticationStatus;

    public MainMenuViewModel()
    {
        _currentStoreAuthenticationStatus = "Đang kiểm tra trạng thái đăng nhập...";

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }

        if (storeAuthenticationService.Context.CurrentStore == null)
        {
            CurrentStoreAuthenticationStatus = $"Bạn chưa đăng nhập";
        }
        else
        {
            CurrentStoreAuthenticationStatus = $"Đã đăng nhập bằng tài khoản: {storeAuthenticationService.Context.CurrentStore.Username}";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string CurrentStoreAuthenticationStatus
    {
        get => _currentStoreAuthenticationStatus;
        set
        {
            _currentStoreAuthenticationStatus = value;
            OnPropertyChanged(nameof(CurrentStoreAuthenticationStatus));
        }
    }
}