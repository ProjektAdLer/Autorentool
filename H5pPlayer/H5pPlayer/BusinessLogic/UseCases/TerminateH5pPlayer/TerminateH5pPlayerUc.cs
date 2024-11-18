using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.BusinessRules;

namespace H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;

public class TerminateH5pPlayerUc : ITerminateH5pPlayerUcPort
{

    public TerminateH5pPlayerUc(
        IJavaScriptAdapter javaScriptAdapter, IFileSystemDataAccess dataAccess)
    {
        JavaScriptAdapter = javaScriptAdapter;
        TemporaryH5pManager = new TemporaryH5psInWwwrootManager(dataAccess);
    }

    public void TerminateH5pPlayer()
    {
        TemporaryH5pManager.CleanDirectoryForTemporaryH5psInWwwroot();
        JavaScriptAdapter.TerminateH5pJavaScriptPlayer();
    }
    
    private TemporaryH5psInWwwrootManager TemporaryH5pManager { get; }
    private IJavaScriptAdapter JavaScriptAdapter { get; }

}
