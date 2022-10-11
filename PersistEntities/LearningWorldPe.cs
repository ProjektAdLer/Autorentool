namespace PersistEntities;

[Serializable]
public class LearningWorldPe : ILearningWorldPe
{
    public LearningWorldPe(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningSpacePe>? learningSpaces = null,  List<LearningPathwayPe>? learningPathWays = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningSpaces = learningSpaces ?? new List<LearningSpacePe>();
        LearningPathWays = learningPathWays ?? new List<LearningPathwayPe>();
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
        LearningSpaces = new List<LearningSpacePe>();
        LearningPathWays = new List<LearningPathwayPe>();
    }

    public List<LearningSpacePe> LearningSpaces { get; set; }
    public List<LearningPathwayPe> LearningPathWays { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
}