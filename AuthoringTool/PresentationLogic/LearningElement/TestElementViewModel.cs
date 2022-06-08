using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement;

public abstract class TestElementViewModel : LearningElementViewModel
{ 
    protected TestElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel content, string authors, string description, string goals, int workload,
        double positionX, double positionY) : base(name, shortname, parent, content, authors, description, goals,
        workload, positionX, positionY)
    {
    }
}