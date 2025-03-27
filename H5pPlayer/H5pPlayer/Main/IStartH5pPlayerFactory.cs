using H5pPlayer.Presentation.PresentationLogic;
using Microsoft.JSInterop;

namespace H5pPlayer.Main;

public interface IStartH5pPlayerFactory
{
    void InitializeStartH5pPlayer(
        Action viewStateNotificationMethod,
        IJSRuntime jsRuntime);

    public H5pPlayerViewModel H5pPlayerVm { get; set; }
    
    public H5pPlayerController H5pPlayerController { get; set; }
}