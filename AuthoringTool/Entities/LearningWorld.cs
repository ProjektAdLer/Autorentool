using System.Collections.ObjectModel;

namespace AuthoringTool.Entities;

internal class LearningWorld : ILearningWorld
{
    internal LearningWorld(string name, string description)
    {
        LearningElements = new Collection<ILearningElement>();
        LearningSpaces = new Collection<ILearningSpace>();
        Name = name;
        Description = description;
    }

    public ICollection<ILearningElement> LearningElements { get; set; }
    public ICollection<ILearningSpace> LearningSpaces { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}