using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class LoadLearningWorld : IUndoCommand
{
    public string Name => nameof(LoadLearningWorld);
    private readonly IBusinessLogic _businessLogic;
    
    private readonly AuthoringToolWorkspace _workspace;
    internal LearningWorld? LearningWorld;
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
    
    public LoadLearningWorld(AuthoringToolWorkspace workspace, Stream stream, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        _filepath = "";
        _workspace = workspace;
        _businessLogic = businessLogic;
        LearningWorld = _businessLogic.LoadLearningWorld(stream);
        _mappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = _workspace.GetMemento();
        
        LearningWorld ??= _businessLogic.LoadLearningWorld(_filepath);
        
        _workspace.LearningWorlds.Add(LearningWorld);
        _workspace.SelectedLearningWorld = LearningWorld;
        LearningWorld.SavePath = _filepath;
        
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