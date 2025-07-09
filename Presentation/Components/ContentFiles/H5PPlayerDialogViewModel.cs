using H5pPlayer.Api;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.JSInterop;
using MudBlazor;

namespace Presentation.Components.ContentFiles;

public class H5PPlayerDialogViewModel : IH5PPlayerDialogViewModel
{


    public H5PPlayerDialogViewModel(
        ILogger<H5PPlayerDialogViewModel> logger,
        IJSRuntime jSRuntime,
        IDialogService dialogService)
    {
        Logger = logger;
        JSRuntime = jSRuntime;
        DialogService = dialogService;
    }

    public async Task<H5pPlayerResultTO?> OpenH5pPlayerDialogAsync(
        string h5pZipSourcePath,
        string unzippedH5psPath,
        H5pDisplayMode displayMode)
    {
        var tcs = new TaskCompletionSource<H5pPlayerResultTO?>();

        var parameters = new DialogParameters
        {
            { "H5pZipSourcePath", h5pZipSourcePath },
            { "UnzippedH5psPath", unzippedH5psPath },
            { "DisplayMode", displayMode },
            { "Logger", Logger },
            { "OnPlayerFinished", new Action<H5pPlayerResultTO>(result =>
                {
                    Logger.LogInformation("Ergebnis erhalten vom Player: {@result}", result);
                    tcs.TrySetResult(result);
                })
            }
        };

        var options = new DialogOptions
        {
            BackdropClick = false,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = false,
        };

        var dialog = await DialogService.ShowAsync<PlayerH5p>("H5P-Player", parameters, options);
        var dialogResult = await dialog.Result;

        try
        {
            await JSRuntime.InvokeVoidAsync("terminateH5pStandalone");
        }
        catch (JSException ex)
        {
            Logger.LogError("JSException: Could not call 'terminateH5pStandalone': {Message}", ex.Message);
        }

        if (dialogResult is { Canceled: true })
        {
            Logger.LogInformation("Dialog wurde abgebrochen.");
            return null;
        }

        return await tcs.Task;
    }


    
    
    public ILogger<H5PPlayerDialogViewModel> Logger { get;  }
    public IJSRuntime JSRuntime { get; }
    public IDialogService DialogService { get; }
    
}