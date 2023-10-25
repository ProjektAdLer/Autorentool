using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;

namespace BusinessLogic.Entities.LearningContent.Adaptivity;

public interface IAdaptivityRule : IEquatable<IAdaptivityRule>
{
    public Guid Id { get; }
    
    /// <summary>
    /// The trigger that activates the rule.
    /// </summary>
    public IAdaptivityTrigger Trigger { get; set; }

    /// <summary>
    /// The action that is performed when the rule is activated.
    /// </summary>
    public IAdaptivityAction Action { get; set; }
}