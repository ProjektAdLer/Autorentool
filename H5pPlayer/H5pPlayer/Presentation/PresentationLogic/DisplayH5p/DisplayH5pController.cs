using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

namespace H5pPlayer.Presentation.PresentationLogic.DisplayH5p;

public class DisplayH5pController : IDisplayH5pController
{

    public DisplayH5pController(IDisplayH5pUc displayH5pUc, IDisplayH5pPresenter displayH5pPresenter)
    {
        DisplayH5pUc = displayH5pUc;
    }

    public void TerminateDisplayH5p()
    {
        DisplayH5pUc.TerminateDisplayH5p();
    }
    
    
    private IDisplayH5pUc DisplayH5pUc { get; }

}