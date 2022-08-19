namespace AuthoringTool.DataAccess.PersistEntities;

public interface ILearningSpacePe : Generator.PersistEntities.ISpacePe
{
    string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    List<Generator.PersistEntities.LearningElementPe> LearningElements { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}