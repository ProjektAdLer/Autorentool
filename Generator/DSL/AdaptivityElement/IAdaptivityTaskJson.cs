using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

[JsonDerivedType(typeof(AdaptivityTaskJson), typeDiscriminator: JsonTypes.AdaptivityTaskType)]
public interface IAdaptivityTaskJson
{
    int TaskId { get; set; }
    string TaskUUID { get; set; }
    string TaskTitle { get; set; }
    bool Optional { get; set; }
    int RequiredDifficulty { get; set; }
    List<IAdaptivityQuestionJson> AdaptivityQuestions { get; set; }
}