using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class CreateLearningWorld : IUndoCommand
{
    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    private readonly Action<AuthoringToolWorkspace> _mappingAction;

    private IMemento? _memento;
    internal LearningWorld LearningWorld { get; }

    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors,
        string language, string description, string goals, Action<AuthoringToolWorkspace> mappingAction)
    {
        LearningWorld = new LearningWorld(name, shortname, authors, language, description, goals);
        AuthoringToolWorkspace = authoringToolWorkspace;
        _mappingAction = mappingAction;
    }
    
    public CreateLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        LearningWorld = learningWorld;
        AuthoringToolWorkspace = authoringToolWorkspace;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = AuthoringToolWorkspace.GetMemento();

        AuthoringToolWorkspace.LearningWorlds.Add(LearningWorld);

        AuthoringToolWorkspace.SelectedLearningWorld = LearningWorld;

        _mappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(_memento);

        _mappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo() => Execute();
}