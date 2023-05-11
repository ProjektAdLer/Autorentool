using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace BusinessLogic.Commands.Element;

public class CreateUnplacedLearningElement : ICreateUnplacedLearningElement
{
    public string Name => nameof(CreateUnplacedLearningElement);
    internal LearningWorld LearningWorld { get; }
    internal LearningElement LearningElement { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;

    public CreateUnplacedLearningElement(LearningWorld learningWorld, string name,
        ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY,
        Action<LearningWorld> mappingAction)
    {
        LearningElement = new LearningElement(name,  learningContent, description, goals,
            difficulty, null, workload, points, positionX, positionY);
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.UnsavedChanges = true;
        LearningWorld.UnplacedLearningElements.Add(LearningElement);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningWorld.RestoreMemento(_memento);
        
        MappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}