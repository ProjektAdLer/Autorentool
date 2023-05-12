using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Topic;

public class DeleteTopic : IDeleteTopic
{
    public string Name => nameof(DeleteTopic);
    internal LearningWorld LearningWorld { get; }
    internal Entities.Topic Topic { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;

    public DeleteTopic(LearningWorld learningWorld, ITopic topic, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        Topic = LearningWorld.Topics.First(t => t.Id == topic.Id);
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        LearningWorld.UnsavedChanges = true;
        LearningWorld.Topics.Remove(Topic);

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