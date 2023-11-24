namespace BusinessLogic.ErrorManagement.BackendAccess;

public class BackendWorldDeletionException : Exception
{
    public BackendWorldDeletionException()
    {
    }

    public BackendWorldDeletionException(string message) : base(message)
    {
    }

    public BackendWorldDeletionException(string exceptionMessage, Exception exception)
    {
    }
}