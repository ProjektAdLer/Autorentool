using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;

namespace BusinessLogic.Entities.LearningContent.Adaptivity;

public class AdaptivityRule : IAdaptivityRule
{
    public AdaptivityRule(IAdaptivityTrigger trigger, IAdaptivityAction action)
    {
        Trigger = trigger;
        Action = action;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityRule()
    {
        Trigger = null!;
        Action = null!;
    }

    public IAdaptivityTrigger Trigger { get; set; }
    public IAdaptivityAction Action { get; set; }

    public bool Equals(IAdaptivityRule? other)
    {
        if (other is not AdaptivityRule)
            return false;
        return Trigger.Equals(other.Trigger) && Action.Equals(other.Action);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AdaptivityRule) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Trigger, Action);
    }

    public static bool operator ==(AdaptivityRule? left, AdaptivityRule? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AdaptivityRule? left, AdaptivityRule? right)
    {
        return !Equals(left, right);
    }
}