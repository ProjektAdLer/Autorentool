using System.ComponentModel;
using Presentation.Components;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Shared;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter : INotifyPropertyChanged
{
    void EditLearningSpace(string name, string shortname, string authors, string description, string goals, int requiredPoints);
    ILearningSpaceViewModel? LearningSpaceVm { get; }
    void SetSelectedLearningElement(ILearningElementViewModel learningElement);
    void DeleteSelectedLearningElement();
    Task LoadLearningElementAsync(int slotIndex);
    Task SaveSelectedLearningElementAsync();
    Task ShowSelectedElementContentAsync();
    void SetLearningSpace(ILearningSpaceViewModel space);
    LearningContentViewModel? DragAndDropLearningContent { get; }
    IDisplayableLearningObject? RightClickedLearningObject { get; }
    void OnWorldPropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragLearningElement(object sender, DraggedEventArgs<ILearningElementViewModel> draggedEventArgs);
    void ClickedLearningElement(ILearningElementViewModel obj);
    void RightClickedLearningElement(ILearningElementViewModel obj);
    void EditLearningElement(ILearningElementViewModel obj);
    void EditLearningElement(int slotIndex);
    void DeleteLearningElement(ILearningElementViewModel obj);
    void HideRightClickMenu();
    void ShowElementContent(ILearningElementViewModel obj);
    void SetLearningSpaceLayout(FloorPlanEnum floorPlanName);
}