
namespace Generator.DSL;

public class LearningWorldJson : ILearningWorldJson
{
    // the lmsElementIdentifierJson has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public LearningWorldJson(LmsElementIdentifierJson lmsElementIdentifierJson, string worldName, List<TopicJson> topics, 
        List<LearningSpaceJson> spaces, List<LearningElementJson> elements, string? worldDescription = null, 
        string[]? worldGoals = null)
    {
        LmsElementIdentifierJson = lmsElementIdentifierJson;
        WorldName = worldName;
        WorldDescription = worldDescription ?? "";
        WorldGoals = worldGoals ?? new []{""};
        Topics = topics;
        Spaces = spaces;
        Elements = elements;
    }
    public LmsElementIdentifierJson LmsElementIdentifierJson { get; set; }
    
    public string WorldName { get; set; }
    
    public string WorldDescription { get; set; }
    
    public string[] WorldGoals { get; set; }
    
    // for the correct structure the topics are added to the learning World
    public List<TopicJson> Topics { get; set; }
    
    // for the correct structure the Spaces are added to the learning World
    public List<LearningSpaceJson> Spaces { get; set; }
    
    // for the correct structure the elements are added to the learning World
    public List<LearningElementJson> Elements { get; set; }
    

}