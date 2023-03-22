using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.TransferElement;

public class PdfTransferElementViewModel : TransferElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private PdfTransferElementViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfTransferElementViewModel"/> class.
    /// </summary>
    internal PdfTransferElementViewModel(string name, string shortname, ILearningSpaceViewModel? parent,
        ILearningContentViewModel learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, parent, learningContent, authors, description, goals, difficulty, workload, points, positionX,
        positionY)
    {
    }
}