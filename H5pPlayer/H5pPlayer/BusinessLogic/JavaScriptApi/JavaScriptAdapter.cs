using H5pPlayer.BusinessLogic.Domain;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.JavaScriptApi;

public class JavaScriptAdapter : IJavaScriptAdapter
{
    private IJSRuntime JsRuntime { get;  }

    
    public JavaScriptAdapter(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }
    public async Task DisplayH5p(H5pEntity h5pEntity)
    {
        var jsonSourcePath = IfJsonSourcePathContainsHttpsDeleteIt(h5pEntity);
        await JsRuntime.InvokeVoidAsync("testH5P", jsonSourcePath);
    }

    /// <summary>
    /// why we must delete https:
    /// https://github.com/ProjektAdLer/Autorentool/issues/570#issuecomment-2275233471
    /// </summary>
    private static string IfJsonSourcePathContainsHttpsDeleteIt(H5pEntity h5pEntity)
    {
        var jsonSourcePath = h5pEntity.H5pJsonSourcePath;
        if (jsonSourcePath.StartsWith("https:"))
        {
            jsonSourcePath = jsonSourcePath.Substring("https:".Length);
        }

        return jsonSourcePath;
    }
}