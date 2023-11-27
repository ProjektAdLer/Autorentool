using System.Text.Json.Serialization;

namespace Generator.ATF;

/// <summary>
/// Every Learning Element, either in the world, in a topic or in a space.
/// </summary>
public class LearningElementJson : ILearningElementJson, IInternalElementJson
{
    // incremented ID for every element, it will also be used as moduleid, sectionid, contextid ...
    [JsonConstructor]
    public LearningElementJson(int elementId, string elementUuid, string elementName,
        string url, string elementCategory, string elementFileType, int learningSpaceParentId,
        int elementMaxScore, string elementModel, string? elementDescription = null, string[]? elementGoals = null)
    {
        ElementId = elementId;
        ElementUUID = elementUuid;
        ElementName = elementName;
        Url = url;
        ElementDescription = elementDescription ?? "";
        ElementGoals = elementGoals ?? new[] { "" };
        ElementCategory = elementCategory;
        ElementFileType = elementFileType;
        LearningSpaceParentId = learningSpaceParentId;
        ElementMaxScore = elementMaxScore;
        ElementModel = elementModel;
    }

    public LearningElementJson(int elementId, string elementUuid, string elementName, string elementCategory,
        string elementFileType, int learningSpaceParentId, int elementMaxScore, string elementModel,
        string? elementDescription = null, string[]? elementGoals = null)
    {
        ElementId = elementId;
        ElementUUID = elementUuid;
        ElementName = elementName;
        Url = "";
        ElementDescription = elementDescription ?? "";
        ElementGoals = elementGoals ?? new[] { "" };
        ElementCategory = elementCategory;
        ElementFileType = elementFileType;
        LearningSpaceParentId = learningSpaceParentId;
        ElementMaxScore = elementMaxScore;
        ElementModel = elementModel;
    }

    //A Description for the Learning Element
    public string? ElementDescription { get; set; }

    //A Goal for the Learning Element
    public string[] ElementGoals { get; set; }

    // learningElementValue describes the Points or Badge the element gives
    public int ElementMaxScore { get; set; }

    public string ElementModel { get; set; }

    // The LearningSpaceParentId describes the Space the current Learning Element is in.
    public int LearningSpaceParentId { get; set; }

    public string Url { get; set; }

    public int ElementId { get; set; }

    public string ElementUUID { get; set; }

    public string ElementName { get; set; }

    public string ElementCategory { get; set; }

    // the elementFileType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    public string ElementFileType { get; set; }
}