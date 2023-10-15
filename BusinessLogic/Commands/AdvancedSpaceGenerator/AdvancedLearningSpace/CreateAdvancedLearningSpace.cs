using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Space.AdvancedLearningSpace;

public class CreateAdvancedLearningSpace: ICreateAdvancedLearningSpace
{
    private IMemento? _memento;

    public CreateAdvancedLearningSpace(LearningWorld learningWorld, string name, string description, string goals,
        int requiredPoints, Theme theme, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction, ILogger<CreateAdvancedLearningSpace> logger)
    {
        AdvancedLearningSpace = new Entities.AdvancedLearningSpaces.AdvancedLearningSpace(name, description, goals, requiredPoints, theme, positionX: positionX,
            positionY: positionY, assignedTopic: topic);
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public CreateAdvancedLearningSpace(LearningWorld learningWorld, Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction, ILogger<CreateAdvancedLearningSpace> logger)
    {
        AdvancedLearningSpace = advancedLearningSpace;
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal Entities.AdvancedLearningSpaces.AdvancedLearningSpace AdvancedLearningSpace { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<CreateAdvancedLearningSpace> Logger { get; }
    public string Name => nameof(CreateAdvancedLearningSpace);

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.LearningSpaces.Add(AdvancedLearningSpace);

        Logger.LogTrace(
            "Created AdvancedLearningSpace {LearningSpaceName} ({LearningSpaceId}). Name: {Name}, Description: {Description}, Goals: {Goals}, RequiredPoints: {RequiredPoints}, Theme: {Theme}, PositionX: {PositionX}, PositionY: {PositionY}, Topic: {Topic}",
            AdvancedLearningSpace.Name, AdvancedLearningSpace.Id, AdvancedLearningSpace.Name, AdvancedLearningSpace.Description, AdvancedLearningSpace.Goals,
            AdvancedLearningSpace.RequiredPoints, AdvancedLearningSpace.Theme, AdvancedLearningSpace.PositionX, AdvancedLearningSpace.PositionY,
            AdvancedLearningSpace.AssignedTopic?.Name);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace("Undone creation of AdvancedLearningSpace {LearningSpaceName} ({LearningSpaceId})", AdvancedLearningSpace.Name,
            AdvancedLearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateLearningSpace");
        Execute();
    }
}