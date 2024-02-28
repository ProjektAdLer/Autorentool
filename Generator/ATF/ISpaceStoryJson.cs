using System.Text.Json.Serialization;

namespace Generator.ATF;

[JsonDerivedType(typeof(SpaceStoryJson), typeDiscriminator: JsonTypes.SpaceStoryType)]
public interface ISpaceStoryJson
{
    IStoryElementJson? IntroStory { get; set; }
    IStoryElementJson? OutroStory { get; set; }
}