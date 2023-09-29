namespace Generator.DSL.AdaptivityElement;

public class ContentReferenceActionJson : IAdaptivityActionJson
{
    public ContentReferenceActionJson(int elementId)
    {
        ElementId = elementId;
    }

    public int ElementId { get; set; }

    public string Type => JsonTypes.ContentReferenceActionType;
}