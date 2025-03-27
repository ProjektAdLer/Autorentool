using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;


namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerController
{
 

   
   
    public H5pPlayerController(
        IStartH5pPlayerUCInputPort startH5pPlayerUc,
        H5pPlayerPresenter h5pPlayerPresenter)
    {
        StartH5PPlayerUc = startH5pPlayerUc;
        H5PPlayerPresenter = h5pPlayerPresenter;
    }


    
    /// <summary>
    /// Testable Constructor
    /// </summary>
    public H5pPlayerController(IStartH5pPlayerUCInputPort? startH5pPlayerUc)
    {
        StartH5PPlayerUc = startH5pPlayerUc;
        H5PPlayerPresenter = null;
    }

    public async Task StartH5pPlayer(H5pDisplayMode h5PDisplayMode, string h5pSourcePath, string unzippedH5psPath)
    {
        var startTo = new StartH5pPlayerInputTO(h5PDisplayMode, h5pSourcePath, unzippedH5psPath);
        await StartH5PPlayerUc!.StartH5pPlayer(startTo);
    }
    
    internal IStartH5pPlayerUCInputPort? StartH5PPlayerUc { get; }
    internal H5pPlayerPresenter? H5PPlayerPresenter { get; }

}