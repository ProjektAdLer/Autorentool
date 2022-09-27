namespace BusinessLogic.Entities;

public interface ILearningSpace : ISpace, ILearningObject
{
    new string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    int RequiredPoints { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}