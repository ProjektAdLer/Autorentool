namespace AuthoringTool.DataAccess.ReadEntities;

public class LearningElement
{
    public int id { get; set; }
    public Identifier identifier { get; set; }
    public string elementType { get; set; }
    public List<LearningElementValue> learningElementValue { get; set; }
    public List<Requirement> requirements { get; set; }
}