using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AutoMapper;
using MudBlazor;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningContent.Story;
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
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    private readonly IPresentationLogic _presentationLogic;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;
    private ILearningSpaceViewModel? _learningSpaceVm;
    private ReplaceLearningElementData? _replaceLearningElementData;
    private ReplaceLearningElementData? _replaceStoryElementData;
    private bool _replaceLearningElementDialogOpen;
    private bool _replaceStoryElementDialogOpen;

    public LearningSpacePresenter(
        IPresentationLogic presentationLogic, IMediator mediator,
        ISelectedViewModelsProvider selectedViewModelsProvider, ILogger<LearningSpacePresenter> logger,
        IErrorService errorService, IMapper mapper)
    {
        Logger = logger;
        _presentationLogic = presentationLogic;
        _mediator = mediator;
        _selectedViewModelsProvider = selectedViewModelsProvider;
        _selectedViewModelsProvider.PropertyChanged += SelectedViewModelsProviderOnPropertyChanged;
        _errorService = errorService;
        _mapper = mapper;
    }

    private ILogger<LearningSpacePresenter> Logger { get; }

    /// <inheritdoc cref="ILearningSpacePresenter.LearningSpaceVm"/>
    public ILearningSpaceViewModel? LearningSpaceVm
    {
        get => _learningSpaceVm;
        private set => SetField(ref _learningSpaceVm, value);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.SetLearningSpace"/>
    public void SetLearningSpace(ILearningSpaceViewModel space)
    {
        LearningSpaceVm = space;
        Logger.LogDebug("LearningSpace set to {Name}", space.Name);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.EditLearningSpace"/>
    public void EditLearningSpace(string name, string description, int requiredPoints, Theme theme,
        ITopicViewModel? topic)
    {
        if (!CheckLearningSpaceNotNull("EditLearningSpace"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.EditLearningSpace(LearningSpaceVm!, name, description,
            requiredPoints, theme, topic);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.ReplaceLearningElementDialogOpen"/>
    public bool ReplaceLearningElementDialogOpen
    {
        get => _replaceLearningElementDialogOpen;
        private set => SetField(ref _replaceLearningElementDialogOpen, value);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.ReplaceStoryElementDialogOpen"/>
    public bool ReplaceStoryElementDialogOpen
    {
        get => _replaceStoryElementDialogOpen;
        private set => SetField(ref _replaceStoryElementDialogOpen, value);
    }

    public event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute
    {
        add => _presentationLogic.OnCommandUndoRedoOrExecute += value;
        remove => _presentationLogic.OnCommandUndoRedoOrExecute -= value;
    }

    /// <inheritdoc cref="ILearningSpacePresenter.SetLearningSpaceLayout"/>
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
        _selectedViewModelsProvider.SetActiveElementSlotInSpace(-1, null);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

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

    #region LearningElement

    /// <inheritdoc cref="ILearningSpacePresenter.OpenReplaceLearningElementDialog"/>
    [MemberNotNull(nameof(_replaceLearningElementData))]
    public void OpenReplaceLearningElementDialog(ILearningWorldViewModel learningWorldVm,
        ILearningElementViewModel dropItem, int slotId)
    {
        _replaceLearningElementData = new ReplaceLearningElementData(learningWorldVm, dropItem, slotId);
        ReplaceLearningElementDialogOpen = true;
    }

    [MemberNotNull(nameof(_replaceStoryElementData))]
    public void OpenReplaceStoryElementDialog(ILearningWorldViewModel learningWorldVm,
        ILearningElementViewModel dropItem,
        int slotId)
    {
        _replaceStoryElementData = new ReplaceLearningElementData(learningWorldVm, dropItem, slotId);
        ReplaceStoryElementDialogOpen = true;
    }

    /// <inheritdoc cref="ILearningSpacePresenter.OnReplaceLearningElementDialogClose"/>
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

    public void OnReplaceStoryElementDialogClose(DialogResult closeResult)
    {
        ReplaceStoryElementDialogOpen = false;
        if (!CheckLearningSpaceNotNull("OnReplaceStoryElementDialogClose"))
            return;

        if (closeResult.Canceled) return;

        _presentationLogic.DragStoryElementFromUnplaced(_replaceStoryElementData!.LearningWorldVm, LearningSpaceVm!,
            _replaceStoryElementData.DropItem, _replaceStoryElementData.SlotId);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.ClickOnElementSlot"/>
    public void ClickOnElementSlot(int i)
    {
        if (LearningSpaceVm?.LearningSpaceLayout.LearningElements.ContainsKey(i) ?? false)
            return;
        if (_selectedViewModelsProvider.ActiveElementSlotInSpace == i)
        {
            _selectedViewModelsProvider.SetActiveElementSlotInSpace(-1, null);
            return;
        }

        SetSelectedLearningElement(null);
        _selectedViewModelsProvider.SetActiveElementSlotInSpace(i, null);
        _mediator.RequestOpenElementDialog();
    }

    public void ClickOnStorySlot(int i)
    {
        if (LearningSpaceVm?.LearningSpaceLayout.StoryElements.ContainsKey(i) ?? false) return;
        if (_selectedViewModelsProvider.ActiveStorySlotInSpace == i)
        {
            _selectedViewModelsProvider.SetActiveStorySlotInSpace(-1, null);
            return;
        }
        
        SetSelectedLearningElement(null);
        _selectedViewModelsProvider.SetActiveStorySlotInSpace(i, null);
        _mediator.RequestOpenStoryElementDialog();
    }

    /// <inheritdoc cref="ILearningSpacePresenter.CreateLearningElementInSlot"/>
    public void CreateLearningElementInSlot(string name, ILearningContentViewModel learningContent,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points)
    {
        if (!CheckLearningSpaceNotNull("CreateLearningElementInSlot"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.CreateLearningElementInSlot(LearningSpaceVm!, _selectedViewModelsProvider.ActiveElementSlotInSpace,
            name, learningContent, description,
            goals, difficulty, elementModel, workload, points);
        _selectedViewModelsProvider.SetActiveElementSlotInSpace(-1, null);
    }
    
    public void CreateStoryElementInSlot(string name, ILearningContentViewModel learningContent,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points)
    {
        if (!CheckLearningSpaceNotNull("CreateStoryElementInSlot"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.CreateStoryElementInSlot(LearningSpaceVm!, _selectedViewModelsProvider.ActiveStorySlotInSpace,
            name, learningContent, description,
            goals, difficulty, elementModel, workload, points);
        _selectedViewModelsProvider.SetActiveStorySlotInSpace(-1, null);
    }

    public void CreateLearningElementInSlotFromFormModel(LearningElementFormModel model)
    {
        CreateLearningElementInSlot(model.Name, _mapper.Map<ILearningContentViewModel>(model.LearningContent),
            model.Description, model.Goals, model.Difficulty, model.ElementModel, model.Workload, model.Points);
    }
    
    public void CreateStoryElementInSlotFromFormModel(LearningElementFormModel model)
    {
        CreateStoryElementInSlot(model.Name, _mapper.Map<ILearningContentViewModel>(model.LearningContent),
            model.Description, model.Goals, model.Difficulty, model.ElementModel, model.Workload, model.Points);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.ClickedLearningElement"/>
    public void ClickedLearningElement(ILearningElementViewModel learningElementViewModel)
    {
        switch (learningElementViewModel.LearningContent)
        {
            case IAdaptivityContentViewModel:
                _mediator.RequestOpenAdaptivityElementDialog();
                break;
            case IStoryContentViewModel:
                _mediator.RequestOpenStoryElementDialog();
                break;
            case IFileContentViewModel or ILinkContentViewModel:
                _mediator.RequestOpenElementDialog();
                break;
            case null:
                throw new ApplicationException("Element has no content");
        }

        _selectedViewModelsProvider.SetActiveElementSlotInSpace(-1, null);
        SetSelectedLearningElement(learningElementViewModel);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.EditLearningElement(ILearningElementViewModel, string, string, string, LearningElementDifficultyEnum, ElementModel, int, int, ILearningContentViewModel)"/>
    public void EditLearningElement(ILearningElementViewModel learningElement, string name, string description,
        string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points,
        ILearningContentViewModel learningContent)
    {
        SetSelectedLearningElement(learningElement);
        _presentationLogic.EditLearningElement(LearningSpaceVm, learningElement, name, description, goals, difficulty,
            elementModel, workload, points, learningContent);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.EditLearningElement(int)"/>
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

    /// <inheritdoc cref="ILearningSpacePresenter.DeleteLearningElement"/>
    public void DeleteLearningElement(ILearningElementViewModel learningElementViewModel)
    {
        if (!CheckLearningSpaceNotNull("DeleteLearningElement"))
            return;
        //Nullability check for learningSpaceVm is done in CheckLearningSpaceNotNull
        _presentationLogic.DeleteLearningElementInSpace(LearningSpaceVm!, learningElementViewModel);
    }

    public void DeleteStoryElement(ILearningElementViewModel learningElementViewModel)
    {
        if (!CheckLearningSpaceNotNull(nameof(DeleteStoryElement)))
            return;
        _presentationLogic.DeleteStoryElementInSpace(LearningSpaceVm!, learningElementViewModel);
    }

    /// <inheritdoc cref="ILearningSpacePresenter.ShowElementContent"/>
    public async void ShowElementContent(ILearningElementViewModel learningElementViewModel)
    {
        SetSelectedLearningElement(learningElementViewModel);
        await ShowSelectedElementContentAsync();
    }

    private void SelectedViewModelsProviderOnPropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(_selectedViewModelsProvider.LearningObjectInPathWay)) return;
        if (caller is not ISelectedViewModelsProvider)
        {
            LogAndSetError("OnSelectedViewModelsProviderOnPropertyChanged",
                $"Caller must be of type ISelectedViewModelsProvider, got {caller?.GetType()}",
                "Caller must be of type ISelectedViewModelsProvider");
            return;
        }

        LearningSpaceVm = _selectedViewModelsProvider.LearningObjectInPathWay switch
        {
            LearningSpaceViewModel space => space,
            null => null,
            _ => LearningSpaceVm
        };
    }

    /// <summary>
    /// Changes the selected <see cref="ILearningElementViewModel"/> in the currently selected learning space.
    /// </summary>
    /// <param name="learningElement">The learning element that should be set as selected</param>
    private void SetSelectedLearningElement(ILearningElementViewModel? learningElement)
    {
        if (!CheckLearningSpaceNotNull("SetSelectedLearningElement"))
            return;
        _selectedViewModelsProvider.SetLearningElement(learningElement, null);
    }

    /// <summary>
    /// Calls the the show learning element content method for the selected learning element.
    /// </summary>
    private async Task ShowSelectedElementContentAsync()
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