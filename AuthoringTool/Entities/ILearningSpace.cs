namespace AuthoringTool.Entities;

public interface ILearningSpace : ISpace
{
    List<LearningElement> LearningElements { get; set; }
    
}