using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityRuleJson
{
    [JsonPropertyName("$type")] string Type { get; }

    int TriggerId { get; set; }
    string TriggerType { get; set; }
    string TriggerCondition { get; set; }
    IAdaptivityActionJson AdaptivityAction { get; set; }
}