using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.LearningSpace
{
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
        /// <param name="elementType">The represented type of the element in the space.</param>
        /// <param name="contentType">Describes, which content the element contains.</param>
        /// <param name="authors">A list of authors of the element.</param>
        /// <param name="description">A description of the element.</param>
        /// <param name="goals">The goals of the element.</param>
        /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
        public void CreateNewLearningElement(string name, string shortname, ILearningElementViewModelParent parent,
            string elementType, string contentType, string authors, string description, string goals)
        {
            if (LearningSpaceVm == null)
                throw new ApplicationException("SelectedLearningSpace is null");
            var learningElement =
                _learningElementPresenter.CreateNewLearningElement(name, shortname, parent, elementType,
                    contentType, authors, description, goals);

            SetSelectedLearningObject(learningElement);
        }

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
            if (LearningSpaceVm == null)
                throw new ApplicationException("SelectedLearningSpace is null");
            learningElement.Parent = LearningSpaceVm;
            LearningSpaceVm.LearningElements.Add(learningElement);
        }
        
        public async Task LoadLearningContent()
        {
            var learningContent = await _presentationLogic.LoadLearningContentAsync();
            if (LearningSpaceVm == null)
            { 
                throw new ApplicationException("SelectedLearningSpace is null");
            }
            switch (LearningSpaceVm.SelectedLearningObject)
            {
                case null:
                    throw new ApplicationException("SelectedLearningObject is null");
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
            var parentElement = GetLearningElementParent();

            var elementType = data["Type"];
            var contentType = data["Content"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
            CreateNewLearningElement(name, shortname, parentElement, elementType, contentType, authors, description, goals);
            return Task.CompletedTask;
        }

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
        /// <param name="returnValueTuple">Return values of dialog</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Thrown if return values of dialog are null
        /// or selected learning object is not a learning element</exception>
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
            var elementType = data["Type"];
            var contentType = data["Content"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

            if (LearningSpaceVm == null)
                throw new ApplicationException("LearningSpaceVm is null");
            if (LearningSpaceVm.SelectedLearningObject is not LearningElementViewModel
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
            if (LearningSpaceVm == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            LearningSpaceVm.SelectedLearningObject = learningObject;
        }

        /// <summary>
        /// Deletes the selected learning object in the currently selected learning world and sets an other space or element as selected learning object.
        /// </summary>
        /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
        /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type as space or element.</exception>
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
}