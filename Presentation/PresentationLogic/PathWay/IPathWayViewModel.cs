namespace Presentation.PresentationLogic.PathWay;

public interface IPathWayViewModel : ISelectableObjectInWorldViewModel
{
    IObjectInPathWayViewModel SourceObject { get; set; }
    IObjectInPathWayViewModel TargetObject { get; set; }
}