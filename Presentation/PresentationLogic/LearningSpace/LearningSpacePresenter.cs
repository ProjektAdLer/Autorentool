using System.ComponentModel;
using System.Runtime.CompilerServices;
using MudBlazor;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningSpace;

public class LearningSpacePresenter : ILearningSpacePresenter, ILearningSpacePresenterToolboxInterface
{
    public LearningSpacePresenter(
        IPresentationLogic presentationLogic, IMediator mediator, ISelectedViewModelsProvider selectedViewModelsProvider, ILogger<LearningSpacePresenter> logger)
    {
        _presentationLogic = presentationLogic;
        _mediator = mediator;
        _selectedViewModelsProvider = selectedViewModelsProvider;
        _selectedViewModelsProvider.PropertyChanged += SelectedViewModelsProviderOnPropertyChanged;
        _logger = logger;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly IMediator _mediator;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;
    private readonly ILogger<LearningSpacePresenter> _logger;
    private int _creationCounter = 0;
    private ILearningSpaceViewModel? _learningSpaceVm;
    private ReplaceLearningElementData _replaceLearningElementData = new();

    public ILearningSpaceViewModel? LearningSpaceVm
    {
        get => _learningSpaceVm;
        private set => SetField(ref _learningSpaceVm, value);
    }

    public ILearningContentViewModel? DragAndDropLearningContent { get; private set; }
    public IDisplayableLearningObject? RightClickedLearningObject { get; private set; }
    
    public void SetLearningSpace(ILearningSpaceViewModel space)
    {
        LearningSpaceVm = space;
    }

    public void EditLearningSpace(string name, string description, string goals,
        int requiredPoints, Theme theme, ITopicViewModel? topic)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        _presentationLogic.EditLearningSpace(LearningSpaceVm, name, description, goals,
            requiredPoints, theme, topic);
    }

    public bool ReplaceLearningElementDialogOpen { get; set; } = false;

    public event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute
    {
        add => _presentationLogic.OnCommandUndoRedoOrExecute += value;
        remove => _presentationLogic.OnCommandUndoRedoOrExecute -= value;
    }

    public void SetLearningSpaceLayout(FloorPlanEnum floorPlanName)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        if (_selectedViewModelsProvider.LearningWorld == null)
            throw new ApplicationException("LearningWorld is null");
        _presentationLogic.ChangeLearningSpaceLayout(LearningSpaceVm, _selectedViewModelsProvider.LearningWorld, floorPlanName);
        _selectedViewModelsProvider.SetActiveSlot(-1);
    }
    
    #region LearningElement

    public void OpenReplaceLearningElementDialog(ILearningWorldViewModel learningWorldVm,
        ILearningElementViewModel dropItem, int slotId)
    {
        _replaceLearningElementData = new ReplaceLearningElementData
        {
            LearningWorldVm = learningWorldVm,
            DropItem = dropItem,
            SlotId = slotId
        };
        ReplaceLearningElementDialogOpen = true;
    }

    public void OnReplaceLearningElementDialogClose(DialogResult closeResult)
    {
        ReplaceLearningElementDialogOpen = false;
        if (LearningSpaceVm == null) throw new ApplicationException("LearningSpaceVm is null");

        if (closeResult.Canceled) return;

        _presentationLogic.DragLearningElementFromUnplaced(_replaceLearningElementData.LearningWorldVm, LearningSpaceVm,
            _replaceLearningElementData.DropItem, _replaceLearningElementData.SlotId);
    }

    public void AddNewLearningElement(int i)
    {
        if (LearningSpaceVm?.LearningSpaceLayout.LearningElements.ContainsKey(i) ?? false)
            return;
        SetSelectedLearningElement(null);
        _selectedViewModelsProvider.SetActiveSlot(i);
        _mediator.RequestOpenElementDialog();
    }

    public void CreateLearningElementInSlot(string name, ILearningContentViewModel learningContent,
        string description, string goals, LearningElementDifficultyEnum difficulty, int workload, int points)
    {
        if(LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        _presentationLogic.CreateLearningElementInSlot(LearningSpaceVm, _selectedViewModelsProvider.ActiveSlot, name, learningContent, description,
            goals, difficulty, workload, points);
        _selectedViewModelsProvider.SetActiveSlot(-1);
    }

    public void DragLearningElement(object sender, DraggedEventArgs<ILearningElementViewModel> args)
    {
        _presentationLogic.DragLearningElement(args.LearningObject, args.OldPositionX, args.OldPositionY);
    }

    public void ClickedLearningElement(ILearningElementViewModel obj)
    {
        _mediator.RequestOpenElementDialog();
        _selectedViewModelsProvider.SetActiveSlot(-1);
        SetSelectedLearningElement(obj);
    }

    public void RightClickedLearningElement(ILearningElementViewModel obj)
    {
        RightClickedLearningObject = obj;
    }

    public void EditLearningElement(ILearningElementViewModel learningElement,
        string name, string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, ILearningContentViewModel learningContent)
    {
        SetSelectedLearningElement(learningElement);
        _presentationLogic.EditLearningElement(LearningSpaceVm, learningElement, name, description,
            goals, difficulty, workload, points, learningContent);
    }

    public void EditLearningElement(int slotIndex)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        var element = LearningSpaceVm.LearningSpaceLayout.GetElement(slotIndex);
        if (element == null)
            throw new ApplicationException($"LearningElement at slotIndex {slotIndex} is null");
        SetSelectedLearningElement(element);
    }

    public void DeleteLearningElement(ILearningElementViewModel obj)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        _presentationLogic.DeleteLearningElementInSpace(LearningSpaceVm, obj);
    }

    public void HideRightClickMenu()
    {
        RightClickedLearningObject = null;
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
                throw new ArgumentException("Caller must be of type ISelectedViewModelsProvider");

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
    /// <exception cref="ApplicationException">Thrown if <see cref="LearningSpaceVm"/> is null</exception>
    public async Task LoadLearningElementAsync(int slotIndex)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        await _presentationLogic.LoadLearningElementAsync(LearningSpaceVm, slotIndex);
    }

    public void AddLearningElement(ILearningElementViewModel element, int slotIndex)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        _presentationLogic.AddLearningElement(LearningSpaceVm, slotIndex, element);
    }

    /// <summary>
    /// Changes the selected <see cref="ILearningElementViewModel"/> in the currently selected learning space.
    /// </summary>
    /// <param name="learningElement">The learning element that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public void SetSelectedLearningElement(ILearningElementViewModel? learningElement)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        _selectedViewModelsProvider.SetLearningElement(learningElement, null);
        HideRightClickMenu();
    }

    /// <summary>
    /// Deletes the selected learning element in the currently selected learning space and sets an other element as selected learning element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public void DeleteSelectedLearningElement()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        if (_selectedViewModelsProvider.LearningElement == null)
            return;
        _presentationLogic.DeleteLearningElementInSpace(LearningSpaceVm,
            _selectedViewModelsProvider.LearningElement);
    }

    /// <summary>
    /// Calls the the Save methode for the selected learning element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public async Task SaveSelectedLearningElementAsync()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        switch (_selectedViewModelsProvider.LearningElement)
        {
            case null:
                throw new ApplicationException("SelectedLearningElement is null");
            case LearningElementViewModel learningElement:
                await _presentationLogic.SaveLearningElementAsync(learningElement);
                break;
        }
    }

    /// <summary>
    /// Calls the the show learning element content method for the selected learning element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected or no learning element
    /// is selected in the learning space.</exception>
    public async Task ShowSelectedElementContentAsync()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        switch (_selectedViewModelsProvider.LearningElement)
        {
            case null:
                throw new ApplicationException("SelectedLearningElement is null");
            case LearningElementViewModel learningElement:
                await _presentationLogic.ShowLearningElementContentAsync(learningElement);
                break;
        }
    }

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

internal class ReplaceLearningElementData
{
    internal ILearningWorldViewModel LearningWorldVm { get; init; }
    public ILearningElementViewModel DropItem { get; init; }
    public int SlotId { get; init; }
}