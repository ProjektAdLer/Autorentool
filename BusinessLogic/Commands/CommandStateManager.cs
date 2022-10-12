using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.Commands;

public sealed class CommandStateManager : ICommandStateManager
{
    private readonly Stack<IUndoCommand> _undo;
    private readonly Stack<IUndoCommand> _redo;

    public CommandStateManager()
    {
        _undo = new Stack<IUndoCommand>();
        _redo = new Stack<IUndoCommand>();
    }

    /// <inheritdoc cref="ICommandStateManager.CanUndo"/>
    public bool CanUndo => _undo.Any();
    
    /// <inheritdoc cref="ICommandStateManager.CanRedo"/>
    public bool CanRedo => _redo.Any();


    public delegate void RemovedCommandsFromStacksHandler(object sender, RemoveCommandsFromStacksEventArgs e);

    public event RemovedCommandsFromStacksHandler? RemovedCommandsFromStacks;
    
    /// <inheritdoc cref="ICommandStateManager.Execute"/>
    public void Execute(ICommand command)
    {
        command.Execute();
        switch (command)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global - suppressed for the time being while we finish the command implementations n.stich
            case ICommandWithError { HasError: true }:
                return;
            case IUndoCommand undoCommand:
                _undo.Push(undoCommand);
                break;
        }

        if (_redo.Any())
        {
            _redo.Clear();
            RemovedCommandsFromStacks?.Invoke(this, new RemoveCommandsFromStacksEventArgs(GetObjects()));
        }
        OnPropertyChanged(nameof(CanUndo));
    }

    /// <inheritdoc cref="ICommandStateManager.Undo"/>
    public void Undo()
    {
        if (!CanUndo) throw new InvalidOperationException("no command to undo");
        var command = PopUndo();
        command.Undo();
        PushRedo(command);
    }

    /// <inheritdoc cref="ICommandStateManager.Redo"/>
    public void Redo()
    {
        if (!CanRedo) throw new InvalidOperationException("no command to redo");
        var command = PopRedo();
        command.Redo();
        PushUndo(command);
    }
    
    private IEnumerable<object> GetObjects()
    {
        var objects = new HashSet<object>();
        foreach (var myObject in _undo.Select(GetObjectFromCommand).Where(obj => obj != null))
        {
            if (myObject != null) objects.Add(myObject);
        }
        // do the same for _redo, if it is possible that RemovedCommandsFromStacks is invoked when _redo is not empty
        return objects;
    }
    
    private static object? GetObjectFromCommand(IUndoCommand command)
    {
        return command switch
        {
            CreateLearningElement c => c.LearningElement,
            CreateLearningSpace c => c.LearningSpace,
            CreateLearningWorld c => c.LearningWorld,
            DeleteLearningElement c => c.LearningElement,
            DeleteLearningSpace c => c.LearningSpace,
            DeleteLearningWorld c => c.LearningWorld,
            LoadLearningElement c => c.LearningElement,
            LoadLearningSpace c => c.LearningSpace,
            LoadLearningWorld c => c.LearningWorld,
            _ => null
        };
    }

    private IUndoCommand PopUndo()
    {
        var canUndoBefore = CanUndo;
        var command = _undo.Pop();
        if (canUndoBefore != CanUndo)
        {
            OnPropertyChanged(nameof(CanUndo));
        }

        return command;
    }
    
    private IUndoCommand PopRedo()
    {
        var canRedoBefore = CanRedo;
        var command = _redo.Pop();
        if (canRedoBefore != CanRedo)
        {
            OnPropertyChanged(nameof(CanRedo));
        }

        return command;
    }
    
    private void PushUndo(IUndoCommand command)
    {
        var canUndoBefore = CanUndo;
        _undo.Push(command);
        if (canUndoBefore != CanUndo)
        {
            OnPropertyChanged(nameof(CanUndo));
        }
    }

    private void PushRedo(IUndoCommand command)
    {
        var canRedoBefore = CanRedo;
        _redo.Push(command);
        if (canRedoBefore != CanRedo)
        {
            OnPropertyChanged(nameof(CanRedo));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RemoveCommandsFromStacksEventArgs
{
    public RemoveCommandsFromStacksEventArgs(IEnumerable<object> objectsInStacks)
    {
        ObjectsInStacks = objectsInStacks;
    }

    public IEnumerable<object> ObjectsInStacks { get; }
}