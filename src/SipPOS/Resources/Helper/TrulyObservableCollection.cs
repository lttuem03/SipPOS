using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SipPOS.Resources.Helper;

// Courtesy of: https://stackoverflow.com/a/269113

/// <summary>
/// Represents a collection that is observable and notifies when any of its items' properties change.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public class TrulyObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
{
    /// <summary>
    /// Called when the collection changes.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        // ignore warnings here, if we add null checks then it doesn't work
        Unsubscribe(e.OldItems);
        Subscribe(e.NewItems);
        base.OnCollectionChanged(e);
    }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    protected override void ClearItems()
    {
        foreach (T element in this)
            element.PropertyChanged -= ContainedElementChanged;

        base.ClearItems();
    }

    /// <summary>
    /// Subscribes to the PropertyChanged event of each item in the list.
    /// </summary>
    /// <param name="iList">The list of items to subscribe to.</param>
    private void Subscribe(IList iList)
    {
        if (iList != null)
        {
            foreach (T element in iList)
                element.PropertyChanged += ContainedElementChanged;
        }
    }

    /// <summary>
    /// Unsubscribes from the PropertyChanged event of each item in the list.
    /// </summary>
    /// <param name="iList">The list of items to unsubscribe from.</param>
    private void Unsubscribe(IList iList)
    {
        if (iList != null)
        {
            foreach (T element in iList)
                element.PropertyChanged -= ContainedElementChanged;
        }
    }

    /// <summary>
    /// Handles the PropertyChanged event of contained elements.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ContainedElementChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e);
    }
}