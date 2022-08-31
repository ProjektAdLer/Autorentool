using BusinessLogic.Entities;
using Presentation.PresentationLogic.LearningContent;

namespace Presentation.PresentationLogic.EntityMapping.LearningElementMapper;

public interface ILearningContentMapper
{
    public BusinessLogic.Entities.LearningContent ToEntity(LearningContentViewModel viewModel);
    public LearningContentViewModel ToViewModel(ILearningContent entity);
}