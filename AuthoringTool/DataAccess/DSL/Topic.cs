namespace AuthoringTool.DataAccess.ReadEntities;

public class Topic
{
    public int topicId { get; set; }
    public string name { get; set; }
    public Identifier identifier {get; set; }
    public List<int> topicContent { get; set; }
    public List<Requirement> requirements { get; set; }
}