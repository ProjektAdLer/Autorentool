namespace Generator.DSL;

public interface ILearningElementJson
{
    int Id { get; set; }
    
    // the identifier has the name of the element
    IdentifierJson Identifier { get; set; }
    
    string? Description { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    string ElementType { get; set; }
    
    // learningElementValue describes the Points or Badge the element gives
    List<LearningElementValueJson> LearningElementValueList { get; set; }
    
    // The LearningSpaceParentId describes the Space the current Learning Element is in.
    int LearningSpaceParentId { get; set; }
    
}