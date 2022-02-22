using System.Collections.ObjectModel;

namespace AuthoringTool.Entities;

internal class LearningWorld : ILearningWorld
{
    internal LearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, ICollection<ILearningElement>? learningElements = null,
        ICollection<ILearningSpace>? learningSpaces = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new Collection<ILearningElement>();
        LearningSpaces = learningSpaces ?? new Collection<ILearningSpace>();
    }

    public ICollection<ILearningElement> LearningElements { get; set; }
    public ICollection<ILearningSpace> LearningSpaces { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
}