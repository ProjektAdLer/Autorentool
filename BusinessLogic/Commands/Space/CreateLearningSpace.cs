using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Space;

public class CreateLearningSpace : ICreateLearningSpace
{
    public string Name => nameof(CreateLearningSpace);
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public CreateLearningSpace(LearningWorld learningWorld, string name, string description, string goals, 
        int requiredPoints, Theme theme, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction)
    {
        LearningSpace = new LearningSpace(name, description, goals, requiredPoints, theme, null, positionX: positionX, positionY: positionY, assignedTopic: topic);
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