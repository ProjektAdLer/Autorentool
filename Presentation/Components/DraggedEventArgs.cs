namespace Presentation.Components;

public class DraggedEventArgs<T>
{
    public DraggedEventArgs(T draggableObject, double oldPositionX, double oldPositionY)
    {
        DraggableObject = draggableObject;
        OldPositionX = oldPositionX;
        OldPositionY = oldPositionY;
    }
    public T DraggableObject { get; }
    public double OldPositionX { get; }
    public double OldPositionY { get; }
    
    public delegate void DraggedEventHandler(object sender, DraggedEventArgs<T> e);
}