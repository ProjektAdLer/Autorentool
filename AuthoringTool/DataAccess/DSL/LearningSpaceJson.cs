namespace AuthoringTool.DataAccess.DSL;

public class LearningSpaceJson
{
    public int spaceId { get; set; }
    public string learningSpaceName { get; set; }
    public IdentifierJson identifier { get; set; }
    public List<int> learningSpaceContent { get; set; }
    public List<RequirementJson> requirements { get; set; }
}