using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using MudBlazor;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningSpace;

public sealed class LearningSpacePresenter : ILearningSpacePresenter
{
    private readonly IErrorService _errorService;
    private readonly IMediator _mediator;

    private readonly IPresentationLogic _presentationLogic;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;
    private ILearningSpaceViewModel? _learningSpaceVm;
    private ReplaceLearningElementData? _replaceLearningElementData;

    public LearningSpacePresenter(
        IPresentationLogic presentationLogic, IMediator mediator,
        ISelectedViewModelsProvider selectedViewModelsProvider, ILogger<LearningSpacePresenter> logger,
        IErrorService errorService)
    {
        Logger = logger;
        _presentationLogic = presentationLogic;
        _mediator = mediator;
        _selectedViewModelsProvider = selectedViewModelsProvider;
        _selectedViewModelsProvider.PropertyChanged += SelectedViewModelsProviderOnPropertyChanged;
        _errorService = errorService;
    }

    private ILogger<LearningSpacePresenter> Logger { get; }

    public ILearningSpaceViewModel? LearningSpaceVm
    {
        get => _learningSpaceVm;
        private set => SetField(ref _learningSpaceVm, value);
    }


    public void SetLearningSpace(ILearningSpaceViewModel space)
    {
        LearningSpaceVm = space;
        Logger.LogDebug("LearningSpace set to {Name}", space.Name);
    }

    public void EditLearningSpace(string name, string description, string goals,
        int requiredPoints, Theme theme, ITopicViewModel? topic)
    {
        if (!CheckLearningSpaceNotNull("EditLearningSpace"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.EditLearningSpace(LearningSpaceVm!, name, description, goals,
            requiredPoints, theme, topic);
    }

    public bool ReplaceLearningElementDialogOpen { get; set; }

    public event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute
    {
        add => _presentationLogic.OnCommandUndoRedoOrExecute += value;
        remove => _presentationLogic.OnCommandUndoRedoOrExecute -= value;
    }

    public void SetLearningSpaceLayout(FloorPlanEnum floorPlanName)
    {
        if (!CheckLearningSpaceNotNull("SetLearningSpaceLayout"))
            return;
        if (_selectedViewModelsProvider.LearningWorld == null)
        {
            LogAndSetError("SetLearningSpaceLayout", "SelectedLearningWorld is null", "No LearningWorld selected");
            return;
        }

        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.ChangeLearningSpaceLayout(LearningSpaceVm!, _selectedViewModelsProvider.LearningWorld,
            floorPlanName);
        _selectedViewModelsProvider.SetActiveSlotInSpace(-1, null);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    private bool CheckLearningSpaceNotNull(string operation)
    {
        if (LearningSpaceVm != null)
            return true;

        LogAndSetError(operation, "SelectedLearningSpace is null", "No learning space selected");
        return false;
    }

    private void LogAndSetError(string operation, string errorDetail, string userMessage)
    {
        Logger.LogError("Error in {Operation}: {ErrorDetail}", operation, errorDetail);
        _errorService.SetError("Operation failed", userMessage);
    }

    #region LearningElement

    [MemberNotNull(nameof(_replaceLearningElementData))]
    public void OpenReplaceLearningElementDialog(ILearningWorldViewModel learningWorldVm,
        ILearningElementViewModel dropItem, int slotId)
    {
        _replaceLearningElementData = new ReplaceLearningElementData(learningWorldVm, dropItem, slotId);
        ReplaceLearningElementDialogOpen = true;
    }

    public void OnReplaceLearningElementDialogClose(DialogResult closeResult)
    {
        ReplaceLearningElementDialogOpen = false;
        if (!CheckLearningSpaceNotNull("OnReplaceLearningElementDialogClose"))
            return;

        if (closeResult.Canceled) return;

        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.DragLearningElementFromUnplaced(_replaceLearningElementData!.LearningWorldVm,
            LearningSpaceVm!,
            _replaceLearningElementData.DropItem, _replaceLearningElementData.SlotId);
    }

    public void ClickOnSlot(int i)
    {
        if (LearningSpaceVm?.LearningSpaceLayout.LearningElements.ContainsKey(i) ?? false)
            return;
        if (_selectedViewModelsProvider.ActiveSlotInSpace == i)
        {
            _selectedViewModelsProvider.SetActiveSlotInSpace(-1, null);
            return;
        }

        SetSelectedLearningElement(null);
        _selectedViewModelsProvider.SetActiveSlotInSpace(i, null);
        _mediator.RequestOpenElementDialog();
    }

    public void CreateLearningElementInSlot(string name, ILearningContentViewModel learningContent,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points)
    {
        if (!CheckLearningSpaceNotNull("CreateLearningElementInSlot"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.CreateLearningElementInSlot(LearningSpaceVm!, _selectedViewModelsProvider.ActiveSlotInSpace,
            name, learningContent, description,
            goals, difficulty, elementModel, workload, points);
        _selectedViewModelsProvider.SetActiveSlotInSpace(-1, null);
    }

    public void DragLearningElement(object sender, DraggedEventArgs<ILearningElementViewModel> args)
    {
        _presentationLogic.DragLearningElement(args.LearningObject, args.OldPositionX, args.OldPositionY);
    }

    public void ClickedLearningElement(ILearningElementViewModel obj)
    {
        _mediator.RequestOpenElementDialog();
        _selectedViewModelsProvider.SetActiveSlotInSpace(-1, null);
        SetSelectedLearningElement(obj);
    }

    public void EditLearningElement(ILearningElementViewModel learningElement, string name, string description,
        string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        ILearningContentViewModel learningContent)
    {
        SetSelectedLearningElement(learningElement);
        _presentationLogic.EditLearningElement(LearningSpaceVm, learningElement, name, description, goals, difficulty,
            elementModel, workload, points, learningContent);
    }

    public void EditLearningElement(int slotIndex)
    {
        if (!CheckLearningSpaceNotNull("EditLearningElement"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        var element = LearningSpaceVm!.LearningSpaceLayout.GetElement(slotIndex);
        if (element == null)
        {
            LogAndSetError("EditLearningElement", $"LearningElement at slotIndex {slotIndex} is null",
                "LearningElement to edit not found");
            return;
        }

        SetSelectedLearningElement(element);
    }

    public void DeleteLearningElement(ILearningElementViewModel obj)
    {
        if (!CheckLearningSpaceNotNull("DeleteLearningElement"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.DeleteLearningElementInSpace(LearningSpaceVm!, obj);
    }

    public async void ShowElementContent(ILearningElementViewModel obj)
    {
        SetSelectedLearningElement(obj);
        await ShowSelectedElementContentAsync();
    }

    private void SelectedViewModelsProviderOnPropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_selectedViewModelsProvider.LearningObjectInPathWay))
        {
            if (caller is not ISelectedViewModelsProvider)
            {
                LogAndSetError("OnSelectedViewModelsProviderOnPropertyChanged",
                    $"Caller must be of type ISelectedViewModelsProvider, got {caller?.GetType()}",
                    "Caller must be of type ISelectedViewModelsProvider");
                return;
            }

            if (_selectedViewModelsProvider.LearningObjectInPathWay is LearningSpaceViewModel space)
                LearningSpaceVm = space;
            else if (_selectedViewModelsProvider.LearningObjectInPathWay is null)
                LearningSpaceVm = null;
        }
    }

    /// <summary>
    /// Calls the LoadLearningElementAsync method in <see cref="_presentationLogic"/> and adds the returned
    /// learning element to its parent.
    /// </summary>
    public async Task LoadLearningElementAsync(int slotIndex)
    {
        if (!CheckLearningSpaceNotNull("LoadLearningElementAsync"))
            return;
        try
        {
            //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
            await _presentationLogic.LoadLearningElementAsync(LearningSpaceVm!, slotIndex);
        }
        catch (SerializationException e)
        {
            _errorService.SetError("Error while loading learning element", e.Message);
        }
        catch (InvalidOperationException e)
        {
            _errorService.SetError("Error while loading learning element", e.Message);
        }
    }

    public void AddLearningElement(ILearningElementViewModel element, int slotIndex)
    {
        if (!CheckLearningSpaceNotNull("AddLearningElement"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.AddLearningElement(LearningSpaceVm!, slotIndex, element);
    }

    /// <summary>
    /// Changes the selected <see cref="ILearningElementViewModel"/> in the currently selected learning space.
    /// </summary>
    /// <param name="learningElement">The learning element that should be set as selected</param>
    public void SetSelectedLearningElement(ILearningElementViewModel? learningElement)
    {
        if (!CheckLearningSpaceNotNull("SetSelectedLearningElement"))
            return;
        _selectedViewModelsProvider.SetLearningElement(learningElement, null);
    }

    /// <summary>
    /// Deletes the selected learning element in the currently selected learning space and sets an other element as selected learning element.
    /// </summary>
    public void DeleteSelectedLearningElement()
    {
        if (!CheckLearningSpaceNotNull("DeleteSelectedLearningElement"))
            return;
        if (_selectedViewModelsProvider.LearningElement == null)
        {
            LogAndSetError("DeleteSelectedLearningElement", "SelectedLearningElement is null",
                "No learning element selected");
            return;
        }

        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.DeleteLearningElementInSpace(LearningSpaceVm!,
            _selectedViewModelsProvider.LearningElement);
    }

    /// <summary>
    /// Calls the the Save methode for the selected learning element.
    /// </summary>
    public async Task SaveSelectedLearningElementAsync()
    {
        if (!CheckLearningSpaceNotNull("SaveSelectedLearningElementAsync"))
            return;
        switch (_selectedViewModelsProvider.LearningElement)
        {
            case null:
                LogAndSetError("SaveSelectedLearningElementAsync", "SelectedLearningElement is null",
                    "No learning element selected");
                return;
            case LearningElementViewModel learningElement:
                try
                {
                    await _presentationLogic.SaveLearningElementAsync(learningElement);
                }
                catch (SerializationException e)
                {
                    _errorService.SetError("Error while loading learning element", e.Message);
                }
                catch (InvalidOperationException e)
                {
                    _errorService.SetError("Error while loading learning element", e.Message);
                }

                break;
        }
    }

    /// <summary>
    /// Calls the the show learning element content method for the selected learning element.
    /// </summary>
    public async Task ShowSelectedElementContentAsync()
    {
        if (!CheckLearningSpaceNotNull("ShowSelectedElementContentAsync"))
            return;
        switch (_selectedViewModelsProvider.LearningElement)
        {
            case null:
                LogAndSetError("ShowSelectedElementContentAsync", "SelectedLearningElement is null",
                    "No learning element selected");
                return;
            case LearningElementViewModel learningElement:
                try
                {
                    await _presentationLogic.ShowLearningElementContentAsync(learningElement);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    _errorService.SetError("Error while showing learning element content", e.Message);
                }
                catch (IOException e)
                {
                    _errorService.SetError("Error while showing learning element content", e.Message);
                }
                catch (InvalidOperationException e)
                {
                    _errorService.SetError("Error while showing learning element content", e.Message);
                }

                break;
        }
    }

    #endregion
}

internal class ReplaceLearningElementData
{
    public ReplaceLearningElementData(ILearningWorldViewModel learningWorldVm, ILearningElementViewModel dropItem,
        int slotId)
    {
        LearningWorldVm = learningWorldVm;
        DropItem = dropItem;
        SlotId = slotId;
    }

    internal ILearningWorldViewModel LearningWorldVm { get; init; }
    public ILearningElementViewModel DropItem { get; init; }
    public int SlotId { get; init; }
}