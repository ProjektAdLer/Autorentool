namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public class ErrorService : IErrorService
{
    public string ErrorMessage { get; private set; } = "";

    public event Action? OnError;

    public void SetError(string errorMessage)
    {
        ErrorMessage = errorMessage;
        OnError?.Invoke();
    }
}