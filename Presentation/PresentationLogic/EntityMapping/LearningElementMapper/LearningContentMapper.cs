using BusinessLogic.Entities;
using Presentation.PresentationLogic.LearningContent;

namespace Presentation.PresentationLogic.EntityMapping.LearningElementMapper;

public class LearningContentMapper : ILearningContentMapper
{
    public BusinessLogic.Entities.LearningContent ToEntity(LearningContentViewModel viewModel)
    {
        return new BusinessLogic.Entities.LearningContent(viewModel.Name, viewModel.Type, viewModel.Content);
    }

    public LearningContentViewModel ToViewModel(ILearningContent entity)
    {
        return new LearningContentViewModel(entity.Name, entity.Type, entity.Content);
    }
}