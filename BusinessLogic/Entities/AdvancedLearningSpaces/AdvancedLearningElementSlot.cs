namespace BusinessLogic.Entities.AdvancedLearningSpaces;

public class AdvancedLearningElementSlot : IAdvancedLearningElementSlot
{
    public double PositionX { get; set; }
    public double PositionY { get; set; }

    public IMemento GetMemento()
    {
        return new AdvancedLearningElementSlotMemento( PositionX, PositionY);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AdvancedLearningElementSlotMemento advancedLearningElementSlotMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }   

        PositionX = advancedLearningElementSlotMemento.PositionX;
        PositionY = advancedLearningElementSlotMemento.PositionY;
    }

    private record AdvancedLearningElementSlotMemento : IMemento
    {
        internal AdvancedLearningElementSlotMemento( double positionX, double positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        internal double PositionX { get; }
        internal double PositionY { get; }
    }
}