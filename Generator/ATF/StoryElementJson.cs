using System.Text.Json.Serialization;
using Shared;

namespace Generator.ATF;

public class StoryElementJson : IStoryElementJson
{
    [JsonConstructor]
    public StoryElementJson(string[] storyTexts, string elementModel, string modelFacialExpression, string storyNpcName)
    {
        StoryTexts = storyTexts;
        ElementModel = elementModel;
        ModelFacialExpression = modelFacialExpression;
        StoryNpcName = storyNpcName;
    }

    public string[] StoryTexts { get; set; }
    public string ElementModel { get; set; }
    public string ModelFacialExpression { get; set; }
    public string StoryNpcName { get; set; }
}