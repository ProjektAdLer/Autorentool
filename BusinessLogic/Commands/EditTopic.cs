using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class EditTopic : IUndoCommand
{
    public string Name => nameof(EditTopic);
    internal Topic Topic { get; }
    private readonly string _name;
    private readonly Action<Topic> _mappingAction;
    private IMemento? _memento;
    
    public EditTopic(Topic topic, string name, Action<Topic> mappingAction)
    {
        Topic = topic;
        _name = name;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = Topic.GetMemento();
        
        if (Topic.Name != _name) Topic.UnsavedChanges = true;
        Topic.Name = _name;
        
        _mappingAction.Invoke(Topic);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        Topic.RestoreMemento(_memento);
        
        _mappingAction.Invoke(Topic);
    }

    public void Redo() => Execute();
}