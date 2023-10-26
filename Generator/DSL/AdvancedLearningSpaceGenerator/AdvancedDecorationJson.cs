namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public class AdvancedDecorationJson : IAdvancedDecorationJson
{
    public AdvancedDecorationJson(int decorationId, double positionX, double positionY, int rotation)
    {
        DecorationId = decorationId;
        PositionX = positionX;
        PositionY = positionY;
        Rotation = rotation;
    }

    public int DecorationId { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public int Rotation { get; set; }
}