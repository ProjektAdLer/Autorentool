using System.ComponentModel;
using MudBlazor;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter : INotifyPropertyChanged
{
    /// <summary>
    /// The currently selected LearningSpaceViewModel.
    /// </summary>
    ILearningSpaceViewModel? LearningSpaceVm { get; }

    /// <summary>
    /// Gets a value indicating whether the dialog for replacing a learning element is currently open.
    /// </summary>
    bool ReplaceLearningElementDialogOpen { get; }

    /// <summary>
    /// Edits an existing learning space with the specified properties.
    /// </summary>
    /// <param name="name">The name of the learning space.</param>
    /// <param name="description">The description of the learning space.</param>
    /// <param name="requiredPoints">The required points for the learning space.</param>
    /// <param name="theme">The theme of the learning space.</param>
    /// <param name="topic">The topic of the learning space, which may be null.</param>
    void EditLearningSpace(string name, string description, int requiredPoints,
        Theme theme,
        ITopicViewModel? topic = null);

    /// <summary>
    /// Calls the LoadLearningElementAsync method in PresentationLogic and adds the returned
    /// learning element to its parent.
    /// </summary>
    Task LoadLearningElementAsync(int slotIndex);

    /// <summary>
    /// Sets the LearningSpaceVm.
    /// </summary>
    /// <param name="space">The learning space that should be set as selected.</param>
    void SetLearningSpace(ILearningSpaceViewModel space);

    event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute;

    /// <summary>
    /// Handles the action when a learning element is clicked.
    /// </summary>
    /// <param name="learningElementViewModel">The learning element that was clicked.</param>
    void ClickedLearningElement(ILearningElementViewModel learningElementViewModel);

    /// <summary>
    /// Edits an existing learning element.
    /// </summary>
    /// <param name="learningElement">The learning element to be edited.</param>
    /// <param name="name">The new name for the learning element.</param>
    /// <param name="description">The new description for the learning element.</param>
    /// <param name="goals">The new goals for the learning element.</param>
    /// <param name="difficulty">The new difficulty level for the learning element.</param>
    /// <param name="elementModel">The new element model for the learning element.</param>
    /// <param name="workload">The new workload value for the learning element.</param>
    /// <param name="points">The new points value for the learning element.</param>
    /// <param name="learningContent">The new learning content for the learning element.</param>
    void EditLearningElement(ILearningElementViewModel learningElement, string name, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        ILearningContentViewModel learningContent);

    /// <summary>
    /// Edits the learning element located at the specified slot index.
    /// </summary>
    /// <param name="slotIndex">The index of the slot containing the learning element to edit.</param>
    void EditLearningElement(int slotIndex);

    /// <summary>
    /// Deletes the specified learning element from the learning space.
    /// </summary>
    /// <param name="learningElementViewModel">The learning element to be deleted.</param>
    void DeleteLearningElement(ILearningElementViewModel learningElementViewModel);

    /// <summary>
    /// Sets the selected learning element and asynchronously displays its content.
    /// </summary>
    /// <param name="learningElementViewModel">The learning element whose content is to be displayed.</param>
    void ShowElementContent(ILearningElementViewModel learningElementViewModel);

    /// <summary>
    /// Sets the layout of the learning space to the specified floor plan.
    /// </summary>
    /// <param name="floorPlanName">The floor plan enumeration value to set as the layout of the learning space.</param>
    void SetLearningSpaceLayout(FloorPlanEnum floorPlanName);

    /// <summary>
    /// Opens the dialog for replacing a learning element, initializing the necessary data.
    /// </summary>
    /// <param name="learningWorldVm">The learning world view model that contains the element to be replaced.</param>
    /// <param name="dropItem">The learning element view model to replace.</param>
    /// <param name="slotId">The slot ID where the learning element is located.</param>
    void OpenReplaceLearningElementDialog(ILearningWorldViewModel learningWorldVm, ILearningElementViewModel dropItem,
        int slotId);

    /// <summary>
    /// Handles the closing of the replace learning element dialog, updating the state accordingly.
    /// </summary>
    /// <param name="closeResult">The result of the dialog close operation, indicating whether the operation was canceled or confirmed.</param>
    void OnReplaceLearningElementDialogClose(DialogResult closeResult);

    /// <summary>
    /// Handles a click event on a specific slot within the learning space layout.
    /// </summary>
    /// <param name="i">The index of the clicked slot.</param>
    void ClickOnSlot(int i);

    /// <summary>
    /// Creates a new learning element in the specified slot of the learning space layout.
    /// </summary>
    /// <param name="name">The name of the learning element.</param>
    /// <param name="learningContent">The content associated with the learning element.</param>
    /// <param name="description">The description of the learning element.</param>
    /// <param name="goals">The goals associated with the learning element.</param>
    /// <param name="difficulty">The difficulty level of the learning element.</param>
    /// <param name="elementModel">The model associated with the learning element.</param>
    /// <param name="workload">The workload associated with the learning element.</param>
    /// <param name="points">The points associated with the learning element.</param>
    void CreateLearningElementInSlot(string name, ILearningContentViewModel learningContent, string description,
        string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points);
}