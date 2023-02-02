using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class LoadWorld : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    private readonly AuthoringToolWorkspace _workspace;
    internal Entities.World? World;
    private readonly string _filepath;
    private readonly Action<AuthoringToolWorkspace> _mappingAction;
    private IMemento? _memento;

    public LoadWorld(AuthoringToolWorkspace workspace, string filepath, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        _workspace = workspace;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    
    public LoadWorld(AuthoringToolWorkspace workspace, Stream stream, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        _filepath = "";
        _workspace = workspace;
        _businessLogic = businessLogic;
        World = _businessLogic.LoadWorld(stream);
        _mappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = _workspace.GetMemento();
        
        World ??= _businessLogic.LoadWorld(_filepath);
        
        _workspace.Worlds.Add(World);
        _workspace.SelectedWorld = World;
        
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