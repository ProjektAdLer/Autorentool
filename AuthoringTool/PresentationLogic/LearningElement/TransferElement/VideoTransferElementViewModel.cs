using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement.TransferElement;

public class VideoTransferElementViewModel : TransferElementViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VideoTransferElementViewModel"/> class.
    /// </summary>
    public VideoTransferElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
        shortname, parent, learningContent, authors, description, goals, difficulty, workload, positionX, positionY)
    {
    }
}