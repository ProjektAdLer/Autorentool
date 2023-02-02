namespace Generator.DSL;

/// <summary>
/// Every  Element, either in the world, in a topic or in a space.
/// </summary>
public class ElementJson : IElementJson
{
    // incremented ID for every element, it will also be used as moduleid, sectionid, contextid ...
    public ElementJson(int id, IdentifierJson identifier, string url, string elementCategory, 
        string elementType, int spaceParentId, List<ElementValueJson> elementValueList, 
        string? description=null, string? goals = null)
    {
        Id = id;
        Identifier = identifier;
        Url = url;
        Description = description ?? "";
        Goals = goals ?? "";
        ElementCategory = elementCategory;
        ElementType = elementType;
        SpaceParentId = spaceParentId;
        ElementValueList = elementValueList;
    }

    public int Id { get; set; }
    
    // the identifier has the name of the element
    public IdentifierJson Identifier { get; set; }
    
    public string Url { get; set; }
    
    //A Description for the  Element
    public string? Description { get; set; }
    
    //A Goal for the  Element
    public string? Goals { get; set; }
    
    public string ElementCategory { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    public string ElementType { get; set; }
    
    // elementValue describes the Points or Badge the element gives
    public List<ElementValueJson> ElementValueList { get; set; }
    
    // The SpaceParentId describes the Space the current  Element is in.
    public int SpaceParentId { get; set; }
    
}