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
        Condition = ConditionEnum.Or;
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
    public const int InputConnectionXOffset = 38;
    public const int InputConnectionYOffset = -10;
    public const int OutputConnectionXOffset = 38;
    public const int OutputConnectionYOffset = 45;
    public double InputConnectionX => PositionX + InputConnectionXOffset;
    public double InputConnectionY => PositionY + InputConnectionYOffset;
    public double OutputConnectionX => PositionX + OutputConnectionXOffset;
    public double OutputConnectionY => PositionY + OutputConnectionYOffset;
    public ICollection<IObjectInPathWayViewModel> InBoundObjects { get; set; }
    public ICollection<IObjectInPathWayViewModel> OutBoundObjects { get; set; }
    public ConditionEnum Condition { get; set; }
}