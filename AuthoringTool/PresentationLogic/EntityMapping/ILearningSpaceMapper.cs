using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public interface ILearningSpaceMapper
{
    public Entities.LearningSpace ToEntity(LearningSpaceViewModel viewModel);
    public LearningSpaceViewModel ToViewModel(Entities.ILearningSpace entity);
}