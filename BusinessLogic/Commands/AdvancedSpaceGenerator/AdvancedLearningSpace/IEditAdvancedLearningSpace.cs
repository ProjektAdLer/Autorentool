namespace BusinessLogic.Commands.Space.AdvancedLearningSpace;

public interface IEditAdvancedLearningSpace :IUndoCommand
{
    bool AnyChanges();
}