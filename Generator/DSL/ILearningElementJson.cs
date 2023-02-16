namespace Generator.DSL;

public interface ILearningElementJson
{
    int Id { get; set; }
    
    // the lmsElementIdentifierJson has the name of the element
    LmsElementIdentifierJson LmsElementIdentifierJson { get; set; }
    
    string? ElementDescription { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    string ElementFileType { get; set; }
    
    // learningElementValue describes the Points or Badge the elem gives
    int ElementMaxScore { get; set; }
    
    // The LearningSpaceParentId deszribes the Space the current Learning Element is in.
    int LearningSpaceParentId { get; set; }
    
}