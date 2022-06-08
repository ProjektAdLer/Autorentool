namespace AuthoringTool.DataAccess.DSL;

public class LearningElementJson
{
    public int id { get; set; }
    public IdentifierJson identifier { get; set; }
    public string elementType { get; set; }
    public List<LearningElementValueJson> learningElementValue { get; set; }
    public List<RequirementJson> requirements { get; set; }
}