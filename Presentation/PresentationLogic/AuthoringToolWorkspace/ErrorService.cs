using System.Text;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public class ErrorService : IErrorService
{
    public ErrorService(IDialogService dialogService)
    {
        DialogService = dialogService;
    }

    private IDialogService DialogService { get; }

    public void SetError(string errorTitle, string errorMessage)
    {
        DialogService.ShowMessageBox(errorTitle, (MarkupString)errorMessage);
    }

    public void SetError(Exception exception)
    {
        DialogService.ShowMessageBox("An error has occurred", (MarkupString)GetFullExceptionMessage(exception));
    }

    /// <summary>
    /// Gets exception message and all inner exception messages without stacktrace
    /// </summary>
    private static string GetFullExceptionMessage(Exception ex)
    {
        var exception = ex;
        var sb = new StringBuilder();
        while (true)
        {
            sb.AppendLine(exception.Message);
            sb.Append("<br/>");
            sb.Append("<br/>");
            exception = exception.InnerException;
            if (exception == null) break;
            sb.AppendLine("Inner exception:");
            sb.Append("<br/>");
        }

        return sb.ToString();
    }
}