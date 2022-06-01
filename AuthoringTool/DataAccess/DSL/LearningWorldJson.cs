namespace AuthoringTool.DataAccess.DSL;

public class LearningWorldJson : ILearningWorldJson
{
    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public IdentifierJson? identifier { get; set; }
    
    // A list that has all the id´s of the included Topics of a learningWorld. 
    public List<int>? learningWorldContent { get; set; }
    
    // for the correct structure the topics are added to the learning World
    public List<TopicJson>? topics { get; set; }
    
    // for the correct structure the Spaces are added to the learning World
    public List<LearningSpaceJson>? learningSpaces { get; set; }
    
    // for the correct structure the elements are added to the learning World
    public List<LearningElementJson>? learningElements { get; set; }
}