using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;

namespace H5pPlayer.Main;

public class DisplayH5pFactory : IDisplayH5pFactory
{
    public DisplayH5pFactory()
    {
        DisplayH5pVm = null;
        DisplayH5pController = null;
    }
    public void CreateDisplayH5pStructure(ICallJavaScriptAdapter callJavaScriptAdapter)
    {
        DisplayH5pVm = new DisplayH5pViewModel();
        DisplayH5pPresenter = new DisplayH5pPresenter(DisplayH5pVm);
        DisplayH5pUc = new DisplayH5pUC(callJavaScriptAdapter);
        DisplayH5pController = new DisplayH5pController(DisplayH5pUc, DisplayH5pPresenter);
    }


    public IDisplayH5pPresenter? DisplayH5pPresenter { get; private set; }
    public IDisplayH5pViewModel? DisplayH5pVm { get; private set; }
    public IDisplayH5pUC? DisplayH5pUc { get; private set; }
    public IDisplayH5pController? DisplayH5pController { get; private set; }
    
    
    
    
   

}