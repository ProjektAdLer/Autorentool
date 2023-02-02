namespace PersistEntities;

public interface ISpacePe : IObjectInPathWayPe
{
    string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    int RequiredPoints { get; set; }
    ISpaceLayoutPe SpaceLayout { get; set; }
}