using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Topic;

public class EditTopic : IEditTopic
{
    public string Name => nameof(EditTopic);
    internal Entities.Topic Topic { get; }
    internal string TopicName { get; }
    internal Action<Entities.Topic> MappingAction { get; }
    private IMemento? _memento;
    
    public EditTopic(Entities.Topic topic, string name, Action<Entities.Topic> mappingAction)
    {
        Topic = topic;
        TopicName = name;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = Topic.GetMemento();
        
        if (Topic.Name != TopicName) Topic.UnsavedChanges = true;
        Topic.Name = TopicName;
        
        MappingAction.Invoke(Topic);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        Topic.RestoreMemento(_memento);
        
        MappingAction.Invoke(Topic);
    }

    public void Redo() => Execute();
}