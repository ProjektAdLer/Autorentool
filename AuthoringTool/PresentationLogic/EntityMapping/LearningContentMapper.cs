using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public class LearningContentMapper : ILearningContentMapper
{
    public Entities.LearningContent ToEntity(LearningContentViewModel viewModel)
    {
        return new Entities.LearningContent(viewModel.Name, viewModel.Type, viewModel.Content);
    }

    public LearningContentViewModel ToViewModel(Entities.ILearningContent entity)
    {
        return new LearningContentViewModel(entity.Name, entity.Type, entity.Content);
    }
}