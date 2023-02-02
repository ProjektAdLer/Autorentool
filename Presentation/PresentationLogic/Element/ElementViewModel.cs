using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Space;
using Shared;

namespace Presentation.PresentationLogic.Element;

public class ElementViewModel : ISerializableViewModel, IElementViewModel
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    protected ElementViewModel()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Parent = null;
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - n.stich
        Content = null!;
        Url = "";
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = ElementDifficultyEnum.None;
        Workload = 0;
        Points = 0;
        PositionX = 0;
        PositionY = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the  element.</param>
    /// <param name="shortname">The short name (abbreviation) of the  element (Maybe not relevant).</param>
    /// <param name="content">Represents the loaded content of the  element.</param>
    /// <param name="url"></param>
    /// <param name="authors">The string containing the names of all the authors working on the  element.</param>
    /// <param name="description">A description of the  element and its contents.</param>
    /// <param name="goals">A description of the goals this  element is supposed to achieve.</param>
    /// <param name="difficulty">Difficulty of the  element.</param>
    /// <param name="parent">Decides whether the  element belongs to a  world or a  space.</param>
    /// <param name="workload">The time required to complete the  element.</param>
    /// <param name="points">The number of points of the  element.</param>
    /// <param name="positionX">x-position of the  element in the workspace.</param>
    /// <param name="positionY">y-position of the  element in the workspace.</param>
    public ElementViewModel(string name, string shortname,
        ContentViewModel content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, ISpaceViewModel? parent = null,
        int workload = 0, int points = 0, double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Parent = parent;
        Content = content;
        Url = url ?? "";
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        Points = points;
        PositionX = positionX;
        PositionY = positionY;
    }
    
    public const string fileEnding = "aef";
    public string FileEnding => fileEnding;
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public ISpaceViewModel? Parent { get; set; }
    public ContentViewModel Content { get; set; }
    public string Url { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public ElementDifficultyEnum Difficulty { get; set; }
    public int Workload { get; set; }
    public int Points { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}