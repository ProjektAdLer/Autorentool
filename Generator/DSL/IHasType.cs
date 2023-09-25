using System.Text.Json.Serialization;

namespace Generator.DSL;

public interface IHasType
{
    [JsonPropertyName("$type")] string Type { get; }
}