using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class EditLearningWorld : IUndoCommand
{
    private readonly LearningWorld _learningWorld;
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
        _learningWorld = learningWorld;
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
        _memento ??= _learningWorld.GetMemento();

        _learningWorld.Name = _name;
        _learningWorld.Shortname = _shortname;
        _learningWorld.Authors = _authors;
        _learningWorld.Language = _language;
        _learningWorld.Description = _description;
        _learningWorld.Goals = _goals;

        _mappingAction.Invoke(_learningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        _learningWorld.RestoreMemento(_memento);

        _mappingAction.Invoke(_learningWorld);
    }

    public void Redo() => Execute();
}