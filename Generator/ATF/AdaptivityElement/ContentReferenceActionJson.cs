namespace Generator.ATF.AdaptivityElement;

public class ContentReferenceActionJson : IAdaptivityActionJson
{
    public ContentReferenceActionJson(int elementId, string? hintText)
    {
        ElementId = elementId;
        HintText = hintText;
    }

    public int ElementId { get; set; }

    public string? HintText { get; set; }
}