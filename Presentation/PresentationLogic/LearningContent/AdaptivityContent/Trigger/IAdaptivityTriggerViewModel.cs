namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;

/// <summary>
/// Represents the trigger condition for a <see cref="IAdaptivityRuleViewModel"/>
/// </summary>
public interface IAdaptivityTriggerViewModel
{
    bool UnsavedChanges { get; set; }
}