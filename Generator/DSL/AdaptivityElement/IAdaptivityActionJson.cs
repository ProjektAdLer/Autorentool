using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityActionJson
{
    [JsonPropertyName("$type")] string Type { get; }
}