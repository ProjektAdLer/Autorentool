using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.World;
using Shared;
using ModalDialogOnCloseResult = Presentation.Components.ModalDialog.ModalDialogOnCloseResult;
using ModalDialogReturnValue = Presentation.Components.ModalDialog.ModalDialogReturnValue;

namespace Presentation.PresentationLogic.Space;

public class SpacePresenter : ISpacePresenter, ISpacePresenterToolboxInterface
{
    public SpacePresenter(
        IPresentationLogic presentationLogic, ILogger<SpacePresenter> logger)
    {
        _presentationLogic = presentationLogic;
        _logger = logger;
        EditSpaceDialogInitialValues = null;
        EditElementDialogInitialValues = null;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILogger<SpacePresenter> _logger;
    private bool _createElementDialogOpen;
    private int _creationCounter = 0;
    private int _activeSlot = -1;
    private ISpaceViewModel? _spaceVm;

    public ISpaceViewModel? SpaceVm
    {
        get => _spaceVm;
        private set => SetField(ref _spaceVm, value);
    }

    public ContentViewModel? DragAndDropContent { get; private set; }
    public IDisplayableObject? RightClickedObject { get; private set; }

    public void EditSpace(string name, string shortname, string authors, string description, string goals, int requiredPoints)
    {
        if (SpaceVm == null)
            throw new ApplicationException("SpaceVm is null");
        _presentationLogic.EditSpace(SpaceVm, name, shortname, authors, description, goals, requiredPoints);
    }

    public bool EditSpaceDialogOpen { get; set; }
    public IDictionary<string, string>? EditSpaceDialogInitialValues { get; private set; }
    public bool EditElementDialogOpen { get; set; }
    public IDictionary<string, string>? EditElementDialogInitialValues { get; private set; }

    public bool CreateElementDialogOpen
    {
        get => _createElementDialogOpen;
        set => SetField(ref _createElementDialogOpen, value);
    }
    
    public event Action OnUndoRedoPerformed
    {
        add => _presentationLogic.OnUndoRedoPerformed += value;
        remove => _presentationLogic.OnUndoRedoPerformed -= value;
    }
    
    public void SetSpaceLayout(FloorPlanEnum floorPlanName)
    {
        if (SpaceVm == null)
            throw new ApplicationException("SpaceVm is null");
        _presentationLogic.ChangeSpaceLayout(SpaceVm, floorPlanName);
    }

    public void DragElement(object sender, DraggedEventArgs<IElementViewModel> args)
    {
        _presentationLogic.DragElement(args.DraggableObject, args.OldPositionX, args.OldPositionY);
    }

    public void ClickedElement(IElementViewModel obj)
    {
        SetSelectedElement(obj);
    }

    public void RightClickedElement(IElementViewModel obj)
    {
        RightClickedObject = obj;
    }

    public void EditElement(IElementViewModel obj)
    {
        SetSelectedElement(obj);
        OpenEditSelectedElementDialog();
    }

    public void EditElement(int slotIndex)
    {
        if (SpaceVm == null)
            throw new ApplicationException("SpaceVm is null");
        var element = SpaceVm.SpaceLayout.GetElement(slotIndex);
        if (element == null)
            throw new ApplicationException($"Element at slotIndex {slotIndex} is null");
        SetSelectedElement(element);
        OpenEditSelectedElementDialog();
    }

    public void DeleteElement(IElementViewModel obj)
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        _presentationLogic.DeleteElement(SpaceVm, obj);
    }

    public void HideRightClickMenu()
    {
        RightClickedObject = null;
    }

    public async void ShowElementContent(IElementViewModel obj)
    {
        SetSelectedElement(obj);
        await ShowSelectedElementContentAsync();
    }

    public void SetSpace(ISpaceViewModel space)
    {
        SpaceVm = space;
    }
    public void OnWorldPropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(WorldViewModel.SelectedObject))
        {
            if (caller is not IWorldViewModel worldVm)
                throw new ArgumentException("Caller must be of type IWorldViewModel");
        
            if(worldVm.SelectedObject is SpaceViewModel space)
                SpaceVm = space;
        }
    }

    #region Space

    /// <summary>
    /// Changes property values of the space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok
    /// or if <see cref="SpaceVm"/> is null.</exception>
    public void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        EditSpaceDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var (key, value) in data)
        {
            _logger.LogTrace("{Key}:{Value}\\n", key, value);
        }

        //required arguments
        var name = data["Name"];
        //optional arguments
        var shortname = data.ContainsKey("Shortname") ? data["Shortname"] : "";
        var description = data.ContainsKey("Description") ? data["Description"] : "";
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        var requiredPoints = data.ContainsKey("Required Points") && data["Required Points"] != "" && !data["Required Points"].StartsWith("e") ? int.Parse(data["Required Points"]) : 0;

        if (SpaceVm == null)
            throw new ApplicationException("SpaceVm is null");
        _presentationLogic.EditSpace(SpaceVm, name, shortname, authors, description, goals, requiredPoints);
    }

    #endregion

    #region Element

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected Element.
    /// </summary>
    /// <exception cref="Exception">Thrown if Element Parent is null.</exception>
    private void OpenEditSelectedElementDialog()
    {
        var element = (ElementViewModel) SpaceVm?.SelectedElement!;
        if (element.Parent == null) throw new Exception("Element Parent is null");
        //prepare dictionary property to pass to dialog
        EditElementDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", element.Name},
            {"Shortname", element.Shortname},
            {"Url", element.Url},
            {"Authors", element.Authors},
            {"Description", element.Description},
            {"Goals", element.Goals},
            {"Difficulty", element.Difficulty.ToString()},
            {"Workload (min)", element.Workload.ToString()},
            {"Points", element.Points.ToString()}
        };
        EditElementDialogOpen = true;
    }

    public void AddNewElement(int slotIndex)
    {
        _activeSlot = slotIndex;
        CreateElementDialogOpen = true;
    }
    
    public void AddNewElement()
    {
        CreateElementDialogOpen = true;
    }

    /// <summary>
    /// Calls the LoadElementAsync method in <see cref="_presentationLogic"/> and adds the returned
    /// element to its parent.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="SpaceVm"/> is null</exception>
    public async Task LoadElementAsync(int slotIndex)
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        await _presentationLogic.LoadElementAsync(SpaceVm, slotIndex);
    }

    public void AddElement(IElementViewModel element, int slotIndex)
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        _presentationLogic.AddElement(SpaceVm, slotIndex, element);
    }
    
    /// <summary>
    /// Calls a load method in <see cref="_presentationLogic"/> depending on the content type and returns a
    /// ContentViewModel.
    /// </summary>
    /// <param name="contentType">The type of the content that can either be an image, a video, a pdf or a h5p.</param>
    /// <exception cref="ApplicationException">Thrown if there is no valid ContentType assigned.</exception>
    private async Task<ContentViewModel> LoadContent(ContentTypeEnum contentType)
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
    /// Creates a element with dialog return values after a content has been loaded.
    /// </summary>
    /// <param name="returnValueTuple">Modal dialog return values.</param>
    /// <exception cref="ApplicationException">Thrown if dialog data null or dropdown value or one of the dropdown
    /// values couldn't get parsed into enum.</exception>
    public void OnCreateElementDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        CreateElementDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel)
        {
            DragAndDropContent = null;
            _activeSlot = -1;
            return;
        }

        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            Console.Write($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        var name = data["Name"];
        var parentElement = GetElementParent();
        if(Enum.TryParse(data["Type"], out ElementTypeEnum elementType) == false)
            throw new ApplicationException("Couldn't parse returned element type");
        if (Enum.TryParse(data["Content"], out ContentTypeEnum contentType) == false)
            throw new ApplicationException("Couldn't parse returned content type");
        var description = data["Description"];
        //optional arguments
        var shortname = data.ContainsKey("Shortname") ? data["Shortname"] : "";
        var url = data.ContainsKey("Url") ? data["Url"] : "";
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        if (Enum.TryParse(data["Difficulty"], out ElementDifficultyEnum difficulty) == false)
            difficulty = ElementDifficultyEnum.None;
        if (Int32.TryParse(data["Workload (min)"], out int workload) == false || workload < 0)
            workload = 0;
        if (Int32.TryParse(data["Points"], out int points) == false || points < 0)
            points = 0;

        try
        { 
            ContentViewModel content;
            if (DragAndDropContent is not null)
            {
                content = DragAndDropContent;
                DragAndDropContent = null;
            }
            else if (contentType == ContentTypeEnum.Video)
            {
                content = new ContentViewModel("url", "url", "");
            }
            else
            {
                content = Task.Run(async () => await LoadContent(contentType)).Result;
            }
            var offset = 15 * _creationCounter;
            _creationCounter = (_creationCounter + 1) % 10;
            //TODO: We need the slotIndex here - AW
            if (_activeSlot < 0)
            {
                _activeSlot = 0;
            }
            _presentationLogic.CreateElement(parentElement, _activeSlot, name, shortname, elementType, contentType,
                content, url, authors, description, goals, difficulty, workload, points, offset, offset);
            _activeSlot = -1;

        }
        catch (AggregateException)
        {
                
        }
    }
    
    public void CreateElementWithPreloadedContent(ContentViewModel content)
    {
        DragAndDropContent = content;
        CreateElementDialogOpen = true;
    }

    /// <summary>
    /// Returns the parent of the element which is the selected space.
    /// </summary>
    /// <exception cref="Exception">Thrown if parent element is null.</exception>
    private ISpaceViewModel GetElementParent()
    {
        ISpaceViewModel? parentElement = SpaceVm;

        if (parentElement == null)
        {
            throw new Exception("Parent element is null");
        }

        return parentElement;
    }

    /// <summary>
    /// Changes property values of element viewmodel with return values of dialog
    /// </summary>
    /// <param name="returnValueTuple">Return values of dialog.</param>
    /// <exception cref="ApplicationException">Thrown if return values of dialog are null
    /// or selected object is not a element.</exception>
    public void OnEditElementDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        EditElementDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var (key, value) in data)
        {
            _logger.LogTrace("{Key}:{Value}\\n", key, value);
        }

        //required arguments
        var name = data["Name"];
        var parentElement = GetElementParent();
        var description = data["Description"];
        //optional arguments
        var shortname = data.ContainsKey("Shortname") ? data["Shortname"] : "";
        var url = data.ContainsKey("Url") ? data["Url"] : "";
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        if (Enum.TryParse(data["Difficulty"], out ElementDifficultyEnum difficulty) == false)
            difficulty = ElementDifficultyEnum.None;
        if (Int32.TryParse(data["Workload (min)"], out int workload) == false || workload < 0)
            workload = 0;
        if (Int32.TryParse(data["Points"], out int points) == false || points < 0)
            points = 0;
        
        if (SpaceVm?.SelectedElement is not ElementViewModel
            elementViewModel) throw new ApplicationException("DraggableObject is not a Element");
        _presentationLogic.EditElement(parentElement, elementViewModel, name, shortname, url, authors,
            description, goals, difficulty, workload, points);
    }

    /// <summary>
    /// Changes the selected <see cref="IElementViewModel"/> in the currently selected space.
    /// </summary>
    /// <param name="element">The element that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no space is currently selected.</exception>
    public void SetSelectedElement(IElementViewModel element)
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        SpaceVm.SelectedElement = element;
        HideRightClickMenu();
    }

    /// <summary>
    /// Deletes the selected element in the currently selected space and sets an other element as selected element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no space is currently selected.</exception>
    public void DeleteSelectedElement()
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        if (SpaceVm.SelectedElement == null)
            return;
        _presentationLogic.DeleteElement(SpaceVm, (ElementViewModel)SpaceVm.SelectedElement);
    }

    /// <summary>
    /// Opens the OpenEditDialog for the selected element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no space is currently selected.</exception>
    public void EditSelectedElement()
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        if (SpaceVm.SelectedElement == null)
            return;
        
        OpenEditSelectedElementDialog();
    }

    /// <summary>
    /// Calls the the Save methode for the selected element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no space is currently selected.</exception>
    public async Task SaveSelectedElementAsync()
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        switch (SpaceVm.SelectedElement)
        {
            case null:
                throw new ApplicationException("SelectedElement is null");
            case ElementViewModel element:
                await _presentationLogic.SaveElementAsync(element);
                break;
        }
    }
    
    /// <summary>
    /// Calls the the show element content method for the selected element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no space is currently selected or no element
    /// is selected in the space.</exception>
    public async Task ShowSelectedElementContentAsync()
    {
        if (SpaceVm == null)
            throw new ApplicationException("SelectedSpace is null");
        switch (SpaceVm.SelectedElement)
        {
            case null:
                throw new ApplicationException("SelectedElement is null");
            case ElementViewModel element:
                await _presentationLogic.ShowElementContentAsync(element);
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