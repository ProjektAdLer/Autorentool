using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class LoadLearningWorld : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    private readonly AuthoringToolWorkspace _workspace;
    private LearningWorld? _learningWorld;
    private readonly string _filepath;
    private readonly Action<AuthoringToolWorkspace> _mappingAction;
    private IMemento? _memento;

    public LoadLearningWorld(AuthoringToolWorkspace workspace, string filepath, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        _workspace = workspace;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = _workspace.GetMemento();
        
        _learningWorld ??= _businessLogic.LoadLearningWorld(_filepath);
        
        _workspace.LearningWorlds.Add(_learningWorld);
        _workspace.SelectedLearningWorld = _learningWorld;
        
        _mappingAction.Invoke(_workspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        _workspace.RestoreMemento(_memento);
        
        _mappingAction.Invoke(_workspace);
    }

    public void Redo() => Execute();
}