namespace AuthoringTool.BusinessLogic.Commands;

/// <summary>
/// Represents a unit of work which can be executed.
/// </summary>
/// <remarks>Implementations should specify circumstances around <see cref="Execute"/> as well as
/// which Exceptions the method throws.</remarks>
public interface ICommand
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    public void Execute();
}