namespace Generator.DSL.AdaptivityElement;

public class ElementReferenceActionJson : IAdaptivityActionJson
{
    public ElementReferenceActionJson(string type, int elementId)
    {
        Type = type;
        ElementId = elementId;
    }

    public int ElementId { get; set; }

    public string Type { get; }
}