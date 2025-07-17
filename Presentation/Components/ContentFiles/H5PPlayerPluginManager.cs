using H5pPlayer.Api;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.JSInterop;
using MudBlazor;
using Presentation.PresentationLogic.API;
using Shared.Configuration;
using Shared.H5P;

namespace Presentation.Components.ContentFiles;

public class H5PPlayerPluginManager : IH5PPlayerPluginManager
{


    public H5PPlayerPluginManager(
        ILogger<H5PPlayerPluginManager> logger,
        IJSRuntime jSRuntime,
        IDialogService dialogService,
        IPresentationLogic presentationLogic)
    {
        Logger = logger;
        JSRuntime = jSRuntime;
        DialogService = dialogService;
        PresentationLogic = presentationLogic;
    }
    
    
    public async Task<H5pPlayerResultTO> ParseH5PFile(ParseH5PFileTO to)
    {
        H5pPlayerResultTO h5PPlayerResultTo = new H5pPlayerResultTO(); 
        
        if (to.FileEnding.ToLowerInvariant()  == ".h5p")
        {
            var h5pZipSourcePath = Path.Combine(ApplicationPaths.ContentFolder , to.FileName);
            var baseUri = new Uri(to.NavigationManager.BaseUri); 
            var unzippedH5ps= new Uri(baseUri, "H5pStandalone/h5p-folder/"); 
                     
            Logger.LogTrace("Start H5P-Player with: " + 
                            "h5pZipSourcePath: " + h5pZipSourcePath + Environment.NewLine +
                            "unzippedH5psPath: " + unzippedH5ps.AbsoluteUri );

            var result = await OpenH5pPlayerDialogAsync(h5pZipSourcePath, unzippedH5ps.AbsoluteUri, H5pDisplayMode.Validate);
            if (result != null)
            {
                Logger.LogTrace("H5P dialog result != null werte werden gesetzt:");
                h5PPlayerResultTo = result.Value;


                Logger.LogTrace("H5P: Set ActiveH5pState in LearningContentViewModel ");
                if (to.FileContentVm != null)
                {
                    to.FileContentVm.IsH5P = true;
                    switch (h5PPlayerResultTo.ActiveH5pState)
                    {
                        case "Completable":
                            to.FileContentVm.H5PState = H5PContentState.Completable;
                            break;
                        case "Primitive":
                            to.FileContentVm.H5PState = H5PContentState.Primitive;
                            break;
                        case "NotUsable":
                            to.FileContentVm.H5PState = H5PContentState.NotUsable;
                            break;
                        case "NotValidated":
                            to.FileContentVm.H5PState = H5PContentState.NotValidated;
                            break;
                    }

                    PresentationLogic.EditH5PFileContent(to.FileContentVm);
                }
            }
        }

        return h5PPlayerResultTo;
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

       await DialogService.ShowAsync<PlayerH5p>("H5P-Player", parameters, options);
  
        return await tcs.Task;
    }


    
    
    public ILogger<H5PPlayerPluginManager> Logger { get;  }
    public IJSRuntime JSRuntime { get; }
    public IDialogService DialogService { get; }
    public IPresentationLogic PresentationLogic { get; }
}