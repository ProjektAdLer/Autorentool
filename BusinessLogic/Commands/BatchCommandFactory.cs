namespace BusinessLogic.Commands;

public class BatchCommandFactory : IBatchCommandFactory
{
    public IBatchCommand GetBatchCommand(IEnumerable<IUndoCommand> commands) => new BatchCommand(commands);
}