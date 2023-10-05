using System.Text.Json.Serialization;

namespace Generator.DSL;

[JsonDerivedType(typeof(DocumentRootJson), typeDiscriminator: JsonTypes.AtfType)]
public interface IDocumentRootJson
{
    ILearningWorldJson World { get; set; }
}