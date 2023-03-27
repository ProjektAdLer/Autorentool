using System.ComponentModel;
using MudBlazor;
using Presentation.Components;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Shared;
using Shared.Command;

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
    ILearningContentViewModel? DragAndDropLearningContent { get; }
    IDisplayableLearningObject? RightClickedLearningObject { get; }
    void OnWorldPropertyChanged(object? caller, PropertyChangedEventArgs e);
    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;
    void DragLearningElement(object sender, DraggedEventArgs<ILearningElementViewModel> draggedEventArgs);
    void ClickedLearningElement(ILearningElementViewModel obj);
    void RightClickedLearningElement(ILearningElementViewModel obj);
    void EditLearningElement(ILearningElementViewModel learningElement, string name, string shortname, string authors,
        string description, string goals, LearningElementDifficultyEnum difficulty, int workload, int points,
        ILearningContentViewModel learningContent);
    void EditLearningElement(int slotIndex);
    void DeleteLearningElement(ILearningElementViewModel obj);
    void HideRightClickMenu();
    void ShowElementContent(ILearningElementViewModel obj);
    void SetLearningSpaceLayout(FloorPlanEnum floorPlanName);
    void OpenReplaceLearningElementDialog(ILearningWorldViewModel learningWorldVm, ILearningElementViewModel dropItem, int slotId);
    bool ReplaceLearningElementDialogOpen { get; set; }
    void OnReplaceLearningElementDialogClose(DialogResult closeResult);
}