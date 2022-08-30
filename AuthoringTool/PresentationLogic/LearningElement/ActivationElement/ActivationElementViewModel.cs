using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement.ActivationElement;

public abstract class ActivationElementViewModel : LearningElementViewModel
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    protected ActivationElementViewModel() : base()
    {
    }

    protected ActivationElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel content, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
        shortname, content, authors, description, goals, difficulty, parent, workload, positionX, positionY)
    {
    }
}