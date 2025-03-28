using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.DataAccess.FileSystem;
using H5pPlayer.Presentation.PresentationLogic;
using H5pPlayer.Presentation.View;
using Microsoft.JSInterop;

namespace H5pPlayer.Main;

public class StartH5pPlayerFactory : IStartH5pPlayerFactory
{
    public StartH5pPlayerFactory()
    {
        H5pPlayerVm = null;
        H5pPlayerController = null;
    }
    public void CreateStartH5pPlayerPresentationAndUseCaseStructure(
        Action viewStateNotificationMethod,
        IJSRuntime jsRuntime)
    {
        H5pPlayerVm = new H5pPlayerViewModel(viewStateNotificationMethod);
        var h5pPlayerPresenter = new H5pPlayerPresenter(H5pPlayerVm);
        var startH5PPlayerUc = CreateStartH5pPlayerUc(h5pPlayerPresenter, jsRuntime);
        H5pPlayerController = new H5pPlayerController(startH5PPlayerUc, h5pPlayerPresenter);
    }


    private StartH5pPlayerUC CreateStartH5pPlayerUc(H5pPlayerPresenter h5pPlayerPresenter, IJSRuntime jsRuntime)
    {
        ICallJavaScriptAdapter callJavaScriptAdapter = new CallJavaScriptAdapter(jsRuntime);
        IDisplayH5pUC displayH5pUC = new DisplayH5pUC(callJavaScriptAdapter);
        IValidateH5pUc validateH5pUc = new ValidateH5pUc(h5pPlayerPresenter ,callJavaScriptAdapter);
        var fileSystemDataAccess = new FileSystemDataAccess();
        var startH5PPlayerUc = new StartH5pPlayerUC(
            validateH5pUc, fileSystemDataAccess, displayH5pUC, h5pPlayerPresenter);
        return startH5PPlayerUc;
    }
    
    public H5pPlayerViewModel? H5pPlayerVm { get; set; }
    public H5pPlayerController? H5pPlayerController { get; set; }
}