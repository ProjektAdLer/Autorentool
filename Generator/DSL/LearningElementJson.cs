namespace Generator.DSL;

/// <summary>
/// Every Learning Element, either in the world, in a topic or in a space.
/// </summary>
public class LearningElementJson : ILearningElementJson
{
    // incremented ID for every element, it will also be used as moduleid, sectionid ...
    public LearningElementJson(int id, IdentifierJson identifier, string url, string elementCategory, 
        string elementType, int learningSpaceParentId, List<LearningElementValueJson> learningElementValueList, 
        string? description=null, string? goals = null)
    {
        Id = id;
        Identifier = identifier;
        Url = url;
        Description = description ?? "";
        Goals = goals ?? "";
        ElementCategory = elementCategory;
        ElementType = elementType;
        LearningSpaceParentId = learningSpaceParentId;
        LearningElementValueList = learningElementValueList;
    }

    public int Id { get; set; }
    
    // the identifier has the name of the element
    public IdentifierJson Identifier { get; set; }
    
    public string Url { get; set; }
    
    //A Description for the Learning Element
    public string? Description { get; set; }
    
    //A Goal for the Learning Element
    public string? Goals { get; set; }
    
    public string ElementCategory { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    public string ElementType { get; set; }
    
    // learningElementValue describes the Points or Badge the element gives
    public List<LearningElementValueJson> LearningElementValueList { get; set; }
    
    // The LearningSpaceParentId describes the Space the current Learning Element is in.
    public int LearningSpaceParentId { get; set; }
    
}