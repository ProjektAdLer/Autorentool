using BusinessLogic.Entities;
using Shared.Extensions;

namespace BusinessLogic.Commands;

public class CreateTopic : IUndoCommand
{
    public string Name => nameof(CreateTopic);
    internal LearningWorld LearningWorld { get; } 
    internal Topic Topic { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;
    
    public CreateTopic(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        Topic = new Topic(name);
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.UnsavedChanges = true;
        if (LearningWorld.Topics.Any(t => t.Name == Topic.Name))
        {
            Topic.Name = StringHelper.GetUniqueName(LearningWorld.Topics.Select(t => t.Name), Topic.Name);
        }

        LearningWorld.Topics.Add(Topic);
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningWorld.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}