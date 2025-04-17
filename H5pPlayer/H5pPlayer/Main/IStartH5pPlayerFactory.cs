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


    
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="StartH5pPlayerFactory.CreateStartH5pPlayerPresentationAndUseCaseStructure"/>
    /// </exception>
    public H5pPlayerViewModel? H5pPlayerVm { get;  }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="StartH5pPlayerFactory.CreateStartH5pPlayerPresentationAndUseCaseStructure"/>
    /// </exception>
    public H5pPlayerController? H5pPlayerController { get; }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="StartH5pPlayerFactory.CreateStartH5pPlayerPresentationAndUseCaseStructure"/>
    /// </exception>
    IDisplayH5pFactory DisplayH5PFactory { get; }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="StartH5pPlayerFactory.CreateStartH5pPlayerPresentationAndUseCaseStructure"/>
    /// </exception>
    IValidateH5pFactory ValidateH5PFactory { get; }


}