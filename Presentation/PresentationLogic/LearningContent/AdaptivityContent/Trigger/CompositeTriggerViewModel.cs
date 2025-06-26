using Shared;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;

/// <summary>
/// Composite trigger that can be used to combine two triggers with a logical condition.
/// </summary>
public class CompositeTriggerViewModel : IAdaptivityTriggerViewModel
{
    public CompositeTriggerViewModel(ConditionEnum condition, IAdaptivityTriggerViewModel left, IAdaptivityTriggerViewModel right)
    {
        Condition = condition;
        Left = left;
        Right = right;
        UnsavedChanges = true;
    }
    
    // ReSharper disable once UnusedMember.Local
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CompositeTriggerViewModel()
    {
        Condition = ConditionEnum.And;
        //suppress null ref exception as automapper will set these properties immediately
        Left = null!;
        Right = null!;
        UnsavedChanges = false;
    }

    /// <summary>
    /// The logical condition between <see cref="Left"/> and <see cref="Right"/>. Either logical OR or logical AND.
    /// </summary>
    public ConditionEnum Condition { get; set; }

    /// <summary>
    /// The left side of the boolean composite trigger expression.
    /// </summary>
    public IAdaptivityTriggerViewModel Left { get; set; }

    /// <summary>
    /// The right side of the boolean composite trigger expression.
    /// </summary>
    public IAdaptivityTriggerViewModel Right { get; set; }

    public bool UnsavedChanges { get; set; }
}