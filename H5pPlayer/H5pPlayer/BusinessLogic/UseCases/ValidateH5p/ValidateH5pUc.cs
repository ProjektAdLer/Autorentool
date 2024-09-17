using System.Text.Json;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public class ValidateH5pUc
{
    // muss nach validate:
    [JSInvokable]
    public static void ReceiveJsonData22222(string jsonData)
    {
        // JSON-Daten deserialisieren
        var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);

        Console.WriteLine($"Received JSON data from JavaScript: {jsonObject}");
        Console.WriteLine($"Received JSON data from JavaScript: {jsonData}");

        // Hier kannst du die JSON-Daten weiterverarbeiten
    }
}