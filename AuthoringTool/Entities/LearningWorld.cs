using System.Collections.ObjectModel;

namespace AuthoringTool.Entities;

[Serializable]
public class LearningWorld : ILearningWorld
{
    internal LearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningElement>? learningElements = null,
        List<LearningSpace>? learningSpaces = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<LearningElement>();
        LearningSpaces = learningSpaces ?? new List<LearningSpace>();
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningWorld()
    {
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        Goals = "";
        LearningElements = new List<LearningElement>();
        LearningSpaces = new List<LearningSpace>();
    }

    public List<LearningElement> LearningElements { get; set; }
    public List<LearningSpace> LearningSpaces { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
}