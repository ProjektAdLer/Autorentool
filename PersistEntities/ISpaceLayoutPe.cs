namespace PersistEntities;

public interface ISpaceLayoutPe
{
    FloorPlanEnumPe FloorPlanName { get; set; }
    IElementPe?[] Elements { get; set; }
    IEnumerable<ElementPe> ContainedElements { get; }
}