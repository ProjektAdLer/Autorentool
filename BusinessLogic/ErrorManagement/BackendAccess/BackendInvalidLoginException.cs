using System.Runtime.Serialization;

namespace BusinessLogic.ErrorManagement.BackendAccess;

/**
 * This exception is thrown when the user tries to login with invalid credentials
 */
public class BackendInvalidLoginException : Exception
{
    public BackendInvalidLoginException()
    {
    }

    protected BackendInvalidLoginException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BackendInvalidLoginException(string? message) : base(message)
    {
    }

    public BackendInvalidLoginException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}