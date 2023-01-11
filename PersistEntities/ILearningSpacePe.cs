namespace PersistEntities;

public interface ILearningSpacePe : ISpacePe, IObjectInPathWayPe
{
    string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    int RequiredPoints { get; set; }
    ILearningSpaceLayoutPe LearningSpaceLayout { get; set; }
}