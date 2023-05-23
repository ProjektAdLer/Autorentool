namespace Generator.DSL;

public interface ILearningElementJson
{
    int ElementId { get; set; }
    
    string ElementUUID { get; set; }
    
    string? ElementDescription { get; set; }
    
    // the elementType describes the Filetype of the element. (H5P, Picture, Video, PDF)
    string ElementFileType { get; set; }
    
    // learningElementValue describes the Points or Badge the elem gives
    int ElementMaxScore { get; set; }
    
    string ElementModel { get; set; }
    
    // The LearningSpaceParentId deszribes the Space the current Learning Element is in.
    int LearningSpaceParentId { get; set; }
    
}