using System.ComponentModel;
using Presentation.Components;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public interface IAdvancedLearningSpaceEditorPresenter: INotifyPropertyChanged
{
    IAdvancedComponentViewModel? SelectedAdvancedComponentViewModel { get; set; }
    IAdvancedLearningSpaceViewModel? AdvancedLearningSpaceViewModel { get; }
    void SetSelectedAdvancedComponentViewModel(IAdvancedComponentViewModel obj);
    void DragSelectedAdvancedComponent(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs);
    void DeleteAdvancedDecoration(object advancedDecorationViewModel);
    void RightClickOnAdvancedComponent();
    void HideRightClickMenu();
    void SetAdvancedLearningSpace(AdvancedLearningSpaceViewModel advSpace);
    void CreateAdvancedLearningElementSlot(double positionX = 50, double positionY = 50);
    void CreateAdvancedDecoration(double positionX = 50, double positionY = 50);
}