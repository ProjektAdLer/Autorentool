using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.Api.JavaScript
{
    
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class CallJavaScriptAdapter : ICallJavaScriptAdapter
    {
        private IJSRuntime? _jsRuntime;

        public CallJavaScriptAdapter(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }


        public async Task DisplayH5p(CallJavaScriptAdapterTO callJavaScriptAdapterTo)
        {
            await CallJavaScriptInterop(callJavaScriptAdapterTo, "displayH5p");
        }
        
        public async Task ValidateH5p(CallJavaScriptAdapterTO callJavaScriptAdapterTo)
        {
            await CallJavaScriptInterop(callJavaScriptAdapterTo, "validateH5p");
        }

        private async Task CallJavaScriptInterop(CallJavaScriptAdapterTO callJavaScriptAdapterTo, string nameOfFunctionToCall)
        {
            try
            {
                var pathOfH5pToPlay = GeneratePathOfH5PToPlay(callJavaScriptAdapterTo);
                await _jsRuntime!.InvokeVoidAsync(nameOfFunctionToCall, pathOfH5pToPlay);
            }
            catch (JSException jsEx)
            {
                Console.WriteLine("JavaScript error when calling " + nameOfFunctionToCall + ": " + jsEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error when calling " + nameOfFunctionToCall + ": " + ex.Message);
            }
        }


        /// <example>
        /// example path: //localhost:8001/H5pStandalone/h5p-folder/Accordion_Test
        /// </example>
        private static string GeneratePathOfH5PToPlay(CallJavaScriptAdapterTO callJavaScriptAdapterTo)
        {
            var nameOfH5pToPlay = Path.GetFileNameWithoutExtension(callJavaScriptAdapterTo.H5pZipSourcePath);
            var pathOfH5pToPlay = callJavaScriptAdapterTo.UnzippedH5psPath + nameOfH5pToPlay;

            pathOfH5pToPlay = IfPathOfH5PToPlayContainsHttpDeleteHttp(pathOfH5pToPlay);

            return pathOfH5pToPlay;
        }

        /// <summary>
        /// why we must delete https or http:
        /// https://github.com/ProjektAdLer/Autorentool/issues/570#issuecomment-2275233471
        /// </summary>
        private static string IfPathOfH5PToPlayContainsHttpDeleteHttp(string pathOfH5pToPlay)
        {
            if (pathOfH5pToPlay.StartsWith("http:") || pathOfH5pToPlay.StartsWith("https:"))
            {
                pathOfH5pToPlay = pathOfH5pToPlay.Substring(pathOfH5pToPlay.IndexOf(':') + 1);
            }

            return pathOfH5pToPlay;
        }
        
        
        public async Task TerminateH5pJavaScriptPlayer()
        {
            try
            {
                await _jsRuntime!.InvokeVoidAsync("terminateH5pStandalone");
            }
            catch (JSException jsEx)
            {
                Console.WriteLine("JavaScript error when calling displayH5p: " + jsEx.Message);

                // Logger.LogError("JSException: Could not call 'terminateH5pStandalone': {Message}", ex.Message);
            }
        }
       
    }
}