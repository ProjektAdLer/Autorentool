using System.Text.Json.Serialization;

namespace Generator.ATF.AdaptivityElement;

public class AdaptivityElementJson : IAdaptivityElementJson
{
    [JsonConstructor]
    public AdaptivityElementJson(int elementId, string elementUuid, string elementName,
        string elementCategory, string elementFileType, int learningSpaceParentId,
        int elementMaxScore, string elementModel, IAdaptivityContentJson adaptivityContent,
        string? elementDescription = null, string[]? elementGoals = null)
    {
        ElementId = elementId;
        ElementUUID = elementUuid;
        ElementName = elementName;
        ElementDescription = elementDescription ?? "";
        ElementGoals = elementGoals ?? new[] { "" };
        ElementCategory = elementCategory;
        ElementFileType = elementFileType;
        LearningSpaceParentId = learningSpaceParentId;
        ElementMaxScore = elementMaxScore;
        ElementModel = elementModel;
        AdaptivityContent = adaptivityContent;
    }

    public int ElementId { get; set; }
    public string ElementUUID { get; set; }
    public string ElementName { get; set; }
    public string? ElementDescription { get; set; }
    public string[] ElementGoals { get; set; }
    public string ElementCategory { get; set; }
    public string ElementFileType { get; set; }
    public int ElementMaxScore { get; set; }
    public string ElementModel { get; set; }
    public int LearningSpaceParentId { get; set; }
    public IAdaptivityContentJson AdaptivityContent { get; set; }
}