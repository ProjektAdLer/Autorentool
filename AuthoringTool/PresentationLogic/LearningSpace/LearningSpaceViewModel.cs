using System.Collections.ObjectModel;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.LearningSpace;

public class LearningSpaceViewModel : ILearningObjectViewModel
{
    public LearningSpaceViewModel(string name, string shortname, string authors, string description,
        string goals, ICollection<LearningElementViewModel>? learningElements = null, int positionX = 0,
        int positionY = 0)
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

    public ICollection<LearningElementViewModel> LearningElements { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}