namespace BusinessLogic.Entities;

public interface ISpace
{
    Guid Id { get; }
    string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    int RequiredPoints { get; set; }
    ISpaceLayout SpaceLayout { get; set; }
    IEnumerable<IElement> ContainedElements => SpaceLayout.ContainedElements;
}