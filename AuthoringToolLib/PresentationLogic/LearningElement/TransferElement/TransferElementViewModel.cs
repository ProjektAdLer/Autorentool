using AuthoringToolLib.PresentationLogic.LearningContent;

namespace AuthoringToolLib.PresentationLogic.LearningElement.TransferElement;

public abstract class TransferElementViewModel : LearningElementViewModel
{ 
        protected TransferElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel content, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
                shortname, parent, content, authors, description, goals, difficulty, workload, positionX, positionY)
        {
        }
}