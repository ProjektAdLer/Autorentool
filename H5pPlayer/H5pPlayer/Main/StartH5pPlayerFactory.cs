using H5pPlayer.Api;
using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
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
        OnH5pPlayerFinished = null;
        H5pPlayerVm = null;
        H5pPlayerController = null;
        DisplayH5PFactory = displayH5pFactory;
        ValidateH5PFactory = validateH5pFactory;
        FileSystemDataAccess = fileSystemDataAccess;
    }
    
    public void CreateStartH5pPlayerPresentationAndUseCaseStructure(
        Action viewStateNotificationMethod,
        IJSRuntime jsRuntime,
        Action<H5pPlayerResultTO> onH5pPlayerFinished)
    {
        OnH5pPlayerFinished = onH5pPlayerFinished;
        H5pPlayerVm = new H5pPlayerViewModel(viewStateNotificationMethod);
        var h5pPlayerPresenter = new H5pPlayerPresenter(H5pPlayerVm);
        var startH5PPlayerUc = CreateStartH5pPlayerUc(h5pPlayerPresenter, jsRuntime);
        H5pPlayerController = new H5pPlayerController(startH5PPlayerUc, h5pPlayerPresenter);
    }


    private StartH5pPlayerUC CreateStartH5pPlayerUc(H5pPlayerPresenter h5pPlayerPresenter, IJSRuntime jsRuntime)
    {
        ICallJavaScriptAdapter callJavaScriptAdapter = new CallJavaScriptAdapter(jsRuntime);
        var terminateH5pPlayerUc = new TerminateH5pPlayerUc(callJavaScriptAdapter, FileSystemDataAccess, OnH5pPlayerFinished!);
        DisplayH5PFactory.CreateDisplayH5pStructure(callJavaScriptAdapter,terminateH5pPlayerUc);
        ValidateH5PFactory.CreateValidateH5pStructure(callJavaScriptAdapter, terminateH5pPlayerUc);
        
        var startH5PPlayerUc = new StartH5pPlayerUC(
            ValidateH5PFactory.ValidateH5pUc!,
            FileSystemDataAccess, 
            DisplayH5PFactory.DisplayH5pUc!, 
            h5pPlayerPresenter);
        return startH5PPlayerUc;
    }

    private Action<H5pPlayerResultTO>? OnH5pPlayerFinished { get; set; }

    public H5pPlayerViewModel? H5pPlayerVm { get; private set; }
    public H5pPlayerController? H5pPlayerController { get; private set; }
    public IDisplayH5pFactory DisplayH5PFactory { get; private set; }
    public IValidateH5pFactory ValidateH5PFactory { get; private set; }
    public IFileSystemDataAccess FileSystemDataAccess { get; private set; }
}