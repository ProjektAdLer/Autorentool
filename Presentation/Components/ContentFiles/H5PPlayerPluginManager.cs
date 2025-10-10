using H5pPlayer.Api;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.JSInterop;
using MudBlazor;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent.FileContent;
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
        H5PZipSourcePath = "init";
        BaseUri = null;
        UnzippedH5PsPath = "init";
    }
    
    

   
    public async Task StartH5pPlayerToValidateAsync(StartH5PPlayerTO parseH5pFileTO)
    {
        if (IsFileH5PFile(parseH5pFileTO))
        {
            InitializeH5PPlayer(parseH5pFileTO);
            Logger.LogTrace("Start H5P-Player with: " + 
                            "h5pZipSourcePath: " + H5PZipSourcePath + Environment.NewLine +
                            "unzippedH5psPath: " + UnzippedH5PsPath );

            var h5PPlayerResult = await OpenH5pPlayerDialogAsync(H5pDisplayMode.Validate);
            ProcessH5PPlayerResult(h5PPlayerResult);
        }
    }

    private static bool IsFileH5PFile(StartH5PPlayerTO parseH5pFileTO)
    {
        var fileEnding = parseH5pFileTO.FileContentVm!.Name.Split(".").Last();
        return  fileEnding == "h5p";
    }

    private void InitializeH5PPlayer(StartH5PPlayerTO parseH5pFileTO)
    {
        FileContentVm = parseH5pFileTO.FileContentVm;
        H5PZipSourcePath = Path.Combine(ApplicationPaths.ContentFolder , FileContentVm!.Name);
        BaseUri = new Uri(parseH5pFileTO.NavigationManager!.BaseUri); 
        UnzippedH5PsPath = new Uri(BaseUri, "h5p-folder/").AbsoluteUri;
    }

    private void ProcessH5PPlayerResult(H5pPlayerResultTO? h5PPlayerResult)
    {
        if (h5PPlayerResult != null)
        {
            Logger.LogTrace("H5P: Set ActiveH5pState in LearningContentViewModel ");
            FileContentVm!.IsH5P = true;
            switch (h5PPlayerResult.Value.ActiveH5pState)
            {
                case "Completable":
                    FileContentVm.H5PState = H5PContentState.Completable;
                    break;
                case "Primitive":
                    FileContentVm.H5PState = H5PContentState.Primitive;
                    break;
                case "NotUsable":
                    FileContentVm.H5PState = H5PContentState.NotUsable;
                    break;
                //case "NotValidated":
                // Skip setNotValidated, because we only set to notValidated if the user cancels the H5P player.
                // But if the user cancels, we need the previous state!
                //  FileContentVm.H5PState = H5PContentState.NotValidated;
                // break;
            }

            PresentationLogic.EditH5PFileContent(FileContentVm);
        }
    }

    private async Task<H5pPlayerResultTO?> OpenH5pPlayerDialogAsync(H5pDisplayMode displayMode)
    {
        var tcs = new TaskCompletionSource<H5pPlayerResultTO?>();

        var parameters = new DialogParameters
        {
            { "H5pZipSourcePath", H5PZipSourcePath },
            { "UnzippedH5psPath", UnzippedH5PsPath },
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
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
        };

       await DialogService.ShowAsync<PlayerH5p>("", parameters, options);
  
        return await tcs.Task;
    }
    
    private IFileContentViewModel? FileContentVm { get; set; }
    public ILogger<H5PPlayerPluginManager> Logger { get;  }
    public IJSRuntime JSRuntime { get; }
    public IDialogService DialogService { get; }
    private IPresentationLogic PresentationLogic { get; }
    private string H5PZipSourcePath { get; set; }
    private Uri? BaseUri { get; set; }
    private string UnzippedH5PsPath { get; set; }

}