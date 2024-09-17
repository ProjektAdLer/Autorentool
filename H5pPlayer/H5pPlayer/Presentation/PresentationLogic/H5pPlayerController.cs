using H5pPlayer.BusinessLogic;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.JavaScriptApi;
using Microsoft.JSInterop;

namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerController
{
 

    public H5pPlayerController(IJSRuntime jsRuntime)
    {
        IJavaScriptAdapter javaScriptAdapter = new JavaScriptAdapter(jsRuntime);
        DisplayH5pPresenter = new H5pPlayerPresenter();
        DisplayH5pUc = new DisplayH5pUc(javaScriptAdapter, DisplayH5pPresenter);
    }

    public void StartH5pPlayer(string h5pPath)
    {
        var displayH5pTo = new DisplayH5pInputTo(h5pPath);
        DisplayH5pUc.StartToDisplayH5p(displayH5pTo);
    }
    
    internal IDisplayH5pUcInputPort DisplayH5pUc { get; }
    internal IDisplayH5pUcOutputPort DisplayH5pPresenter { get; }

}