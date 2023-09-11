using Shared;

namespace PersistEntities.LearningContent.Trigger;

/// <summary>
/// Composite trigger that can be used to combine two triggers with a logical condition.
/// </summary>
public class CompositeTriggerPe : IAdaptivityTriggerPe
{
    public CompositeTriggerPe(ConditionEnum condition, IAdaptivityTriggerPe left, IAdaptivityTriggerPe right)
    {
        Condition = condition;
        Left = left;
        Right = right;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CompositeTriggerPe()
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
    public IAdaptivityTriggerPe Left { get; set; }

    /// <summary>
    /// The right side of the boolean composite trigger expression.
    /// </summary>
    public IAdaptivityTriggerPe Right { get; set; }
}