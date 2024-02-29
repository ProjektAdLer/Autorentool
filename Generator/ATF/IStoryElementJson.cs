using System.Text.Json.Serialization;

namespace Generator.ATF;

[JsonDerivedType(typeof(StoryElementJson), typeDiscriminator: JsonTypes.StoryElementType)]
public interface IStoryElementJson
{
    string[] StoryTexts { get; set; }
    string ElementModel { get; set; }
}