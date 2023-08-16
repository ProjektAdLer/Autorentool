using System.ComponentModel;
using Presentation.Components;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public interface IAdvancedLearningSpaceEditorPresenter: INotifyPropertyChanged
{
    IAdvancedComponentViewModel? SelectedAdvancedComponentViewModel { get; set; }
    void RightClickOnAdvancedComponent();
    void DeleteSelectedAdvancedComponent();
    void DragSelectedAdvancedComponent(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs);
    void SetSelectedAdvancedComponentViewModel(IAdvancedComponentViewModel obj);

    void SetAdvancedLearningSpace(AdvancedLearningSpaceViewModel advSpace);
}