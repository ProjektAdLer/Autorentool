using Shared;

namespace BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;

/// <summary>
/// Composite trigger that can be used to combine two triggers with a logical condition.
/// </summary>
public class CompositeTrigger : IAdaptivityTrigger
{
    public CompositeTrigger(ConditionEnum condition, IAdaptivityTrigger left, IAdaptivityTrigger right)
    {
        Condition = condition;
        Left = left;
        Right = right;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CompositeTrigger()
    {
        Condition = ConditionEnum.And;
        //suppress null ref exception as automapper will set these properties immediately
        Left = null!;
        Right = null!;
    }

    /// <summary>
    /// The logical condition between <see cref="Left"/> and <see cref="Right"/>. Either logical OR or logical AND.
    /// </summary>
    public ConditionEnum Condition { get; set; }

    /// <summary>
    /// The left side of the boolean composite trigger expression.
    /// </summary>
    public IAdaptivityTrigger Left { get; set; }

    /// <summary>
    /// The right side of the boolean composite trigger expression.
    /// </summary>
    public IAdaptivityTrigger Right { get; set; }

    public bool Equals(IAdaptivityTrigger? other)
    {
        if (other is not CompositeTrigger compositeTrigger)
            return false;
        return Condition == compositeTrigger.Condition && Left.Equals(compositeTrigger.Left) &&
               Right.Equals(compositeTrigger.Right);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CompositeTrigger) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int) Condition, Left, Right);
    }

    public static bool operator ==(CompositeTrigger? left, CompositeTrigger? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CompositeTrigger? left, CompositeTrigger? right)
    {
        return !Equals(left, right);
    }
}