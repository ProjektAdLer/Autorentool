namespace Generator.DSL;

public interface ILearningWorldJson
{
    string WorldName { get; set; }
    
    string WorldUUID { get; set; }
    
    string WorldDescription { get; set; }
    
    string[] WorldGoals { get; set; }
    
    // for the correct structure the topics are added to the learning World
    List<TopicJson> Topics { get; set; }
    
    // for the correct structure the Spaces are added to the learning World
    List<LearningSpaceJson> Spaces { get; set; }
    
    // for the correct structure the elements are added to the learning World
    List<LearningElementJson> Elements { get; set; }
}