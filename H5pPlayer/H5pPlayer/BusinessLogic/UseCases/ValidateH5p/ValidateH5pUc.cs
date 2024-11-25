using System.Text.Json;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public class ValidateH5pUc
{

    public ValidateH5pUc(IJavaScriptAdapter javaScriptAdapter)
    {
        JavaScriptAdapter = javaScriptAdapter;
    }

    public async Task  StartToValidateH5p(H5pEntity h5pEntity)
    {
        var javaScriptAdapterTO = new JavaScriptAdapterTO(h5pEntity.UnzippedH5psPath, h5pEntity.H5pZipSourcePath);
        await JavaScriptAdapter.ValidateH5p(javaScriptAdapterTO);
    }
    
    
    
    
    
    public IJavaScriptAdapter JavaScriptAdapter { get; }

}