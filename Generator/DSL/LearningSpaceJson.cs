namespace Generator.DSL;

/// <summary>
/// This class has all the information about the Space
/// </summary>
public class LearningSpaceJson : ILearningSpaceJson
{
    // the id is incremented and is set for every Space
    public LearningSpaceJson(int spaceId, IdentifierJson identifier, List<int> learningSpaceContent,
        string? description=null)
    {
        SpaceId = spaceId;
        Identifier = identifier;
        Description = description ?? "";
        LearningSpaceContent = learningSpaceContent;
    }

    public int SpaceId { get; set; }

    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public IdentifierJson Identifier { get; set; }
    
    //A Description for the Learning Space
    public string? Description { get; set; }
    
    // A list that has all the id´s of the included elements of a space. 
    public List<int> LearningSpaceContent { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // it is a list of topics, spaces or elements that need to be completed, before a particular element can be startet
    public List<RequirementJson>? Requirements { get;}
}