using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class CreateLearningSpace : IUndoCommand
{
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public CreateLearningSpace(LearningWorld learningWorld, string name, string shortname, string authors,
        string description, string goals, int requiredPoints, double positionX, double positionY, Action<LearningWorld> mappingAction)
    {
        LearningSpace = new LearningSpace(name, shortname, authors, description, goals, requiredPoints, null, positionX, positionY);
        LearningWorld = learningWorld;
        _mappingAction = mappingAction;
    }

    public CreateLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace, Action<LearningWorld> mappingAction)
    {
        LearningSpace = learningSpace;
        LearningWorld = learningWorld;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.LearningSpaces.Add(LearningSpace);
        LearningWorld.SelectedLearningObject = LearningSpace;
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningWorld.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}