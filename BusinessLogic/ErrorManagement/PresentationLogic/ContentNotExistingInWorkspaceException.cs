namespace BusinessLogic.ErrorManagement.PresentationLogic;

public class ContentNotExistingInWorkspaceException : Exception
{
    public ContentNotExistingInWorkspaceException()
    {
        
    }
    
    public ContentNotExistingInWorkspaceException(string? message) : base(message)
    {
        
    }
}