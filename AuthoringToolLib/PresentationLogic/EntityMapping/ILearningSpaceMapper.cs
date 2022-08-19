using AuthoringToolLib.PresentationLogic.LearningSpace;

namespace AuthoringToolLib.PresentationLogic.EntityMapping;

public interface ILearningSpaceMapper
{
    public Entities.LearningSpace ToEntity(ILearningSpaceViewModel viewModel);
    public ILearningSpaceViewModel ToViewModel(Entities.ILearningSpace entity);
}