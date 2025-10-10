using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.BusinessRules;
using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;

public class TerminateH5pPlayerUc : ITerminateH5pPlayerUcPort
{

    public TerminateH5pPlayerUc(
        ICallJavaScriptAdapter iCallJavaScriptAdapter, 
        IFileSystemDataAccess dataAccess, 
        Action<H5pPlayerResultTO> onH5pPlayerFinished)
    {
        ICallJavaScriptAdapter = iCallJavaScriptAdapter;
        TemporaryH5pManager = new TemporaryH5psManager(dataAccess);
        OnH5pPlayerFinished = onH5pPlayerFinished; 
    }

    public async Task TerminateH5pPlayer(H5pState activeH5pState)
    {
        TemporaryH5pManager.CleanDirectoryForTemporaryUnzippedH5ps();
        await ICallJavaScriptAdapter.TerminateH5pJavaScriptPlayer();
        SendResultToCallerSystem(activeH5pState);
    }
    
    private void SendResultToCallerSystem(H5pState activeH5pState)
    {
        var result = new H5pPlayerResultTO(activeH5pState.ToString());

        OnH5pPlayerFinished?.Invoke(result);
    }
    
    private TemporaryH5psManager TemporaryH5pManager { get; }
    private ICallJavaScriptAdapter ICallJavaScriptAdapter { get; }
    private Action<H5pPlayerResultTO>? OnH5pPlayerFinished { get; set; }
}
