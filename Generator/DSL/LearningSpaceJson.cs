namespace Generator.DSL;

/// <summary>
/// This class has all the information about the Space
/// </summary>
public class LearningSpaceJson : ILearningSpaceJson
{
    // the id is incremented and is set for every Space
    public LearningSpaceJson(int spaceId, IdentifierJson identifier, List<int> learningSpaceContent, int requiredPoints,
        int includedPoints,string? description=null, string? goals = null, string? requirements = null)
    {
        SpaceId = spaceId;
        Identifier = identifier;
        Description = description ?? "";
        Goals = goals ?? "";
        LearningSpaceContent = learningSpaceContent;
        RequiredPoints = requiredPoints;
        IncludedPoints = includedPoints;
        Requirements = requirements;
    }

    public int SpaceId { get; set; }

    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public IdentifierJson Identifier { get; set; }
    
    //A Description for the Learning Space
    public string? Description { get; set; }
    
    public string? Goals { get; set; }
    
    // A list that has all the id´s of the included elements of a space. 
    public List<int> LearningSpaceContent { get; set; }
    
    // Maximum Points and Points that are needed to complete the Space
    public int RequiredPoints { get; set; }
    
    // Maximum Points and Points that are needed to complete the Space
    public int IncludedPoints { get; set; }
    
    // requirements are needed to describe the Path of the Spaces. 
    // It is a boolean algebra string, that describes which spaces are needed to complete the space.
    public string? Requirements { get; set; }
}