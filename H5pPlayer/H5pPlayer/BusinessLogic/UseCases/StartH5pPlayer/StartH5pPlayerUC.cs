using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.JavaScriptApi;

namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

/// <summary>
/// Documentation:
/// https://projektadler.github.io/Documentation/hse2.html#definition
/// </summary>
public class StartH5pPlayerUC : IStartH5pPlayerUCInputPort
{
    public StartH5pPlayerUC(
        IJavaScriptAdapter javaScriptAdapter,
        IStartH5pPlayerUCOutputPort startH5PPlayerUcOutputPort)
    {
        JavaScriptAdapter = javaScriptAdapter;
        StartH5PPlayerUcOutputPort = startH5PPlayerUcOutputPort;
        H5pEntity = null;
    }


    public void StartToDisplayH5p(StartH5pPlayerInputTO displayH5PInputTo)
    {
        
            MapTOtoEntity(displayH5PInputTo);
            // call dataaccess
            JavaScriptAdapter.DisplayH5p(H5pEntity);
      
        
        
    }

    private void MapTOtoEntity(StartH5pPlayerInputTO displayH5PInputTo)
    {
        try
        {
            CreateH5pEntity();
            H5pEntity.H5pJsonSourcePath = displayH5PInputTo.H5pJsonSourcePath;
        }
        catch (ArgumentException e)
        {
            CreateErrorOutputForInvalidPath(displayH5PInputTo);
        }
    }

    private void CreateErrorOutputForInvalidPath(StartH5pPlayerInputTO displayH5PInputTo)
    {
        var errorOutputTo = new StartH5pPlayerErrorOutputTO();
        errorOutputTo.InvalidH5pJsonSourcePath = displayH5PInputTo.H5pJsonSourcePath;
        errorOutputTo.H5pJsonSourcePathErrorText = "H5P Json Path was wrong!";
        StartH5PPlayerUcOutputPort.ErrorOutput(errorOutputTo);
    }

    private void CreateH5pEntity()
    {
        H5pEntity = new H5pEntity();
    }

   

    
    
    
    
    
   

    internal IJavaScriptAdapter JavaScriptAdapter { get; }
    internal H5pEntity H5pEntity { get; set; }
    internal IStartH5pPlayerUCOutputPort StartH5PPlayerUcOutputPort { get;  }
}