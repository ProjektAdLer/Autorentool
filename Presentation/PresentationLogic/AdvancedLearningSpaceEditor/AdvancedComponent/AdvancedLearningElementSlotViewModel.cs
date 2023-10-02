namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

public class AdvancedLearningElementSlotViewModel : IAdvancedLearningElementSlotViewModel
{
    public AdvancedLearningElementSlotViewModel(Guid spaceId, int slotKey, double positionX, double positionY)
    {
        SpaceId = spaceId;
        SlotKey = slotKey;
        PositionX = positionX;
        PositionY = positionY;
    }
    
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string Identifier => SpaceId + SlotKey.ToString();
    public Guid SpaceId { get; }
    public int SlotKey { get; }
}