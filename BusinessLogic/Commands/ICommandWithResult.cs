namespace BusinessLogic.Commands;

public interface ICommandWithResult
{
    public bool HasResult { get; }
}

public interface ICommandWithResult<out T> : ICommandWithResult
{
    public T? Result { get; }
}