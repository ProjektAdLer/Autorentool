using AuthoringToolLib.PresentationLogic.LearningElement;
using AuthoringToolLib.PresentationLogic.LearningElement.TestElement;

namespace AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;

public class H5PTestElementMapper : IH5PTestElementMapper
{
    private readonly ILearningContentMapper _contentMapper;

    public H5PTestElementMapper(ILearningContentMapper contentMapper)
    {
        _contentMapper = contentMapper;
    }
    public Entities.LearningElement ToEntity(ILearningElementViewModel viewModel)
    {
        return new Entities.H5PTestElement(viewModel.Name, viewModel.Shortname,
            viewModel.Parent?.Name, _contentMapper.ToEntity(viewModel.LearningContent),
            viewModel.Authors, viewModel.Description, viewModel.Goals, viewModel.Difficulty,
            viewModel.Workload, viewModel.PositionX, viewModel.PositionY);
    }

    public ILearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null)
    {
        return new H5PTestElementViewModel(entity.Name, entity.Shortname, caller,
            _contentMapper.ToViewModel(entity.Content), entity.Authors, entity.Description, entity.Goals,
            entity.Difficulty, entity.Workload, entity.PositionX, entity.PositionY);
    }
}