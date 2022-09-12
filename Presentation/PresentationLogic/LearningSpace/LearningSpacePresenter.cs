using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Shared;
using ModalDialogOnCloseResult = Presentation.Components.ModalDialog.ModalDialogOnCloseResult;
using ModalDialogReturnValue = Presentation.Components.ModalDialog.ModalDialogReturnValue;

namespace Presentation.PresentationLogic.LearningSpace;

public class LearningSpacePresenter : ILearningSpacePresenter, ILearningSpacePresenterToolboxInterface
{
    public LearningSpacePresenter(
        IPresentationLogic presentationLogic, ILogger<LearningSpacePresenter> logger)
    {
        _presentationLogic = presentationLogic;
        _logger = logger;
        EditLearningSpaceDialogInitialValues = null;
        EditLearningElementDialogInitialValues = null;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILogger<LearningSpacePresenter> _logger;

    public ILearningSpaceViewModel? LearningSpaceVm { get; private set; }

    public LearningContentViewModel? DragAndDropLearningContent { get; private set; }

    public void EditLearningSpace(string name, string shortname, string authors, string description, string goals)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        _presentationLogic.EditLearningSpace(LearningSpaceVm, name, shortname, authors, description, goals);
    }

    public bool EditLearningSpaceDialogOpen { get; set; }
    public IDictionary<string, string>? EditLearningSpaceDialogInitialValues { get; private set; }
    public bool EditLearningElementDialogOpen { get; set; }
    public IDictionary<string, string>? EditLearningElementDialogInitialValues { get; private set; }
    public bool CreateLearningElementDialogOpen { get; set; }

    public void SetLearningSpace(ILearningSpaceViewModel space)
    {
        LearningSpaceVm = space;
    }

    #region LearningSpace

    /// <summary>
    /// Changes property values of the learning space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok
    /// or if <see cref="LearningSpaceVm"/> is null.</exception>
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

        if (LearningSpaceVm == null)
            throw new ApplicationException("LearningSpaceVm is null");
        _presentationLogic.EditLearningSpace(LearningSpaceVm, name, shortname, authors, description, goals);
    }

    #endregion

    #region LearningElement

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected LearningElement.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if SelectedLearningObject is not a LearningElementViewModel.
    /// Shouldn't occur, because this is checked in <see cref="EditSelectedLearningObject"/></exception>
    private void OpenEditSelectedLearningElementDialog()
    {
        var element = (LearningElementViewModel) LearningSpaceVm?.SelectedLearningObject!;
        if (element.Parent == null) throw new Exception("Element Parent is null");
        //prepare dictionary property to pass to dialog
        EditLearningElementDialogInitialValues = new Dictionary<string, string>
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

    public void AddNewLearningElement()
    {
        CreateLearningElementDialogOpen = true;
    }

    /// <summary>
    /// Calls the LoadLearningElementAsync method in <see cref="_presentationLogic"/> and adds the returned
    /// learning element to its parent.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="LearningSpaceVm"/> is null</exception>
    public async Task LoadLearningElementAsync()
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        await _presentationLogic.LoadLearningElementAsync(LearningSpaceVm);
        var learningElement = LearningSpaceVm.LearningElements.Last();
        SetSelectedLearningObject(learningElement);
    }

    public void AddLearningElement(ILearningElementViewModel element)
    {
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
        LearningSpaceVm.LearningElements.Add(element);
        element.Parent = LearningSpaceVm;
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
        if (LearningSpaceVm == null)
            throw new ApplicationException("SelectedLearningSpace is null");
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
            if (DragAndDropLearningContent is not null)
            {
                learningContent = DragAndDropLearningContent;
                DragAndDropLearningContent = null;
            }
            else
            {
                learningContent = Task.Run(async () => await LoadLearningContent(contentType)).Result;
            }
                        
            _presentationLogic.CreateLearningElement(parentElement, name, shortname, elementType, contentType,
                learningContent, authors, description, goals, difficulty, workload);
            LearningSpaceVm.SelectedLearningObject = LearningSpaceVm.LearningElements.Last();

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
        _presentationLogic.EditLearningElement(parentElement, learningElementViewModel, name, shortname, authors,
            description, goals, difficulty, workload);
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
                _presentationLogic.DeleteLearningElement(LearningSpaceVm, learningElement);
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
    public void EditSelectedLearningObject()
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