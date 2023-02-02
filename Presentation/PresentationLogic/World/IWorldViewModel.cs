using System.ComponentModel;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;

namespace Presentation.PresentationLogic.World;

public interface IWorldViewModel : ISerializableViewModel, IDisplayableObject, INotifyPropertyChanged
{
    new string FileEnding { get; }
    int Workload { get; }
    int Points { get; }
    ICollection<ISpaceViewModel> Spaces { get; }
    ICollection<PathWayConditionViewModel> PathWayConditions { get; }
    IEnumerable<IObjectInPathWayViewModel> ObjectsInPathWays { get; }
    ICollection<IPathWayViewModel> PathWays { get; }
    new string Name { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    bool UnsavedChanges { get; set; }
    ISelectableObjectInWorldViewModel? SelectedObject { get; set; }
    IObjectInPathWayViewModel? OnHoveredObjectInPathWay { get; set; }
    bool ShowingSpaceView { get; set; }
    ICollection<IElementViewModel> UnplacedElements { get; set; }
}