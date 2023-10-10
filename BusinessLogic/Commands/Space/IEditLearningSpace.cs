namespace BusinessLogic.Commands.Space;

public interface IEditLearningSpace : IUndoCommand
{
    bool AnyChanges();
}