using BusinessLogic.Entities;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.EntityMapping;

public interface ILearningWorldMapper
{
    public BusinessLogic.Entities.LearningWorld ToEntity(LearningWorldViewModel viewModel);
    public LearningWorldViewModel ToViewModel(ILearningWorld entity);
}