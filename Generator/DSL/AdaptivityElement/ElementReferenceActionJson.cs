namespace Generator.DSL.AdaptivityElement;

public class ElementReferenceActionJson : IAdaptivityActionJson
{
    public ElementReferenceActionJson(int elementId)
    {
        ElementId = elementId;
    }

    public int ElementId { get; set; }
}