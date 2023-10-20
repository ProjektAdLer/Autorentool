namespace Generator.DSL.AdaptivityElement;

public class ElementReferenceActionJson : IAdaptivityActionJson
{
    public ElementReferenceActionJson(int elementId, string? hintText)
    {
        ElementId = elementId;
        HintText = hintText;
    }

    public int ElementId { get; set; }

    public string? HintText { get; set; }
}