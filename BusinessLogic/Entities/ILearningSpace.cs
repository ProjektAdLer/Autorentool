namespace BusinessLogic.Entities;

public interface ILearningSpace : ISpace
{
    Guid Id { get; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}