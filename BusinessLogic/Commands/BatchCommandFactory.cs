namespace BusinessLogic.Commands;

public class BatchCommandFactory : IBatchCommandFactory
{
    public IBatchCommand GetBatchCommand(IEnumerable<IUndoCommand> commands, string name) => new BatchCommand(commands, name);
}