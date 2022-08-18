namespace Generator.DSL;

/// <summary>
/// Includes the points or badge an element has
/// </summary>
public class LearningElementValueJson : ILearningElementValueJson
{
    // describes if the type are points or badge
    public LearningElementValueJson(string type, string value)
    {
        Type = type;
        Value = value;
    }

    public string Type { get; set; }
    
    // describes the amount of points or name of the badge
    public string Value { get; set; }
}