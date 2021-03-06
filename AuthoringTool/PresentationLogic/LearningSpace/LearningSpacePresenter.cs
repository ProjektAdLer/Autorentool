using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.LearningSpace;

internal class LearningSpacePresenter : ILearningSpacePresenter, ILearningSpacePresenterToolboxInterface
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

    private LearningContentViewModel? _dragAndDropLearningContent = null;
    public bool DraggedLearningContentIsPresent => _dragAndDropLearningContent is not null;

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

    #endregion

    #region LearningElement

    /// <summary>
    /// Creates a new learning element and assigns it to the selected learning space.
    /// </summary>
    /// <param name="name">Name of the element.</param>
    /// <param name="shortname">Shortname of the element.</param>
    /// <param name="parent">Parent of the learning element (selected learning space).</param>
    /// <param name="elementType">Type of the element.</param>
    /// <param name="contentType">Type of the content that the element contains.</param>
    /// <param name="learningContent">The content of the element.</param>
    /// <param name="authors">A list of authors of the element.</param>
    /// <param name="description">A description of the element.</param>
    /// <param name="goals">The goals of the element.</param>
    /// <param name="difficulty">The difficulty of the element.</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public void CreateNewLearningElement(string name, string shortname, ILearningElementViewModelParent parent,
        ElementTypeEnum elementType, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
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
        var element = (LearningElementViewModel) LearningSpaceVm?.SelectedLearningObject!;
        if (element.Parent == null) throw new Exception("Element Parent is null");
        //prepare dictionary property to pass to dialog
        LearningSpaceVm!.EditDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", element.Name},
            {"Shortname", element.Shortname},
            {"Authors", element.Authors},
            {"Description", element.Description},
            {"Goals", element.Goals},
            {"Difficulty", element.Difficulty.ToString()},
            {"Workload (min)", element.Workload.ToString()}
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
        AddLearningElement(learningElement);
    }

    public void AddLearningElement(LearningElementViewModel element)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        LearningSpaceVm.LearningElements.Add(element);
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

        if (response == ModalDialogReturnValue.Cancel)
        {
            _dragAndDropLearningContent = null;
            return Task.CompletedTask;
        }

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
            if (_dragAndDropLearningContent is not null)
            {
                learningContent = _dragAndDropLearningContent;
                _dragAndDropLearningContent = null;
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
        return Task.CompletedTask;
    }
    
    public void CreateLearningElementWithPreloadedContent(LearningContentViewModel learningContent)
    {
        _dragAndDropLearningContent = learningContent;
        CreateLearningElementDialogOpen = true;
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
            throw new Exception("Parent element is null");
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
        if (Enum.TryParse(data["Difficulty"], out LearningElementDifficultyEnum difficulty) == false)
            throw new ApplicationException("Couldn't parse returned difficulty");
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        if (Int32.TryParse(data["Workload (min)"], out int workload) == false || workload < 0)
            workload = 0;
        
        if (LearningSpaceVm?.SelectedLearningObject is not LearningElementViewModel
            learningElementViewModel) throw new ApplicationException("LearningObject is not a LearningElement");
        _learningElementPresenter.EditLearningElement(learningElementViewModel, name, shortname, parentElement,
            authors, description, goals, difficulty, workload);
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
                            new[]
                            {
                                ElementTypeEnum.Transfer.ToString(), ElementTypeEnum.Activation.ToString(),
                                ElementTypeEnum.Interaction.ToString(), ElementTypeEnum.Test.ToString()
                            })
                    }, true),
                new ModalDialogDropdownInputField("Content",
                    new[]
                    {
                        new ModalDialogDropdownInputFieldChoiceMapping(
                            new Dictionary<string, string> {{"Type", ElementTypeEnum.Transfer.ToString()}},
                            new[]
                            {
                                ContentTypeEnum.Image.ToString(), ContentTypeEnum.Video.ToString(),
                                ContentTypeEnum.Pdf.ToString()
                            }),
                        new ModalDialogDropdownInputFieldChoiceMapping(
                            new Dictionary<string, string> {{"Type", ElementTypeEnum.Activation.ToString()}},
                            new[] {ContentTypeEnum.H5P.ToString(), ContentTypeEnum.Video.ToString()}),
                        new ModalDialogDropdownInputFieldChoiceMapping(
                            new Dictionary<string, string> {{"Type", ElementTypeEnum.Interaction.ToString()}},
                            new[] {ContentTypeEnum.H5P.ToString()}),
                        new ModalDialogDropdownInputFieldChoiceMapping(
                            new Dictionary<string, string> {{"Type", ElementTypeEnum.Test.ToString()}},
                            new[] {ContentTypeEnum.H5P.ToString()})
                    }, true),
                new("Authors", ModalDialogInputType.Text),
                new("Description", ModalDialogInputType.Text, true),
                new("Goals", ModalDialogInputType.Text),
                new ModalDialogDropdownInputField("Difficulty",
                    new[]
                    {
                        new ModalDialogDropdownInputFieldChoiceMapping(null,
                            new[] {LearningElementDifficultyEnum.Easy.ToString(),
                                LearningElementDifficultyEnum.Medium.ToString(),
                                LearningElementDifficultyEnum.Hard.ToString(),
                                LearningElementDifficultyEnum.None.ToString() })
                    }, true),
                new("Workload (min)", ModalDialogInputType.Number)
            };
        }
    }

    public IEnumerable<ModalDialogInputField> ModalDialogCreateElementCustomInputFields
    {
        get
        {
            if (_dragAndDropLearningContent is null)
            {
                throw new Exception(
                    "ModalDialogCreateElementCustomInputFields where called, but _dragAndDropLearningContent is null");
            }


            ModalDialogDropdownInputField typeField;
            ModalDialogDropdownInputField contentField;

            ContentTypeEnum contentType;
            switch (_dragAndDropLearningContent.Type)
            {
                case "jpg":
                case "png":
                case "webp":
                case "bmp":
                    contentType = ContentTypeEnum.Image;
                    break;
                case "mp4":
                    contentType = ContentTypeEnum.Video;
                    break;
                case "h5p":
                    contentType = ContentTypeEnum.H5P;
                    break;
                case "pdf":
                    contentType = ContentTypeEnum.Pdf;
                    break;
                    default: throw new Exception($"Can not map the file extension '{_dragAndDropLearningContent.Type}' to an ContentType ");
                
            }

            switch (contentType)
            {
                case ContentTypeEnum.Image:
                    typeField = new ModalDialogDropdownInputField("Type",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(null,
                                new[]
                                {
                                    ElementTypeEnum.Transfer.ToString()
                                })
                        }, true);
                    contentField = new ModalDialogDropdownInputField("Content",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Transfer.ToString()}},
                                new[]
                                {
                                    ContentTypeEnum.Image.ToString()
                                })
                        }, true);
                    break;
                case ContentTypeEnum.Video:
                    typeField = new ModalDialogDropdownInputField("Type",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(null,
                                new[]
                                {
                                    ElementTypeEnum.Transfer.ToString(), ElementTypeEnum.Activation.ToString()
                                })
                        }, true);
                    contentField = new ModalDialogDropdownInputField("Content",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Transfer.ToString()}},
                                new[] {ContentTypeEnum.Video.ToString()}),
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Activation.ToString()}},
                                new[] {ContentTypeEnum.Video.ToString()})
                        }, true);
                    break;
                case ContentTypeEnum.Pdf:
                    typeField = new ModalDialogDropdownInputField("Type",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(null,
                                new[] {ElementTypeEnum.Transfer.ToString()})
                        }, true);
                    contentField = new ModalDialogDropdownInputField("Content",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Transfer.ToString()}},
                                new[] {ContentTypeEnum.Pdf.ToString()})
                        }, true);
                    break;
                case ContentTypeEnum.H5P:
                    typeField = new ModalDialogDropdownInputField("Type",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(null,
                                new[]
                                {
                                    ElementTypeEnum.Activation.ToString(),
                                    ElementTypeEnum.Interaction.ToString(), ElementTypeEnum.Test.ToString()
                                })
                        }, true);
                    contentField = new ModalDialogDropdownInputField("Content",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Activation.ToString()}},
                                new[] {ContentTypeEnum.H5P.ToString()}),
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Interaction.ToString()}},
                                new[] {ContentTypeEnum.H5P.ToString()}),
                            new ModalDialogDropdownInputFieldChoiceMapping(
                                new Dictionary<string, string> {{"Type", ElementTypeEnum.Test.ToString()}},
                                new[] {ContentTypeEnum.H5P.ToString()})
                        }, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new ModalDialogInputField[]
            {
                new("Name", ModalDialogInputType.Text, true),
                new("Shortname", ModalDialogInputType.Text, true),
                typeField,
                contentField,
                new("Authors", ModalDialogInputType.Text),
                new("Description", ModalDialogInputType.Text, true),
                new("Goals", ModalDialogInputType.Text),
                new ModalDialogDropdownInputField("Difficulty",
                    new[]
                    {
                        new ModalDialogDropdownInputFieldChoiceMapping(null,
                            new[] {LearningElementDifficultyEnum.Easy.ToString(),
                                LearningElementDifficultyEnum.Medium.ToString(),
                                LearningElementDifficultyEnum.Hard.ToString() })
                    }, true),
                new("Workload (min)", ModalDialogInputType.Number)
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
                    new("Goals", ModalDialogInputType.Text),
                    new ModalDialogDropdownInputField("Difficulty",
                        new[]
                        {
                            new ModalDialogDropdownInputFieldChoiceMapping(null,
                                new[] {LearningElementDifficultyEnum.Easy.ToString(),
                                    LearningElementDifficultyEnum.Medium.ToString(),
                                    LearningElementDifficultyEnum.Hard.ToString(),
                                    LearningElementDifficultyEnum.None.ToString() })
                        }, true),
                    new("Workload (min)", ModalDialogInputType.Number)
                };
        }
    }

    #endregion

    #region LearningObject

    /// <summary>
    /// Changes the selected <see cref="ILearningObjectViewModel"/> in the currently selected learning space.
    /// </summary>
    /// <param name="learningObject">The learning object that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public void SetSelectedLearningObject(ILearningObjectViewModel learningObject)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        LearningSpaceVm.SelectedLearningObject = learningObject;
    }

    /// <summary>
    /// Deletes the selected learning object in the currently selected learning space and sets an other element as selected learning object.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than element.</exception>
    public void DeleteSelectedLearningObject()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
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
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than element.</exception>
    public void OpenEditSelectedLearningObjectDialog()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
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
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
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