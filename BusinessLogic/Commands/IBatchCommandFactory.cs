namespace BusinessLogic.Commands;

public interface IBatchCommandFactory
{
    public IBatchCommand GetBatchCommand(IEnumerable<IUndoCommand> commands);
}