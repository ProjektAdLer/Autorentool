// using H5pPlayer.BusinessLogic.Entities;


using System;
using System.Threading.Tasks;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.Api.JavaScript
{
    public class JavaScriptAdapter : IJavaScriptAdapter
    {
        private readonly IJSRuntime _jsRuntime;

        public JavaScriptAdapter(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

     

        public async Task DisplayH5p(JavaScriptAdapterTO javascriptAdapterTO)
        {
            try
            {
                var pathOfH5pToPlay = GeneratePathOfH5PToPlay(javascriptAdapterTO);
                await _jsRuntime.InvokeVoidAsync("displayH5p", pathOfH5pToPlay);
            }
            catch (JSException jsEx)
            {
                Console.WriteLine("JavaScript error when calling displayH5p: " + jsEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error when calling displayH5p: " + ex.Message);
            }
        }

        /// <example>
        /// example path: //localhost:8001/H5pStandalone/h5p-folder/Accordion_Test
        /// </example>
        private static string GeneratePathOfH5PToPlay(JavaScriptAdapterTO javascriptAdapterTO)
        {
            var nameOfH5pToPlay = Path.GetFileNameWithoutExtension(javascriptAdapterTO.H5pZipSourcePath);
            var pathOfH5pToPlay = javascriptAdapterTO.UnzippedH5psPath + nameOfH5pToPlay;

            pathOfH5pToPlay = IfPathOfH5PToPlayPathContainsHttpDeleteHttp(pathOfH5pToPlay);

            return pathOfH5pToPlay;
        }

        /// <summary>
        /// why we must delete https or http:
        /// https://github.com/ProjektAdLer/Autorentool/issues/570#issuecomment-2275233471
        /// </summary>
        private static string IfPathOfH5PToPlayPathContainsHttpDeleteHttp(string pathOfH5pToPlay)
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
                await _jsRuntime.InvokeVoidAsync("terminateH5pStandalone");
            }
            catch (JSException jsEx)
            {
                Console.WriteLine("JavaScript error when calling displayH5p: " + jsEx.Message);

                // Logger.LogError("JSException: Could not call 'terminateH5pStandalone': {Message}", ex.Message);
            }
        }
    }
}