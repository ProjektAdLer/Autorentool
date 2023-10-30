using System.Text.Json.Serialization;

namespace Generator.ATF.AdaptivityElement;

[JsonDerivedType(typeof(AdaptivityContentJson), typeDiscriminator: JsonTypes.AdaptivityContentType)]
public interface IAdaptivityContentJson
{
    string IntroText { get; set; }
    List<IAdaptivityTaskJson> AdaptivityTasks { get; set; }
}