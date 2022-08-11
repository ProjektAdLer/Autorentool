namespace AuthoringTool.DataAccess.PersistEntities;

public interface ILearningElementPe
{
    string Name { get; set; }
    string Shortname { get; set; }
    LearningContentPe Content { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    LearningElementDifficultyEnumPe Difficulty { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
    string? ParentName { get; set; }
}