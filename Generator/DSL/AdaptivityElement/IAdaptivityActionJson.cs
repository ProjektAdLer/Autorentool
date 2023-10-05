using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

[JsonDerivedType(typeof(CommentActionJson), typeDiscriminator: JsonTypes.CommentActionType)]
[JsonDerivedType(typeof(ElementReferenceActionJson), typeDiscriminator: JsonTypes.ElementReferenceActionType)]
[JsonDerivedType(typeof(ContentReferenceActionJson), typeDiscriminator: JsonTypes.ContentReferenceActionType)]
public interface IAdaptivityActionJson
{
}