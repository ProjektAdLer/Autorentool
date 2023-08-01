using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared.Extensions;

namespace BusinessLogic.Commands.Topic;

public class CreateTopic : ICreateTopic
{
    private IMemento? _memento;

    public CreateTopic(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction,
        ILogger<CreateTopic> logger)
    {
        LearningWorld = learningWorld;
        Topic = new Entities.Topic(name);
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal Entities.Topic Topic { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<CreateTopic> Logger { get; }
    public string Name => nameof(CreateTopic);

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.UnsavedChanges = true;
        if (LearningWorld.Topics.Any(t => t.Name == Topic.Name))
        {
            Topic.Name = StringHelper.GetUniqueName(LearningWorld.Topics.Select(t => t.Name), Topic.Name);
        }

        LearningWorld.Topics.Add(Topic);
        Logger.LogTrace("Created Topic {TopicName} ({TopicId})", Topic.Name, Topic.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);
        Logger.LogTrace("Undone creation of Topic {TopicName} ({TopicId})", Topic.Name, Topic.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateTopic");
        Execute();
    }
}