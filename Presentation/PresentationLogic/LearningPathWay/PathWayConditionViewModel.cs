using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Shared;

namespace Presentation.PresentationLogic.LearningPathway;

public class PathWayConditionViewModel : IObjectInPathWayViewModel
{
    public const int InputConnectionXOffset = 38;
    public const int InputConnectionYOffset = -10;
    public const int OutputConnectionXOffset = 38;
    public const int OutputConnectionYOffset = 45;

    private double _positionX;
    private double _positionY;

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
        UnsavedChanges = false;
    }

    public PathWayConditionViewModel(ConditionEnum condition, bool unsavedChanges,
        double positionX = 0, double positionY = 0,
        ICollection<IObjectInPathWayViewModel>? inBoundObjects = null,
        ICollection<IObjectInPathWayViewModel>? outBoundObjects = null)
    {
        Id = Guid.NewGuid();
        Condition = condition;
        UnsavedChanges = unsavedChanges;
        InBoundObjects = inBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = outBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        PositionX = positionX;
        PositionY = positionY;
    }

    public ConditionEnum Condition { get; set; }

    public Guid Id { get; private set; }

    public double PositionX
    {
        get => _positionX;
        set
        {
            _positionX = value switch
            {
                < 0 => 0,
                _ => value
            };
        }
    }

    public double PositionY
    {
        get => _positionY;
        set
        {
            _positionY = value switch
            {
                < 0 => 0,
                _ => value
            };
        }
    }

    public double InputConnectionX => PositionX + InputConnectionXOffset;
    public double InputConnectionY => PositionY + InputConnectionYOffset;
    public double OutputConnectionX => PositionX + OutputConnectionXOffset;
    public double OutputConnectionY => PositionY + OutputConnectionYOffset;
    public bool UnsavedChanges { get; }
    public ICollection<IObjectInPathWayViewModel> InBoundObjects { get; set; }
    public ICollection<IObjectInPathWayViewModel> OutBoundObjects { get; set; }
}