using PersistEntities.LearningContent.Action;
using PersistEntities.LearningContent.Trigger;

namespace PersistEntities.LearningContent;

public interface IAdaptivityRulePe
{
    /// <summary>
    /// The trigger that activates the rule.
    /// </summary>
    public IAdaptivityTriggerPe Trigger { get; set; }
    /// <summary>
    /// The action that is performed when the rule is activated.
    /// </summary>
    public IAdaptivityActionPe Action { get; set; }
}