using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands.Element;

public class EditLearningElement : IEditLearningElement
{
    public string Name => nameof(EditLearningElement);
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
    private IMemento? _memento;
    
    public EditLearningElement(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points, ILearningContent learningContent, Action<LearningElement> mappingAction)
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
    }

    public void Execute()
    {
        _memento = LearningElement.GetMemento();

        if(AnyChange()) LearningElement.UnsavedChanges = true;
        LearningElement.Name = ElementName;
        LearningElement.Parent = ParentSpace;
        LearningElement.Description = Description;
        LearningElement.Goals = Goals;
        LearningElement.Difficulty = Difficulty;
        LearningElement.ElementModel = ElementModel;
        LearningElement.Workload = Workload;
        LearningElement.Points = Points;
        LearningElement.LearningContent = LearningContent;
        
        MappingAction.Invoke(LearningElement);
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

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningElement.RestoreMemento(_memento);

        MappingAction.Invoke(LearningElement);
    }

    public void Redo() => Execute();
}