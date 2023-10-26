namespace Generator.DSL.AdvancedLearningSpaceGenerator;

public class AdvancedCornerPointJson : IAdvancedCornerPointJson
{
    public AdvancedCornerPointJson(int cornerPointId, double positionX, double positionY)
    {
        CornerPointId = cornerPointId;
        PositionX = positionX;
        PositionY = positionY;
    }

    public int CornerPointId { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}