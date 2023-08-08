using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Pathway;

public class CreateLearningPathWay : ICreateLearningPathWay
{
    private IMemento? _memento;


    public CreateLearningPathWay(LearningWorld learningWorld, IObjectInPathWay sourceObject,
        IObjectInPathWay targetObject,
        Action<LearningWorld> mappingAction, ILogger<CreateLearningPathWay> logger)
    {
        LearningPathway = new LearningPathway(sourceObject, targetObject);
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal LearningPathway LearningPathway { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<CreateLearningPathWay> Logger { get; }
    public string Name => nameof(CreateLearningPathWay);
    public bool HasError { get; private set; }


    public void Execute()
    {
        if (LearningWorld.LearningPathways.Any(x =>
                x.SourceObject == LearningPathway.SourceObject && x.TargetObject == LearningPathway.TargetObject))
        {
            HasError = true;
            Logger.LogWarning("LearningPathway already exists in world");
            return;
        }

        if (LearningPathway.SourceObject.Id == LearningPathway.TargetObject.Id)
        {
            HasError = true;
            Logger.LogWarning("LearningPathway source equals target");
            return;
        }

        if (IsCircular(LearningPathway))
        {
            HasError = true;
            Logger.LogWarning("LearningPathway is circular");
            return;
        }

        if (LearningPathway.TargetObject is LearningSpace &&
            LearningWorld.LearningPathways.Any(x => x.TargetObject.Id == LearningPathway.TargetObject.Id))
        {
            HasError = true;
            Logger.LogWarning("Space already has a path going into it");
            return;
        }

        _memento = LearningWorld.GetMemento();

        LearningWorld.UnsavedChanges = true;
        LearningWorld.LearningPathways.Add(LearningPathway);

        Logger.LogTrace(
            "Created LearningPathway from {SourceObjectId} to {TargetObjectId} in LearningWorld {LearningWorldName} {LearningWorldId}",
            LearningPathway.SourceObject.Id, LearningPathway.TargetObject.Id, LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone creation of LearningPathway from {SourceObjectId} to {TargetObjectId} in LearningWorld {LearningWorldName} {LearningWorldId}",
            LearningPathway.SourceObject.Id, LearningPathway.TargetObject.Id, LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateLearningPathWay");
        Execute();
    }

    private bool IsCircular(LearningPathway learningPathway)
    {
        var isCircular = false;
        var sourceObject = LearningWorld.ObjectsInPathWays.First(x => x.Id == learningPathway.SourceObject.Id);
        var targetObject = LearningWorld.ObjectsInPathWays.First(x => x.Id == learningPathway.TargetObject.Id);

        if (!targetObject.OutBoundObjects.Any())
        {
            return false;
        }

        var outBoundObject = targetObject.OutBoundObjects;
        var outBoundObjectSum = new List<IObjectInPathWay>();

        while (isCircular == false)
        {
            foreach (var space in outBoundObject)
            {
                if (space.Id == sourceObject.Id)
                {
                    isCircular = true;
                }

                outBoundObjectSum.AddRange(space.OutBoundObjects);
            }

            if (!outBoundObject.Any())
            {
                break;
            }

            outBoundObject = outBoundObjectSum;
            outBoundObjectSum = new List<IObjectInPathWay>();
        }

        return isCircular;
    }
}