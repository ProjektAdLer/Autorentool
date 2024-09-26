using System.Text.Json;
using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.JavaScriptApi;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

/// <summary>
/// Documentation:
/// https://projektadler.github.io/Documentation/hse2.html#definition
/// </summary>
public class DisplayH5pUc : IDisplayH5pUcInputPort
{
    public DisplayH5pUc(
        IJavaScriptAdapter javaScriptAdapter,
        IDisplayH5pUcOutputPort displayH5pUcOutputPort)
    {
        JavaScriptAdapter = javaScriptAdapter;
        DisplayH5pUcOutputPort = displayH5pUcOutputPort;
        H5pEntity = null;
    }


    public void StartToDisplayH5p(DisplayH5pInputTo displayH5PTo)
    {
        try
        {
            CreateH5pEntity();
            H5pEntity.H5pJsonSourcePath = displayH5PTo.H5pJsonSourcePath;
            JavaScriptAdapter.DisplayH5p(H5pEntity);
        }
        catch (ArgumentException e)
        {
            CreateErrorOutputForInvalidPath(displayH5PTo);
        }
        
        
    }

    private void CreateErrorOutputForInvalidPath(DisplayH5pInputTo displayH5PTo)
    {
        var errorOutputTo = new DisplayH5pErrorOutputTo();
        errorOutputTo.InvalidH5pJsonSourcePath = displayH5PTo.H5pJsonSourcePath;
        errorOutputTo.H5pJsonSourcePathErrorText = "H5P Json Path was wrong!";
        DisplayH5pUcOutputPort.ErrorOutput(errorOutputTo);
    }

    private void CreateH5pEntity()
    {
        H5pEntity = new H5pEntity();
    }

   

    
    
    
    
    
   

    internal IJavaScriptAdapter JavaScriptAdapter { get; }
    internal H5pEntity H5pEntity { get; set; }
    internal IDisplayH5pUcOutputPort DisplayH5pUcOutputPort { get;  }
}