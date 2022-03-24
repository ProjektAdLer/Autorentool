using System.Collections.ObjectModel;

namespace AuthoringTool.Entities;

[Serializable]
public class LearningSpace : ILearningSpace
{
    public LearningSpace()
    {
        LearningElements = new List<LearningElement>();
    }
    
    
    public List<LearningElement> LearningElements { get; set; }
}