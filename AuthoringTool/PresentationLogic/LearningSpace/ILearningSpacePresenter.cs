using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter
{
    ILearningSpaceViewModel CreateNewLearningSpace(string name, string shortname, string authors, string description,
        string goals);

    ILearningSpaceViewModel EditLearningSpace(ILearningSpaceViewModel space, string name, string shortname,
        string authors, string description, string goals);
    

    bool EditLearningSpaceDialogOpen { get; set; }
    IDictionary<string, string> EditLearningSpaceDialogInitialValues { get; }
    bool EditLearningElementDialogOpen { get; set; }
    IDictionary<string, string> EditLearningElementDialogInitialValues { get; }
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