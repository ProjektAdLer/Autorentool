using System.Collections;
using System.Collections.Specialized;

namespace Shared;

public sealed class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue> where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _dictionary;
    public ObservableDictionary()
    {
        _dictionary = new Dictionary<TKey, TValue>();
    }
    public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _dictionary.Add(item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
    }

    public void Clear()
    {
        _dictionary.Clear();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        var removed = _dictionary.Remove(item);
        if (removed)
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        return removed;
    }

    public int Count => _dictionary.Count;
    public bool IsReadOnly => _dictionary.IsReadOnly;
    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
            new KeyValuePair<TKey, TValue>(key, value)));
    }

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public bool Remove(TKey key)
    {
        var keyExists = TryGetValue(key, out var value);
        if (!keyExists)
            return false;
        var removed = _dictionary.Remove(key);
        if (removed)
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey,TValue>(key, value)));
        return removed;
    }

    public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            var keyExists = TryGetValue(key, out var previousValue);
            _dictionary[key] = value;
            if (keyExists)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                    new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey,TValue>(key, previousValue)));
            }
            else
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                    new KeyValuePair<TKey, TValue>(key, value)));
            }
        }
    }

    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }
}