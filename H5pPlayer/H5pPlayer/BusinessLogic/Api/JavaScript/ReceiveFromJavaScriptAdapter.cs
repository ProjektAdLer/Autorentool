using System.Text.Json.Nodes;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using Microsoft.JSInterop;

namespace H5pPlayer.BusinessLogic.Api.JavaScript;

public class ReceiveFromJavaScriptAdapter
{
    
    public static IValidateH5pUc ValidateH5pUc { get; set; }


    /// <summary>
    /// Ausführlicher Testen:
    /// - gibt es die json-member überhaupt usw
    ///
    /// todo nochmal durchspielen, da kein integrationstest aktuell
    /// </summary>
    /// <param name="jsonData"></param>
    [JSInvokable]
    public static void ReceiveJsonData(string jsonData)
    {
        try
        {
            var root = JsonNode.Parse(jsonData);
            var statement = root?["data"]?["statement"];
            var verbId = statement?["verb"]?["id"]?.GetValue<string>();


            ValidateReceivedJsonData(verbId, statement);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing JSON data: {ex.Message}");
        }

    }

    private static void ValidateReceivedJsonData(string? verbId, JsonNode? statement)
    {
        var validateH5pTO = new ValidateH5pTO(IsCompleted(verbId, statement));
        ValidateH5pUc.ValidateH5p(validateH5pTO);
    }

    private static bool IsCompleted(string? verbId, JsonNode? statement)
    {
        return IsAnsweredOrCompleted(verbId) && IsNotChild(statement);
    }
    private static bool IsAnsweredOrCompleted(string? verbId)
    {
        return verbId == "http://adlnet.gov/expapi/verbs/answered" ||
               verbId == "http://adlnet.gov/expapi/verbs/completed";
    }
    
    private static bool IsNotChild(JsonNode? statement)
    {
       return !CheckIfStatementHasParentContext(statement);
     
    }

    private static bool CheckIfStatementHasParentContext(JsonNode? statement)
    {
        var isChild = statement?["context"]?["contextActivities"]?["parent"]?[0]?["id"] != null;
        return isChild;
    }
  
}