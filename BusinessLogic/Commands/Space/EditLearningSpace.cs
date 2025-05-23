using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Theme;

namespace BusinessLogic.Commands.Space;

public class EditLearningSpace : IEditLearningSpace
{
    private IMemento? _memento;

    public EditLearningSpace(ILearningSpace learningSpace, string name, string description,
        int requiredPoints, SpaceTheme spaceTheme, Entities.Topic? topic, Action<ILearningSpace> mappingAction,
        ILogger<EditLearningSpace> logger)
    {
        LearningSpace = learningSpace;
        SpaceName = name;
        Description = description;
        Topic = topic;
        RequiredPoints = requiredPoints;
        SpaceTheme = spaceTheme;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal ILearningSpace LearningSpace { get; }
    internal string SpaceName { get; }
    internal string Description { get; }
    internal Entities.Topic? Topic { get; }
    internal int RequiredPoints { get; }
    internal SpaceTheme SpaceTheme { get; }
    internal Action<ILearningSpace> MappingAction { get; }
    private ILogger<EditLearningSpace> Logger { get; }
    public string Name => nameof(EditLearningSpace);

    public void Execute()
    {
        _memento = LearningSpace.GetMemento();

        Logger.LogTrace(
            "Editing LearningSpace {Id}. Previous Values: Name {PreviousName}, Description {PreviousDescription}, RequiredPoints {PreviousRequiredPoints}, Theme {PreviousTheme}, Topic {PreviousTopic}",
            LearningSpace.Id, LearningSpace.Name, LearningSpace.Description,
            LearningSpace.RequiredPoints, LearningSpace.SpaceTheme, LearningSpace.AssignedTopic?.Name);


        if (AnyChanges()) LearningSpace.UnsavedChanges = true;
        LearningSpace.Name = SpaceName;
        LearningSpace.Description = Description;
        LearningSpace.AssignedTopic = Topic;
        LearningSpace.RequiredPoints = RequiredPoints;
        LearningSpace.SpaceTheme = SpaceTheme;

        Logger.LogTrace(
            "Edited LearningSpace {Id}. Updated Values: Name {UpdatedName}, Description {UpdatedDescription},  RequiredPoints {UpdatedRequiredPoints}, Theme {UpdatedTheme}, Topic {UpdatedTopic}",
            LearningSpace.Id, LearningSpace.Name, LearningSpace.Description,
            LearningSpace.RequiredPoints, LearningSpace.SpaceTheme, LearningSpace.AssignedTopic?.Name);

        MappingAction.Invoke(LearningSpace);
    }

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

    public bool AnyChanges() =>
        LearningSpace.Name != SpaceName ||
        LearningSpace.Description != Description ||
        LearningSpace.AssignedTopic != Topic ||
        LearningSpace.RequiredPoints != RequiredPoints ||
        LearningSpace.SpaceTheme != SpaceTheme;
}