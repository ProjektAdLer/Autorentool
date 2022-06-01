namespace AuthoringTool.DataAccess.DSL;

/// <summary>
/// This class has all the information about the Space
/// </summary>
public class LearningSpaceJson : ILearningSpaceJson
{
    // the id is incremented and is set for every Space
    public int spaceId { get; set; }
    
    // The Name of the learning Space
    public string? learningSpaceName { get; set; }
    
    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public IdentifierJson? identifier { get; set; }
    
    // A list that has all the id´s of the included elements of a space. 
    public List<int>? learningSpaceContent { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // it is a list of topics, spaces or elements that need to be completed, before a particular element can be startet
    public List<RequirementJson>? requirements { get; set; }
}