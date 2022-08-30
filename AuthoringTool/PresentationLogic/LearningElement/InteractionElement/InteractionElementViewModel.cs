using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement.InteractionElement;

public abstract class InteractionElementViewModel : LearningElementViewModel
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    protected InteractionElementViewModel() : base()
    {
    }

    protected InteractionElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel content, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
        shortname, content, authors, description, goals, difficulty, parent, workload, positionX, positionY)
    {
    }
}