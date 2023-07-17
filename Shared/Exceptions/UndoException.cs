namespace Shared.Exceptions;

public class UndoException : Exception
{
    public UndoException()
    {
    }
    
    public UndoException(string message, Exception inner) : base(message, inner)
    {
    }
}