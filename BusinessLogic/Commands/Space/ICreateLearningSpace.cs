using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public interface ICreateLearningSpace : IUndoCommand
{
    ILearningSpace NewSpace { get; }
}