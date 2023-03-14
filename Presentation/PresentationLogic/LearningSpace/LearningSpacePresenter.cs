using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

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

    public ILearningSpaceViewModel? LearningSpaceVm
    {
        get => _learningSpaceVm;
        private set => SetField(ref _learningSpaceVm, value);
    }

    public LearningContentViewModel? DragAndDropLearningContent { get; private set; }
    public IDisplayableLearningObject? RightClickedLearningObject { get; private set; }

    public void EditLearningSpace(string name, string shortname, string authors, string description, string goals, int requiredPoints)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        _presentationLogic.EditLearningSpace(LearningSpaceVm, name, shortname, authors, description, goals, requiredPoints);
    }

    public event Action OnUndoRedoPerformed
    {
        add => _presentationLogic.OnUndoRedoPerformed += value;
        remove => _presentationLogic.OnUndoRedoPerformed -= value;
    }
    
    public void SetLearningSpaceLayout(FloorPlanEnum floorPlanName)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        _presentationLogic.ChangeLearningSpaceLayout(LearningSpaceVm, floorPlanName);
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

    public void EditLearningElement(ILearningElementViewModel obj)
    {
        SetSelectedLearningElement(obj);
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
        _presentationLogic.DeleteLearningElement(LearningSpaceVm, obj);
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
        if (e.PropertyName == nameof(LearningWorldViewModel.SelectedLearningObject))
        {
            if (caller is not ILearningWorldViewModel worldVm)
                throw new ArgumentException("Caller must be of type ILearningWorldViewModel");
        
            if(worldVm.SelectedLearningObject is LearningSpaceViewModel space)
                LearningSpaceVm = space;
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
    private async Task<LearningContentViewModel> LoadLearningContent(ContentTypeEnum contentType)
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
        _presentationLogic.DeleteLearningElement(LearningSpaceVm, (LearningElementViewModel)LearningSpaceVm.SelectedLearningElement);
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