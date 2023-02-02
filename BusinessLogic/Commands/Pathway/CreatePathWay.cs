using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class CreatePathWay : IUndoCommand, ICommandWithError
{
    public bool HasError { get; private set; }
    internal Entities.World World { get; }
    internal Entities.Pathway Pathway { get; }
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _memento;

    public CreatePathWay(Entities.World world, IObjectInPathWay sourceObject, IObjectInPathWay targetObject,
        Action<Entities.World> mappingAction)
    {
        Pathway = new Entities.Pathway(sourceObject, targetObject);
        World = world;
        _mappingAction = mappingAction;
    }

    
    public void Execute()
    {
        if( World.Pathways.
                Any(x => x.SourceObject == Pathway.SourceObject && x.TargetObject == Pathway.TargetObject) 
            || Pathway.SourceObject.Id == Pathway.TargetObject.Id || IsCircular(Pathway)
            || (Pathway.TargetObject is Entities.Space && World.Pathways.Any(x => x.TargetObject.Id == Pathway.TargetObject.Id)))
        {
            HasError = true;
            return;
        }
            
        _memento = World.GetMemento();
        
        World.Pathways.Add(Pathway); 
        
        _mappingAction.Invoke(World);
    }
    
    private bool IsCircular(Entities.Pathway pathway)
    {
        var isCircular = false;
        var sourceObject = World.ObjectsInPathWays.First(x => x.Id == pathway.SourceObject.Id);
        var targetObject = World.ObjectsInPathWays.First(x => x.Id == pathway.TargetObject.Id);

        if(!targetObject.OutBoundObjects.Any())
        {
            return false;
        }
        var outBoundObject = targetObject.OutBoundObjects;
        var outBoundObjectSum = new List<IObjectInPathWay>();
        
        while (isCircular == false)
        {
            foreach (var space in outBoundObject)
            {
                if(space.Id == sourceObject.Id)
                {
                    isCircular = true;
                }
                outBoundObjectSum.AddRange(space.OutBoundObjects);
            }
            if (!outBoundObject.Any())
            {
                break;
            }
            outBoundObject  = outBoundObjectSum;
            outBoundObjectSum = new List<IObjectInPathWay>();
        }
        return isCircular;
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