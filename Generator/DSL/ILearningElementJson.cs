namespace Generator.DSL;

public interface ILearningElementJson
{
    int Id { get; set; }
    
    // the identifier has the name of the element
    IdentifierJson Identifier { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    string ElementType { get; set; }
    
    // learningElementValue describes the Points or Badge the element gives
    List<LearningElementValueJson>? LearningElementValue { get; set; }
    
    // The LearningSpaceParentId describes the Space the current Learning Element is in.
    int LearningSpaceParentId { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // it is a list of topics, spaces or elements that need to be completed, before a particular element can be started
    List<RequirementJson>? Requirements { get; set; }
}