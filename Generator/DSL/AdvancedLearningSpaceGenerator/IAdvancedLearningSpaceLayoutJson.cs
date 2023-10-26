using Shared;

namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public interface IAdvancedLearningSpaceLayoutJson
{
    List<AdvancedLearningElementSlotJson> AdvancedLearningElementSlots { get; set; }
    List<AdvancedDecorationJson> AdvancedDecorations { get; set; }
    List<AdvancedCornerPointJson> AdvancedCornerPoints { get; set; }
    Double EntryDoorPositionX { get; set; }
    Double EntryDoorPositionY { get; set; }
    Double ExitDoorPositionX { get; set; }
    Double ExitDoorPositionY { get; set; }
}