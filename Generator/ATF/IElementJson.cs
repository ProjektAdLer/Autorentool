using System.Text.Json.Serialization;
using Generator.ATF.AdaptivityElement;

namespace Generator.ATF;

[JsonDerivedType(typeof(LearningElementJson), typeDiscriminator: JsonTypes.LearningElementType)]
[JsonDerivedType(typeof(AdaptivityElementJson), typeDiscriminator: JsonTypes.AdaptivityElementType)]
[JsonDerivedType(typeof(BaseLearningElementJson), typeDiscriminator: JsonTypes.BaseLearningElementType)]
public interface IElementJson
{
    int ElementId { get; set; }
    string ElementUUID { get; set; }
    string ElementName { get; set; }
    string ElementCategory { get; set; }
    string ElementFileType { get; set; }
}