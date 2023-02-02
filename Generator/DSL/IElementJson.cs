namespace Generator.DSL;

public interface IElementJson
{
    int Id { get; set; }
    
    // the identifier has the name of the element
    IdentifierJson Identifier { get; set; }
    
    string? Description { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    string ElementType { get; set; }
    
    // ElementValue describes the Points or Badge the element gives
    List<ElementValueJson> ElementValueList { get; set; }
    
    // The SpaceParentId describes the Space the current  Element is in.
    int SpaceParentId { get; set; }
    
}