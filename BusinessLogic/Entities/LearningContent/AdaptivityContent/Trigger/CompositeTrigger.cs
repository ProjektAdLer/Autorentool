using Shared;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Trigger;

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
}