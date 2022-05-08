using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.LearningWorld;

internal class LearningWorldPresenter : ILearningWorldPresenter
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

    public LearningWorldViewModel? LearningWorldVm { get; private set; }

    public bool SelectedLearningObjectIsSpace =>
        LearningWorldVm?.SelectedLearningObject?.GetType() == typeof(LearningSpaceViewModel);

    public bool ShowingLearningSpaceView => LearningWorldVm != null && LearningWorldVm.ShowingLearningSpaceView;
    public bool EditLearningSpaceDialogOpen { get; set; }
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
        LearningWorldVm.LearningSpaces.Add(learningSpace);
        SetSelectedLearningObject(learningSpace);
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected LearningSpace.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if SelectedLearningObject is not a LearningSpace. Shouldn't occur, because this is checked in <see cref="OpenEditSelectedLearningObjectDialog"/></exception>
    private void OpenEditSelectedLearningSpaceDialog()
    {
        if (LearningWorldVm?.SelectedLearningObject is not LearningSpaceViewModel
            space) throw new ApplicationException("Type of LearningObject is not LearningSpace");
        //prepare dictionary property to pass to dialog
        LearningWorldVm.EditDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", space.Name},
            {"Shortname", space.Shortname},
            {"Authors", space.Authors},
            {"Description", space.Description},
            {"Goals", space.Goals},
        };
        EditLearningSpaceDialogOpen = true;
    }

    /// <summary>
    /// Calls the LoadLearningSpaceAsync methode in <see cref="_presentationLogic"/> and adds the returned learning space to the current learning world.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="LearningWorldVm"/> is null</exception>
    public async Task LoadLearningSpace()
    {
        var learningSpace = await _presentationLogic.LoadLearningSpaceAsync();
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        LearningWorldVm.LearningSpaces.Add(learningSpace);
    }

    /// <summary>
    /// Creates a learning space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok</exception>
    public Task OnCreateSpaceDialogClose(
        Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        CreateLearningSpaceDialogueOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
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
        return Task.CompletedTask;
    }

    /// <summary>
    /// Changes property values of learning space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok
    /// or if the selected learning object not a learning space</exception>
    public Task OnEditSpaceDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        EditLearningSpaceDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
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
        return Task.CompletedTask;
    }

    public IEnumerable<ModalDialogInputField> ModalDialogSpaceInputFields
    {
        get
        {
            return new ModalDialogInputField[]
            {
                new("Name", ModalDialogInputType.Text, true),
                new("Shortname", ModalDialogInputType.Text, true),
                new("Authors", ModalDialogInputType.Text),
                new("Description", ModalDialogInputType.Text, true),
                new("Goals", ModalDialogInputType.Text)
            };
        }
    }

    #endregion

    #region LearningElement

    /// <summary>
    /// Creates a new learning element and assigns it to the selected learning world or to a learning space in the
    /// selected learning world.
    /// </summary>
    /// <param name="name">Name of the element.</param>
    /// <param name="shortname">Shortname of the element.</param>
    /// <param name="parent">Decides whether the learning element belongs to a learning world or a learning space</param>
    /// <param name="elementType">The represented type of the element in the space/world.</param>
    /// <param name="contentType">Describes, which content the element contains.</param>
    /// <param name="authors">A list of authors of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void CreateNewLearningElement(string name, string shortname, ILearningElementViewModelParent parent,
        string elementType, string contentType, string authors, string description, string goals)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var learningElement =
            _learningElementPresenter.CreateNewLearningElement(name, shortname, parent, elementType,
                contentType, authors, description, goals);

        SetSelectedLearningObject(learningElement);
    }

    private void OpenEditSelectedLearningElementDialog()
    {
        if (LearningWorldVm?.SelectedLearningObject is not LearningElementViewModel
            element) throw new ApplicationException("Type of LearningObject is not implemented");
        if (element.Parent == null) throw new Exception("Element Parent is null");
        //prepare dictionary property to pass to dialog
        LearningWorldVm.EditDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", element.Name},
            {"Shortname", element.Shortname},
            {
                "Parent",
                element.Parent switch
                {
                    LearningWorldViewModel => "Learning world", LearningSpaceViewModel => "Learning space",
                    _ => ""
                }
            },
            {"Assignment", element.Parent.Name},
            {"Type", element.ElementType},
            {"Content", element.ContentType},
            {"Authors", element.Authors},
            {"Description", element.Description},
            {"Goals", element.Goals},
        };
        EditLearningElementDialogOpen = true;
    }

    public async Task LoadLearningElement()
    {
        var learningElement = await _presentationLogic.LoadLearningElementAsync();
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        learningElement.Parent = LearningWorldVm;
        LearningWorldVm.LearningElements.Add(learningElement);
    }

    public async Task LoadLearningContent()
    {
        var learningContent = await _presentationLogic.LoadLearningContentAsync();
        if (LearningWorldVm == null)
        {
            throw new ApplicationException("SelectedLearningWorld is null");
        }

        switch (LearningWorldVm.SelectedLearningObject)
        {
            case null:
                throw new ApplicationException("SelectedLearningObject is null");
            case LearningSpaceViewModel:
                throw new ApplicationException("LearningSpace instead of LearningElement selected");
            case LearningElementViewModel learningElement:
                learningElement.LearningContent = learningContent;
                break;
        }
    }


    public Task OnCreateElementDialogClose(
        Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        CreateLearningElementDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            Console.Write($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        var parent = data["Parent"];
        var assignment = data["Assignment"];
        var parentElement = GetLearningElementParent(parent, assignment);

        var elementType = data["Type"];
        var contentType = data["Content"];
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        CreateNewLearningElement(name, shortname, parentElement, elementType, contentType, authors, description,
            goals);
        return Task.CompletedTask;
    }

    private ILearningElementViewModelParent GetLearningElementParent(string parent, string assignment)
    {
        ILearningElementViewModelParent? parentElement;
        if (parent == "Learning space")
        {
            parentElement =
                LearningWorldVm?.LearningSpaces.FirstOrDefault(space =>
                    space.Name == assignment);
        }
        else
        {
            parentElement = LearningWorldVm;
        }

        if (parentElement == null)
        {
            throw new Exception("no parent for element");
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
    public Task OnEditElementDialogClose(
        Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        EditLearningElementDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var (key, value) in data)
        {
            _logger.LogTrace($"{key}:{value}\n");
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        var parent = data["Parent"];
        var assignment = data["Assignment"];
        var parentElement = GetLearningElementParent(parent, assignment);
        var elementType = data["Type"];
        var contentType = data["Content"];
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

        if (LearningWorldVm == null)
            throw new ApplicationException("LearningWorld is null");
        if (LearningWorldVm.SelectedLearningObject is not LearningElementViewModel
            learningElementViewModel) throw new ApplicationException("LearningObject is not a LearningElement");
        _learningElementPresenter.EditLearningElement(learningElementViewModel, name, shortname, parentElement,
            elementType, contentType, authors, description, goals);
        return Task.CompletedTask;
    }

    public IEnumerable<ModalDialogInputField> ModalDialogElementInputFields
    {
        get
        {
            return new ModalDialogInputField[]
            {
                new("Name", ModalDialogInputType.Text, true),
                new("Shortname", ModalDialogInputType.Text, true),
                new ModalDialogDropdownInputField("Parent",
                    new[]
                    {
                        new ModalDialogDropdownInputFieldChoiceMapping(null,
                            new[] {"Learning world", "Learning space"})
                    }, true),
                new ModalDialogDropdownInputField("Assignment",
                    new[]
                    {
                        new ModalDialogDropdownInputFieldChoiceMapping(
                            new Dictionary<string, string> {{"Parent", "Learning space"}},
                            LearningWorldVm!.LearningSpaces.Select(space => space.Name)),
                        new ModalDialogDropdownInputFieldChoiceMapping(
                            new Dictionary<string, string> {{"Parent", "Learning world"}},
                            new[] {LearningWorldVm.Name})
                    }, true),
                new ModalDialogDropdownInputField("Type",
                    new[]
                    {
                        new ModalDialogDropdownInputFieldChoiceMapping(null,
                            new[] {"Transfer", "Activation", "Interaction", "Test"})
                    }, true),
                new ModalDialogDropdownInputField("Content",
                    new[]
                    {
                        new ModalDialogDropdownInputFieldChoiceMapping(null,
                            new[] {"Picture", "Video", "H5P", "PDF"})
                    }, true),
                new("Authors", ModalDialogInputType.Text),
                new("Description", ModalDialogInputType.Text, true),
                new("Goals", ModalDialogInputType.Text)
            };
        }
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