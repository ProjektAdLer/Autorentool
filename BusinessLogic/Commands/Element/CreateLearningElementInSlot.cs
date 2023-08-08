using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Element;

public class CreateLearningElementInSlot : ICreateLearningElementInSlot
{
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public CreateLearningElementInSlot(LearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points, double positionX,
        double positionY,
        Action<LearningSpace> mappingAction, ILogger<CreateLearningElementInSlot> logger)
    {
        LearningElement = new LearningElement(name, learningContent, description, goals,
            difficulty, elementModel, parentSpace, workload: workload, points: points, positionX: positionX,
            positionY: positionY);
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public CreateLearningElementInSlot(LearningSpace parentSpace, int slotIndex, LearningElement learningElement,
        Action<LearningSpace> mappingAction, ILogger<CreateLearningElementInSlot> logger)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningSpace ParentSpace { get; }
    internal int SlotIndex { get; }
    internal LearningElement LearningElement { get; }
    internal Action<LearningSpace> MappingAction { get; }
    private ILogger<CreateLearningElementInSlot> Logger { get; }
    public string Name => nameof(CreateLearningElementInSlot);

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();

        ParentSpace.UnsavedChanges = true;
        ParentSpace.LearningSpaceLayout.LearningElements[SlotIndex] = LearningElement;

        Logger.LogTrace(
            "Created LearningElement {LearningElementName} ({LearningElementId}) in slot {SlotIndex} of ParentSpace {ParentSpaceName}({ParentSpaceId}). LearningContent: {LearningContent}, Description: {Description}, Goals: {Goals}, Difficulty: {Difficulty}, ElementModel: {ElementModel}, Workload: {Workload}, Points: {Points}",
            LearningElement.Name, LearningElement.Id, SlotIndex, ParentSpace.Name, ParentSpace.Id,
            LearningElement.LearningContent.Name, LearningElement.Description, LearningElement.Goals,
            LearningElement.Difficulty, LearningElement.ElementModel, LearningElement.Workload, LearningElement.Points);

        MappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        ParentSpace.RestoreMemento(_memento);
        ParentSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        Logger.LogTrace(
            "Undone creation of LearningElement {LearningElementName} ({LearningElementId}). Restored ParentSpace {ParentSpaceName} ({ParentSpaceId}) and LearningSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateLearningElementInSlot");
        Execute();
    }
}