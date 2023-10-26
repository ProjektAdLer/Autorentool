namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public interface IAdvancedLearningElementSlotJson
{
    int SlotId { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
    int Rotation { get; set; }
}