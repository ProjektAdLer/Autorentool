using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public interface ILearningContentMapper
{
    public Entities.LearningContent ToEntity(LearningContentViewModel viewModel);
    public LearningContentViewModel ToViewModel(Entities.ILearningContent entity);
}