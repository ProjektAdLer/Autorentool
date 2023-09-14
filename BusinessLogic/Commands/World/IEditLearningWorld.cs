namespace BusinessLogic.Commands.World;

public interface IEditLearningWorld : IUndoCommand
{
    bool AnyChanges();
}