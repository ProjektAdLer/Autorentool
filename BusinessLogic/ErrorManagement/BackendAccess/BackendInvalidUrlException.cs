using System.Runtime.Serialization;

namespace BusinessLogic.ErrorManagement.BackendAccess;

/**
 * This exception is thrown when the entered URL is invalid
 */
public class BackendInvalidUrlException : Exception
{
    public BackendInvalidUrlException()
    {
    }

    protected BackendInvalidUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BackendInvalidUrlException(string? message) : base(message)
    {
    }

    public BackendInvalidUrlException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}