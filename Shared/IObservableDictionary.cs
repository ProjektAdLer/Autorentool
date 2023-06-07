using System.Collections.Specialized;

namespace Shared;

public interface IObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged
{
}