using System.Text.Json.Serialization;

namespace Generator.ATF;

public class StoryElementJson : IStoryElementJson
{
    [JsonConstructor]
    public StoryElementJson(string[] storyTexts, string elementModel)
    {
        StoryTexts = storyTexts;
        ElementModel = elementModel;
    }

    public string[] StoryTexts { get; set; }
    public string ElementModel { get; set; }
}