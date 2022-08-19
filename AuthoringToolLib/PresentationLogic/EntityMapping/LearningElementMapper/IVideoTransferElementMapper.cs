using AuthoringToolLib.PresentationLogic.LearningElement;

namespace AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;

public interface IVideoTransferElementMapper
{
    public Entities.LearningElement ToEntity(ILearningElementViewModel viewModel);
    public ILearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null);
}