using System.Text.Json;
using H5pPlayer.BusinessLogic;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.JavaScriptApi;
using Microsoft.JSInterop;

namespace Presentation.H5pPlayer.PresentationLogic;

public class H5pPlayerController
{

    public void StartH5pPlayer(IJSRuntime jsRuntime)
    {
        IJavaScriptAdapter javaScriptAdapter = new JavaScriptAdapter(jsRuntime);
        IDisplayH5pUcOutputPort displayH5PUcOutputPort = new H5pPlayerPresenter();
        IDisplayH5pUcInputPort displayH5PUcInputPort = new DisplayH5pUc(javaScriptAdapter, displayH5PUcOutputPort);
        var displayH5pTo = new DisplayH5pInputTo("testpath0");
        displayH5PUcInputPort.StartToDisplayH5p(displayH5pTo);
    }
    


}