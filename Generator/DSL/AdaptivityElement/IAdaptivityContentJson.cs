using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

[JsonDerivedType(typeof(AdaptivityContentJson), typeDiscriminator: JsonTypes.AdaptivityContentType)]
public interface IAdaptivityContentJson
{
    string IntroText { get; set; }
    bool ShuffleTasks { get; set; }
    List<IAdaptivityTaskJson> AdaptivityTasks { get; set; }
}