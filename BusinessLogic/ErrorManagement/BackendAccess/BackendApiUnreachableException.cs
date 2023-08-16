namespace BusinessLogic.ErrorManagement.BackendAccess;

public class BackendApiUnreachableException : Exception
{
    public BackendApiUnreachableException()
    {
    }

    public BackendApiUnreachableException(string message) : base(message)
    {
    }
}