namespace AuthoringTool.Entities;

[Serializable]
public class LearningElement : ILearningElement
{
    internal LearningElement(string name, string shortname, string elementType, string? parentName,
        string contentType, LearningContent? content, string authors, string description, string goals, double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        ElementType = elementType;
        ContentType = contentType;
        Content = content ?? new LearningContent();
        Authors = authors;
        Description = description;
        Goals = goals;
        PositionX = positionX;
        PositionY = positionY;
        ParentName = parentName;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningElement()
    {
        Name = "";
        Shortname = "";
        ElementType = "";
        ContentType = "";
        Content = new LearningContent();
        Authors = "";
        Description = "";
        Goals = "";
        PositionX = 0;
        PositionY = 0;
        ParentName = null;
    }


    public string Name { get; set; }
    public string Shortname { get; set; }
    public string ElementType { get; set; }
    public string ContentType { get; set; }
    public LearningContent Content { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string? ParentName { get; set; }
}

