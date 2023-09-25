namespace Generator.DSL.AdaptivityElement;

public class AdaptivityContentJson : IAdaptivityContentJson
{
    public AdaptivityContentJson(string introText, bool shuffleTasks,
        List<IAdaptivityTaskJson> adaptivityTask)
    {
        IntroText = introText;
        ShuffleTasks = shuffleTasks;
        AdaptivityTask = adaptivityTask;
    }

    public string Type => JsonTypes.AdaptivityContentType;
    public string IntroText { get; set; }
    public bool ShuffleTasks { get; set; }
    public List<IAdaptivityTaskJson> AdaptivityTask { get; set; }
}