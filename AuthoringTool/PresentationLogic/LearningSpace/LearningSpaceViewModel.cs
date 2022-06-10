using System.Collections.ObjectModel;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.LearningSpace;

public class LearningSpaceViewModel : ILearningObjectViewModel, ILearningElementViewModelParent, ISerializableViewModel, IDisplayableLearningObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning space</param>
    /// <param name="shortname">The short name (abbreviation) of the learning world.(Maybe not relevant)</param>
    /// <param name="authors">The string containing the names of all the authors working on the learning space.</param>
    /// <param name="description">A description of the learning space and its contents.</param>
    /// <param name="goals">A description of the goals this learning space is supposed to achieve.</param>
    /// <param name="learningElements">Optional collection of learning elements contained in the learning space.</param>
    /// <param name="positionX">x-position of the learning space in the workspace</param>
    /// <param name="positionY">y-position of the learning space in the workspace</param>
    public LearningSpaceViewModel(string name, string shortname, string authors, string description, string goals,
        ICollection<LearningElementViewModel>? learningElements = null, double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new Collection<LearningElementViewModel>();
        PositionX = positionX;
        PositionY = positionY;
    }

    public const string fileEnding = "asf";
    public string FileEnding => fileEnding;
    public ICollection<LearningElementViewModel> LearningElements { get; set; }
    public int Workload => LearningElements.Sum(element => element.Workload);
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public ILearningObjectViewModel? SelectedLearningObject { get; set; }
    public Dictionary<string, string> EditDialogInitialValues { get; set; }
}
