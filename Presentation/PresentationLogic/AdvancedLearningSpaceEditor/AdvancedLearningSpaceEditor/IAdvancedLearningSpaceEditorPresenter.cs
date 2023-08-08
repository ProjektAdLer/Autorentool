using Presentation.Components;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public interface IAdvancedLearningSpaceEditorPresenter
{
    IAdvancedComponentViewModel? SelectedAdvancedComponentViewModel { get; set; }
    void RightClickOnAdvancedComponent();
    void DeleteSelectedAdvancedComponent();
    void DragSelectedAdvancedComponent(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs);
    void SetSelectedAdvancedComponentViewModel(IAdvancedComponentViewModel obj);
    
}