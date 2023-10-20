using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

public class AdaptivityContentJson : IAdaptivityContentJson
{
    [JsonConstructor]
    public AdaptivityContentJson(string introText, bool shuffleTasks,
        List<IAdaptivityTaskJson> adaptivityTasks)
    {
        IntroText = introText;
        ShuffleTasks = shuffleTasks;
        AdaptivityTasks = adaptivityTasks;
    }

    public string IntroText { get; set; }
    public bool ShuffleTasks { get; set; }
    public List<IAdaptivityTaskJson> AdaptivityTasks { get; set; }
}