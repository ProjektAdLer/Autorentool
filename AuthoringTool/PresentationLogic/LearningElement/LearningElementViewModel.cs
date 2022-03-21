namespace AuthoringTool.PresentationLogic.LearningElement;

public class LearningElementViewModel : ILearningObjectViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningElementViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning element</param>
    /// <param name="shortname">The short name (abbreviation) of the learning element.(Maybe not relevant)</param>
    /// <param name="authors">The string containing the names of all the authors working on the learning element.</param>
    /// <param name="description">A description of the learning element and its contents.</param>
    /// <param name="goals">A description of the goals this learning element is supposed to achieve.</param>
    /// <param name="positionX">x-position of the learning element in the workspace</param>
    /// <param name="positionY">y-position of the learning element in the workspace</param>

    public LearningElementViewModel(string name, string shortname, string authors, string description, string goals,
        double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        PositionX = positionX;
        PositionY = positionY;
    }
    
    
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}