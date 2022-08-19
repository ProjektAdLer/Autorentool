namespace AuthoringToolLib.Entities;

public interface IOriginator
{
    IMemento GetMemento();
    void RestoreMemento(IMemento memento);
}