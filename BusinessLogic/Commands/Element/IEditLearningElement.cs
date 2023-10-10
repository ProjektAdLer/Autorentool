namespace BusinessLogic.Commands.Element;

public interface IEditLearningElement : IUndoCommand
{
    bool AnyChanges();
}