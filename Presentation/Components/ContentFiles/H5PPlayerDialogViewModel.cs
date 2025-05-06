using H5pPlayer.Api;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using H5pPlayer.DataAccess.FileSystem;
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

    public async void OpenH5pPlayerDialog(
        string h5pZipSourcePath, 
        string unzippedH5psPath, 
        H5pDisplayMode displayMode)
    {
        
        H5pPlayerResultTO? finalResult = null;

        var parameters = new DialogParameters
        {
            { "H5pZipSourcePath", h5pZipSourcePath },
            { "UnzippedH5psPath", unzippedH5psPath },
            { "DisplayMode", displayMode },
            { "OnPlayerFinished", new Action<H5pPlayerResultTO>(result =>
                {
                    finalResult = result;
                    Logger.LogInformation("Ergebnis erhalten vom Player: {@result}", result);
                })
            }
        };
        var options = new DialogOptions
        {
            BackdropClick = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
        };
    
        
        var dialog = await DialogService.ShowAsync<PlayerH5p>("H5P-Player", parameters, options);

        var result = await dialog.Result;
        
        var javaScriptAdapter = new CallJavaScriptAdapter(JSRuntime);
        var dataAccess = new FileSystemDataAccess();
        ITerminateH5pPlayerUcPort terminateH5pPlayer = new TerminateH5pPlayerUc(javaScriptAdapter, dataAccess);
        await terminateH5pPlayer.TerminateH5pPlayer();
        
        try
        {
            await JSRuntime.InvokeVoidAsync("terminateH5pStandalone");

        }
        catch (JSException ex)
        {
            Logger.LogError("JSException: Could not call 'terminateH5pStandalone': {Message}", ex.Message);
        }
        
        
        if (result?.Canceled == true)
        {
            Logger.LogInformation("Dialog wurde abgebrochen.");
        }
        else if (result != null)
        {
            Logger.LogInformation("Dialog wurde mit Ergebnis geschlossen: {@Data}", result.Data);
        }
    }
    
    
    public ILogger<H5PPlayerDialogViewModel> Logger { get;  }
    public IJSRuntime JSRuntime { get; }
    public IDialogService DialogService { get; }
    
}