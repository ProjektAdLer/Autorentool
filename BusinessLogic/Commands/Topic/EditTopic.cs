using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Topic;

public class EditTopic : IEditTopic
{
    private IMemento? _memento;

    public EditTopic(Entities.Topic topic, string name, Action<Entities.Topic> mappingAction, ILogger<EditTopic> logger)
    {
        Topic = topic;
        TopicName = name;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal Entities.Topic Topic { get; }
    internal string TopicName { get; }
    internal Action<Entities.Topic> MappingAction { get; }
    private ILogger<EditTopic> Logger { get; }
    public string Name => nameof(EditTopic);

    public void Execute()
    {
        _memento = Topic.GetMemento();

        Logger.LogTrace(
            "Editing Topic {id}. Previous Name: {PreviousName}",
            Topic.Id, Topic.Name
        );

        if (Topic.Name != TopicName) Topic.UnsavedChanges = true;
        Topic.Name = TopicName;

        Logger.LogTrace(
            "Edited Topic {id}. Updated Name: {UpdatedName}",
            Topic.Id, Topic.Name
        );

        MappingAction.Invoke(Topic);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        Topic.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone editing of Topic {id}. Restored Name: {RestoredName}",
            Topic.Id, Topic.Name
        );

        MappingAction.Invoke(Topic);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditTopic");
        Execute();
    }
}