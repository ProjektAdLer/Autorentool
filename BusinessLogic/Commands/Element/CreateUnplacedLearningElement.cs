using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace BusinessLogic.Commands.Element;

public class CreateUnplacedLearningElement : IUndoCommand
{
    public string Name => nameof(CreateUnplacedLearningElement);
    internal LearningWorld LearningWorld { get; }
    internal LearningElement LearningElement { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;
    
    public CreateUnplacedLearningElement(LearningWorld learningWorld, string name, string shortName,
        LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY,
        Action<LearningWorld> mappingAction)
    {
        LearningElement = new LearningElement(name, shortName, learningContent, authors, description, goals,
            difficulty, null, workload, points, positionX, positionY);
        LearningWorld = learningWorld;
        _mappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        LearningWorld.UnplacedLearningElements.Add(LearningElement);
        
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