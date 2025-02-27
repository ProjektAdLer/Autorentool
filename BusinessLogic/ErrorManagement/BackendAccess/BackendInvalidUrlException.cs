namespace BusinessLogic.ErrorManagement.BackendAccess;

/// <summary>
/// This exception is thrown when the entered URL is invalid
/// </summary>
public class BackendInvalidUrlException : Exception
{
    public BackendInvalidUrlException()
    {
    }

    public BackendInvalidUrlException(string? message) : base(message)
    {
    }

    public BackendInvalidUrlException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}