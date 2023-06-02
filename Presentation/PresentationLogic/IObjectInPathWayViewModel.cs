namespace Presentation.PresentationLogic;

public interface IObjectInPathWayViewModel : ISelectableObjectInWorldViewModel
{
    double PositionX { get; set; }
    double PositionY { get; set; }
    double InputConnectionX { get; }
    double InputConnectionY { get; }
    double OutputConnectionX { get; }
    double OutputConnectionY { get; }
    ICollection<IObjectInPathWayViewModel> InBoundObjects { get; set; }
    ICollection<IObjectInPathWayViewModel> OutBoundObjects { get; set; }
    bool UnsavedChanges { get; }
}