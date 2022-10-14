namespace Presentation.Components;

public class DraggedEventArgs<T>
{
    public DraggedEventArgs(T learningObject, double oldPositionX, double oldPositionY)
    {
        LearningObject = learningObject;
        OldPositionX = oldPositionX;
        OldPositionY = oldPositionY;
    }
    public T LearningObject { get; }
    public double OldPositionX { get; }
    public double OldPositionY { get; }
    
    public delegate void DraggedEventHandler(object sender, DraggedEventArgs<T> e);
}