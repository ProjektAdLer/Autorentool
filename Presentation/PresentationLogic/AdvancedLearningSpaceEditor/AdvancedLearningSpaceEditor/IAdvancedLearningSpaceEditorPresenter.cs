using System.ComponentModel;
using Presentation.Components;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public interface IAdvancedLearningSpaceEditorPresenter: INotifyPropertyChanged, IAdvancedPositioningService
{
    IAdvancedComponentViewModel? SelectedAdvancedComponentViewModel { get; set; }
    IAdvancedLearningSpaceViewModel? AdvancedLearningSpaceViewModel { get; }
    void RightClickOnAdvancedComponent();
    void DeleteSelectedAdvancedComponent();
    void DragSelectedAdvancedComponent(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs);
    void SetSelectedAdvancedComponentViewModel(IAdvancedComponentViewModel obj);

    void SetAdvancedLearningSpace(AdvancedLearningSpaceViewModel advSpace);
    void CreateAdvancedLearningElementSlot(double positionX = 50, double positionY = 50);
}