using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Space;

public class EditLearningSpace : IEditLearningSpace
{
    public string Name => nameof(EditLearningSpace);
    internal ILearningSpace LearningSpace { get; }
    internal string SpaceName { get; }
    internal string Description { get; }
    internal string Goals { get; }
    internal Entities.Topic? Topic { get; }
    internal int RequiredPoints { get; }
    internal Theme Theme { get; }
    internal Action<ILearningSpace> MappingAction { get; }
    private ILogger<SpaceCommandFactory> Logger { get; }
    private IMemento? _memento;

    public EditLearningSpace(ILearningSpace learningSpace, string name, string description, string goals,
        int requiredPoints, Theme theme, Entities.Topic? topic, Action<ILearningSpace> mappingAction,
        ILogger<SpaceCommandFactory> logger)
    {
        LearningSpace = learningSpace;
        SpaceName = name;
        Description = description;
        Goals = goals;
        Topic = topic;
        RequiredPoints = requiredPoints;
        Theme = theme;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public void Execute()
    {
        _memento = LearningSpace.GetMemento();

        Logger.LogTrace(
            "Editing LearningSpace {id}. Previous Values: Name {PreviousName}, Description {PreviousDescription}, Goals {PreviousGoals}, RequiredPoints {PreviousRequiredPoints}, Theme {PreviousTheme}, Topic {PreviousTopic}",
            LearningSpace.Id, LearningSpace.Name, LearningSpace.Description, LearningSpace.Goals,
            LearningSpace.RequiredPoints, LearningSpace.Theme, LearningSpace.AssignedTopic?.Name);


        if (AnyChanges()) LearningSpace.UnsavedChanges = true;
        LearningSpace.Name = SpaceName;
        LearningSpace.Description = Description;
        LearningSpace.Goals = Goals;
        LearningSpace.AssignedTopic = Topic;
        LearningSpace.RequiredPoints = RequiredPoints;
        LearningSpace.Theme = Theme;

        Logger.LogTrace(
            "Edited LearningSpace {id}. Updated Values: Name {UpdatedName}, Description {UpdatedDescription}, Goals {UpdatedGoals}, RequiredPoints {UpdatedRequiredPoints}, Theme {UpdatedTheme}, Topic {UpdatedTopic}",
            LearningSpace.Id, LearningSpace.Name, LearningSpace.Description, LearningSpace.Goals,
            LearningSpace.RequiredPoints, LearningSpace.Theme, LearningSpace.AssignedTopic?.Name);

        MappingAction.Invoke(LearningSpace);
    }

    private bool AnyChanges() =>
        LearningSpace.Name != SpaceName ||
        LearningSpace.Description != Description ||
        LearningSpace.Goals != Goals ||
        LearningSpace.AssignedTopic != Topic ||
        LearningSpace.RequiredPoints != RequiredPoints;

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningSpace.RestoreMemento(_memento);

        Logger.LogTrace("Undone edit of LearningSpace {LearningSpaceName} ({LearningSpaceId})", LearningSpace.Name,
            LearningSpace.Id);

        MappingAction.Invoke(LearningSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditLearningSpace");
        Execute();
    }
}