using BusinessLogic.Entities;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningElement.TransferElement;

namespace Presentation.PresentationLogic.EntityMapping.LearningElementMapper;

public class ImageTransferElementMapper : IImageTransferElementMapper
{
    private readonly ILearningContentMapper _contentMapper;

    public ImageTransferElementMapper(ILearningContentMapper contentMapper)
    {
        _contentMapper = contentMapper;
    }
    public BusinessLogic.Entities.LearningElement ToEntity(ILearningElementViewModel viewModel)
    {
        return new ImageTransferElement(viewModel.Name, viewModel.Shortname,
            viewModel.Parent?.Name, _contentMapper.ToEntity(viewModel.LearningContent),
            viewModel.Authors, viewModel.Description, viewModel.Goals, viewModel.Difficulty,
            viewModel.Workload, viewModel.PositionX, viewModel.PositionY);
    }

    public ILearningElementViewModel ToViewModel(BusinessLogic.Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null)
    {
        return new ImageTransferElementViewModel(entity.Name, entity.Shortname, caller,
            _contentMapper.ToViewModel(entity.Content), entity.Authors, entity.Description, entity.Goals,
            entity.Difficulty, entity.Workload, entity.PositionX, entity.PositionY);
    }
}