namespace Generator.DSL;

public interface ISpaceJson
{
    int SpaceId { get; set; }

    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
   IdentifierJson Identifier { get; set; }
   
   string? Description { get; set; }
    
    // A list that has all the id´s of the included Elements of a space. 
   List<int> SpaceContent { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // It is a boolean algebra string, that describes which spaces are needed to complete the space.
    string? Requirements { get; set; }
}