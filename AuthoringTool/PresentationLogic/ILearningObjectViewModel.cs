namespace AuthoringTool.PresentationLogic;

public interface ILearningObjectViewModel
{
    string Name { get; set; }
    string Description { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}