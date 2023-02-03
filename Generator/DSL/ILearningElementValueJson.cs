namespace Generator.DSL;

public interface ILearningElementValueJson
{
    string Type { get; set; }
    
    // describes the amount of points or name of the badge
    string Value { get; set; }
}