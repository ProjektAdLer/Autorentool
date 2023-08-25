namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IErrorService
{
    void SetError(string errorTitle, string errorMessage);
    void SetError(Exception exception);
}