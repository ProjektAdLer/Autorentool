namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

public class AdvancedLearningElementSlotViewModel : IAdvancedLearningElementSlotViewModel
{
    public AdvancedLearningElementSlotViewModel(Guid spaceId, int slotKey, double positionX, double positionY, int rotation = 0)
    {
        SpaceId = spaceId;
        SlotKey = slotKey;
        PositionX = positionX;
        PositionY = positionY;
        Rotation = rotation;
    }
    
    public double PositionX { get; set; }
    public double PositionY { get; set; }  
    public int Rotation { get; set; }
    public string Identifier => SpaceId + SlotKey.ToString();
    public Guid SpaceId { get; set; }
    public int SlotKey { get; }
}