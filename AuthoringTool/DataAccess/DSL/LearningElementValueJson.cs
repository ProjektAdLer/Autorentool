namespace AuthoringTool.DataAccess.DSL;

/// <summary>
/// Includes the points or badge an element has
/// </summary>
public class LearningElementValueJson : ILearningElementValueJson
{
    // describes if the type are points or badge
    public string? type { get; set; }
    
    // describes the amount of points or name of the badge
    public string? value { get; set; }
}