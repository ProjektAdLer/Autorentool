using System.ComponentModel;
using System.Runtime.CompilerServices;
using MudBlazor;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningSpace;

public class LearningSpacePresenter : ILearningSpacePresenter, ILearningSpacePresenterToolboxInterface
{
    public LearningSpacePresenter(
        IPresentationLogic presentationLogic, ILogger<LearningSpacePresenter> logger)
    {
        _presentationLogic = presentationLogic;
        _logger = logger;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILogger<LearningSpacePresenter> _logger;
    private int _creationCounter = 0;
    private int _activeSlot = -1;
    private ILearningSpaceViewModel? _learningSpaceVm;
    private ReplaceLearningElementData _replaceLearningElementData = new();

    public ILearningSpaceViewModel? LearningSpaceVm
    {
        get => _learningSpaceVm;
        private set => SetField(ref _learningSpaceVm, value);
    }

    public ILearningContentViewModel? DragAndDropLearningContent { get; private set; }
    public IDisplayableLearningObject? RightClickedLearningObject { get; private set; }

    public void EditLearningSpace(string name, string description, string goals,
        int requiredPoints, ITopicViewModel? topic)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        _presentationLogic.EditLearningSpace(LearningSpaceVm, name, description, goals,
            requiredPoints, topic);
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
        _presentationLogic.ChangeLearningSpaceLayout(LearningSpaceVm, floorPlanName);
    }

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

    public void DragLearningElement(object sender, DraggedEventArgs<ILearningElementViewModel> args)
    {
        _presentationLogic.DragLearningElement(args.LearningObject, args.OldPositionX, args.OldPositionY);
    }

    public void ClickedLearningElement(ILearningElementViewModel obj)
    {
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

    public void SetLearningSpace(ILearningSpaceViewModel space)
    {
        LearningSpaceVm = space;
    }

    public void OnWorldPropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LearningWorldViewModel.SelectedLearningObjectInPathWay))
        {
            if (caller is not ILearningWorldViewModel worldVm)
                throw new ArgumentException("Caller must be of type ILearningWorldViewModel");

            if (worldVm.SelectedLearningObjectInPathWay is LearningSpaceViewModel space)
                LearningSpaceVm = space;
            else if (worldVm.SelectedLearningObjectInPathWay is null)
                LearningSpaceVm = null;
        }
    }

    #region LearningElement

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
    /// Calls a load method in <see cref="_presentationLogic"/> depending on the content type and returns a
    /// LearningContentViewModel.
    /// </summary>
    /// <param name="contentType">The type of the content that can either be an image, a video, a pdf or a h5p.</param>
    /// <exception cref="ApplicationException">Thrown if there is no valid ContentType assigned.</exception>
    private async Task<ILearningContentViewModel> LoadLearningContent(ContentTypeEnum contentType)
    {
        return contentType switch
        {
            ContentTypeEnum.Image => await _presentationLogic.LoadImageAsync(),
            ContentTypeEnum.Video => await _presentationLogic.LoadVideoAsync(),
            ContentTypeEnum.PDF => await _presentationLogic.LoadPdfAsync(),
            ContentTypeEnum.H5P => await _presentationLogic.LoadH5PAsync(),
            ContentTypeEnum.Text => await _presentationLogic.LoadTextAsync(),
            _ => throw new ApplicationException("No valid ContentType assigned")
        };
    }

    /// <summary>
    /// Returns the parent of the learning element which is the selected learning space.
    /// </summary>
    /// <exception cref="Exception">Thrown if parent element is null.</exception>
    private ILearningSpaceViewModel GetLearningElementParent()
    {
        ILearningSpaceViewModel? parentElement = LearningSpaceVm;

        if (parentElement == null)
        {
            throw new Exception("Parent element is null");
        }

        return parentElement;
    }

    /// <summary>
    /// Changes the selected <see cref="ILearningElementViewModel"/> in the currently selected learning space.
    /// </summary>
    /// <param name="learningElement">The learning element that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public void SetSelectedLearningElement(ILearningElementViewModel learningElement)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        LearningSpaceVm.SelectedLearningElement = learningElement;
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
        if (LearningSpaceVm.SelectedLearningElement == null)
            return;
        _presentationLogic.DeleteLearningElementInSpace(LearningSpaceVm,
            (LearningElementViewModel) LearningSpaceVm.SelectedLearningElement);
    }

    /// <summary>
    /// Calls the the Save methode for the selected learning element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public async Task SaveSelectedLearningElementAsync()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        switch (LearningSpaceVm.SelectedLearningElement)
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
        switch (LearningSpaceVm.SelectedLearningElement)
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