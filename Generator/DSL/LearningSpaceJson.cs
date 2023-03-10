namespace Generator.DSL;

/// <summary>
/// This class has all the information about the Space
/// </summary>
public class LearningSpaceJson : ILearningSpaceJson
{
    // the id is incremented and is set for every Space
    public LearningSpaceJson(int spaceId, LmsElementIdentifierJson lmsElementIdentifier, string spaceName,
        List<int> spaceContents, int requiredPointsToComplete, string? spaceDescription=null, string[]? spaceGoals = null,
        string? requiredSpacesToEnter = null)
    {
        SpaceId = spaceId;
        LmsElementIdentifier = lmsElementIdentifier;
        SpaceName = spaceName;
        SpaceDescription = spaceDescription ?? "";
        SpaceGoals = spaceGoals ?? new []{""};
        SpaceContents = spaceContents;
        RequiredPointsToComplete = requiredPointsToComplete;
        RequiredSpacesToEnter = requiredSpacesToEnter;
    }

    public int SpaceId { get; set; }

    // the lmsElementIdentifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public LmsElementIdentifierJson LmsElementIdentifier { get; set; }
    
    //A Name for the Learning Space
    public string SpaceName { get; set; }
    
    //A Description for the Learning Space
    public string? SpaceDescription { get; set; }
    
    public string[] SpaceGoals { get; set; }
    
    // A list that has all the id´s of the included elements of a space. 
    public List<int> SpaceContents { get; set; }
    
    // Maximum Points and Points that are needed to complete the Space
    public int RequiredPointsToComplete { get; set; }
    
    // requirements are needed to describe the Path of the Spaces. 
    // It is a boolean algebra string, that describes which spaces are needed to complete the space.
    public string? RequiredSpacesToEnter { get; set; }
}