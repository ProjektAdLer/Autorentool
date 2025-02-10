namespace BusinessLogic.ErrorManagement.BackendAccess;

public class BackendMoodleApiUnreachableException : Exception
{
    public BackendMoodleApiUnreachableException()
    {
    }

    public BackendMoodleApiUnreachableException(string message) : base(message)
    {
    }
    
    public BackendMoodleApiUnreachableException(string message, Exception innerException) : base(message, innerException)
    {
    }
}