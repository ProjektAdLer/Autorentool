using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityContentJson
{
    [JsonPropertyName("$type")] string Type { get; }

    string IntroText { get; set; }
    bool ShuffleTasks { get; set; }
    List<IAdaptivityTaskJson> AdaptivityTask { get; set; }
}