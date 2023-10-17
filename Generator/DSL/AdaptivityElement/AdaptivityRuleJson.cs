using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

public class AdaptivityRuleJson : IAdaptivityRuleJson
{
    [JsonConstructor]
    public AdaptivityRuleJson(int triggerId, string triggerCondition,
        IAdaptivityActionJson adaptivityAction)
    {
        TriggerId = triggerId;
        TriggerCondition = triggerCondition;
        AdaptivityAction = adaptivityAction;
    }

    public int TriggerId { get; set; }
    public string TriggerCondition { get; set; }
    public IAdaptivityActionJson AdaptivityAction { get; set; }
}