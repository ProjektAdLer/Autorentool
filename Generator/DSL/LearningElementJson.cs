namespace Generator.DSL;

/// <summary>
/// Every Learning Element, either in the world, in a topic or in a space.
/// </summary>
public class LearningElementJson : ILearningElementJson
{
    // incremented ID for every element, it will also be used as moduleid, sectionid, contextid ...
    public LearningElementJson(int id, LmsElementIdentifierJson lmsElementIdentifierJson, string elementName,
        string url, string elementCategory, string elementFileType, int learningSpaceParentId,
        int elementMaxScore, string? elementDescription=null, string[]? elementGoals = null)
    {
        Id = id;
        LmsElementIdentifierJson = lmsElementIdentifierJson;
        ElementName = elementName;
        Url = url;
        ElementDescription = elementDescription ?? "";
        ElementGoals = elementGoals ?? new []{""};
        ElementCategory = elementCategory;
        ElementFileType = elementFileType;
        LearningSpaceParentId = learningSpaceParentId;
        ElementMaxScore = elementMaxScore;
    }

    public int Id { get; set; }
    
    // the lmsElementIdentifierJson has the name of the element
    public LmsElementIdentifierJson LmsElementIdentifierJson { get; set; }
    
    public string ElementName { get; set; }
    
    public string Url { get; set; }
    
    //A Description for the Learning Element
    public string? ElementDescription { get; set; }
    
    //A Goal for the Learning Element
    public string[] ElementGoals { get; set; }
    
    public string ElementCategory { get; set; }
    
    // the elementFileType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    public string ElementFileType { get; set; }
    
    // learningElementValue describes the Points or Badge the element gives
    public int ElementMaxScore { get; set; }
    
    // The LearningSpaceParentId describes the Space the current Learning Element is in.
    public int LearningSpaceParentId { get; set; }
    
}