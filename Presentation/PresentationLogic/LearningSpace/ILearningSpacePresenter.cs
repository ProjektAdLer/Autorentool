using System.ComponentModel;
using Presentation.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter : INotifyPropertyChanged
{
    void EditLearningSpace(string name, string shortname, string authors, string description, string goals, int requiredPoints);
    bool EditLearningSpaceDialogOpen { get; set; }
    IDictionary<string, string>? EditLearningSpaceDialogInitialValues { get; }
    bool EditLearningElementDialogOpen { get; set; }
    IDictionary<string, string>? EditLearningElementDialogInitialValues { get; }
    bool CreateLearningElementDialogOpen { get; set; }
    ILearningSpaceViewModel? LearningSpaceVm { get; }
    void SetSelectedLearningElement(ILearningElementViewModel learningElement);
    void DeleteSelectedLearningElement();
    void AddNewLearningElement();
    Task LoadLearningElementAsync();
    Task SaveSelectedLearningElementAsync();
    Task ShowSelectedElementContentAsync();
    void OnCreateElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void EditSelectedLearningElement();
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void SetLearningSpace(ILearningSpaceViewModel space);
    void CreateLearningElementWithPreloadedContent(LearningContentViewModel learningContent);
    LearningContentViewModel? DragAndDropLearningContent { get; }
    IDisplayableLearningObject? RightClickedLearningObject { get; }
    void OnWorldPropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
    void DragLearningElement(object sender, DraggedEventArgs<ILearningElementViewModel> draggedEventArgs);
    void ClickedLearningElement(ILearningElementViewModel obj);
    void RightClickedLearningElement(ILearningElementViewModel obj);
    void EditLearningElement(ILearningElementViewModel obj);
    void DeleteLearningElement(ILearningElementViewModel obj);
    void HideRightClickMenu();
    void ShowElementContent(ILearningElementViewModel obj);
}