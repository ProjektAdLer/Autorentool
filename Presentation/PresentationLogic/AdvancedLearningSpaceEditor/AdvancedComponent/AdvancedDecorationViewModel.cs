namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

public class AdvancedDecorationViewModel : IAdvancedDecorationViewModel
{
    public AdvancedDecorationViewModel(Guid spaceId, int decorationKey, double positionX, double positionY)
    {
        SpaceId = spaceId;
        DecorationKey = decorationKey;
        PositionX = positionX;
        PositionY = positionY;
    }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string Identifier => SpaceId + DecorationKey.ToString();
    public Guid SpaceId { get; }
    public int DecorationKey { get; }
}