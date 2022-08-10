namespace AuthoringTool.Entities;

public class LearningSpace : ILearningSpace
{
    internal LearningSpace(string name, string shortname, string authors, string description,
        string goals, List<LearningElement>? learningElements = null, double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<LearningElement>();
        PositionX = positionX;
        PositionY = positionY;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Goals { get; set; }
    public List<LearningElement> LearningElements { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}