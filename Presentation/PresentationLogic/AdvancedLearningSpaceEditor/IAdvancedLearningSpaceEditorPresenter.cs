using Presentation.Components;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor;

public interface IAdvancedLearningSpaceEditorPresenter
{
    IAdvancedComponentViewModel? SelectedAdvancedComponentViewModel { get; set; }
    void RightClickOnAdvancedComponent();
    void DeleteSelectedAdvancedComponent();
    void DragSelectedAdvancedComponent(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs);
    void SetSelectedAdvancedComponentViewModel(IAdvancedComponentViewModel obj);
    
}