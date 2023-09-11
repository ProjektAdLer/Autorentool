using PersistEntities.LearningContent.Action;
using PersistEntities.LearningContent.Trigger;

namespace PersistEntities.LearningContent;

public class AdaptivityRulePe : IAdaptivityRulePe
{
    public AdaptivityRulePe(IAdaptivityTriggerPe trigger, IAdaptivityActionPe action)
    {
        Trigger = trigger;
        Action = action;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityRulePe()
    {
        Trigger = null!;
        Action = null!;
    }

    public IAdaptivityTriggerPe Trigger { get; set; }
    public IAdaptivityActionPe Action { get; set; }
}