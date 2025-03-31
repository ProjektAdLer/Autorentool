namespace BusinessLogic.Commands;

public class BatchCommand : IBatchCommand
{
    public string Name { get; }
    internal IEnumerable<IUndoCommand> Commands { get; }
    
    public BatchCommand(IEnumerable<IUndoCommand> commands, string name)
    {
        Commands = commands;
        Name = name;
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