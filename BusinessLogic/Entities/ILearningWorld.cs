namespace BusinessLogic.Entities;

public interface ILearningWorld
{
    
    List<LearningElement> LearningElements { get; set; }
    List<LearningSpace> LearningSpaces { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Goals { get; set; }
}