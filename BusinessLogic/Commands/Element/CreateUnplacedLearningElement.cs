using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Element;

public class CreateUnplacedLearningElement : ICreateUnplacedLearningElement
{
    public string Name => nameof(CreateUnplacedLearningElement);
    internal LearningWorld LearningWorld { get; }
    internal LearningElement LearningElement { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<ElementCommandFactory> Logger { get; }
    private IMemento? _memento;

    public CreateUnplacedLearningElement(LearningWorld learningWorld, string name,
        ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points, double positionX, double positionY,
        Action<LearningWorld> mappingAction, ILogger<ElementCommandFactory> logger)
    {
        LearningElement = new LearningElement(name,  learningContent, description, goals,
            difficulty, elementModel, null, workload, points, positionX, positionY);
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.UnsavedChanges = true;
        LearningWorld.UnplacedLearningElements.Add(LearningElement);

        Logger.LogTrace("Created unplaced LearningElement {LearningElementName} ({LearningElementId}) in LearningWorld {LearningWorldName} ({LearningWorldId})", LearningElement.Name, LearningElement.Id, LearningWorld.Name, LearningWorld.Id);
        
        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace("Undone creation of unplaced LearningElement {LearningElementName} ({LearningElementId}). Restored LearningWorld {LearningWorldName} ({LearningWorldId}) to previous state", LearningElement.Name, LearningElement.Id, LearningWorld.Name, LearningWorld.Id);
        
        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateUnplacedLearningElement");
        Execute();
    }
}