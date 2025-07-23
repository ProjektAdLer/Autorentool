using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
using Shared.Theme;

namespace BusinessLogic.Commands.World;

public class CreateLearningWorld : ICreateLearningWorld
{
    private IMemento? _memento;

    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors, string language, string description, LearningOutcomeCollection learningOutcomeCollection,
        WorldTheme worldTheme, string evaluationLink,
        string evaluationLinkName, string evaluationLinkText, string enrolmentKey, string storyStart, string storyEnd,
        Action<AuthoringToolWorkspace> mappingAction,
        ILogger<CreateLearningWorld> logger)
    {
        LearningWorld = new LearningWorld(name, shortname, authors, language, description, learningOutcomeCollection,
            worldTheme, evaluationLink,evaluationLinkName, evaluationLinkText,
            enrolmentKey, storyStart, storyEnd);
        AuthoringToolWorkspace = authoringToolWorkspace;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction, ILogger<CreateLearningWorld> logger)
    {
        LearningWorld = learningWorld;
        AuthoringToolWorkspace = authoringToolWorkspace;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }
    internal LearningWorld LearningWorld { get; }
    private ILogger<CreateLearningWorld> Logger { get; }
    public string Name => nameof(CreateLearningWorld);

    public void Execute()
    {
        _memento = AuthoringToolWorkspace.GetMemento();

        if (AuthoringToolWorkspace.LearningWorlds.Any(lw => lw.Name == LearningWorld.Name))
        {
            LearningWorld.Name = StringHelper.GetUniqueName(AuthoringToolWorkspace.LearningWorlds.Select(lw => lw.Name),
                LearningWorld.Name);
        }

        AuthoringToolWorkspace.LearningWorlds.Add(LearningWorld);

        Logger.LogTrace(
            "Created LearningWorld ({Id}). Name: {Name}, Shortname: {Shortname}, Authors: {Authors}, Language: {Language}, Description: {Description}, LearningOutcomeCollection: {LearningOutcomeCollection}, Theme: {WorldTheme}, EvaluationLink: {EvaluationLink}, EvaluationLinkName: {EvaluationLinkName}, EvaluationLinkText: {EvaluationLinkText}, EnrolmentKey: {EnrolmentKey}, StoryStart: {StoryStart}, StoryEnd: {StoryEnd}",
            LearningWorld.Id, LearningWorld.Name, LearningWorld.Shortname, LearningWorld.Authors,
            LearningWorld.Language, LearningWorld.Description, LearningWorld.LearningOutcomeCollection,
            LearningWorld.WorldTheme, LearningWorld.EvaluationLink,LearningWorld.EvaluationLinkName, LearningWorld.EvaluationLinkText, LearningWorld.EnrolmentKey,
            LearningWorld.StoryStart, LearningWorld.StoryEnd);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(_memento);

        Logger.LogTrace("Undone creation of LearningWorld {Name} ({Id})", LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateLearningWorld");
        Execute();
    }
}