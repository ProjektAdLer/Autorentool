namespace AuthoringTool.Entities;

internal interface ILearningSpace : ISpace
{
    ICollection<ILearningElement> LearningElements { get; set; }
    
}