namespace BusinessLogic.Commands;

/// <summary>
/// Represents a unit of work which can be executed, undone and redone.
/// </summary>
/// <remarks>Implementations should specify circumstances around <see cref="ICommand.Execute"/>, <see cref="Undo"/> and <see cref="Redo"/> as well as
/// which Exceptions these methods throw.</remarks>
public interface IUndoCommand : ICommand
{
    /// <summary>
    /// Undoes the command.
    /// </summary>
    public void Undo();
    /// <summary>
    /// Redoes the command.
    /// </summary>
    public void Redo();
}