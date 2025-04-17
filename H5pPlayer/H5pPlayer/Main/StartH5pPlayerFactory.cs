using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.Presentation.PresentationLogic;
using Microsoft.JSInterop;

namespace H5pPlayer.Main;

public class StartH5pPlayerFactory : IStartH5pPlayerFactory
{

    public StartH5pPlayerFactory(
        IDisplayH5pFactory displayH5pFactory,
        IValidateH5pFactory validateH5pFactory,
        IFileSystemDataAccess fileSystemDataAccess)
    {
        H5pPlayerVm = null;
        H5pPlayerController = null;
        DisplayH5PFactory = displayH5pFactory;
        ValidateH5PFactory = validateH5pFactory;
        FileSystemDataAccess = fileSystemDataAccess;
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
        DisplayH5PFactory.CreateDisplayH5pStructure(callJavaScriptAdapter);
        ValidateH5PFactory.CreateValidateH5pStructure(callJavaScriptAdapter);
        
        var startH5PPlayerUc = new StartH5pPlayerUC(
            ValidateH5PFactory.ValidateH5pUc!,
            FileSystemDataAccess, 
            DisplayH5PFactory.DisplayH5pUc!, 
            h5pPlayerPresenter);
        return startH5PPlayerUc;
    }



    

    public H5pPlayerViewModel? H5pPlayerVm { get; private set; }
    public H5pPlayerController? H5pPlayerController { get; private set; }
    public IDisplayH5pFactory DisplayH5PFactory { get; private set; }
    public IValidateH5pFactory ValidateH5PFactory { get; private set; }
    public IFileSystemDataAccess FileSystemDataAccess { get; }
}