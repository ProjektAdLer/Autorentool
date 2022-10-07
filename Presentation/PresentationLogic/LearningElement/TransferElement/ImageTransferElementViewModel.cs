using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.TransferElement;

public class ImageTransferElementViewModel : TransferElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private ImageTransferElementViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageTransferElementViewModel"/> class.
    /// </summary>
    public ImageTransferElementViewModel(string name, string shortname, ILearningSpaceViewModel? parent,
        LearningContentViewModel learningContent, string? url, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, parent, learningContent, url, authors, description, goals, difficulty, workload, points, positionX,
        positionY)
    {
    }

}