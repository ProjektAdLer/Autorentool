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
        if (title is null)
        {
            if (parameters is null)
                await DialogProvider.InvokeAsync(() => reference = service.Show<T>());
            else
            {
                title = "foo";
                await DialogProvider.InvokeAsync(() => reference = service.Show<T>(title, parameters));
            }
        }
        else
        {
            if (options is null)
                await DialogProvider.InvokeAsync(() => reference = service.Show<T>(title));
            else
            {
                if (parameters is null)
                    await DialogProvider.InvokeAsync(() => reference = service.Show<T>(title, options));
                else
                    await DialogProvider.InvokeAsync(() =>
                        reference = service.Show<T>(title, parameters, options));
            }
        }

        return reference!;
    }
}