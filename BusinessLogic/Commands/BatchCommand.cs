namespace BusinessLogic.Commands;

public class BatchCommand : IBatchCommand
{
    public string Name => nameof(BatchCommand);
    internal IEnumerable<IUndoCommand> Commands { get; }
    
    public BatchCommand(IEnumerable<IUndoCommand> commands)
    {
        Commands = commands;
    }

    public void Execute()
    {
        foreach (var command in Commands)
        {
            command.Execute();
        }
    }

    public void Undo()
    {
        foreach (var command in Commands.Reverse())
        {
            command.Undo();
        }
    }
    
    public void Redo()
    {
        foreach (var command in Commands)
        {
            command.Redo();
        }
    }
}