namespace AuthoringTool.Entities;

public interface ILearningElement
{
    string Name { get; set; }
    string Shortname { get; set; }
    string ElementType { get; set; }
    string ContentType { get; set; }
    LearningContent Content { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
    string? ParentName { get; set; }
}