using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class LoadSpace : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    internal Entities.World World { get; }
    internal Entities.Space? Space;
    private readonly string _filepath;
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _memento;

    public LoadSpace(Entities.World world, string filepath, IBusinessLogic businessLogic,
        Action<Entities.World> mappingAction)
    {
        World = world;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    
    public LoadSpace(Entities.World world, Stream stream, IBusinessLogic businessLogic,
        Action<Entities.World> mappingAction)
    {
        World = world;
        _filepath = "";
        _businessLogic = businessLogic;
        Space = _businessLogic.LoadSpace(stream);
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = World.GetMemento();
        
        Space ??= _businessLogic.LoadSpace(_filepath);
        World.Spaces.Add(Space);
        World.SelectedObject = Space;

        _mappingAction.Invoke(World);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        World.RestoreMemento(_memento);
        
        _mappingAction.Invoke(World);
    }

    public void Redo() => Execute();
}