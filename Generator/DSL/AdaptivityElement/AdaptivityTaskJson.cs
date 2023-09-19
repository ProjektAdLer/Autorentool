namespace Generator.DSL.AdaptivityElement;

public class AdaptivityTaskJson : IAdaptivityTaskJson
{
    public AdaptivityTaskJson(string type, int taskId, string taskUuid, string taskTitle, bool optional,
        int requiredDifficulty, List<IAdaptivityQuestionJson> adaptivityQuestions)
    {
        Type = type;
        TaskId = taskId;
        TaskUUID = taskUuid;
        TaskTitle = taskTitle;
        Optional = optional;
        RequiredDifficulty = requiredDifficulty;
        AdaptivityQuestions = adaptivityQuestions;
    }

    public string Type { get; }
    public int TaskId { get; set; }
    public string TaskUUID { get; set; }
    public string TaskTitle { get; set; }
    public bool Optional { get; set; }
    public int RequiredDifficulty { get; set; }
    public List<IAdaptivityQuestionJson> AdaptivityQuestions { get; set; }
}