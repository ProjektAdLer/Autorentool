using System.Collections.ObjectModel;

namespace AuthoringTool.Entities;

[Serializable]
public class LearningSpace : ILearningSpace
{
    internal LearningSpace(string name, string shortname, string authors, string description,
        string goals, List<LearningElement>? learningElements = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<LearningElement>();
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningSpace()
    {
        Name = "";
        Shortname = "";
        Authors = "";
        Description = "";
        Goals = "";
        LearningElements = new List<LearningElement>();
    }


    public string Name { get; set; }
    public string Description { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Goals { get; set; }
    public List<LearningElement> LearningElements { get; set; }
}