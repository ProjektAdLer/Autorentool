using System.Text.Json.Nodes;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.Api.JavaScript;

public class ReceiveFromJavaScriptAdapter
{
    
    
    
    public static IValidateH5pUc VaidateH5pUc { get; set; }


    [JSInvokable]
    public static void ReceiveJsonData(string jsonData)
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
}