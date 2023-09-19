namespace Generator.DSL.AdaptivityElement;

public class AdaptivityRuleJson : IAdaptivityRuleJson
{
    public AdaptivityRuleJson(string type, int triggerId, string triggerType, string triggerCondition,
        IAdaptivityActionJson adaptivityAction)
    {
        Type = type;
        TriggerId = triggerId;
        TriggerType = triggerType;
        TriggerCondition = triggerCondition;
        AdaptivityAction = adaptivityAction;
    }

    public string Type { get; }
    public int TriggerId { get; set; }
    public string TriggerType { get; set; }
    public string TriggerCondition { get; set; }
    public IAdaptivityActionJson AdaptivityAction { get; set; }
}