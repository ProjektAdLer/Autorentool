using BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;
using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;
using BusinessLogic.Entities.LearningContent.AdaptivityContent.Trigger;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

public interface IAdaptivityRule
{
    /// <summary>
    /// The trigger that activates the rule.
    /// </summary>
    public IAdaptivityTrigger Trigger { get; set; }
    /// <summary>
    /// The action that is performed when the rule is activated.
    /// </summary>
    public IAdaptivityAction Action { get; set; }
}