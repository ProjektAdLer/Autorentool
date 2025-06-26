namespace Shared;

[AttributeUsage(AttributeTargets.Field)]
public class AlternateValueAttribute : Attribute
{
    public ElementModel AlternateValue { get; protected set; }

    public AlternateValueAttribute(ElementModel value)
    {
        this.AlternateValue = value;
    }
}