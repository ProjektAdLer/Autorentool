namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IErrorService
{
    string ErrorMessage { get; }
    void SetError(string errorMessage);
    event Action? OnError;
}