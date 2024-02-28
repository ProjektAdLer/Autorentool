using System.ComponentModel;

namespace Shared.Observer;

public class PropertyChangingWithValueEventArgs<T> : PropertyChangingEventArgs
{
    public PropertyChangingWithValueEventArgs(string? propertyName, T oldValue, T newValue) : base(propertyName)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }

    public T OldValue { get; set; }
    public T NewValue { get; set; }
}