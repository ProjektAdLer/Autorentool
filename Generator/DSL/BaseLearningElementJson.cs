namespace Generator.DSL;

public class BaseLearningElementJson : IBaseLearningElementJson
{
    public BaseLearningElementJson(int elementId, string elementUuid, string elementName, string? elementDescription,
        string[] elementGoals, string elementCategory, string elementFileType)
    {
        ElementId = elementId;
        ElementUUID = elementUuid;
        ElementName = elementName;
        ElementDescription = elementDescription;
        ElementGoals = elementGoals;
        ElementCategory = elementCategory;
        ElementFileType = elementFileType;
    }

    public string Type => JsonTypes.BaseLearningElementType;
    public int ElementId { get; set; }
    public string ElementUUID { get; set; }
    public string ElementName { get; set; }
    public string? ElementDescription { get; set; }
    public string[] ElementGoals { get; set; }
    public string ElementCategory { get; set; }
    public string ElementFileType { get; set; }
}