using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.AdvancedSpaceGenerator.AdvancedSpace;

public class EditAdvancedLearningSpace : IEditAdvancedLearningSpace
{
    private IMemento? _memento;

    public EditAdvancedLearningSpace(IAdvancedLearningSpace advancedLearningSpace, string name, string description, string goals,
        int requiredPoints, Theme theme, Entities.Topic? topic, Action<IAdvancedLearningSpace> mappingAction,
        ILogger<EditAdvancedLearningSpace> logger)
    {
        AdvancedLearningSpace = advancedLearningSpace;
        SpaceName = name;
        Description = description;
        Goals = goals;
        Topic = topic;
        RequiredPoints = requiredPoints;
        Theme = theme;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal IAdvancedLearningSpace AdvancedLearningSpace { get; }
    internal string SpaceName { get; }
    internal string Description { get; }
    internal string Goals { get; }
    internal Entities.Topic? Topic { get; }
    internal int RequiredPoints { get; }
    internal Theme Theme { get; }
    internal Action<IAdvancedLearningSpace> MappingAction { get; }
    private ILogger<EditAdvancedLearningSpace> Logger { get; }
    public string Name => nameof(EditLearningSpace);

    public void Execute()
    {
        _memento = AdvancedLearningSpace.GetMemento();

        Logger.LogTrace(
            "Editing AdvancedLearningSpace {Id}. Previous Values: Name {PreviousName}, Description {PreviousDescription}, Goals {PreviousGoals}, RequiredPoints {PreviousRequiredPoints}, Theme {PreviousTheme}, Topic {PreviousTopic}",
            AdvancedLearningSpace.Id, AdvancedLearningSpace.Name, AdvancedLearningSpace.Description, AdvancedLearningSpace.Goals,
            AdvancedLearningSpace.RequiredPoints, AdvancedLearningSpace.Theme, AdvancedLearningSpace.AssignedTopic?.Name);


        if (AnyChanges()) AdvancedLearningSpace.UnsavedChanges = true;
        AdvancedLearningSpace.Name = SpaceName;
        AdvancedLearningSpace.Description = Description;
        AdvancedLearningSpace.Goals = Goals;
        AdvancedLearningSpace.AssignedTopic = Topic;
        AdvancedLearningSpace.RequiredPoints = RequiredPoints;
        AdvancedLearningSpace.Theme = Theme;

        Logger.LogTrace(
            "Edited AdvancedLearningSpace {Id}. Updated Values: Name {UpdatedName}, Description {UpdatedDescription}, Goals {UpdatedGoals}, RequiredPoints {UpdatedRequiredPoints}, Theme {UpdatedTheme}, Topic {UpdatedTopic}",
            AdvancedLearningSpace.Id, AdvancedLearningSpace.Name, AdvancedLearningSpace.Description, AdvancedLearningSpace.Goals,
            AdvancedLearningSpace.RequiredPoints, AdvancedLearningSpace.Theme, AdvancedLearningSpace.AssignedTopic?.Name);

        MappingAction.Invoke(AdvancedLearningSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AdvancedLearningSpace.RestoreMemento(_memento);

        Logger.LogTrace("Undone edit of AdvancedLearningSpace {LearningSpaceName} ({LearningSpaceId})", AdvancedLearningSpace.Name,
            AdvancedLearningSpace.Id);

        MappingAction.Invoke(AdvancedLearningSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditAdvancedLearningSpace");
        Execute();
    }

    public bool AnyChanges() =>
        AdvancedLearningSpace.Name != SpaceName ||
        AdvancedLearningSpace.Description != Description ||
        AdvancedLearningSpace.Goals != Goals ||
        AdvancedLearningSpace.AssignedTopic != Topic ||
        AdvancedLearningSpace.RequiredPoints != RequiredPoints;
}