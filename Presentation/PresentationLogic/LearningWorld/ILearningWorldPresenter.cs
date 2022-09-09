using System.ComponentModel;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter : INotifyPropertyChanged, INotifyPropertyChanging
{
    bool CreateLearningSpaceDialogOpen { get; }
    bool EditLearningSpaceDialogOpen { get; }
    Dictionary<string, string>? EditSpaceDialogInitialValues { get; }
    Dictionary<string, string>? EditElementDialogInitialValues { get; }
    bool EditLearningElementDialogOpen { get; }
    bool CreateLearningElementDialogOpen { get; }
    ILearningWorldViewModel? LearningWorldVm { get; }
    bool SelectedLearningObjectIsSpace { get; }
    bool ShowingLearningSpaceView { get; }
    void SetSelectedLearningObject(ILearningObjectViewModel learningObject);
    void DeleteSelectedLearningObject();
    Task LoadLearningSpaceAsync();
    Task LoadLearningElementAsync();
    Task SaveSelectedLearningObjectAsync();
    void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnCreateElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OpenEditSelectedLearningObjectDialog();
    void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void OnEditElementDialogClose(ModalDialogOnCloseResult returnValueTuple);
    void SetLearningWorld(object? caller, LearningWorldViewModel? world);
    void ShowSelectedLearningSpaceView();
    void CloseLearningSpaceView();
    void CreateLearningElementWithPreloadedContent(LearningContentViewModel learningContent);
    LearningContentViewModel? DragAndDropLearningContent { get; }
    void AddNewLearningSpace();
    void AddNewLearningElement();
}