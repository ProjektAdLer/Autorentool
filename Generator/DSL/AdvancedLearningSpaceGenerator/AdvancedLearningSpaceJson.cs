using System.Text.Json.Serialization;

namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public class AdvancedLearningSpaceJson : IAdvancedLearningSpaceJson
{
    // the id is incremented and is set for every Space
    [JsonConstructor]
    public AdvancedLearningSpaceJson(int spaceId, string spaceUuid, string spaceName,
        List<int?> spaceSlotContents, int requiredPointsToComplete, IAdvancedLearningSpaceLayoutJson advancedLearningSpaceLayout, string spaceTheme,
        string? spaceDescription = null, string[]? spaceGoals = null, string? requiredSpacesToEnter = null)
    {
        SpaceId = spaceId;
        SpaceUUID = spaceUuid;
        SpaceName = spaceName;
        SpaceDescription = spaceDescription ?? "";
        SpaceGoals = spaceGoals ?? new[] { "" };
        SpaceSlotContents = spaceSlotContents;
        RequiredPointsToComplete = requiredPointsToComplete;
        RequiredSpacesToEnter = requiredSpacesToEnter;
        AdvancedLearningSpaceLayout = advancedLearningSpaceLayout;
        SpaceTemplate = "advanced";
        SpaceTemplateStyle = spaceTheme;
    }

    public string[] SpaceGoals { get; set; }

    //A Name for the Learning Space
    public string SpaceName { get; set; }

    // Maximum Points and Points that are needed to complete the Space
    public int RequiredPointsToComplete { get; set; }

    public int SpaceId { get; set; }

    public string SpaceUUID { get; set; }

    //A Description for the Learning Space
    public string? SpaceDescription { get; set; }

    // A list that has all the id´s of the included elements of a space. 
    public List<int?> SpaceSlotContents { get; set; }

    // requirements are needed to describe the Path of the Spaces. 
    // It is a boolean algebra string, that describes which spaces are needed to complete the space.
    public string? RequiredSpacesToEnter { get; set; }
    public IAdvancedLearningSpaceLayoutJson AdvancedLearningSpaceLayout { get; set; }
    public string SpaceTemplateStyle { get; set; }
    public string SpaceTemplate { get; set; }
}