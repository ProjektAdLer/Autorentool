using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class EditLearningSpace : IUndoCommand
{
    private readonly LearningWorld _learningWorld;
    private readonly LearningSpace _learningSpace;
    private readonly string _name;
    private readonly string _shortname;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public EditLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace, string name, string shortname,
        string authors, string description, string goals, Action<LearningWorld> mappingAction)
    {
        _learningWorld = learningWorld;
        _learningSpace = learningSpace;
        _name = name;
        _shortname = shortname;
        _authors = authors;
        _description = description;
        _goals = goals;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = _learningWorld.GetMemento();

        var space = _learningWorld.LearningSpaces.FirstOrDefault(x => x.Name == _learningSpace.Name);

        if (space == null)
        {
            throw new ApplicationException("LearningSpace is null");
        }

        _learningWorld.LearningSpaces.Remove(space);

        var editedSpace = new LearningSpace(_name, _shortname, _authors, _description, _goals, space.LearningElements,
            space.PositionX, space.PositionY);
        
        _learningWorld.LearningSpaces.Add(editedSpace);
        
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