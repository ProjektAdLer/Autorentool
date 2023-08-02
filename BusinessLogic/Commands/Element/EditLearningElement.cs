using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Element;

public class EditLearningElement : IEditLearningElement
{
    private IMemento? _memento;

    public EditLearningElement(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points, ILearningContent learningContent, Action<LearningElement> mappingAction,
        ILogger<EditLearningElement> logger)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        ElementName = name;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        ElementModel = elementModel;
        Workload = workload;
        Points = points;
        LearningContent = learningContent;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningElement LearningElement { get; }
    internal LearningSpace? ParentSpace { get; }
    internal string ElementName { get; }
    internal string Description { get; }
    internal string Goals { get; }
    internal LearningElementDifficultyEnum Difficulty { get; }
    internal ElementModel ElementModel { get; }
    internal int Workload { get; }
    internal int Points { get; }
    internal ILearningContent LearningContent { get; }
    internal Action<LearningElement> MappingAction { get; }
    private ILogger<EditLearningElement> Logger { get; }
    public string Name => nameof(EditLearningElement);

    public void Execute()
    {
        _memento = LearningElement.GetMemento();

        Logger.LogTrace(
            "Editing LearningElement {Id}. Previous Values: Name {PreviousName}, Parent {PreviousParentName}, Description {PreviousDescription}, Goals {PreviousGoals}, Difficulty {PreviousDifficulty}, ElementModel {PreviousElementModel}, Workload {PreviousWorkload}, Points {PreviousPoints}, LearningContent {PreviousLearningContent}",
            LearningElement.Id, LearningElement.Name, LearningElement.Parent?.Name, LearningElement.Description,
            LearningElement.Goals, LearningElement.Difficulty, LearningElement.ElementModel, LearningElement.Workload,
            LearningElement.Points, LearningElement.LearningContent.Name);

        if (AnyChange()) LearningElement.UnsavedChanges = true;
        LearningElement.Name = ElementName;
        LearningElement.Parent = ParentSpace;
        LearningElement.Description = Description;
        LearningElement.Goals = Goals;
        LearningElement.Difficulty = Difficulty;
        LearningElement.ElementModel = ElementModel;
        LearningElement.Workload = Workload;
        LearningElement.Points = Points;
        LearningElement.LearningContent = LearningContent;

        Logger.LogTrace(
            "Edited LearningElement {Id}. Updated Values: Name {PreviousName}, Parent {PreviousParentName}, Description {PreviousDescription}, Goals {PreviousGoals}, Difficulty {PreviousDifficulty}, ElementModel {PreviousElementModel}, Workload {PreviousWorkload}, Points {PreviousPoints}, LearningContent {PreviousLearningContent}",
            LearningElement.Id, LearningElement.Name, LearningElement.Parent?.Name, LearningElement.Description,
            LearningElement.Goals, LearningElement.Difficulty, LearningElement.ElementModel, LearningElement.Workload,
            LearningElement.Points, LearningElement.LearningContent.Name);

        MappingAction.Invoke(LearningElement);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningElement.RestoreMemento(_memento);

        Logger.LogTrace("Undone edit of LearningElement {LearningElementName} ({LearningElementId})",
            LearningElement.Name, LearningElement.Id);

        MappingAction.Invoke(LearningElement);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditLearningElement");
        Execute();
    }

    private bool AnyChange() =>
        LearningElement.Name != ElementName ||
        LearningElement.Parent != ParentSpace ||
        LearningElement.Description != Description ||
        LearningElement.Goals != Goals ||
        LearningElement.Difficulty != Difficulty ||
        LearningElement.ElementModel != ElementModel ||
        LearningElement.Workload != Workload ||
        LearningElement.Points != Points ||
        LearningElement.LearningContent != LearningContent;
}