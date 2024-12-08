using System.ComponentModel;

using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;

namespace SipPOS.ViewModels.Configuration;

public class StoreConfigurationViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _editStoreNameText = string.Empty;
    private string _editStoreAddressText = string.Empty;
    private string _editStoreEmailText = string.Empty;
    private string _editStoreTelText = string.Empty;
    private string _editStoreTaxCodeText = string.Empty;

    public StoreConfigurationViewModel()
    {
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }
        
        if (storeAuthenticationService.Context.CurrentStore == null)
        {
            EditStoreNameText = "Chưa đăng nhập";
            EditStoreAddressText = "Chưa đăng nhập";
            EditStoreEmailText = "Chưa đăng nhập";
            EditStoreTelText = "Chưa đăng nhập";
            EditStoreTaxCodeText = "Chưa đăng nhập";

            return;
        }


        EditStoreNameText = storeAuthenticationService.Context.CurrentStore.Name;
        EditStoreAddressText = storeAuthenticationService.Context.CurrentStore.Address;
        EditStoreEmailText = storeAuthenticationService.Context.CurrentStore.Email;
        EditStoreTelText = storeAuthenticationService.Context.CurrentStore.Tel;
        EditStoreTaxCodeText = storeAuthenticationService.Context.CurrentStore.TaxCode;
    }

    public string EditStoreNameText
    {
        get => _editStoreNameText;
        set
        {
            _editStoreNameText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditStoreNameText)));
        }
    }

    public string EditStoreAddressText
    {
        get => _editStoreAddressText;
        set
        {
            _editStoreAddressText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditStoreAddressText)));
        }
    }

    public string EditStoreEmailText
    {
        get => _editStoreEmailText;
        set
        {
            _editStoreEmailText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditStoreEmailText)));
        }
    }

    public string EditStoreTelText
    {
        get => _editStoreTelText;
        set
        {
            _editStoreTelText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditStoreTelText)));
        }
    }

    public string EditStoreTaxCodeText
    {
        get => _editStoreTaxCodeText;
        set
        {
            _editStoreTaxCodeText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditStoreTaxCodeText)));
        }
    }

}