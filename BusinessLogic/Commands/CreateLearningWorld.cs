using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class CreateLearningWorld : IUndoCommand
{
    private readonly AuthoringToolWorkspace _authoringToolWorkspace;
    private readonly string _name;
    private readonly string _shortname;
    private readonly string _authors;
    private readonly string _language;
    private readonly string _description;
    private readonly string _goals;
    private readonly Action<AuthoringToolWorkspace> _mappingAction;

    private IMemento? _memento;
    private LearningWorld? _learningWorld;

    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors,
        string language, string description, string goals, Action<AuthoringToolWorkspace> mappingAction)
    {
        _authoringToolWorkspace = authoringToolWorkspace;
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
        _memento = _authoringToolWorkspace.GetMemento();

        _learningWorld = new LearningWorld(_name, _shortname, _authors, _language, _description, _goals);

        _authoringToolWorkspace.LearningWorlds.Add(_learningWorld);

        _authoringToolWorkspace.SelectedLearningWorld = _learningWorld;

        _mappingAction.Invoke(_authoringToolWorkspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        _authoringToolWorkspace.RestoreMemento(_memento);

        _mappingAction.Invoke(_authoringToolWorkspace);
    }

    public void Redo()
    {
        if (_learningWorld == null)
        {
            throw new InvalidOperationException("_learningWorld is null");
        }

        _authoringToolWorkspace.LearningWorlds.Add(_learningWorld);

        _authoringToolWorkspace.SelectedLearningWorld = _learningWorld;

        _mappingAction.Invoke(_authoringToolWorkspace);
    }
}