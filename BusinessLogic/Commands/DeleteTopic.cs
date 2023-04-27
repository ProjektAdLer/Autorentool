using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DeleteTopic : IUndoCommand
{
    public string Name => nameof(DeleteTopic);
    internal LearningWorld LearningWorld { get; }
    internal Topic Topic { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public DeleteTopic(LearningWorld learningWorld, Topic topic, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        Topic = LearningWorld.Topics.First(t => t.Id == topic.Id);
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        LearningWorld.UnsavedChanges = true;
        LearningWorld.Topics.Remove(Topic);

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