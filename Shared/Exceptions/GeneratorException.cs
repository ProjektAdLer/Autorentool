namespace Shared.Exceptions;

public class GeneratorException : Exception
{
    public GeneratorException()
    {
    }
    
    public GeneratorException(string message, Exception inner) : base(message, inner)
    {
    }
}