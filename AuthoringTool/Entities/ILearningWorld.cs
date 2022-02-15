namespace AuthoringTool.Entities;

internal interface ILearningWorld
{
    
    ICollection<ILearningElement> LearningElements { get; set; }
    ICollection<ILearningSpace> LearningSpaces { get; set; }
    string Name { get; set; }
    string Description { get; set; }
}