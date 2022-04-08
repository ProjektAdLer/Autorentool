using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.LearningWorld
{
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
        public bool EditLearningSpaceDialogOpen { get; set; }
        public bool CreateLearningSpaceDialogueOpen { get; set; }
        public bool EditLearningElementDialogOpen { get; set; }
        public bool CreateLearningElementDialogOpen { get; set; }

        public void SetLearningWorld(object? caller, LearningWorldViewModel? world)
        {
            LearningWorldVm = world;
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

        private void OpenEditSelectedLearningSpaceDialog()
        {
            if (LearningWorldVm?.SelectedLearningObject is not LearningSpaceViewModel
                space) throw new ApplicationException("Type of LearningObject is not implemented");
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

        public async Task LoadLearningSpace()
        {
            var learningSpace = await _presentationLogic.LoadLearningSpaceAsync();
            if (LearningWorldVm == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            LearningWorldVm.LearningSpaces.Add(learningSpace);
        }


        public Task OnCreateSpaceDialogClose(
            Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            CreateLearningSpaceDialogueOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexectedly null after Ok return value");

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

        public Task OnEditSpaceDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            EditLearningSpaceDialogOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

            //TODO: change this into a trace ILogger call
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
        /// Creates a new learning element and assigns it to a learning world or a learning space.
        /// </summary>
        /// <param name="name">Name of the element.</param>
        /// <param name="shortname">Shortname of the element.</param>
        /// <param name="parent">Decides whether the learning element belongs to a learning world or a learning space</param>
        /// <param name="type">The represented type of the element in the space/world.</param>
        /// <param name="content">Describes, which content the element contains.</param>
        /// <param name="authors">A list of authors of the element.</param>
        /// <param name="description">A description of the element.</param>
        /// <param name="goals">The goals of the element.</param>
        /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
        public void CreateNewLearningElement(string name, string shortname, ILearningElementViewModelParent parent,
            string type, string content, string authors, string description, string goals)
        {
            if (LearningWorldVm == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            var learningElement =
                _learningElementPresenter.CreateNewLearningElement(name, shortname, parent, type,
                    content, authors, description, goals);

            switch (parent)
            {
                case LearningWorldViewModel:
                    LearningWorldVm.LearningElements.Add(learningElement);
                    break;
                case LearningSpaceViewModel space:
                    space.LearningElements.Add(learningElement);
                    break;
                default:
                    throw new NotImplementedException("Type of Assignment is not implemented");
            }

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
                {"Parent", element.Parent switch{LearningWorldViewModel => "Learning world", LearningSpaceViewModel => "Learning space", _ => ""}},
                {"Assignment", element.Parent.Name},
                {"Type", element.Type},
                {"Content", element.Content},
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
            LearningWorldVm.LearningElements.Add(learningElement);
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

            var type = data["Type"];
            var content = data["Content"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
            CreateNewLearningElement(name, shortname, parentElement, type, content, authors, description, goals);
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

        public Task OnEditElementDialogClose(
            Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
        {
            var (response, data) = returnValueTuple;
            EditLearningElementDialogOpen = false;

            if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
            if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

            //TODO: change this into a trace ILogger call
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
            var type = data["Type"];
            var content = data["Content"];
            var description = data["Description"];
            //optional arguments
            var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
            var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

            if (LearningWorldVm == null)
                throw new ApplicationException("LearningWorld is null");
            if (LearningWorldVm.SelectedLearningObject is not LearningElementViewModel
                learningElementViewModel) throw new ApplicationException("LearningObject is not a LearningElement");
            if (assignment != learningElementViewModel.Parent?.Name)
            {
                RemoveLearningElementParentAssignment(learningElementViewModel);
            }
            _learningElementPresenter.EditLearningElement(learningElementViewModel, name, shortname, parentElement,
                type, content, authors, description, goals);
            return Task.CompletedTask;
        }

        private void RemoveLearningElementParentAssignment(LearningElementViewModel learningElementViewModel)
        {
            switch (learningElementViewModel.Parent)
            {
                case LearningWorldViewModel:
                    LearningWorldVm?.LearningElements.Remove(learningElementViewModel);
                    break;
                case LearningSpaceViewModel space:
                    space.LearningElements.Remove(learningElementViewModel);
                    break;
            }
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
                                new[] {"Text", "Picture", "Video"})
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
        }

        /// <summary>
        /// Deletes the selected learning object in the currently selected learning world and sets an other space or element as selected learning object.
        /// </summary>
        /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
        /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type as space or element.</exception>
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
                    RemoveLearningElementParentAssignment(learningElement);
                    break;
                default:
                    throw new NotImplementedException("Type of LearningObject is not implemented");
            }

            LearningWorldVm.SelectedLearningObject =
                (ILearningObjectViewModel?) LearningWorldVm?.LearningSpaces
                    .LastOrDefault() ??
                LearningWorldVm?.LearningElements.LastOrDefault();
        }

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

        public void SaveSelectedLearningObject()
        {
            if (LearningWorldVm == null)
                throw new ApplicationException("SelectedLearningWorld is null");
            switch (LearningWorldVm.SelectedLearningObject)
            {
                case null:
                    throw new ApplicationException("SelectedLearningObject is null");
                case LearningSpaceViewModel learningSpace:
                    _presentationLogic.SaveLearningSpaceAsync(learningSpace);
                    break;
                case LearningElementViewModel learningElement:
                    _presentationLogic.SaveLearningElementAsync(learningElement);
                    break;
                default:
                    throw new NotImplementedException("Type of LearningObject is not implemented");
            }
        }

        #endregion
    }
}