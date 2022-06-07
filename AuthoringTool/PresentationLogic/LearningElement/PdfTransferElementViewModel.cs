using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement;

public class PdfTransferElementViewModel : TransferElementViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PdfTransferElementViewModel"/> class.
    /// </summary>
    public PdfTransferElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel learningContent, string authors, string description, string goals, double positionX, double positionY) : base(name, shortname, parent,
        learningContent, authors, description, goals, positionX, positionY)
    {
    }
}