using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public interface IAdaptivityRuleViewModel
{
    /// <summary>
    /// The trigger that activates the rule.
    /// </summary>
    public IAdaptivityTriggerViewModel Trigger { get; set; }
    /// <summary>
    /// The action that is performed when the rule is activated.
    /// </summary>
    public IAdaptivityActionViewModel Action { get; set; }
}