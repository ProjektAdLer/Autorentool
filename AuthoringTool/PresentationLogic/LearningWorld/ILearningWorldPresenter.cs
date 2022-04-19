using AuthoringTool.Components.ModalDialog;

namespace AuthoringTool.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter
{
    LearningWorldViewModel CreateNewLearningWorld(string name, string shortname, string authors,
        string language, string description, string goals);

    LearningWorldViewModel EditLearningWorld(LearningWorldViewModel world, string name, string shortname,
        string authors, string language, string description, string goals);

    bool CreateLearningSpaceDialogueOpen { get; set; }
    bool EditLearningSpaceDialogOpen { get; set; }
    IEnumerable<ModalDialogInputField> ModalDialogSpaceInputFields { get; }
    bool EditLearningElementDialogOpen { get; set; }
    IEnumerable<ModalDialogInputField> ModalDialogElementInputFields { get; }
    bool CreateLearningElementDialogOpen { get; set; }
    LearningWorldViewModel? LearningWorldVm { get; }
    bool SelectedLearningObjectIsSpace { get; }
    bool ShowingLearningSpaceView { get; }
    void SetSelectedLearningObject(ILearningObjectViewModel learningObject);
    void DeleteSelectedLearningObject();
    Task LoadLearningSpace();
    Task LoadLearningElement();
    Task SaveSelectedLearningObjectAsync();
    Task OnCreateSpaceDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple);
    Task OnCreateElementDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple);
    void OpenEditSelectedLearningObjectDialog();
    Task OnEditSpaceDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple);
    Task OnEditElementDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple);
    void SetLearningWorld(object? caller, LearningWorldViewModel? world);
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();
}