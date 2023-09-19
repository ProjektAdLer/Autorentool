namespace Generator.DSL.AdaptivityElement;

public class AdaptivityContentJson : IAdaptivityContentJson
{
    public AdaptivityContentJson(string type, string introText, bool shuffleTasks,
        List<IAdaptivityTaskJson> adaptivityTask)
    {
        Type = type;
        IntroText = introText;
        ShuffleTasks = shuffleTasks;
        AdaptivityTask = adaptivityTask;
    }

    public string Type { get; }
    public string IntroText { get; set; }
    public bool ShuffleTasks { get; set; }
    public List<IAdaptivityTaskJson> AdaptivityTask { get; set; }
}