namespace Generator.PersistEntities;

[Serializable]
public class LearningWorldPe : Generator.PersistEntities.ILearningWorldPe
{
    internal LearningWorldPe(string name, string shortname, string authors, string language, string description,
        string goals, List<Generator.PersistEntities.LearningElementPe>? learningElements = null,
        List<Generator.PersistEntities.LearningSpacePe>? learningSpaces = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<Generator.PersistEntities.LearningElementPe>();
        LearningSpaces = learningSpaces ?? new List<Generator.PersistEntities.LearningSpacePe>();
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
        LearningElements = new List<Generator.PersistEntities.LearningElementPe>();
        LearningSpaces = new List<Generator.PersistEntities.LearningSpacePe>();
    }

    public List<Generator.PersistEntities.LearningElementPe> LearningElements { get; set; }
    public List<Generator.PersistEntities.LearningSpacePe>? LearningSpaces { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
}