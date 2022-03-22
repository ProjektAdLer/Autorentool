namespace AuthoringTool.Entities;

public interface ILearningSpace : ISpace
{
    ICollection<ILearningElement> LearningElements { get; set; }
    
}