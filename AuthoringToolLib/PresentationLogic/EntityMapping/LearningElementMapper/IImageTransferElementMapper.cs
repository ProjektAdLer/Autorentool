using AuthoringToolLib.PresentationLogic.LearningElement;

namespace AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;

public interface IImageTransferElementMapper
{
    public Entities.LearningElement ToEntity(ILearningElementViewModel viewModel);
    public ILearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null);
}