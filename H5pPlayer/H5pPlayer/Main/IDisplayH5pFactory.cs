using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;

namespace H5pPlayer.Main;

public interface IDisplayH5pFactory
{
    void CreateDisplayH5pPresentationStructure(IDisplayH5pUC displayH5pUC);

    IDisplayH5pViewModel? DisplayH5pVm { get; set; }
    IDisplayH5pController? DisplayH5pController { get; set; }
}