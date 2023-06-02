namespace Shared.Command;

public class CommandUndoRedoOrExecuteArgs
{
    public CommandUndoRedoOrExecuteArgs(string commandName, CommandExecutionState executionState)
    {
        CommandName = commandName;
        ExecutionState = executionState;
    }

    public string CommandName { get; }
    public CommandExecutionState ExecutionState { get; }
}