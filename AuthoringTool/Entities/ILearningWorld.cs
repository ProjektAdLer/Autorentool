namespace AuthoringTool.Entities;

public interface ILearningWorld
{
    
    ICollection<ILearningElement> LearningElements { get; set; }
    ICollection<ILearningSpace> LearningSpaces { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Goals { get; set; }
}