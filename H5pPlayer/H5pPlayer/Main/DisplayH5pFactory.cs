using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;
using Microsoft.Extensions.Logging;

namespace H5pPlayer.Main;

public class DisplayH5pFactory : IDisplayH5pFactory
{
    public DisplayH5pFactory()
    {
        DisplayH5pVm = null;
        DisplayH5pPresenter = null;
        TerminateH5pPlayerUc = null;
        DisplayH5pUc = null;
        DisplayH5pController = null;
    }
    public void CreateDisplayH5pStructure(
        ICallJavaScriptAdapter callJavaScriptAdapter,
        ITerminateH5pPlayerUcPort terminateH5pPlayerUc,
        ILoggerFactory loggerFactory)
    {
        DisplayH5pVm = new DisplayH5pViewModel();
        DisplayH5pPresenter = new DisplayH5pPresenter(DisplayH5pVm);
        TerminateH5pPlayerUc = terminateH5pPlayerUc;
        var logger = loggerFactory.CreateLogger<DisplayH5pUc>();
        DisplayH5pUc = new DisplayH5pUc(callJavaScriptAdapter, terminateH5pPlayerUc, logger);
        DisplayH5pController = new DisplayH5pController(DisplayH5pUc, DisplayH5pPresenter);
    }

    public IDisplayH5pViewModel? DisplayH5pVm { get; private set; }
    public IDisplayH5pPresenter? DisplayH5pPresenter { get; private set; }
    public ITerminateH5pPlayerUcPort? TerminateH5pPlayerUc { get; private set; }
    public IDisplayH5pUc? DisplayH5pUc { get; private set; }
    public IDisplayH5pController? DisplayH5pController { get; private set; }
    
    
    
    
   

}