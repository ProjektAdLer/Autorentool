using H5pPlayer.BusinessLogic;
using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.JavaScriptApi;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using Microsoft.JSInterop;

namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerController
{
 

    public H5pPlayerController(IJSRuntime jsRuntime)
    {
        IJavaScriptAdapter javaScriptAdapter = new JavaScriptAdapter(jsRuntime);
        IDisplayH5pUC displayH5pUC = new DisplayH5pUC(javaScriptAdapter);
        StartH5PPlayerPresenter = new H5PPlayerPlayerPresenter();
        StartH5PPlayerUc = new StartH5pPlayerUC(displayH5pUC, StartH5PPlayerPresenter);
    }

    public void StartH5pPlayer(string h5pSourcePath, string unzippedH5psPath)
    {
        var displayH5pTo = new StartH5pPlayerInputTO(H5pDisplayMode.Display, h5pSourcePath, unzippedH5psPath);
        StartH5PPlayerUc.StartH5pPlayer(displayH5pTo);
    }
    
    internal IStartH5pPlayerUCInputPort StartH5PPlayerUc { get; }
    internal IStartH5pPlayerUCOutputPort StartH5PPlayerPresenter { get; }

}