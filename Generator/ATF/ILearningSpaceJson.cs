using System.Text.Json.Serialization;

namespace Generator.ATF;

[JsonDerivedType(typeof(LearningSpaceJson), typeDiscriminator: JsonTypes.LearningSpaceType)]
public interface ILearningSpaceJson
{
    int SpaceId { get; set; }

    string SpaceUUID { get; set; }

    string? SpaceDescription { get; set; }

    // A list that has all the id´s of the included elements of a space. 
    List<int?> SpaceSlotContents { get; set; }

    // requirements are needed to describe the Path of the Topic, Space and element. 
    // It is a boolean algebra string, that describes which spaces are needed to complete the space.
    string? RequiredSpacesToEnter { get; set; }

    string SpaceTemplate { get; set; }

    string SpaceTemplateStyle { get; set; }
    string SpaceName { get; set; }
    int RequiredPointsToComplete { get; set; }
    ISpaceStoryJson SpaceStory { get; set; }
}