namespace BusinessLogic.Commands;

public interface ICommandWithError
{
    public bool HasError { get; }
}
public interface ICommandWithError<out T> : ICommandWithError
{
    public T? Error { get; }
}