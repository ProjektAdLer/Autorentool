namespace Generator.DSL.AdaptivityElement;

public class ContentReferenceActionJson : IAdaptivityActionJson
{
    public ContentReferenceActionJson(string type, int elementId)
    {
        Type = type;
        ElementId = elementId;
    }

    public int ElementId { get; set; }

    public string Type { get; }
}