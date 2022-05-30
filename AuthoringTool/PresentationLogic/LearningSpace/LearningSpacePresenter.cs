using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.LearningSpace;

internal class LearningSpacePresenter : ILearningSpacePresenter
{
    public LearningSpacePresenter(
        IPresentationLogic presentationLogic, ILearningElementPresenter learningElementPresenter,
        ILogger<LearningWorldPresenter> logger)
    {
        _learningElementPresenter = learningElementPresenter;
        _presentationLogic = presentationLogic;
        _logger = logger;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningElementPresenter _learningElementPresenter;
    private readonly ILogger<LearningWorldPresenter> _logger;

    public LearningSpaceViewModel? LearningSpaceVm { get; private set; }

    public LearningSpaceViewModel CreateNewLearningSpace(string name, string shortname, string authors,
        string description, string goals)
    {
        return new LearningSpaceViewModel(name, shortname, authors, description, goals);
    }

    public LearningSpaceViewModel EditLearningSpace(LearningSpaceViewModel space, string name, string shortname,
        string authors, string description, string goals)
    {
        space.Name = name;
        space.Shortname = shortname;
        space.Authors = authors;
        space.Description = description;
        space.Goals = goals;
        return space;
    }

    public bool EditLearningSpaceDialogOpen { get; set; }
    public bool EditLearningElementDialogOpen { get; set; }
    public bool CreateLearningElementDialogOpen { get; set; }

    public void SetLearningSpace(LearningSpaceViewModel space)
    {
        LearningSpaceVm = space;
    }

    #region LearningSpace

    /// <summary>
    /// Opens the edit dialog for the currently opened learning space. (This methode is not yet in use)
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="LearningSpaceVm"/> is null</exception>
    private void OpenEditThisLearningSpaceDialog()
    {
        if (LearningSpaceVm is null) throw new ApplicationException("LearningSpaceVm is null");
        //prepare dictionary property to pass to dialog
        LearningSpaceVm.EditDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", LearningSpaceVm.Name},
            {"Shortname", LearningSpaceVm.Shortname},
            {"Authors", LearningSpaceVm.Authors},
            {"Description", LearningSpaceVm.Description},
            {"Goals", LearningSpaceVm.Goals},
        };
        EditLearningSpaceDialogOpen = true;
    }

    /// <summary>
    /// Changes property values of the learning space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok
    /// or if <see cref="LearningSpaceVm"/> is null.</exception>
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

        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        EditLearningSpace(LearningSpaceVm, name, shortname, authors,
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
    /// Creates a new learning element and assigns it to the selected learning space.
    /// </summary>
    /// <param name="name">Name of the element.</param>
    /// <param name="shortname">Shortname of the element.</param>
    /// <param name="parent">Parent of the learning element (selected learning space)</param>
    /// <param name="learningContent">The content of the element.</param>
    /// <param name="authors">A list of authors of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public void CreateNewLearningElement(string name, string shortname, ILearningElementViewModelParent parent,
        LearningContentViewModel learningContent,string authors, string description, string goals)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        var learningElement =
            _learningElementPresenter.CreateNewLearningElement(name, shortname, parent, learningContent, authors,
                description, goals);

        SetSelectedLearningObject(learningElement);
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected LearningElement.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if SelectedLearningObject is not a LearningElementViewModel.
    /// Shouldn't occur, because this is checked in <see cref="OpenEditSelectedLearningObjectDialog"/></exception>
    private void OpenEditSelectedLearningElementDialog()
    {
        if (LearningSpaceVm?.SelectedLearningObject is not LearningElementViewModel
            element) throw new ApplicationException("Type of LearningObject is not implemented");
        if (element.Parent == null) throw new Exception("Element Parent is null");
        //prepare dictionary property to pass to dialog
        LearningSpaceVm.EditDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", element.Name},
            {"Shortname", element.Shortname},
            {
                "Parent",
                element.Parent switch
                {
                    LearningWorldViewModel => ElementParentEnum.World.ToString(),
                    LearningSpaceViewModel => ElementParentEnum.Space.ToString(),
                    _ => ""
                }
            },
            {"Assignment", element.Parent.Name},
            {"Authors", element.Authors},
            {"Description", element.Description},
            {"Goals", element.Goals},
        };
        EditLearningElementDialogOpen = true;
    }

    /// <summary>
    /// Calls the LoadLearningElementAsync method in <see cref="_presentationLogic"/> and adds the returned
    /// learning element to its parent.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="LearningSpaceVm"/> is null</exception>
    public async Task LoadLearningElement()
    {
        var learningElement = await _presentationLogic.LoadLearningElementAsync();
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        learningElement.Parent = LearningSpaceVm;
        LearningSpaceVm.LearningElements.Add(learningElement);
    }
    
    /// <summary>
    /// Calls a load method in <see cref="_presentationLogic"/> depending on the content type and returns a
    /// LearningContentViewModel.
    /// </summary>
    /// <param name="contentType">The type of the content that can either be an image, a video, a pdf or a h5p.</param>
    /// <exception cref="ApplicationException">Thrown if there is no valid ContentType assigned.</exception>
    private async Task<LearningContentViewModel> LoadLearningContent(ContentTypeEnum contentType)
    {
        if (LearningSpaceVm == null)
        {
            throw new ApplicationException("SelectedLearningSpace is null");
        }

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
        var parentElement = GetLearningElementParent();
        if(Enum.TryParse(data["Type"], out ElementTypeEnum elementType) == false)
            throw new ApplicationException("Couldn't parse returned element type");
        if (Enum.TryParse(data["Content"], out ContentTypeEnum contentType) == false)
            throw new ApplicationException("Couldn't parse returned content type");
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        
        try
        {
            var learningContent = Task.Run(async () => await LoadLearningContent(contentType)).Result;
            CreateNewLearningElement(name, shortname, parentElement, learningContent, authors, description, goals);
        }
        catch (AggregateException)
        {
                
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns the parent of the learning element which is the selected learning space.
    /// </summary>
    /// <exception cref="Exception">Thrown if parent element is null.</exception>
    private ILearningElementViewModelParent GetLearningElementParent()
    {
        ILearningElementViewModelParent? parentElement = LearningSpaceVm;

        if (parentElement == null)
        {
            throw new Exception("no parent for element");
        }

        return parentElement;
    }

    /// <summary>
    /// Changes property values of learning element viewmodel with return values of dialog
    /// </summary>
    /// <param name="returnValueTuple">Return values of dialog.</param>
    /// <exception cref="ApplicationException">Thrown if return values of dialog are null
    /// or selected learning object is not a learning element.</exception>
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
        var parentElement = GetLearningElementParent();
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        if (LearningSpaceVm.SelectedLearningObject is not LearningElementViewModel
            learningElementViewModel) throw new ApplicationException("LearningObject is not a LearningElement");
        _learningElementPresenter.EditLearningElement(learningElementViewModel, name, shortname, parentElement,
            authors, description, goals);
        return Task.CompletedTask;
    }

    public IEnumerable<ModalDialogInputField> ModalDialogCreateElementInputFields
        {
            get
            {
                return new ModalDialogInputField[]
                {
                    new("Name", ModalDialogInputType.Text, true),
                    new("Shortname", ModalDialogInputType.Text, true),
                    new ModalDialogDropdownInputField("Type",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(null, 
                                new[] {ElementTypeEnum.Transfer.ToString(), ElementTypeEnum.Activation.ToString(),
                                    ElementTypeEnum.Interaction.ToString(), ElementTypeEnum.Test.ToString()})
                        }, true),
                    new ModalDialogDropdownInputField("Content",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Transfer.ToString()}},
                                new[] {ContentTypeEnum.Image.ToString(), ContentTypeEnum.Video.ToString(), ContentTypeEnum.Pdf.ToString()}),
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Activation.ToString()}},
                                new[] {ContentTypeEnum.H5P.ToString(),ContentTypeEnum.Video.ToString()}),
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Interaction.ToString()}},
                                new[] {ContentTypeEnum.H5P.ToString()}),
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Test.ToString()}},
                                new[] {ContentTypeEnum.H5P.ToString()})
                        }, true),
                    new("Authors", ModalDialogInputType.Text),
                    new("Description", ModalDialogInputType.Text, true),
                    new("Goals", ModalDialogInputType.Text)
                };
            }
        }
        
        public IEnumerable<ModalDialogInputField> ModalDialogEditElementInputFields
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

    #region LearningObject

    /// <summary>
    /// Changes the selected <see cref="ILearningObjectViewModel"/> in the currently selected learning world.
    /// </summary>
    /// <param name="learningObject">The learning object that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void SetSelectedLearningObject(ILearningObjectViewModel learningObject)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        LearningSpaceVm.SelectedLearningObject = learningObject;
    }

    /// <summary>
    /// Deletes the selected learning object in the currently selected learning world and sets an other element as selected learning object.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than element.</exception>
    public void DeleteSelectedLearningObject()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        switch (LearningSpaceVm.SelectedLearningObject)
        {
            case null:
                return;
            case LearningElementViewModel learningElement:
                _learningElementPresenter.RemoveLearningElementFromParentAssignment(learningElement);
                break;
            default:
                throw new NotImplementedException("Type of LearningObject is not implemented");
        }

        LearningSpaceVm.SelectedLearningObject = LearningSpaceVm?.LearningElements.LastOrDefault();
    }

    /// <summary>
    /// Opens the OpenEditDialog for Learning Element if the selected learning object is an learning element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than element.</exception>
    public void OpenEditSelectedLearningObjectDialog()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        switch (LearningSpaceVm.SelectedLearningObject)
        {
            case null:
                return;
            case LearningElementViewModel:
                OpenEditSelectedLearningElementDialog();
                break;
            default:
                throw new NotImplementedException("Type of LearningObject is not implemented");
        }
    }

    /// <summary>
    /// Calls the the Save methode for Learning Element if the selected learning object is an learning element.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than element.</exception>
    public async Task SaveSelectedLearningObjectAsync()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        switch (LearningSpaceVm.SelectedLearningObject)
        {
            case null:
                throw new ApplicationException("SelectedLearningObject is null");
            case LearningElementViewModel learningElement:
                await _presentationLogic.SaveLearningElementAsync(learningElement);
                break;
            default:
                throw new NotImplementedException("Type of LearningObject is not implemented");
        }
    }

    #endregion
}