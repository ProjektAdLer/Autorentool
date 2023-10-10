using BusinessLogic.Validation;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.LearningElement;

public class LearningElementNamesProvider : ILearningElementNamesProvider
{
    public LearningElementNamesProvider(ILearningWorldPresenter learningWorldPresenter)
    {
        LearningWorldPresenter = learningWorldPresenter;
    }

    private ILearningWorldPresenter LearningWorldPresenter { get; }

    public IEnumerable<(Guid, string)> ElementNames => GetLearningElements().Select(el => (el.Id, el.Name));

    private IEnumerable<ILearningElementViewModel> GetLearningElements()
    {
        return LearningWorldPresenter.LearningWorldVm?.AllLearningElements ??
               Enumerable.Empty<ILearningElementViewModel>();
    }
}