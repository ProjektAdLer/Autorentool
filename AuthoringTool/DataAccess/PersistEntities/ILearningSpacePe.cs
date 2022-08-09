namespace AuthoringTool.DataAccess.PersistEntities;

public interface ILearningSpace : ISpace
{
    string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    List<LearningElementPe> LearningElements { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}