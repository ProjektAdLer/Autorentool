using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter
{
    LearningWorldViewModel CreateNewLearningWorld(string name, string shortname, string authors,
        string language, string description, string goals);

    LearningWorldViewModel EditLearningWorld(LearningWorldViewModel world, string name, string shortname,
        string authors, string language, string description, string goals);

    bool CreateLearningSpaceDialogOpen { get; set; }
    bool EditLearningSpaceDialogOpen { get; set; }
    Dictionary<string, string>? EditSpaceDialogInitialValues { get; }
    Dictionary<string, string>? EditElementDialogInitialValues { get; }
    bool EditLearningElementDialogOpen { get; set; }
    bool CreateLearningElementDialogOpen { get; set; }
    ILearningWorldViewModel? LearningWorldVm { get; }
    bool SelectedLearningObjectIsSpace { get; }
    bool ShowingLearningSpaceView { get; }
    void SetSelectedLearningObject(ILearningObjectViewModel learningObject);
    void DeleteSelectedLearningObject();
    Task LoadLearningSpace();
    Task LoadLearningElement();
    Task SaveSelectedLearningObjectAsync();
    void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnCreateElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OpenEditSelectedLearningObjectDialog();
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void SetLearningWorld(object? caller, LearningWorldViewModel? world);
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();

    /// <summary>
    /// Adds the provided learning space to the selected world view model.
    /// </summary>
    /// <param name="learningSpace">The space to be added.</param>
    /// <exception cref="ApplicationException"><see cref="LearningWorldVm"/> is null or space exists in world with same name.</exception>
    public void AddLearningSpace(ILearningSpaceViewModel learningSpace);
    
    /// <summary>
    /// Adds the provided learning element to the selected world view model.
    /// </summary>
    /// <param name="learningElement">The space to be added.</param>
    /// <exception cref="ApplicationException"><see cref="LearningWorldVm"/> is null or element exists in world with same name.</exception>
    public void AddLearningElement(ILearningElementViewModel learningElement);
    
    void CreateLearningElementWithPreloadedContent(LearningContentViewModel learningContent);
    LearningContentViewModel? DragAndDropLearningContent { get; }
}