namespace Generator.DSL.AdaptivityElement;

public class AdaptivityRuleJson : IAdaptivityRuleJson
{
    public AdaptivityRuleJson(int triggerId, string triggerCondition,
        IAdaptivityActionJson adaptivityAction)
    {
        TriggerId = triggerId;
        TriggerCondition = triggerCondition;
        AdaptivityAction = adaptivityAction;
    }

    public string Type => JsonTypes.CorrectnessTriggerType;
    public int TriggerId { get; set; }
    public string TriggerCondition { get; set; }
    public IAdaptivityActionJson AdaptivityAction { get; set; }
}