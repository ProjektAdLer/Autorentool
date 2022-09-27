namespace BusinessLogic.Entities;

public interface ILearningElementParent
{
    public string Name { get; }
    List<LearningElement> LearningElements { get; set; }
    public IMemento GetMemento();
    public void RestoreMemento(IMemento memento);
    ILearningObject? SelectedLearningObject { get; set; }
}