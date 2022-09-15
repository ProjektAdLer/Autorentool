namespace Generator.DSL;

public interface ILearningWorldJson
{
    string IdNumber { get; set; }

    IdentifierJson Identifier { get; set; }
    
    string? Description { get; set; }
    
    string? Goals { get; set; }
    
    // A list that has all the id´s of the included Topics of a learningWorld. 
    List<int> LearningWorldContent { get; set; }
    
    // for the correct structure the topics are added to the learning World
    List<TopicJson> Topics { get; set; }
    
    // for the correct structure the Spaces are added to the learning World
    List<LearningSpaceJson> LearningSpaces { get; set; }
    
    // for the correct structure the elements are added to the learning World
    List<LearningElementJson> LearningElements { get; set; }
}