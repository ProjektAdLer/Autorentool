using System.Collections.ObjectModel;

namespace AuthoringTool.Entities;

internal class LearningSpace : ILearningSpace
{
    public LearningSpace()
    {
        LearningElements = new Collection<ILearningElement>();
    }
    
    
    public ICollection<ILearningElement> LearningElements { get; set; }
}