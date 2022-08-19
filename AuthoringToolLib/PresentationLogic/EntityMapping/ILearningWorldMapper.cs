using AuthoringToolLib.PresentationLogic.LearningWorld;

namespace AuthoringToolLib.PresentationLogic.EntityMapping;

public interface ILearningWorldMapper
{
    public Entities.LearningWorld ToEntity(LearningWorldViewModel viewModel);
    public LearningWorldViewModel ToViewModel(Entities.ILearningWorld entity);
}