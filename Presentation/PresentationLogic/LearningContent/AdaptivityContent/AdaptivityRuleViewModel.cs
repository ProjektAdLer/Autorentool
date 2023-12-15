using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public class AdaptivityRuleViewModel : IAdaptivityRuleViewModel
{
    public AdaptivityRuleViewModel(IAdaptivityTriggerViewModel trigger, IAdaptivityActionViewModel action)
    {
        Trigger = trigger;
        Action = action;
        UnsavedChanges = true;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityRuleViewModel()
    {
        Trigger = null!;
        Action = null!;
        UnsavedChanges = false;
    }

    public Guid Id { get; private set; }
    public IAdaptivityTriggerViewModel Trigger { get; set; }
    public IAdaptivityActionViewModel Action { get; set; }
    private bool InternalUnsavedChanges { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Trigger.UnsavedChanges || Action.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

}