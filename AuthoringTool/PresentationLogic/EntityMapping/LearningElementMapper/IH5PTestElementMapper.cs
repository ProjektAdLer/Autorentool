using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;

public interface IH5PTestElementMapper
{
    public Entities.LearningElement ToEntity(LearningElementViewModel viewModel);
    public LearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null);
}