using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public class LearningWorldMapper : ILearningWorldMapper
{
    private readonly ILearningSpaceMapper _spaceMapper;
    private readonly ILearningElementMapper _elementMapper;

    public LearningWorldMapper(ILearningSpaceMapper spaceMapper, ILearningElementMapper elementMapper)
    {
        _spaceMapper = spaceMapper;
        _elementMapper = elementMapper;
    }
    
    public Entities.LearningWorld ToEntity(LearningWorldViewModel viewModel)
    {
        return new Entities.LearningWorld(viewModel.Name, viewModel.Shortname, viewModel.Authors, viewModel.Language,
            viewModel.Description, viewModel.Goals,
            viewModel.LearningElements.Select(element => _elementMapper.ToEntity(element)).ToList(),
            viewModel.LearningSpaces.Select(space => _spaceMapper.ToEntity(space)).ToList());
    }

    public LearningWorldViewModel ToViewModel(ILearningWorld entity)
    {
        return new LearningWorldViewModel(entity.Name, entity.Shortname, entity.Authors, entity.Language,
            entity.Description, entity.Goals,
            entity.LearningElements.Select(element => _elementMapper.ToViewModel(element)).ToList(),
            entity.LearningSpaces.Select(space => _spaceMapper.ToViewModel(space)).ToList());
    }
}