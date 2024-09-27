using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.JavaScriptApi;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

/// <summary>
/// Documentation:
/// https://projektadler.github.io/Documentation/hse7.html
/// </summary>
public class StartH5pPlayerUC : IStartH5pPlayerUCInputPort
{

    public StartH5pPlayerUC(
        IDisplayH5pUC displayH5PUc,
        IStartH5pPlayerUCOutputPort startH5PPlayerUcOutputPort)
    {
        DisplayH5pUC = displayH5PUc;
        StartH5PPlayerUcOutputPort = startH5PPlayerUcOutputPort;
        H5pEntity = null;
    }


    public void StartH5pPlayer(StartH5pPlayerInputTO displayH5PInputTo)
    {
        MapTOtoEntity(displayH5PInputTo);
        // call dataaccess via zipfilesystemaccess  to entzipp h5p
        DisplayH5pUC.StartToDisplayH5pUC(H5pEntity);
    }

    private void MapTOtoEntity(StartH5pPlayerInputTO displayH5PInputTo)
    {
        try
        {
            CreateH5pEntity();
            H5pEntity.ActiveDisplayMode = displayH5PInputTo.DisplayMode;
            H5pEntity.H5pJsonSourcePath = displayH5PInputTo.H5pJsonSourcePath;
        }
        catch (ArgumentException e)
        {
            CreateErrorOutputForInvalidPath(displayH5PInputTo);
        }
    }

    private void CreateErrorOutputForInvalidPath(StartH5pPlayerInputTO startH5PPlayerInputT0)
    {
        var errorOutputTo = new StartH5pPlayerErrorOutputTO();
        errorOutputTo.InvalidH5pJsonSourcePath = startH5PPlayerInputT0.H5pJsonSourcePath;
        errorOutputTo.H5pJsonSourcePathErrorText = "H5P Json Path was wrong!";
        StartH5PPlayerUcOutputPort.ErrorOutput(errorOutputTo);
    }

    private void CreateH5pEntity()
    {
        H5pEntity = new H5pEntity();
    }

   

    
    
    
    
    
   

    internal IDisplayH5pUC DisplayH5pUC { get; }
    internal H5pEntity H5pEntity { get; set; }
    internal IStartH5pPlayerUCOutputPort StartH5PPlayerUcOutputPort { get;  }
}