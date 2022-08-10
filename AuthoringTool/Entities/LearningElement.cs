using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

public class LearningElement : ILearningElement
{
    internal LearningElement(string name, string shortname,  string? parentName, LearningContent? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload = 0,
        double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Content = content;
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        PositionX = positionX;
        PositionY = positionY;
        ParentName = parentName;
    }

    public string Name { get; set; }
    public string Shortname { get; set; }
    public LearningContent? Content { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int Workload { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string? ParentName { get; set; }
}

