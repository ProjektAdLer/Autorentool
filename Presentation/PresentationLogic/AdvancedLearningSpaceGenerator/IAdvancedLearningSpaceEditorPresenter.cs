using Presentation.PresentationLogic.AdvancedComponent;
using Presentation.Components;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceGenerator;

public interface IAdvancedLearningSpaceEditorPresenter
{
    IAdvancedComponentViewModel SelectedViewModel { get; set; }
    void RightClickOnObject();
    void DeleteSelectedObject();
    void DragSelectedObject(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs);
    void SetSelectedViewModel(IAdvancedComponentViewModel obj);
    
}