using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;
using Shared.Theme;

namespace BusinessLogic.Commands.World;

public class EditLearningWorld : IEditLearningWorld
{
    private IMemento? _memento;

    public EditLearningWorld(LearningWorld learningWorld, string name, string shortname,
        string authors, string language, string description, LearningOutcomeCollection learningOutcomeCollection,
        WorldTheme worldTheme, string evaluationLink, string enrolmentKey, string storyStart, string storyEnd,
        Action<LearningWorld> mappingAction,
        ILogger<EditLearningWorld> logger)
    {
        LearningWorld = learningWorld;
        WorldName = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        LearningOutcomeCollection = learningOutcomeCollection;
        WorldTheme = worldTheme;
        EvaluationLink = evaluationLink;
        EnrolmentKey = enrolmentKey;
        StoryStart = storyStart;
        StoryEnd = storyEnd;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal string WorldName { get; }
    internal string Shortname { get; }
    internal string Authors { get; }
    internal string Language { get; }
    internal string Description { get; }
    internal LearningOutcomeCollection LearningOutcomeCollection { get; }
    internal WorldTheme WorldTheme { get; }
    internal string EvaluationLink { get; }
    internal string EnrolmentKey { get; }
    internal string StoryStart { get; }
    internal string StoryEnd { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<EditLearningWorld> Logger { get; }
    public string Name => nameof(EditLearningWorld);

    public void Execute()
    {
        _memento ??= LearningWorld.GetMemento();

        Logger.LogTrace(
            "Editing LearningWorld {OldName} ({Id}). Previous Name: {Name}, Shortname: {Shortname}, Authors: {Authors}, Language: {Language}, Description: {Description}, LearningOutcomeCollection: {LearningOutcomeCollection}, Theme: {WorldTheme}, EvaluationLink: {EvaluationLink}, EnrolmentKey: {EnrolmentKey}, StoryStart: {StoryStart}, StoryEnd: {StoryEnd}",
            LearningWorld.Name, LearningWorld.Id, WorldName, Shortname, Authors, Language, Description,
            LearningOutcomeCollection, WorldTheme, EvaluationLink, EnrolmentKey, StoryStart, StoryEnd);

        if (AnyChanges()) LearningWorld.UnsavedChanges = true;
        LearningWorld.Name = WorldName;
        LearningWorld.Shortname = Shortname;
        LearningWorld.Authors = Authors;
        LearningWorld.Language = Language;
        LearningWorld.Description = Description;
        LearningWorld.LearningOutcomeCollection = LearningOutcomeCollection;
        LearningWorld.WorldTheme = WorldTheme;
        LearningWorld.EvaluationLink = EvaluationLink;
        LearningWorld.EnrolmentKey = EnrolmentKey;
        LearningWorld.StoryStart = StoryStart;
        LearningWorld.StoryEnd = StoryEnd;

        Logger.LogTrace(
            "Edited LearningWorld ({Id}). Updated Name: {Name}, Shortname: {Shortname}, Authors: {Authors}, Language: {Language}, Description: {Description}, LearningOutcomeCollection: {LearningOutcomeCollection}, Theme: {WorldTheme}, EvaluationLink: {EvaluationLink}, EnrolmentKey: {EnrolmentKey}, StoryStart: {StoryStart}, StoryEnd: {StoryEnd}",
            LearningWorld.Id, LearningWorld.Name, LearningWorld.Shortname, LearningWorld.Authors,
            LearningWorld.Language, LearningWorld.Description, LearningWorld.LearningOutcomeCollection,
            LearningWorld.WorldTheme, LearningWorld.EvaluationLink, LearningWorld.EnrolmentKey,
            LearningWorld.StoryStart, LearningWorld.StoryEnd);

        MappingAction.Invoke(LearningWorld);
    }


    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace("Undone edit of LearningWorld {Name} ({Id})", LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditLearningWorld");
        Execute();
    }

    public bool AnyChanges() =>
        LearningWorld.Name != WorldName ||
        LearningWorld.Shortname != Shortname ||
        LearningWorld.Authors != Authors ||
        LearningWorld.Language != Language ||
        LearningWorld.Description != Description ||
        LearningWorld.LearningOutcomeCollection != LearningOutcomeCollection ||
        LearningWorld.WorldTheme != WorldTheme ||
        LearningWorld.EvaluationLink != EvaluationLink ||
        LearningWorld.EnrolmentKey != EnrolmentKey ||
        LearningWorld.StoryStart != StoryStart ||
        LearningWorld.StoryEnd != StoryEnd;
}