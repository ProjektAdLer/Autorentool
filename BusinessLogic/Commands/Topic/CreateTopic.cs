using BusinessLogic.Entities;
using Shared.Extensions;

namespace BusinessLogic.Commands.Topic;

public class CreateTopic : ICreateTopic
{
    public string Name => nameof(CreateTopic);
    internal LearningWorld LearningWorld { get; } 
    internal Entities.Topic Topic { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;
    
    public CreateTopic(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        Topic = new Entities.Topic(name);
        MappingAction = mappingAction;
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
        
        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningWorld.RestoreMemento(_memento);
        
        MappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}