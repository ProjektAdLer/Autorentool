using System.Text.Json.Serialization;

namespace Generator.ATF;

public class StoryElementJson : IStoryElementJson
{
    [JsonConstructor]
    public StoryElementJson(string[] storyTexts, string elementModel, string modelFacialExpression, string storyNpcName,
        bool exitAfterStorySequence)
    {
        StoryTexts = storyTexts;
        ElementModel = elementModel;
        ModelFacialExpression = modelFacialExpression;
        StoryNpcName = storyNpcName;
        ExitAfterStorySequence = exitAfterStorySequence;
    }

    public string[] StoryTexts { get; set; }
    public string ElementModel { get; set; }
    public string ModelFacialExpression { get; set; }
    public string StoryNpcName { get; set; }
    public bool ExitAfterStorySequence { get; set; }
}