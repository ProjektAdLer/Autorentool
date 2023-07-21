using BusinessLogic.Entities;
using Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class CreateLearningWorld : ICreateLearningWorld
{
    public string Name => nameof(CreateLearningWorld);
    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }

    private IMemento? _memento;
    internal LearningWorld LearningWorld { get; }
    private ILogger<WorldCommandFactory> Logger { get; }

    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors, string language, string description, string goals, Action<AuthoringToolWorkspace> mappingAction,
        ILogger<WorldCommandFactory> logger)
    {
        LearningWorld = new LearningWorld(name, shortname, authors, language, description, goals);
        AuthoringToolWorkspace = authoringToolWorkspace;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction, ILogger<WorldCommandFactory> logger)
    {
        LearningWorld = learningWorld;
        AuthoringToolWorkspace = authoringToolWorkspace;
        MappingAction = mappingAction;
        Logger = logger;
    }

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
            "Created LearningWorld {name} ({id}). Name: {name}, Shortname: {shortname}, Authors: {authors}, Language: {language}, Description: {description}, Goals: {goals}",
            LearningWorld.Name, LearningWorld.Id, LearningWorld.Name, LearningWorld.Shortname, LearningWorld.Authors,
            LearningWorld.Language, LearningWorld.Description, LearningWorld.Goals);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(_memento);

        Logger.LogTrace("Undone creation of LearningWorld {name} ({id})", LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateLearningWorld");
        Execute();
    }
}