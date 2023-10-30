using System.Text.Json.Serialization;

namespace Generator.ATF.AdaptivityElement;

public class AdaptivityContentJson : IAdaptivityContentJson
{
    [JsonConstructor]
    public AdaptivityContentJson(string introText, List<IAdaptivityTaskJson> adaptivityTasks)
    {
        IntroText = introText;
        AdaptivityTasks = adaptivityTasks;
    }

    public string IntroText { get; set; }
    public List<IAdaptivityTaskJson> AdaptivityTasks { get; set; }
}