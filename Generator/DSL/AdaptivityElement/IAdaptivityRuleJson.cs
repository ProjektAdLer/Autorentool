namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityRuleJson : IHasType
{
    int TriggerId { get; set; }
    string TriggerType { get; set; }
    string TriggerCondition { get; set; }
    IAdaptivityActionJson AdaptivityAction { get; set; }
}