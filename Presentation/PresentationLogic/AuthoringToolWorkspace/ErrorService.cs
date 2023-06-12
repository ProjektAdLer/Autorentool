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
}