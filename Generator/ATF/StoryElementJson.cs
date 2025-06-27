using System.Text.Json.Serialization;
using Shared;

namespace Generator.ATF;

public class StoryElementJson : IStoryElementJson
{
    [JsonConstructor]
    public StoryElementJson(string[] storyTexts, string elementModel, string modalFacialExpression, string storyNpcName)
    {
        StoryTexts = storyTexts;
        ElementModel = elementModel;
        ModalFacialExpression = modalFacialExpression;
        StoryNpcName = storyNpcName;
    }

    public string[] StoryTexts { get; set; }
    public string ElementModel { get; set; }
    public string ModalFacialExpression { get; set; }
    public string StoryNpcName { get; set; }
}