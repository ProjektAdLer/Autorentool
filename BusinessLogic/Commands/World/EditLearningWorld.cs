using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class EditLearningWorld : IEditLearningWorld
{
    public string Name => nameof(EditLearningWorld);
    internal LearningWorld LearningWorld { get; }
    internal string WorldName { get; }
    internal string Shortname { get; }
    internal string Authors { get; }
    internal string Language { get; }
    internal string Description { get; }
    internal string Goals { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;

    public EditLearningWorld(LearningWorld learningWorld, string name, string shortname,
        string authors, string language, string description, string goals, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        WorldName = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento ??= LearningWorld.GetMemento();

        if (AnyChanges()) LearningWorld.UnsavedChanges = true;
        LearningWorld.Name = WorldName;
        LearningWorld.Shortname = Shortname;
        LearningWorld.Authors = Authors;
        LearningWorld.Language = Language;
        LearningWorld.Description = Description;
        LearningWorld.Goals = Goals;

        MappingAction.Invoke(LearningWorld);
    }

    private bool AnyChanges() =>
        LearningWorld.Name != WorldName ||
        LearningWorld.Shortname != Shortname ||
        LearningWorld.Authors != Authors ||
        LearningWorld.Language != Language ||
        LearningWorld.Description != Description ||
        LearningWorld.Goals != Goals;
    

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}