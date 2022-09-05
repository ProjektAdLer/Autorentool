namespace BusinessLogic.Entities;

public interface ILearningObject
{
    public string Name { get; }
    public string Description { get; }
    public double PositionX { get; }
    public double PositionY { get; }
}