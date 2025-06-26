using System.Text.Json.Serialization;

namespace Generator.ATF;

[JsonDerivedType(typeof(LearningWorldJson), typeDiscriminator: JsonTypes.LearningWorldType)]
public interface ILearningWorldJson
{
    string WorldName { get; set; }
    // ReSharper disable once InconsistentNaming
    string WorldUUID { get; set; }

    string WorldDescription { get; set; }

    string[] WorldGoals { get; set; }

    // for the correct structure the topics are added to the learning World
    List<ITopicJson> Topics { get; set; }

    // for the correct structure the Spaces are added to the learning World
    List<ILearningSpaceJson> Spaces { get; set; }

    // for the correct structure the elements are added to the learning World
    List<IElementJson> Elements { get; set; }
    string EnrolmentKey { get; set; }
    
    string Theme { get; set; }
    IFrameStoryJson FrameStory { get; set; }
}