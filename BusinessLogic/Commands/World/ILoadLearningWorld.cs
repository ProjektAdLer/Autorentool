using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public interface ILoadLearningWorld : IUndoCommand
{
    ILearningWorld? LearningWorld { get; }
}