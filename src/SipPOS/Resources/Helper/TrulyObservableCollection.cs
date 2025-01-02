using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Resources.Helper;

// Courtesy of: https://stackoverflow.com/a/269113
public class TrulyObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
{
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        // ignore warnings here, if we add null checks then it doesn't work
        Unsubscribe(e.OldItems);
        Subscribe(e.NewItems);
        base.OnCollectionChanged(e);
    }

    protected override void ClearItems()
    {
        foreach (T element in this)
            element.PropertyChanged -= ContainedElementChanged;

        base.ClearItems();
    }

    private void Subscribe(IList iList)
    {
        if (iList != null)
        {
            foreach (T element in iList)
                element.PropertyChanged += ContainedElementChanged;
        }
    }

    private void Unsubscribe(IList iList)
    {
        if (iList != null)
        {
            foreach (T element in iList)
                element.PropertyChanged -= ContainedElementChanged;
        }
    }

    private void ContainedElementChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e);
    }
}