using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Theme;

namespace BusinessLogic.Commands.Space;

public class CreateLearningSpace : ICreateLearningSpace
{
    private IMemento? _memento;

    public CreateLearningSpace(LearningWorld learningWorld, string name, string description,
        LearningOutcomeCollection learningOutcomes,
        int requiredPoints, SpaceTheme spaceTheme, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction, ILogger<CreateLearningSpace> logger)
    {
        LearningSpace = new LearningSpace(name, description, requiredPoints, spaceTheme, learningOutcomes,
            positionX: positionX,
            positionY: positionY, assignedTopic: topic);
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public CreateLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction, ILogger<CreateLearningSpace> logger)
    {
        LearningSpace = learningSpace;
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<CreateLearningSpace> Logger { get; }
    public string Name => nameof(CreateLearningSpace);

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.LearningSpaces.Add(LearningSpace);

        Logger.LogTrace(
            "Created LearningSpace {LearningSpaceName} ({LearningSpaceId}). Name: {Name}, Description: {Description}, Goals: {Goals}, RequiredPoints: {RequiredPoints}, Theme: {Theme}, PositionX: {PositionX}, PositionY: {PositionY}, Topic: {Topic}",
            LearningSpace.Name, LearningSpace.Id, LearningSpace.Name, LearningSpace.Description,
            LearningSpace.LearningOutcomeCollection,
            LearningSpace.RequiredPoints, LearningSpace.SpaceTheme, LearningSpace.PositionX, LearningSpace.PositionY,
            LearningSpace.AssignedTopic?.Name);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace("Undone creation of LearningSpace {LearningSpaceName} ({LearningSpaceId})", LearningSpace.Name,
            LearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateLearningSpace");
        Execute();
    }

    public ILearningSpace NewSpace => LearningSpace;
}