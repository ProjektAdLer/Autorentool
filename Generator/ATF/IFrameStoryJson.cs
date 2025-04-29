using System.Text.Json.Serialization;

namespace Generator.ATF;

[JsonDerivedType(typeof(FrameStoryJson), typeDiscriminator: JsonTypes.FrameStoryType)]
public interface IFrameStoryJson
{
    string FrameStoryIntro { get; set; }
    string FrameStoryOutro { get; set; }
}