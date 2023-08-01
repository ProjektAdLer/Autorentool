using Presentation.Components;
using Presentation.PresentationLogic.AdvancedComponent;
using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceGenerator;

public class AdvancedLearningSpaceEditorPresenter : IAdvancedLearningSpaceEditorPresenter
{
    public IAdvancedComponentViewModel SelectedViewModel { get; set; }
    public void DragSelectedObject(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs)
    {
        throw new NotImplementedException();
    }

    public void SetSelectedViewModel(IAdvancedComponentViewModel obj)
    {
        SelectedViewModel = obj;
    }

    public void DeleteSelectedObject()
    {
        
    }

    public void RightClickOnObject()
    {
        
    }
}