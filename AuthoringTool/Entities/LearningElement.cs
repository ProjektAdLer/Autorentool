namespace AuthoringTool.Entities;

[Serializable]
public class LearningElement : ILearningElement
{
    internal LearningElement(string name, string shortname, string type, string content, string authors,
        string description, string goals, double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Type = type;
        Content = content;
        Authors = authors;
        Description = description;
        Goals = goals;
        PositionX = positionX;
        PositionY = positionY;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningElement()
    {
        Name = "";
        Shortname = "";
        Type = "";
        Content = "";
        Authors = "";
        Description = "";
        Goals = "";
        PositionX = 0;
        PositionY = 0;
    }


    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}

