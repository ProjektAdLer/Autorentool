using System.Text.Json;
using System.Text.Json.Nodes;
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

     

        public async Task DisplayH5p(JavaScriptAdapterTO javaScriptAdapterTo)
        {
            await CallJavaScriptInterop(javaScriptAdapterTo, "displayH5p");
        }
        
        public async Task ValidateH5p(JavaScriptAdapterTO javaScriptAdapterTo)
        {
            await CallJavaScriptInterop(javaScriptAdapterTo, "validateH5p");
        }

        private async Task CallJavaScriptInterop(JavaScriptAdapterTO javaScriptAdapterTo, string nameOfFunctionToCall)
        {
            try
            {
                var pathOfH5pToPlay = GeneratePathOfH5PToPlay(javaScriptAdapterTo);
                await _jsRuntime.InvokeVoidAsync(nameOfFunctionToCall, pathOfH5pToPlay);
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
        private static string GeneratePathOfH5PToPlay(JavaScriptAdapterTO javaScriptAdapterTo)
        {
            var nameOfH5pToPlay = Path.GetFileNameWithoutExtension(javaScriptAdapterTo.H5pZipSourcePath);
            var pathOfH5pToPlay = javaScriptAdapterTo.UnzippedH5psPath + nameOfH5pToPlay;

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
                await _jsRuntime.InvokeVoidAsync("terminateH5pStandalone");
            }
            catch (JSException jsEx)
            {
                Console.WriteLine("JavaScript error when calling displayH5p: " + jsEx.Message);

                // Logger.LogError("JSException: Could not call 'terminateH5pStandalone': {Message}", ex.Message);
            }
        }
        
        [JSInvokable]
        public Task ReceiveJsonData(string jsonData)
        {
            // if verb contains completed -> Dann ist H5P abschließbar
            try
            {
                // JSON parsen
                var root = JsonNode.Parse(jsonData);

                // `verb` extrahieren
                var verbPart = root?["data"]?["statement"]?["verb"];

                // Prüfung, ob die `verb`-ID "completed" enthält
                bool isCompleted = verbPart?["id"]?.GetValue<string>() == "http://adlnet.gov/expapi/verbs/completed";

                // Ergebnisse anzeigen
                Console.WriteLine("Verb-Teil:");
                Console.WriteLine(verbPart?.ToJsonString() ?? "Verb nicht gefunden");

                Console.WriteLine("\nPrüfungen:");
                Console.WriteLine($"Abgeschlossen (verb: completed): {isCompleted}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Verarbeiten des JSON: {ex.Message}");
            }
        }
        
        
        // [JSInvokable]
        // public static void ReceiveJsonData(string jsonData)
        // {
        //     // if verb contains completed -> Dann ist H5P abschließbar
        //     try
        //     {
        //         // JSON parsen
        //         var root = JsonNode.Parse(jsonData);
        //
        //         // `verb` extrahieren
        //         var verbPart = root?["data"]?["statement"]?["verb"];
        //
        //         // Prüfung, ob die `verb`-ID "completed" enthält
        //         bool isCompleted = verbPart?["id"]?.GetValue<string>() == "http://adlnet.gov/expapi/verbs/completed";
        //
        //         // Ergebnisse anzeigen
        //         Console.WriteLine("Verb-Teil:");
        //         Console.WriteLine(verbPart?.ToJsonString() ?? "Verb nicht gefunden");
        //
        //         Console.WriteLine("\nPrüfungen:");
        //         Console.WriteLine($"Abgeschlossen (verb: completed): {isCompleted}");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Fehler beim Verarbeiten des JSON: {ex.Message}");
        //     }
        // }
        

    
    }
}