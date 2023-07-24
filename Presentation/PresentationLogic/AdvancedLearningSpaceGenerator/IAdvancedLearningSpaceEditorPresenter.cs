using Presentation.PresentationLogic.AdvancedComponent;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceGenerator;

public interface IAdvancedLearningSpaceEditorPresenter
{
    IAdvancedComponentViewModel SelectedViewModel { get; set; }
    void SetSelectedViewModel(IAdvancedComponentViewModel obj);
}