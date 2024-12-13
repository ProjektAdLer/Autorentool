using H5pPlayer.BusinessLogic;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.DataAccess.FileSystem;
using Microsoft.JSInterop;

namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerController
{
 

    public H5pPlayerController(IJSRuntime jsRuntime)
    {
        ICallJavaScriptAdapter callJavaScriptAdapter = new CallJavaScriptAdapter(jsRuntime);
        H5PPlayerPresenter = new H5pPlayerPresenter();
        IDisplayH5pUC displayH5pUC = new DisplayH5pUC(callJavaScriptAdapter);
        IValidateH5pUc validateH5pUc = new ValidateH5pUc(H5PPlayerPresenter ,callJavaScriptAdapter);
        var fileSystemDataAccess = new FileSystemDataAccess();
        StartH5PPlayerUc = new StartH5pPlayerUC(
            validateH5pUc, fileSystemDataAccess, displayH5pUC, H5PPlayerPresenter);
    }

    public async Task StartH5pPlayer(string h5pSourcePath, string unzippedH5psPath)
    {
        var displayH5pTo = new StartH5pPlayerInputTO(H5pDisplayMode.Validate, h5pSourcePath, unzippedH5psPath);
        await StartH5PPlayerUc.StartH5pPlayer(displayH5pTo);
    }
    
    internal IStartH5pPlayerUCInputPort StartH5PPlayerUc { get; }
    internal H5pPlayerPresenter H5PPlayerPresenter { get; }

}