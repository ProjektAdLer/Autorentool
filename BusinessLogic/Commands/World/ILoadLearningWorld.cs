using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public interface ILoadLearningWorld : ICommand
{
    ILearningWorld? LearningWorld { get; }
}