using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Topic;

public class DeleteTopic : IDeleteTopic
{
    public string Name => nameof(DeleteTopic);
    internal LearningWorld LearningWorld { get; }
    internal Entities.Topic Topic { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<TopicCommandFactory> Logger { get; }
    private IMemento? _memento;

    public DeleteTopic(LearningWorld learningWorld, ITopic topic, Action<LearningWorld> mappingAction, ILogger<TopicCommandFactory> logger)
    {
        LearningWorld = learningWorld;
        Topic = LearningWorld.Topics.First(t => t.Id == topic.Id);
        MappingAction = mappingAction;
        Logger = logger;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        LearningWorld.UnsavedChanges = true;
        LearningWorld.Topics.Remove(Topic);

        Logger.LogTrace("Deleted Topic {TopicName} ({TopicId})", Topic.Name, Topic.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace("Undone deletion of Topic {TopicName} ({TopicId})", Topic.Name, Topic.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteTopic");
        Execute();
    }
}