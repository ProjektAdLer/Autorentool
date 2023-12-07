namespace Shared.Exceptions;

public class BackendException : Exception
{
    public BackendException(string message, Exception inner) : base(message, inner)
    {
    }
}