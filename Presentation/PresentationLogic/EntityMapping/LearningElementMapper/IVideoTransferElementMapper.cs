using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.EntityMapping.LearningElementMapper;

public interface IVideoTransferElementMapper
{
    public BusinessLogic.Entities.LearningElement ToEntity(ILearningElementViewModel viewModel);
    public ILearningElementViewModel ToViewModel(BusinessLogic.Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null);
}