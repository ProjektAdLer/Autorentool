namespace AuthoringTool.DataAccess.DSL;

public class LearningWorldJson : ILearningWorldJson
{
    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public LearningWorldJson(IdentifierJson identifier, List<int> learningWorldContent, List<TopicJson> topics, 
        List<LearningSpaceJson> learningSpaces, List<LearningElementJson> learningElements)
    {
        Identifier = identifier;
        LearningWorldContent = learningWorldContent;
        Topics = topics;
        LearningSpaces = learningSpaces;
        LearningElements = learningElements;
    }

    public IdentifierJson Identifier { get; set; }
    
    // A list that has all the id´s of the included Topics of a learningWorld. 
    public List<int> LearningWorldContent { get; set; }
    
    // for the correct structure the topics are added to the learning World
    public List<TopicJson> Topics { get; set; }
    
    // for the correct structure the Spaces are added to the learning World
    public List<LearningSpaceJson> LearningSpaces { get; set; }
    
    // for the correct structure the elements are added to the learning World
    public List<LearningElementJson> LearningElements { get; set; }
}