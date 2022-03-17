namespace AuthoringTool.PresentationLogic.LearningElement;

public class LearningElementViewModel : ILearningObjectViewModel
{
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}