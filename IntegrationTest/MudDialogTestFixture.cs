using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IntegrationTest;

public class MudDialogTestFixture<T> : MudBlazorTestFixture<T> where T : ComponentBase
{
    protected IRenderedComponent<MudDialogProvider> DialogProvider { get; set; } = null!;

    protected async Task<IDialogReference> OpenDialogAndGetDialogReferenceAsync(string? title = null,
        DialogOptions? options = null, DialogParameters? parameters = null)
    {
        DialogProvider = Context.RenderComponent<MudDialogProvider>();
        var service = (DialogService)Context.Services.GetService<IDialogService>()!;
        IDialogReference? reference = null;
        options ??= new DialogOptions();
        parameters ??= new DialogParameters();
        title ??= "title";
        await DialogProvider.InvokeAsync(() => reference = service.Show<T>(title, parameters, options));
        DialogProvider.Render();
        return reference!;
    }
}