using System.Collections.ObjectModel;

namespace AuthoringTool.Entities;

public class LearningSpace : ILearningSpace
{
    public LearningSpace()
    {
        LearningElements = new Collection<ILearningElement>();
    }
    
    
    public ICollection<ILearningElement> LearningElements { get; set; }
}