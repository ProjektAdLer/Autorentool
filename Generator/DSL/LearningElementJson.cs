namespace Generator.DSL;

/// <summary>
/// Every Learning Element, either in the world, in a topic or in a space.
/// </summary>
public class LearningElementJson : ILearningElementJson
{
    // incremented ID for every element, it will also be used as moduleid, sectionid ...
    public LearningElementJson(int id, IdentifierJson identifier, string elementType)
    {
        Id = id;
        Identifier = identifier;
        ElementType = elementType;
    }

    public int Id { get; set; }
    
    // the identifier has the name of the element
    public IdentifierJson Identifier { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    public string ElementType { get; set; }
    
    // learningElementValue describes the Points or Badge the element gives
    public List<LearningElementValueJson>? LearningElementValue { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // it is a list of topics, spaces or elements that need to be completed, before a particular element can be startet
    public List<RequirementJson>? Requirements { get; set; }
}