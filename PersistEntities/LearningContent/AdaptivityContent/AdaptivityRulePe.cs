using System.Runtime.Serialization;
using PersistEntities.LearningContent.Action;
using PersistEntities.LearningContent.Trigger;

namespace PersistEntities.LearningContent;

[KnownType(typeof(CompositeTriggerPe))]
[KnownType(typeof(CorrectnessTriggerPe))]
[KnownType(typeof(TimeTriggerPe))]
[KnownType(typeof(CommentActionPe))]
[KnownType(typeof(ContentReferenceActionPe))]
[KnownType(typeof(ElementReferenceActionPe))]
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

    [DataMember] public IAdaptivityTriggerPe Trigger { get; set; }
    [DataMember] public IAdaptivityActionPe Action { get; set; }
}