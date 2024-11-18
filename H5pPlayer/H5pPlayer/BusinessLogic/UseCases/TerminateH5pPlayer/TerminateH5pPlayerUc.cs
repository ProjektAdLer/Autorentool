using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.BusinessRules;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;

public class TerminateH5pPlayerUc : ITerminateH5pPlayerUcPort
{

    public TerminateH5pPlayerUc(
        IJSRuntime jsRuntime, IFileSystemDataAccess dataAccess)
    {
        JsRuntime = jsRuntime;
        TemporaryH5pManager = new TemporaryH5psInWwwrootManager(dataAccess);
    }

    public void TerminateH5pPlayer()
    {
        TemporaryH5pManager.CleanDirectoryForTemporaryH5psInWwwroot();
        JsRuntime.InvokeVoidAsync("terminateH5pPlayer");
    }
    
    private TemporaryH5psInWwwrootManager TemporaryH5pManager { get; }
    private IJSRuntime JsRuntime { get; }

}




// try
// {
//     await JsRuntime.InvokeVoidAsync("terminateH5pStandalone");
//
// }
// catch (JSException ex)
// {
//     Logger.LogError("JSException: Could not call 'terminateH5pStandalone': {Message}", ex.Message);
// }