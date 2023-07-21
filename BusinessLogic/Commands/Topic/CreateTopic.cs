using BusinessLogic.Entities;
using Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Topic;

public class CreateTopic : ICreateTopic
{
    public string Name => nameof(CreateTopic);
    internal LearningWorld LearningWorld { get; } 
    internal Entities.Topic Topic { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<TopicCommandFactory> Logger { get; }
    private IMemento? _memento;
    
    public CreateTopic(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction, ILogger<TopicCommandFactory> logger)
    {
        LearningWorld = learningWorld;
        Topic = new Entities.Topic(name);
        MappingAction = mappingAction;
        Logger = logger;
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