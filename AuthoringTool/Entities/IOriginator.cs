namespace AuthoringTool.Entities;

public interface IOriginator
{
    IMemento GetMemento();
    void RestoreMemento(IMemento memento);
}