using Shared;

namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public class AdvancedLearningElementSlotJson : IAdvancedLearningElementSlotJson
{
    public AdvancedLearningElementSlotJson(int slotId, double positionX, double positionY, int rotation)
    {
        SlotId = slotId;
        PositionX = positionX;
        PositionY = positionY;
        Rotation = rotation;
    }

    public int SlotId { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public int Rotation { get; set; }
}