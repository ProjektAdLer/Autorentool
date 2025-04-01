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
    public void CreateDisplayH5pPresentationStructure(IDisplayH5pUC displayH5pUC)
    {
        DisplayH5pVm = new DisplayH5pViewModel();
        var h5pPlayerPresenter = new DisplayH5pPresenter(DisplayH5pVm);
        DisplayH5pController = new DisplayH5pController(displayH5pUC, h5pPlayerPresenter);
    }



    
    public DisplayH5pViewModel? DisplayH5pVm { get; set; }
    public DisplayH5pController? DisplayH5pController { get; set; }
}