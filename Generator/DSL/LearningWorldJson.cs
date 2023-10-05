using System.Text.Json.Serialization;

namespace Generator.DSL;

public class LearningWorldJson : ILearningWorldJson
{
    // the lmsElementIdentifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    [JsonConstructor]
    public LearningWorldJson(string worldName, string worldUuid, List<ITopicJson> topics,
        List<ILearningSpaceJson> spaces, List<IElementJson> elements, string? worldDescription = null,
        string[]? worldGoals = null, string? evaluationLink = null)
    {
        WorldName = worldName;
        WorldUUID = worldUuid;
        WorldDescription = worldDescription ?? "";
        WorldGoals = worldGoals ?? new[] { "" };
        EvaluationLink = evaluationLink ?? "";
        Topics = topics;
        Spaces = spaces;
        Elements = elements;
    }

    public string EvaluationLink { get; set; }

    public string WorldName { get; set; }

    public string WorldUUID { get; set; }

    public string WorldDescription { get; set; }

    public string[] WorldGoals { get; set; }

    // for the correct structure the topics are added to the learning World
    public List<ITopicJson> Topics { get; set; }

    // for the correct structure the Spaces are added to the learning World
    public List<ILearningSpaceJson> Spaces { get; set; }

    // for the correct structure the elements are added to the learning World
    public List<IElementJson> Elements { get; set; }
}