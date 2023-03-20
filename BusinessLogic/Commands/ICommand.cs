namespace BusinessLogic.Commands;

/// <summary>
/// Represents a unit of work which can be executed.
/// </summary>
/// <remarks>Implementations should specify circumstances around <see cref="Execute"/> as well as
/// which Exceptions the method throws.</remarks>
public interface ICommand
{
    /// <summary>
    /// Returns the name of the command.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Executes the command.
    /// </summary>
    public void Execute();
}