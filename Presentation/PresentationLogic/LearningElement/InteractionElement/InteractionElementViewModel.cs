using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.InteractionElement;

public abstract class InteractionElementViewModel : LearningElementViewModel
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    protected InteractionElementViewModel()
    {
    }

    protected InteractionElementViewModel(string name, string shortname, ILearningSpaceViewModel? parent,
        ILearningContentViewModel content, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, content, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}