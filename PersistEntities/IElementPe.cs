

namespace PersistEntities;

public interface IElementPe
{
    string Name { get; set; }
    string Shortname { get; set; }
    ContentPe Content { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    ElementDifficultyEnumPe Difficulty { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}