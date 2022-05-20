namespace AuthoringTool.DataAccess.DSL;

public class LearningWorldJson
{
    public IdentifierJson identifier { get; set; }
    public List<int> learningWorldContent { get; set; }
    public List<TopicJson> topics { get; set; }
    public List<LearningSpaceJson> learningSpaces { get; set; }
    public List<LearningElementJson> learningElements { get; set; }
}