namespace AuthoringTool.DataAccess.DSL;

/// <summary>
/// Every Learning Element, either in the world, in a topic or in a space.
/// </summary>
public class LearningElementJson : ILearningElementJson
{
    // incremented ID for every element, it will also be used as moduleid, sectionid ...
    public int id { get; set; }
    
    // the identifier has the name of the element
    public IdentifierJson? identifier { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    public string? elementType { get; set; }
    
    // learningElementValue describes the Points or Badge the element gives
    public List<LearningElementValueJson>? learningElementValue { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // it is a list of topics, spaces or elements that need to be completed, before a particular element can be startet
    public List<RequirementJson>? requirements { get; set; }
}