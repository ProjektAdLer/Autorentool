namespace Generator.DSL;

public interface ILearningSpaceJson
{
    int SpaceId { get; set; }

    // the lmsElementIdentifierJson has the name of the element, this information is needed for the API calls from the 2D3D Team.
   LmsElementIdentifierJson LmsElementIdentifier { get; set; }
   
   string? SpaceDescription { get; set; }
    
    // A list that has all the id´s of the included elements of a space. 
   List<int> SpaceContents { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // It is a boolean algebra string, that describes which spaces are needed to complete the space.
    string? RequiredSpacesToEnter { get; set; }
}