namespace AuthoringTool.PresentationLogic;

public interface ILearningObjectViewModel
{
    string Name { get; set; }
    string Shortname { get; set; }
    string Content { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}