using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Presentation.PresentationLogic;
using Microsoft.JSInterop;

namespace H5pPlayer.Main;

public interface IStartH5pPlayerFactory
{
    void CreateStartH5pPlayerPresentationAndUseCaseStructure(
        Action viewStateNotificationMethod,
        IJSRuntime jsRuntime);

    public H5pPlayerViewModel H5pPlayerVm { get; set; }
    
    public H5pPlayerController H5pPlayerController { get; set; }
    public IDisplayH5pUC DisplayH5pUc { get; set; }
    public IValidateH5pUc ValidateH5pUc { get; set; }
}