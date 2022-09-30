using System.ComponentModel;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter
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
    void OnCreateElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void EditSelectedLearningElement();
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void SetLearningSpace(ILearningSpaceViewModel space);
    void CreateLearningElementWithPreloadedContent(LearningContentViewModel learningContent);
    LearningContentViewModel? DragAndDropLearningContent { get; }
    void OnWorldPropertyChanged(object? caller, PropertyChangedEventArgs e);
    event Action OnUndoRedoPerformed;
}