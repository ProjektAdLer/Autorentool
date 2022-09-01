namespace PersistEntities;

[Serializable]
public class LearningWorldPe : ILearningWorldPe
{
    public LearningWorldPe(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningElementPe>? learningElements = null,
        List<LearningSpacePe>? learningSpaces = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<LearningElementPe>();
        LearningSpaces = learningSpaces ?? new List<LearningSpacePe>();
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningWorldPe()
    {
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        Goals = "";
        LearningElements = new List<LearningElementPe>();
        LearningSpaces = new List<LearningSpacePe>();
    }

    public List<LearningElementPe> LearningElements { get; set; }
    public List<LearningSpacePe> LearningSpaces { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
}