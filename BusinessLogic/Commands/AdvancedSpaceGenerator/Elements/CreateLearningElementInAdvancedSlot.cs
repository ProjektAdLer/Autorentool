using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.AdvancedSpaceGenerator.Elements;

public class CreateLearningElementInAdvancedSlot : ICreateLearningElementInAdvancedSlot
{
    private IMemento? _memento;
    private IMemento? _mementoAdvancedSpaceLayout;

    public CreateLearningElementInAdvancedSlot(AdvancedLearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points, 
        Action<AdvancedLearningSpace> mappingAction, ILogger<CreateLearningElementInAdvancedSlot> logger)
    {
        LearningElement = new LearningElement(name, learningContent, description, goals,
            difficulty, elementModel, parentSpace, workload: workload, points: points);
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public CreateLearningElementInAdvancedSlot(AdvancedLearningSpace parentSpace, int slotIndex, LearningElement learningElement,
        Action<AdvancedLearningSpace> mappingAction, ILogger<CreateLearningElementInAdvancedSlot> logger)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AdvancedLearningSpace ParentSpace { get; }
    internal int SlotIndex { get; }
    internal LearningElement LearningElement { get; }
    internal Action<AdvancedLearningSpace> MappingAction { get; }
    private ILogger<CreateLearningElementInAdvancedSlot> Logger { get; }
    public string Name => nameof(CreateLearningElementInAdvancedSlot);

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoAdvancedSpaceLayout = ParentSpace.AdvancedLearningSpaceLayout.GetMemento();

        ParentSpace.UnsavedChanges = true;
        ParentSpace.AdvancedLearningSpaceLayout.LearningElements[SlotIndex] = LearningElement;

        Logger.LogTrace(
            "Created LearningElement {LearningElementName} ({LearningElementId}) in advanced slot {SlotIndex} of ParentSpace {ParentSpaceName}({ParentSpaceId}). LearningContent: {LearningContent}, Description: {Description}, Goals: {Goals}, Difficulty: {Difficulty}, ElementModel: {ElementModel}, Workload: {Workload}, Points: {Points}",
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

        if (_mementoAdvancedSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoAdvancedSpaceLayout is null");
        }

        ParentSpace.RestoreMemento(_memento);
        ParentSpace.AdvancedLearningSpaceLayout.RestoreMemento(_mementoAdvancedSpaceLayout);

        Logger.LogTrace(
            "Undone creation of LearningElement {LearningElementName} ({LearningElementId}). Restored ParentSpace {ParentSpaceName} ({ParentSpaceId}) and AdvancedLearningSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateLearningElementInAdvancedSlot");
        Execute();
    }
}