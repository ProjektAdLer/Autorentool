using System.Text.Json.Serialization;

namespace Generator.ATF;

[JsonDerivedType(typeof(DocumentRootJson), typeDiscriminator: JsonTypes.AtfType)]
public interface IDocumentRootJson
{
    ILearningWorldJson World { get; set; }
}