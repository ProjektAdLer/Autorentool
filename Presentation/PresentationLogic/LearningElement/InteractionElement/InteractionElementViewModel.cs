using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.InteractionElement;

public abstract class InteractionElementViewModel : LearningElementViewModel
{ 
    protected InteractionElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel content, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
        shortname, parent, content, authors, description, goals, difficulty, workload, positionX, positionY)
    {
    }
}