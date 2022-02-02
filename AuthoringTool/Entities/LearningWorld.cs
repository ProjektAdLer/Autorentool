using System.Collections.ObjectModel;

namespace AuthoringTool.Entities;

internal class LearningWorld : ILearningWorld
{
    internal LearningWorld()
    {
        LearningElements = new Collection<ILearningElement>();
        LearningSpaces = new Collection<ILearningSpace>();
    }

    public ICollection<ILearningElement> LearningElements { get; set; }
    public ICollection<ILearningSpace> LearningSpaces { get; set; }
}