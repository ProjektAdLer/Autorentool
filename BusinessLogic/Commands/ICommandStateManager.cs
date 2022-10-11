using System.ComponentModel;

namespace BusinessLogic.Commands;

/// <summary>
/// Executes <see cref="ICommand"/> and handles undo and redo of <see cref="IUndoCommand"/>
/// </summary>
public interface ICommandStateManager : INotifyPropertyChanged
{
    /// <summary>
    /// Whether or not <see cref="Undo"/> can be run.
    /// </summary>
    public bool CanUndo { get; }
    /// <summary>
    /// Whether or not <see cref="Redo"/> can be run.
    /// </summary>
    public bool CanRedo { get; }
    /// <summary>
    /// Executes the passed command and saves it to the undo stack.
    /// </summary>
    /// <param name="command">The command that shall be saved to the undo stack.</param>
    /// <remarks>Executing a new command will clear the Redo stack.</remarks>
    public void Execute(ICommand command);
    /// <summary>
    /// Undoes the last command and saves it to the redo stack, if any commands are available.
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="CanUndo"/> is false.</exception>
    public void Undo();
    /// <summary>
    /// Redoes the last command and saves it to the undo stack, if any commands are available.
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="CanRedo"/> is false.</exception>
    public void Redo();
    event CommandStateManager.RemovedCommandsFromStacksHandler RemovedCommandsFromStacks;
}