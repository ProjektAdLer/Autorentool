using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class CreateLearningWorld : IUndoCommand
{
    private readonly AuthoringToolWorkspace _authoringToolWorkspace;
    private readonly Action<AuthoringToolWorkspace> _mappingAction;

    private IMemento? _memento;
    private readonly LearningWorld _learningWorld;

    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors,
        string language, string description, string goals, Action<AuthoringToolWorkspace> mappingAction)
    {
        _learningWorld = new LearningWorld(name, shortname, authors, language, description, goals);
        _authoringToolWorkspace = authoringToolWorkspace;
        _mappingAction = mappingAction;
    }
    
    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        _learningWorld = learningWorld;
        _authoringToolWorkspace = authoringToolWorkspace;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = _authoringToolWorkspace.GetMemento();

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

    public void Redo() => Execute();
}