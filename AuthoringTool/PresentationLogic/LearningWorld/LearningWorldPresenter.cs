using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.LearningWorld;

internal class LearningWorldPresenter : ILearningWorldPresenter, ILearningWorldPresenterToolboxInterface
{
    public LearningWorldPresenter(
        IPresentationLogic presentationLogic, ILearningSpacePresenter learningSpacePresenter,
        ILearningElementPresenter learningElementPresenter, ILogger<LearningWorldPresenter> logger)
    {
        _learningSpacePresenter = learningSpacePresenter;
        _learningElementPresenter = learningElementPresenter;
        _presentationLogic = presentationLogic;
        _logger = logger;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILearningElementPresenter _learningElementPresenter;
    private readonly ILogger<LearningWorldPresenter> _logger;

    /// <summary>
    /// The currently selected LearningWorldViewModel.
    /// </summary>
    public ILearningWorldViewModel? LearningWorldVm { get; private set; }
    
    public LearningContentViewModel? DragAndDropLearningContent { get; private set; }

    public bool SelectedLearningObjectIsSpace =>
        LearningWorldVm?.SelectedLearningObject?.GetType() == typeof(LearningSpaceViewModel);

    public bool ShowingLearningSpaceView => LearningWorldVm != null && LearningWorldVm.ShowingLearningSpaceView;
    public bool EditLearningSpaceDialogOpen { get; set; }
    public Dictionary<string, string>? EditSpaceDialogInitialValues { get; private set; }
    public Dictionary<string, string>? EditElementDialogInitialValues { get; private set; }
    public bool CreateLearningSpaceDialogueOpen { get; set; }
    public bool EditLearningElementDialogOpen { get; set; }
    public bool CreateLearningElementDialogOpen { get; set; }

    public void SetLearningWorld(object? caller, LearningWorldViewModel? world)
    {
        LearningWorldVm = world;
        if (LearningWorldVm != null) LearningWorldVm.ShowingLearningSpaceView = false;
    }

    public void ShowSelectedLearningSpaceView()
    {
        if (LearningWorldVm != null) LearningWorldVm.ShowingLearningSpaceView = true;
    }

    public void CloseLearningSpaceView()
    {
        if (LearningWorldVm != null) LearningWorldVm.ShowingLearningSpaceView = false;
    }

    public LearningWorldViewModel CreateNewLearningWorld(string name, string shortname, string authors,
        string language, string description, string goals)
    {
        return new LearningWorldViewModel(name, shortname, authors, language, description, goals);
    }

    public LearningWorldViewModel EditLearningWorld(LearningWorldViewModel world, string name, string shortname,
        string authors, string language, string description, string goals)
    {
        world.Name = name;
        world.Shortname = shortname;
        world.Authors = authors;
        world.Language = language;
        world.Description = description;
        world.Goals = goals;
        return world;
    }

    #region LearningSpace

    /// <summary>
    /// Creates a new learning space in the currently selected learning world.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void CreateNewLearningSpace(string name, string shortname,
        string authors, string description, string goals)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var learningSpace =
            _learningSpacePresenter.CreateNewLearningSpace(name, shortname, authors, description, goals);
        AddLearningSpace(learningSpace);
        SetSelectedLearningObject(learningSpace);
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected LearningSpace.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if SelectedLearningObject is not a LearningSpace. Shouldn't occur, because this is checked in <see cref="OpenEditSelectedLearningObjectDialog"/></exception>
    private void OpenEditSelectedLearningSpaceDialog()
    {
        var space = (LearningSpaceViewModel) LearningWorldVm?.SelectedLearningObject!;
        //prepare dictionary property to pass to dialog
        EditSpaceDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", space.Name},
            {"Shortname", space.Shortname},
            {"Authors", space.Authors},
            {"Description", space.Description},
            {"Goals", space.Goals},
        };
        EditLearningSpaceDialogOpen = true;
    }
    
    /// <inheritdoc cref="ILearningWorldPresenter.AddLearningSpace"/>
    public void AddLearningSpace(ILearningSpaceViewModel learningSpace)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (LearningWorldVm.LearningSpaces.Any(space => space.Name == learningSpace.Name))
            throw new ApplicationException("World already contains a space with same name");
        LearningWorldVm.LearningSpaces.Add(learningSpace);
    }

    /// <summary>
    /// Calls the LoadLearningSpaceAsync methode in <see cref="_presentationLogic"/> and adds the returned learning space to the current learning world.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="LearningWorldVm"/> is null</exception>
    public async Task LoadLearningSpace()
    {
        var learningSpace = await _presentationLogic.LoadLearningSpaceAsync();
        AddLearningSpace(learningSpace);
    }

    /// <summary>
    /// Creates a learning space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok</exception>
    public void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        CreateLearningSpaceDialogueOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            Console.Write($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        CreateNewLearningSpace(name, shortname, authors, description, goals);
    }

    /// <summary>
    /// Changes property values of learning space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok
    /// or if the selected learning object not a learning space</exception>
    public void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        EditLearningSpaceDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var (key, value) in data)
        {
            _logger.LogTrace($"{key}:{value}\n");
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

        if (LearningWorldVm == null)
            throw new ApplicationException("LearningWorld is null");
        if (LearningWorldVm.SelectedLearningObject is not LearningSpaceViewModel
            learningSpaceViewModel) throw new ApplicationException("LearningObject is not a LearningSpace");
        _learningSpacePresenter.EditLearningSpace(learningSpaceViewModel, name, shortname, authors,
            description, goals);
    }

    #endregion

    #region LearningElement

    /// <summary>
    /// Creates a new learning element and assigns it to the selected learning world or to a learning space in the
    /// selected learning world.
    /// </summary>
    /// <param name="name">Name of the element.</param>
    /// <param name="shortname">Shortname of the element.</param>
    /// <param name="parent">Parent of the element that can either be a world or a space.</param>
    /// <param name="elementType">Type of the element.</param>
    /// <param name="contentType">Type of the content that the element contains.</param>
    /// <param name="learningContent">The content of the element.</param>
    /// <param name="authors">A list of authors of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <param name="difficulty">The difficulty of the element.</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void CreateNewLearningElement(string name, string shortname, ILearningElementViewModelParent parent,
        ElementTypeEnum elementType, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var learningElement = elementType switch
        {
            ElementTypeEnum.Transfer => _learningElementPresenter.CreateNewTransferElement(name, shortname,
                parent, contentType, learningContent, authors, description, goals, difficulty, workload),
            ElementTypeEnum.Activation => _learningElementPresenter.CreateNewActivationElement(name, shortname,
                parent, contentType, learningContent, authors, description, goals, difficulty, workload),
            ElementTypeEnum.Interaction => _learningElementPresenter.CreateNewInteractionElement(name, shortname,
                parent, contentType, learningContent, authors, description, goals, difficulty, workload),
            ElementTypeEnum.Test => _learningElementPresenter.CreateNewTestElement(name, shortname,
                parent, contentType, learningContent, authors, description, goals, difficulty, workload),
            _ => throw new ApplicationException("no valid ElementType assigned")
        };

        SetSelectedLearningObject(learningElement);
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected LearningElement.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if SelectedLearningObject is not a LearningElementViewModel.
    /// Shouldn't occur, because this is checked in <see cref="OpenEditSelectedLearningObjectDialog"/></exception>
    private void OpenEditSelectedLearningElementDialog()
    {
        var element = (LearningElementViewModel) LearningWorldVm?.SelectedLearningObject!;
        if (element.Parent == null) throw new Exception("Element Parent is null");
        //prepare dictionary property to pass to dialog
        EditElementDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", element.Name},
            {"Shortname", element.Shortname},
            {"Parent", ElementParentEnum.World.ToString()},
            {"Assignment", element.Parent.Name},
            {"Authors", element.Authors},
            {"Description", element.Description},
            {"Goals", element.Goals},
            {"Difficulty", element.Difficulty.ToString()},
            {"Workload (min)", element.Workload.ToString()}
        };
        EditLearningElementDialogOpen = true;
    }
    
    public void AddLearningElement(ILearningElementViewModel learningElement)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (LearningWorldVm.LearningElements.Any(elements => elements.Name == learningElement.Name))
            throw new ApplicationException("World already contains an element with same name");
        learningElement.Parent = LearningWorldVm;
        LearningWorldVm.LearningElements.Add(learningElement);
    }

    /// <summary>
    /// Calls the LoadLearningElementAsync method in <see cref="_presentationLogic"/> and adds the returned
    /// learning element to its parent.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="LearningWorldVm"/> is null</exception>
    public async Task LoadLearningElement()
    {
        var learningElement = await _presentationLogic.LoadLearningElementAsync();
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        learningElement.Parent = LearningWorldVm;
        AddLearningElement(learningElement);
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
            ContentTypeEnum.Pdf => await _presentationLogic.LoadPdfAsync(),
            ContentTypeEnum.H5P => await _presentationLogic.LoadH5pAsync(),
            _ => throw new ApplicationException("No valid ContentType assigned")
        };
    }

    /// <summary>
    /// Creates a learning element with dialog return values after a content has been loaded.
    /// </summary>
    /// <param name="returnValueTuple">Modal dialog return values.</param>
    /// <exception cref="ApplicationException">Thrown if dialog data null or dropdown value or one of the dropdown
    /// values couldn't get parsed into enum.</exception>
    public void OnCreateElementDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        CreateLearningElementDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel)
        {
            DragAndDropLearningContent = null;
            return;
        }
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            Console.Write($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        if (Enum.TryParse(data["Parent"], out ElementParentEnum parent) == false)
            throw new ApplicationException("Couldn't parse returned parent type");
        var assignment = data["Assignment"];
        var parentElement = GetLearningElementParent(parent, assignment);
        if (Enum.TryParse(data["Type"], out ElementTypeEnum elementType) == false)
            throw new ApplicationException("Couldn't parse returned element type");
        if (Enum.TryParse(data["Content"], out ContentTypeEnum contentType) == false)
            throw new ApplicationException("Couldn't parse returned content type");
        var description = data["Description"];
        if (Enum.TryParse(data["Difficulty"], out LearningElementDifficultyEnum difficulty) == false)
            throw new ApplicationException("Couldn't parse returned difficulty");
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        if (Int32.TryParse(data["Workload (min)"], out int workload) == false || workload < 0)
            workload = 0;

        try
        {
            LearningContentViewModel learningContent;
            if (DragAndDropLearningContent is not null)
            {
                learningContent = DragAndDropLearningContent;
                DragAndDropLearningContent = null;
            }
            else
            {
                learningContent = Task.Run(async () => await LoadLearningContent(contentType)).Result;
            }
            
            CreateNewLearningElement(name, shortname, parentElement, elementType, contentType, learningContent, authors,
                description, goals, difficulty, workload);
        }
        catch (AggregateException)
        {
                
        }
    }
    
    public void CreateLearningElementWithPreloadedContent(LearningContentViewModel learningContent)
    {
        DragAndDropLearningContent = learningContent;
        CreateLearningElementDialogOpen = true;
    }

    /// <summary>
    /// Returns the parent viewmodel of the learning element. 
    /// </summary>
    /// <param name="parent">Type of parent that can be either a space or a world.</param>
    /// <param name="assignment">Name of the parent.</param>
    /// <exception cref="ApplicationException">Thrown if parent is neither a space or a world.</exception>
    /// <exception cref="Exception">Thrown if parent element is null</exception>
    private ILearningElementViewModelParent GetLearningElementParent(ElementParentEnum parent, string assignment)
    {
        ILearningElementViewModelParent? parentElement = parent switch
        {
            ElementParentEnum.Space => LearningWorldVm?.LearningSpaces.FirstOrDefault(space =>
                space.Name == assignment),
            ElementParentEnum.World => LearningWorldVm,
            _ => throw new ApplicationException("No valid element parent")
        };

        if (parentElement == null)
        {
            throw new Exception("Parent element is null");
        }

        return parentElement;
    }

    /// <summary>
    /// Changes property values of learning element viewmodel with return values of dialog
    /// </summary>
    /// <param name="returnValueTuple">Return values of dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if return values of dialog null
    /// or selected learning object not a learning element</exception>
    public void OnEditElementDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        EditLearningElementDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var (key, value) in data)
        {
            _logger.LogTrace($"{key}:{value}\n");
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        if(Enum.TryParse(data["Parent"], out ElementParentEnum parent) == false)
            throw new ApplicationException("Couldn't parse returned parent type");
        var assignment = data["Assignment"];
        var parentElement = GetLearningElementParent(parent, assignment);
        var description = data["Description"];
        if (Enum.TryParse(data["Difficulty"], out LearningElementDifficultyEnum difficulty) == false)
            throw new ApplicationException("Couldn't parse returned difficulty");
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        if (Int32.TryParse(data["Workload (min)"], out int workload) == false || workload < 0)
            workload = 0;

        if (LearningWorldVm == null)
            throw new ApplicationException("LearningWorldVm is null");
        if (LearningWorldVm.SelectedLearningObject is not LearningElementViewModel
            learningElementViewModel) throw new ApplicationException("LearningObject is not a LearningElement");
        _learningElementPresenter.EditLearningElement(learningElementViewModel, name, shortname, parentElement,
            authors, description, goals, difficulty, workload);
    }

    #endregion

    #region LearningObject

    /// <summary>
    /// Changes the selected <see cref="ILearningObjectViewModel"/> in the currently selected learning world.
    /// </summary>
    /// <param name="learningObject">The learning object that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void SetSelectedLearningObject(ILearningObjectViewModel learningObject)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        LearningWorldVm.SelectedLearningObject = learningObject;
        if (SelectedLearningObjectIsSpace)
            _learningSpacePresenter.SetLearningSpace(
                (LearningSpaceViewModel) LearningWorldVm.SelectedLearningObject);
    }

    /// <summary>
    /// Deletes the selected learning object in the currently selected learning world and sets an other space or element as selected learning object.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than space or element.</exception>
    public void DeleteSelectedLearningObject()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        switch (LearningWorldVm.SelectedLearningObject)
        {
            case null:
                return;
            case LearningSpaceViewModel learningSpace:
                LearningWorldVm.LearningSpaces.Remove(learningSpace);
                break;
            case LearningElementViewModel learningElement:
                _learningElementPresenter.RemoveLearningElementFromParentAssignment(learningElement);
                break;
            default:
                throw new NotImplementedException("Type of LearningObject is not implemented");
        }

        LearningWorldVm.SelectedLearningObject =
            (ILearningObjectViewModel?) LearningWorldVm?.LearningSpaces
                .LastOrDefault() ??
            LearningWorldVm?.LearningElements.LastOrDefault();
    }

    /// <summary>
    /// Opens the respective OpenEditDialog for Learning Space or Learning Element depending on which learning object is selected.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than space or element.</exception>
    public void OpenEditSelectedLearningObjectDialog()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        switch (LearningWorldVm.SelectedLearningObject)
        {
            case null:
                return;
            case LearningSpaceViewModel:
                OpenEditSelectedLearningSpaceDialog();
                break;
            case LearningElementViewModel:
                OpenEditSelectedLearningElementDialog();
                break;
            default:
                throw new NotImplementedException("Type of LearningObject is not implemented");
        }
    }

    /// <summary>
    /// Calls the respective Save methode for Learning Space or Learning Element depending on which learning object is selected
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than space or element.</exception>
    public async Task SaveSelectedLearningObjectAsync()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        switch (LearningWorldVm.SelectedLearningObject)
        {
            case null:
                throw new ApplicationException("SelectedLearningObject is null");
            case LearningSpaceViewModel learningSpace:
                await _presentationLogic.SaveLearningSpaceAsync(learningSpace);
                break;
            case LearningElementViewModel learningElement:
                await _presentationLogic.SaveLearningElementAsync(learningElement);
                break;
            default:
                throw new NotImplementedException("Type of LearningObject is not implemented");
        }
    }

    #endregion
}