namespace AuthoringTool.DataAccess.ReadEntities;

public class LearningSpace
{
    public int spaceId { get; set; }
    public string learningSpaceName { get; set; }
    public Identifier identifier { get; set; }
    public List<int> learningSpaceContent { get; set; }
    public List<Requirement> requirements { get; set; }
}