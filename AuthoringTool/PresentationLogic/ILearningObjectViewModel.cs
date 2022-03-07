namespace AuthoringTool.PresentationLogic;

public interface ILearningObjectViewModel
{
    string Name { get; set; }
    string Description { get; set; }
    int PositionX { get; set; }
    int PositionY { get; set; }
}
