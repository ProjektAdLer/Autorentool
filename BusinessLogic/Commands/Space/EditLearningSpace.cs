using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Space;

public class EditLearningSpace : IEditLearningSpace
{
    public string Name => nameof(EditLearningSpace);
    internal ILearningSpace LearningSpace { get; }
    internal string SpaceName { get; }
    internal string Description { get; }
    internal string Goals { get; }
    internal Entities.Topic? Topic { get; }
    internal int RequiredPoints { get; }
    internal Theme Theme { get; }
    internal Action<ILearningSpace> MappingAction { get; }
    private IMemento? _memento;

    public EditLearningSpace(ILearningSpace learningSpace, string name, string description, string goals,
        int requiredPoints, Theme theme, Entities.Topic? topic, Action<ILearningSpace> mappingAction)
    {
        LearningSpace = learningSpace;
        SpaceName = name;
        Description = description;
        Goals = goals;
        Topic = topic;
        RequiredPoints = requiredPoints;
        Theme = theme;
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
        LearningSpace.Theme = Theme;
        
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