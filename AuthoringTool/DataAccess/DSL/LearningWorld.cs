namespace AuthoringTool.DataAccess.ReadEntities;

public class LearningWorld
{
    public Identifier identifier { get; set; }
    public List<int> learningWorldContent { get; set; }
    public List<Topic> topics { get; set; }
    public List<LearningSpace> learningSpaces { get; set; }
    public List<LearningElement> learningElements { get; set; }
}