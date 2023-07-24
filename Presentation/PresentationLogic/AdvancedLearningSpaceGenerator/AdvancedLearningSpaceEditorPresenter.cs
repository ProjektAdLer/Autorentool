using Presentation.PresentationLogic.AdvancedComponent;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceGenerator;

public class AdvancedLearningSpaceEditorPresenter : IAdvancedLearningSpaceEditorPresenter
{
    public IAdvancedComponentViewModel SelectedViewModel { get; set; }
    public void SetSelectedViewModel(IAdvancedComponentViewModel obj)
    {
        SelectedViewModel = obj;
    }
}