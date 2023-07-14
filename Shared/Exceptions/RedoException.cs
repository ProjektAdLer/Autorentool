namespace Shared.Exceptions;

public class RedoException : Exception
{
    public RedoException()
    {
    }
    
    public RedoException(string message) : base(message)
    {
    }
    
    public RedoException(string message, Exception inner) : base(message, inner)
    {
    }
}