using System.Text.Json.Serialization;

namespace Generator.ATF;

[JsonDerivedType(typeof(TopicJson), typeDiscriminator: JsonTypes.TopicType)]
public interface ITopicJson
{
    int TopicId { get; set; }
    string TopicName { get; set; }
    List<int> TopicContents { get; set; }
}