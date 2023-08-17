namespace BusinessLogic.ErrorManagement.BackendAccess;

/// <summary>
/// This Exception is thrown when our saved token is invalid, indicating that we must log in again.
/// </summary>
public class BackendInvalidTokenException : Exception
{
    public BackendInvalidTokenException() : base()
    {
    }

    public BackendInvalidTokenException(string message) : base(message)
    {
    }

    public BackendInvalidTokenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}