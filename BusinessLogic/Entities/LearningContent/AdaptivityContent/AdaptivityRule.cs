using BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;
using BusinessLogic.Entities.LearningContent.AdaptivityContent.Trigger;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

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
}