using System.Text.Json.Serialization;

namespace Generator.ATF;

public class BaseLearningElementJson : IBaseLearningElementJson
{
    [JsonConstructor]
    public BaseLearningElementJson(int elementId, string elementUuid, string elementName, string url,
        string elementCategory, string elementFileType)
    {
        ElementId = elementId;
        ElementUUID = elementUuid;
        ElementName = elementName;
        Url = url;
        ElementCategory = elementCategory;
        ElementFileType = elementFileType;
    }

    public string Url { get; set; }

    public int ElementId { get; set; }
    public string ElementUUID { get; set; }
    public string ElementName { get; set; }
    public string ElementCategory { get; set; }
    public string ElementFileType { get; set; }
}