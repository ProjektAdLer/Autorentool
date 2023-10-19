namespace BusinessLogic.Commands.AdvancedSpaceGenerator.AdvancedSpace;

public interface IEditAdvancedLearningSpace :IUndoCommand
{
    bool AnyChanges();
}