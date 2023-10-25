using System.Text.Json.Serialization;

namespace Generator.DSL;

[JsonDerivedType(typeof(AdvancedLearningSpaceJson), typeDiscriminator: JsonTypes.AdvancedLearningSpaceType)]
public interface IAdvancedLearningSpaceJson : ILearningSpaceJson
{
    object AdvancedLearningSpaceLayout { get; set; }
}