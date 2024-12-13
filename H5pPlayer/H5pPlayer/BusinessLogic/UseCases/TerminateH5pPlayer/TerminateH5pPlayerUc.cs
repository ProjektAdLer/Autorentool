using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.BusinessRules;

namespace H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;

public class TerminateH5pPlayerUc : ITerminateH5pPlayerUcPort
{

    public TerminateH5pPlayerUc(
        ICallJavaScriptAdapter iCallJavaScriptAdapter, IFileSystemDataAccess dataAccess)
    {
        ICallJavaScriptAdapter = iCallJavaScriptAdapter;
        TemporaryH5pManager = new TemporaryH5psInWwwrootManager(dataAccess);
    }

    public async Task TerminateH5pPlayer()
    {
        TemporaryH5pManager.CleanDirectoryForTemporaryH5psInWwwroot();
        await ICallJavaScriptAdapter.TerminateH5pJavaScriptPlayer();
    }
    
    private TemporaryH5psInWwwrootManager TemporaryH5pManager { get; }
    private ICallJavaScriptAdapter ICallJavaScriptAdapter { get; }

}
