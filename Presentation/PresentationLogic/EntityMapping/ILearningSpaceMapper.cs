using BusinessLogic.Entities;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.EntityMapping;

public interface ILearningSpaceMapper
{
    public BusinessLogic.Entities.LearningSpace ToEntity(ILearningSpaceViewModel viewModel);
    public ILearningSpaceViewModel ToViewModel(ILearningSpace entity);
}