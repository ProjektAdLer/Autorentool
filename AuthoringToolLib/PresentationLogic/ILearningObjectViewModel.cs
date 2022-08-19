namespace AuthoringToolLib.PresentationLogic;

public interface ILearningObjectViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}