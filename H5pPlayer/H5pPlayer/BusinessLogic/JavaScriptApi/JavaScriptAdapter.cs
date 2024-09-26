using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.JavaScriptApi;

public class JavaScriptAdapter : IJavaScriptAdapter
{
    private IJSRuntime JsRuntime { get;  }

    
    public JavaScriptAdapter(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }
    // hier muss auch der Pfad zur H5pJson abgeschnitteten werden also hier muss http: weggeschnitten werden!!!!!
    public async void  DisplayH5p(string h5pJsonSourcePath)
    {
        // _h5pTestButtonText  = new (await JsRuntime.InvokeAsync<string>("javaScriptHalloWorldFunction"));
       // _h5pTestButtonText  = new (await JsRuntime.InvokeAsync<string>("testH5P", "h5p-container"));
        await JsRuntime.InvokeVoidAsync("testH5P", "h5p-container");
        // await InvokeAsync(StateHasChanged);
    
    }
}