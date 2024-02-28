using System.Text.Json.Serialization;

namespace Generator.ATF;

public class SpaceStoryJson : ISpaceStoryJson
{
    [JsonConstructor]
    public SpaceStoryJson(IStoryElementJson? introStory, IStoryElementJson? outroStory)
    {
        IntroStory = introStory;
        OutroStory = outroStory;
    }

    public IStoryElementJson? IntroStory { get; set; }
    public IStoryElementJson? OutroStory { get; set; }
}