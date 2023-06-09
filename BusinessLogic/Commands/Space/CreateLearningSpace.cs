using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Space;

public class CreateLearningSpace : ICreateLearningSpace
{
    public string Name => nameof(CreateLearningSpace);
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;

    public CreateLearningSpace(LearningWorld learningWorld, string name, string description, string goals, 
        int requiredPoints, Theme theme, bool advancedMode, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction)
    {
        LearningSpace = new LearningSpace(name, description, goals, requiredPoints, theme, advancedMode, null, positionX: positionX, positionY: positionY, assignedTopic: topic);
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
    }

    public CreateLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace, Action<LearningWorld> mappingAction)
    {
        LearningSpace = learningSpace;
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.LearningSpaces.Add(LearningSpace);

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