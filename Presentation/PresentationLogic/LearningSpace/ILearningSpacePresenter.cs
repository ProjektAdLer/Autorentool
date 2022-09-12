using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter
{
    void EditLearningSpace(string name, string shortname, string authors, string description, string goals);
    bool EditLearningSpaceDialogOpen { get; set; }
    IDictionary<string, string>? EditLearningSpaceDialogInitialValues { get; }
    bool EditLearningElementDialogOpen { get; set; }
    IDictionary<string, string>? EditLearningElementDialogInitialValues { get; }
    bool CreateLearningElementDialogOpen { get; set; }
    ILearningSpaceViewModel? LearningSpaceVm { get; }
    void SetSelectedLearningObject(ILearningObjectViewModel learningObject);
    void DeleteSelectedLearningObject();
    void AddNewLearningElement();
    Task LoadLearningElementAsync();
    Task SaveSelectedLearningObjectAsync();
    void OnCreateElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void EditSelectedLearningObject();
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void SetLearningSpace(ILearningSpaceViewModel space);
    void CreateLearningElementWithPreloadedContent(LearningContentViewModel learningContent);
    LearningContentViewModel? DragAndDropLearningContent { get; }
}