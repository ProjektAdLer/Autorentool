using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public interface ILoadLearningWorld : IUndoCommand
{
    LearningWorld? LearningWorld { get; }
}