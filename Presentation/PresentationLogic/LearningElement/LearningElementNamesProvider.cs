using BusinessLogic.Validation;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.LearningElement;

public class LearningElementNamesProvider : ILearningElementNamesProvider
{
    public LearningElementNamesProvider(ILearningSpacePresenter learningSpacePresenter, ILearningWorldPresenter learningWorldPresenter)
    {
        LearningSpacePresenter = learningSpacePresenter;
        LearningWorldPresenter = learningWorldPresenter;
    }
    
    private ILearningSpacePresenter LearningSpacePresenter { get; set; }
    private ILearningWorldPresenter LearningWorldPresenter { get; set; }
    
    private IEnumerable<ILearningElementViewModel> GetLearningElements()
    {
        IEnumerable<ILearningElementViewModel> learningElements = new List<ILearningElementViewModel>();
        var unplacedElements = LearningWorldPresenter.LearningWorldVm?.UnplacedLearningElements;
        var placedElements = LearningSpacePresenter.LearningSpaceVm?.LearningSpaceLayout.ContainedLearningElements;
        if (unplacedElements != null)
        {
            learningElements = learningElements.Union(unplacedElements);
        }
        if (placedElements != null)
        {
            learningElements = learningElements.Union(placedElements);
        }

        return learningElements;
    }

    public IEnumerable<(Guid, string)> ElementNames => GetLearningElements().Select(el => (el.Id, el.Name));
}