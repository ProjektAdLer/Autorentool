using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class EditLearningWorld : IUndoCommand
{
    internal LearningWorld LearningWorld { get; }
    private readonly string _name;
    private readonly string _shortname;
    private readonly string _authors;
    private readonly string _language;
    private readonly string _description;
    private readonly string _goals;
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public EditLearningWorld(LearningWorld learningWorld, string name, string shortname,
        string authors, string language, string description, string goals, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        _name = name;
        _shortname = shortname;
        _authors = authors;
        _language = language;
        _description = description;
        _goals = goals;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento ??= LearningWorld.GetMemento();

        LearningWorld.Name = _name;
        LearningWorld.Shortname = _shortname;
        LearningWorld.Authors = _authors;
        LearningWorld.Language = _language;
        LearningWorld.Description = _description;
        LearningWorld.Goals = _goals;

        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}