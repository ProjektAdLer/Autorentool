using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter
{
    LearningSpaceViewModel CreateNewLearningSpace(string name, string shortname, string authors, string description,
        string goals);

    LearningSpaceViewModel EditLearningSpace(LearningSpaceViewModel space, string name, string shortname,
        string authors, string description, string goals);
    
    bool EditLearningSpaceDialogOpen { get; set; }
    IEnumerable<ModalDialogInputField> ModalDialogSpaceInputFields { get; }
    bool EditLearningElementDialogOpen { get; set; }
    IEnumerable<ModalDialogInputField> ModalDialogCreateElementInputFields { get; }
    IEnumerable<ModalDialogInputField> ModalDialogCreateElementCustomInputFields { get; }
    IEnumerable<ModalDialogInputField> ModalDialogEditElementInputFields { get; }
    bool CreateLearningElementDialogOpen { get; set; }
    LearningSpaceViewModel? LearningSpaceVm { get; }
    void SetSelectedLearningObject(ILearningObjectViewModel learningObject);
    void DeleteSelectedLearningObject();
    Task LoadLearningElement();
    Task SaveSelectedLearningObjectAsync();
    Task OnCreateElementDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple);
    void OpenEditSelectedLearningObjectDialog();
    Task OnEditSpaceDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple);
    Task OnEditElementDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple);
    void SetLearningSpace(LearningSpaceViewModel space);
    void CreateLearningElementWithPreloadedContent(LearningContentViewModel learningContent);
    bool DraggedLearningContentIsPresent { get; }
}