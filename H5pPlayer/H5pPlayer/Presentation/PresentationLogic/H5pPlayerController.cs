using H5pPlayer.BusinessLogic;
using H5pPlayer.BusinessLogic.JavaScriptApi;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using Microsoft.JSInterop;

namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerController
{
 

    public H5pPlayerController(IJSRuntime jsRuntime)
    {
        IJavaScriptAdapter javaScriptAdapter = new JavaScriptAdapter(jsRuntime);
        StartH5PPlayerPresenter = new H5PPlayerPlayerPresenter();
        StartH5PPlayerUc = new StartH5pPlayerUC(javaScriptAdapter, StartH5PPlayerPresenter);
    }

    public void StartH5pPlayer(string h5pPath)
    {
        var displayH5pTo = new StartH5pPlayerInputTO(h5pPath);
        StartH5PPlayerUc.StartToDisplayH5p(displayH5pTo);
    }
    
    internal IStartH5pPlayerUCInputPort StartH5PPlayerUc { get; }
    internal IStartH5pPlayerUCOutputPort StartH5PPlayerPresenter { get; }

}