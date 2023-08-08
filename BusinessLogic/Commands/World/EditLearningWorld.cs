using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class EditLearningWorld : IEditLearningWorld
{
    private IMemento? _memento;

    public EditLearningWorld(LearningWorld learningWorld, string name, string shortname,
        string authors, string language, string description, string goals, Action<LearningWorld> mappingAction,
        ILogger<EditLearningWorld> logger)
    {
        LearningWorld = learningWorld;
        WorldName = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal string WorldName { get; }
    internal string Shortname { get; }
    internal string Authors { get; }
    internal string Language { get; }
    internal string Description { get; }
    internal string Goals { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<EditLearningWorld> Logger { get; }
    public string Name => nameof(EditLearningWorld);

    public void Execute()
    {
        _memento ??= LearningWorld.GetMemento();

        Logger.LogTrace(
            "Editing LearningWorld {OldName} ({Id}). Previous Name: {Name}, Shortname: {Shortname}, Authors: {Authors}, Language: {Language}, Description: {Description}, Goals: {Goals}",
            LearningWorld.Name, LearningWorld.Id, WorldName, Shortname, Authors, Language, Description, Goals);

        if (AnyChanges()) LearningWorld.UnsavedChanges = true;
        LearningWorld.Name = WorldName;
        LearningWorld.Shortname = Shortname;
        LearningWorld.Authors = Authors;
        LearningWorld.Language = Language;
        LearningWorld.Description = Description;
        LearningWorld.Goals = Goals;

        Logger.LogTrace(
            "Edited LearningWorld ({Id}). Updated Name: {Name}, Shortname: {Shortname}, Authors: {Authors}, Language: {Language}, Description: {Description}, Goals: {Goals}",
            LearningWorld.Id, LearningWorld.Name, LearningWorld.Shortname, LearningWorld.Authors,
            LearningWorld.Language, LearningWorld.Description, LearningWorld.Goals);

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

    private bool AnyChanges() =>
        LearningWorld.Name != WorldName ||
        LearningWorld.Shortname != Shortname ||
        LearningWorld.Authors != Authors ||
        LearningWorld.Language != Language ||
        LearningWorld.Description != Description ||
        LearningWorld.Goals != Goals;
}