namespace BusinessLogic.ErrorManagement.BackendAccess;

public class BackendWorldDeletionException : Exception
{
    public BackendWorldDeletionException(string message) : base(message)
    {
    }
}