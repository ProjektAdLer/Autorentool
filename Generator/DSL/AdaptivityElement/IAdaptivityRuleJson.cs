using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

[JsonDerivedType(typeof(AdaptivityRuleJson), typeDiscriminator: JsonTypes.CorrectnessTriggerType)]
public interface IAdaptivityRuleJson
{
    int TriggerId { get; set; }
    string TriggerCondition { get; set; }
    IAdaptivityActionJson AdaptivityAction { get; set; }
}