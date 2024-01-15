using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;

namespace BusinessLogic.Entities.LearningContent.Adaptivity;

public class AdaptivityRule : IAdaptivityRule
{
    public AdaptivityRule(IAdaptivityTrigger trigger, IAdaptivityAction action)
    {
        Trigger = trigger;
        Action = action;
        Id = Guid.NewGuid();
        UnsavedChanges = true;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityRule()
    {
        Trigger = null!;
        Action = null!;
        Id = Guid.Empty;
        UnsavedChanges = false;
    }

    public Guid Id { get; private set; }
    public IAdaptivityTrigger Trigger { get; set; }
    public IAdaptivityAction Action { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Trigger.UnsavedChanges || Action.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

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