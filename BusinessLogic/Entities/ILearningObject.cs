namespace BusinessLogic.Entities;

public interface ILearningObject
{
    Guid Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}