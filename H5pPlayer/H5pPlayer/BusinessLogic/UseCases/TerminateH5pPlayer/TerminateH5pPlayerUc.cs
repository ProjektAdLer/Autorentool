using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.BusinessRules;

namespace H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;

public class TerminateH5pPlayerUc : ITerminateH5pPlayerUcPort
{

    public TerminateH5pPlayerUc(IFileSystemDataAccess dataAccess)
    {
        TemporaryH5pManager = new TemporaryH5PsInWwwrootManager(dataAccess);
        
    }


    public void TerminateH5pPlayer()
    {
        TemporaryH5pManager.CleanDirectoryForTemporaryH5psInWwwroot();
    }
    
    private TemporaryH5PsInWwwrootManager TemporaryH5pManager { get; set; }

    
    

}