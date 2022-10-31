using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Shared;

namespace Presentation.PresentationLogic.LearningPathway;

public class PathWayConditionViewModel : IObjectInPathWayViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private PathWayConditionViewModel()
    {
        Id = Guid.Empty;
        InBoundObjects = new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = new Collection<IObjectInPathWayViewModel>();
        PositionX = 0;
        PositionY = 0;
        Condition = ConditionEnum.None;
    }
    
    public PathWayConditionViewModel(ConditionEnum condition, double positionX = 0, double positionY = 0,
        ICollection<IObjectInPathWayViewModel>? inBoundObjects = null, 
        ICollection<IObjectInPathWayViewModel>? outBoundObjects = null)
    {
        Id = Guid.NewGuid();
        Condition = condition;
        InBoundObjects = inBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = outBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        PositionX = positionX;
        PositionY = positionY;
    }
    
    public Guid Id { get; private set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public double InputConnectionX => PositionX - 26;
    public double InputConnectionY => PositionY;
    public double OutputConnectionX => PositionX + 26;
    public double OutputConnectionY => PositionY;
    public ICollection<IObjectInPathWayViewModel> InBoundObjects { get; set; }
    public ICollection<IObjectInPathWayViewModel> OutBoundObjects { get; set; }
    public ConditionEnum Condition { get; set; }
}