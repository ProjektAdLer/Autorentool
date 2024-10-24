﻿using H5pPlayer.BusinessLogic.Domain;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.Api.JavaScript;

public class JavaScriptAdapter : IJavaScriptAdapter
{
    private IJSRuntime JsRuntime { get;  }

    
    public JavaScriptAdapter(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }
    public async Task DisplayH5p(H5pEntity h5pEntity)
    {
        var pathOfH5pToPlay = GeneratePathOfH5PToPlay(h5pEntity);
        await JsRuntime.InvokeVoidAsync("testH5P", pathOfH5pToPlay);
    }

    private static string GeneratePathOfH5PToPlay(H5pEntity h5pEntity)
    {
        var nameOfH5pToPlay = Path.GetFileNameWithoutExtension(h5pEntity.H5pZipSourcePath);
        var pathOfH5pToPlay = h5pEntity.UnzippedH5psPath + "/" + nameOfH5pToPlay;
        
        pathOfH5pToPlay = IfPathOfH5PToPlayPathContainsHttpsDeleteHttps(pathOfH5pToPlay);

        return pathOfH5pToPlay;
    }
    
    
    /// <summary>
    /// why we must delete https:
    /// https://github.com/ProjektAdLer/Autorentool/issues/570#issuecomment-2275233471
    /// </summary>
    private static string IfPathOfH5PToPlayPathContainsHttpsDeleteHttps(string pathOfH5pToPlay)
    {
        if (pathOfH5pToPlay.StartsWith("https:"))
        {
            pathOfH5pToPlay = pathOfH5pToPlay.Substring("https:".Length);
        }

        return pathOfH5pToPlay;
    }


}