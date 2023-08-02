using System.ComponentModel;
using MudBlazor;
using Presentation.Components;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter : INotifyPropertyChanged
{
    ILearningSpaceViewModel? LearningSpaceVm { get; }

    bool ReplaceLearningElementDialogOpen { get; set; }

    void EditLearningSpace(string name, string description, string goals, int requiredPoints, Theme theme,
        ITopicViewModel? topic = null);

    void SetSelectedLearningElement(ILearningElementViewModel? learningElement);
    void DeleteSelectedLearningElement();
    Task LoadLearningElementAsync(int slotIndex);
    Task SaveSelectedLearningElementAsync();
    Task ShowSelectedElementContentAsync();
    void SetLearningSpace(ILearningSpaceViewModel space);
    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;
    void DragLearningElement(object sender, DraggedEventArgs<ILearningElementViewModel> draggedEventArgs);
    void ClickedLearningElement(ILearningElementViewModel obj);

    void EditLearningElement(ILearningElementViewModel learningElement, string name, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        ILearningContentViewModel learningContent);

    void EditLearningElement(int slotIndex);
    void DeleteLearningElement(ILearningElementViewModel obj);
    void ShowElementContent(ILearningElementViewModel obj);
    void SetLearningSpaceLayout(FloorPlanEnum floorPlanName);

    void OpenReplaceLearningElementDialog(ILearningWorldViewModel learningWorldVm, ILearningElementViewModel dropItem,
        int slotId);

    void OnReplaceLearningElementDialogClose(DialogResult closeResult);
    void ClickOnSlot(int i);

    void CreateLearningElementInSlot(string name, ILearningContentViewModel learningContent, string description,
        string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points);
}