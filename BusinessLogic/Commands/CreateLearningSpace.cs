using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class CreateLearningSpace : IUndoCommand
{
    private readonly LearningWorld _learningWorld;
    private readonly LearningSpace _learningSpace;
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public CreateLearningSpace(LearningWorld learningWorld, string name, string shortname, string authors,
        string description, string goals, Action<LearningWorld> mappingAction)
    {
        _learningSpace = new LearningSpace(name, shortname, authors, description, goals);
        _learningWorld = learningWorld;
        _mappingAction = mappingAction;
    }

    public CreateLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace, Action<LearningWorld> mappingAction)
    {
        _learningSpace = learningSpace;
        _learningWorld = learningWorld;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = _learningWorld.GetMemento();

        _learningWorld.LearningSpaces.Add(_learningSpace);
        
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