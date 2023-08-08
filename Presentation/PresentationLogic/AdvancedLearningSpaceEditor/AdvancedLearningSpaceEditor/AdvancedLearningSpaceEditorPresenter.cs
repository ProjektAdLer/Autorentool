using Presentation.Components;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public class AdvancedLearningSpaceEditorPresenter : IAdvancedLearningSpaceEditorPresenter, IAdvancedPositioningService
{
    public AdvancedLearningSpaceEditorPresenter()
    {
        
    }

    public IAdvancedLearningSpaceEditorViewModel? AdvancedLearningSpaceEditorViewModel;
    public IAdvancedComponentViewModel? SelectedAdvancedComponentViewModel { get; set; }

    public void DragSelectedAdvancedComponent(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs)
    {
        throw new NotImplementedException();
    }

    public void SetSelectedAdvancedComponentViewModel(IAdvancedComponentViewModel obj)
    {
        SelectedAdvancedComponentViewModel = obj;
    }

    public void DeleteSelectedAdvancedComponent()
    {

    }

    public void RightClickOnAdvancedComponent()
    {

    }

    public void CreateAdvancedComponent(IAdvancedComponentViewModel sourceObject, double x, double y)
    {
        throw new NotImplementedException();
    }

    public void DeleteAdvancedComponent(IAdvancedComponentViewModel targetObject)
    {
        throw new NotImplementedException();
    }

    public void SetOnHoveredAdvancedComponent(IAdvancedComponentViewModel sourceObject, double x, double y)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var objectAtPosition = GetObjectAtPosition(x, y);
        if (objectAtPosition == null || objectAtPosition == sourceObject)
        {
            LearningWorldVm.OnHoveredObjectInPathWay = null;
        }
        else
        {
            LearningWorldVm.OnHoveredObjectInPathWay = objectAtPosition;
            _logger.LogDebug("ObjectAtPosition: {0} ", sourceObject.Id);
        }
    }

    public IAdvancedComponentViewModel GetOnHoveredAdvancedComponent()
    {
        throw new NotImplementedException();
    }
}
