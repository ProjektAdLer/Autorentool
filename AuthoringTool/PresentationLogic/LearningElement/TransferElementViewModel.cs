using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement;

public abstract class TransferElementViewModel : LearningElementViewModel
{ 
        protected TransferElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel content, string authors, string description, string goals, double positionX,
        double positionY) : base(name, shortname, parent, content, authors, description, goals, positionX, positionY)
        {
        }
}