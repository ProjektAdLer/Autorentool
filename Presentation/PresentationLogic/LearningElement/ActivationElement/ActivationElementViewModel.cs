using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.ActivationElement;

public abstract class ActivationElementViewModel : LearningElementViewModel
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    protected ActivationElementViewModel()
    {
    }

    protected ActivationElementViewModel(string name, string shortname, ILearningSpaceViewModel? parent,
        ILearningContentViewModel content, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, content, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}