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
        DisplayH5pUc = null;
        ValidateH5pUc = null;
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
        DisplayH5pUc = new DisplayH5pUC(callJavaScriptAdapter);
        ValidateH5pUc = new ValidateH5pUc(h5pPlayerPresenter ,callJavaScriptAdapter);
        var fileSystemDataAccess = new FileSystemDataAccess();
        var startH5PPlayerUc = new StartH5pPlayerUC(
            ValidateH5pUc, fileSystemDataAccess, DisplayH5pUc, h5pPlayerPresenter);
        return startH5PPlayerUc;
    }
    
    public H5pPlayerViewModel? H5pPlayerVm { get; set; }
    public H5pPlayerController? H5pPlayerController { get; set; }
    public IDisplayH5pUC? DisplayH5pUc { get; set; }
    public IValidateH5pUc? ValidateH5pUc { get; set; }
}