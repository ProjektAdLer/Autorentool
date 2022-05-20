namespace AuthoringTool.DataAccess.DSL;

public class TopicJson
{
    public int topicId { get; set; }
    public string name { get; set; }
    public IdentifierJson identifier {get; set; }
    public List<int> topicContent { get; set; }
    public List<RequirementJson> requirements { get; set; }
}