using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Space;

public class EditLearningSpace : IEditLearningSpace
{
    public string Name => nameof(EditLearningSpace);
    internal LearningSpace LearningSpace { get; }
    internal string SpaceName { get; }
    internal string Description { get; }
    internal string Goals { get; }
    internal Entities.Topic? Topic { get; }
    internal int RequiredPoints { get; }
    internal Theme _theme { get; }
    internal Action<LearningSpace> MappingAction { get; }
    private IMemento? _memento;

    public EditLearningSpace(LearningSpace learningSpace, string name, string description, string goals,
        int requiredPoints, Theme theme, Entities.Topic? topic, Action<LearningSpace> mappingAction)
    {
        LearningSpace = learningSpace;
        SpaceName = name;
        Description = description;
        Goals = goals;
        Topic = topic;
        RequiredPoints = requiredPoints;
        _theme = theme;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningSpace.GetMemento();

        if(AnyChanges()) LearningSpace.UnsavedChanges = true;
        LearningSpace.Name = SpaceName;
        LearningSpace.Description = Description;
        LearningSpace.Goals = Goals;
        LearningSpace.AssignedTopic = Topic;
        LearningSpace.RequiredPoints = RequiredPoints;
        LearningSpace.Theme = _theme;
        
        MappingAction.Invoke(LearningSpace);
    }
    
    private bool AnyChanges() =>
        LearningSpace.Name != SpaceName ||
        LearningSpace.Description != Description ||
        LearningSpace.Goals != Goals ||
        LearningSpace.AssignedTopic != Topic ||
        LearningSpace.RequiredPoints != RequiredPoints;

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningSpace.RestoreMemento(_memento);
        
        MappingAction.Invoke(LearningSpace);
    }

    public void Redo() => Execute();
}