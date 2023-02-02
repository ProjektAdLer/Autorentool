using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space.SpaceLayout;

namespace Presentation.PresentationLogic.Space;

public interface ISpaceViewModel : IDisplayableObject, IObjectInPathWayViewModel
{
    IEnumerable<IElementViewModel> ContainedElements => SpaceLayout.ContainedElements;
    ISpaceLayoutViewModel SpaceLayout { get; set; }
    int Workload { get; }
    int Points { get; }
    int RequiredPoints { get; }
    new string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    IElementViewModel? SelectedElement { get; set; }
}