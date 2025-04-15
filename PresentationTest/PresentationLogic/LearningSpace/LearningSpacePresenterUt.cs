using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;
using Shared.Theme;
using TestHelpers;

namespace PresentationTest.PresentationLogic.LearningSpace;

[TestFixture]
public class LearningSpacePresenterUt
{
    [Test]
    public void EditLearningSpace_LearningSpaceVmIsNull_SetsError()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        systemUnderTest.EditLearningSpace("a", "d", 5, Theme.CampusAschaffenburg, null);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    // ANF-ID: [AWA0023]
    public void EditLearningSpace_CallsPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var topic = ViewModelProvider.GetTopic();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.EditLearningSpace("space", "d", 5, Theme.CampusAschaffenburg, topic);

        presentationLogic.Received().EditLearningSpace(space, "space", "d", 5, Theme.CampusAschaffenburg, topic);
    }

    [Test]
    public void ClickedLearningElement_CallsMediatorAndSelectedViewModelsProvider()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var element = ViewModelProvider.GetLearningElement(content: ViewModelProvider.GetFileContent());

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.ClickedLearningElement(element);

        mediator.Received().RequestOpenElementDialog();
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, null);
        selectedViewModelsProvider.Received().SetLearningElement(element, null);
    }

    [Test]
    public void ClickedAdaptivityElement_CallsMediatorAndSelectedViewModelsProvider()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var adaptivityElement = ViewModelProvider.GetAdaptivityElement();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.ClickedLearningElement(adaptivityElement);

        mediator.Received().RequestOpenAdaptivityElementDialog();
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, null);
        selectedViewModelsProvider.Received().SetLearningElement(adaptivityElement, null);
    }

    [Test]
    public void ClickedStoryElement_CallsMediatorAndSelectedViewModelsProvider()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var storyElement = ViewModelProvider.GetStoryElement();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.ClickedLearningElement(storyElement);

        mediator.Received().RequestOpenStoryElementDialog();
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, null);
        selectedViewModelsProvider.Received().SetLearningElement(storyElement, null);
    }

    [Test]
    public void ClickedLearningElement_CallsErrorServiceWhenSpaceIsNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var element = ViewModelProvider.GetLearningElement(content: ViewModelProvider.GetFileContent());

        var systemUnderTest =
            CreatePresenterForTesting(errorService: mockErrorService);

        systemUnderTest.ClickedLearningElement(element);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public void ClickOnSlot_ElementInSlot_Returns()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var element = ViewModelProvider.GetLearningElement();
        space.LearningSpaceLayout.PutElement(1, element);

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.ClickOnElementSlot(1);

        mediator.DidNotReceive().RequestOpenElementDialog();
        selectedViewModelsProvider.DidNotReceive().SetActiveElementSlotInSpace(1, null);
    }

    [Test]
    public void ClickOnSlot_SlotAlreadyActive_SetsToInactive()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();

        selectedViewModelsProvider.ActiveElementSlotInSpace.Returns(1);

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.ClickOnElementSlot(1);

        mediator.DidNotReceive().RequestOpenElementDialog();
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, null);
    }

    [Test]
    public void ClickOnSlot_CallsSelectedViewModelsProviderAndMediator()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.ClickOnElementSlot(1);

        mediator.Received().RequestOpenElementDialog();
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(1, null);
    }

    [Test]
    public void ClickOnStorySlot_ElementInSlot_Returns()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var storyElement = ViewModelProvider.GetStoryElement();
        space.LearningSpaceLayout.PutStoryElement(1, storyElement);

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.ClickOnStorySlot(1);

        mediator.DidNotReceive().RequestOpenStoryElementDialog();
        selectedViewModelsProvider.DidNotReceive().SetActiveStorySlotInSpace(1, null);
    }

    [Test]
    public void ClickOnStorySlot_SlotAlreadyActive_SetsToInactive()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();

        selectedViewModelsProvider.ActiveStorySlotInSpace.Returns(1);

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.ClickOnStorySlot(1);

        mediator.DidNotReceive().RequestOpenStoryElementDialog();
        selectedViewModelsProvider.Received().SetActiveStorySlotInSpace(-1, null);
    }

    [Test]
    public void ClickOnStorySlot_CallsSelectedViewModelsProviderAndMediator()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.ClickOnStorySlot(1);

        mediator.Received().RequestOpenStoryElementDialog();
        selectedViewModelsProvider.Received().SetActiveStorySlotInSpace(1, null);
    }

    [Test]
    public void CreateLearningElementInSlot_SpaceVmIsNull_SetsError()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.ActiveElementSlotInSpace.Returns(1);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var mockErrorService = Substitute.For<IErrorService>();
        var name = "name";
        var content = new FileContentViewModel("abc", "def", "ghi");
        var difficulty = LearningElementDifficultyEnum.Easy;
        var description = "description";
        var goals = "goals";
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var workload = 0;
        var points = 0;

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic, errorService: mockErrorService);

        systemUnderTest.CreateLearningElementInSlot(name, content, description, goals, difficulty, elementModel,
            workload, points);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    // ANF-ID: [AWA0002]
    public void CreateLearningElementInSlot_CallsSelectedViewModelsProviderAndPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.ActiveElementSlotInSpace.Returns(1);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var name = "name";
        var content = new FileContentViewModel("abc", "def", "ghi");
        var difficulty = LearningElementDifficultyEnum.Easy;
        var description = "description";
        var goals = "goals";
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var workload = 0;
        var points = 0;

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.CreateLearningElementInSlot(name, content, description, goals, difficulty, elementModel,
            workload, points);

        presentationLogic.Received().CreateLearningElementInSlot(space, 1, name, content, description, goals,
            difficulty, elementModel, workload, points);
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, null);
    }

    [Test]
    public void CreateStoryElementInSlot_SpaceVmIsNull_SetsError()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.ActiveStorySlotInSpace.Returns(1);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var mockErrorService = Substitute.For<IErrorService>();
        var name = "name";
        var content = new FileContentViewModel("abc", "def", "ghi");
        var difficulty = LearningElementDifficultyEnum.Easy;
        var description = "description";
        var goals = "goals";
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var workload = 0;
        var points = 0;

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic, errorService: mockErrorService);

        systemUnderTest.CreateStoryElementInSlot(name, content, description, goals, difficulty, elementModel,
            workload, points);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public void CreateLearningElementInSlotFromFormModel_CallsSelectedViewModelsProviderAndPresentationLogic()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.ActiveElementSlotInSpace.Returns(1);
        var learningSpace = ViewModelProvider.GetLearningSpace();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var mockErrorService = Substitute.For<IErrorService>();
        var name = "name";
        var content = new FileContentFormModel("abc", "def", "ghi");
        var difficulty = LearningElementDifficultyEnum.Easy;
        var description = "description";
        var goals = "goals";
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var workload = 0;
        var points = 0;

        var formModel = new LearningElementFormModel()
        {
            Name = name,
            LearningContent = content,
            Description = description,
            Goals = goals,
            Difficulty = difficulty,
            ElementModel = elementModel,
            Workload = workload,
            Points = points
        };

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic, errorService: mockErrorService);

        systemUnderTest.SetLearningSpace(learningSpace);

        systemUnderTest.CreateLearningElementInSlotFromFormModel(formModel);

        presentationLogic.Received().CreateLearningElementInSlot(learningSpace, 1, name,
            Arg.Any<ILearningContentViewModel>(), description, goals,
            difficulty, elementModel, workload, points);
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, null);
    }

    [Test]
    // ANF-ID: [ASN0011]
    public void CreateStoryElementInSlotFromFormModel_CallsSelectedViewModelsProviderAndPresentationLogic()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.ActiveStorySlotInSpace.Returns(1);
        var learningSpace = ViewModelProvider.GetLearningSpace();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var mockErrorService = Substitute.For<IErrorService>();
        var name = "name";
        var content = new FileContentFormModel("abc", "def", "ghi");
        var difficulty = LearningElementDifficultyEnum.Easy;
        var description = "description";
        var goals = "goals";
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var workload = 0;
        var points = 0;

        var formModel = new LearningElementFormModel()
        {
            Name = name,
            LearningContent = content,
            Description = description,
            Goals = goals,
            Difficulty = difficulty,
            ElementModel = elementModel,
            Workload = workload,
            Points = points
        };

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic, errorService: mockErrorService);

        systemUnderTest.SetLearningSpace(learningSpace);

        systemUnderTest.CreateStoryElementInSlotFromFormModel(formModel);

        presentationLogic.Received().CreateStoryElementInSlot(learningSpace, 1, name,
            Arg.Any<ILearningContentViewModel>(), description, goals,
            difficulty, elementModel, workload, points);
        selectedViewModelsProvider.Received().SetActiveStorySlotInSpace(-1, null);
    }

    [Test]
    public void CreateStoryElementInSlot_CallsSelectedViewModelsProviderAndPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.ActiveStorySlotInSpace.Returns(1);
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var name = "name";
        var content = new FileContentViewModel("abc", "def", "ghi");
        var difficulty = LearningElementDifficultyEnum.Easy;
        var description = "description";
        var goals = "goals";
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var workload = 0;
        var points = 0;

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.CreateStoryElementInSlot(name, content, description, goals, difficulty, elementModel,
            workload, points);

        presentationLogic.Received().CreateStoryElementInSlot(space, 1, name, content, description, goals,
            difficulty, elementModel, workload, points);
        selectedViewModelsProvider.Received().SetActiveStorySlotInSpace(-1, null);
    }

    [Test]
    // ANF-ID: [AWA0015]
    public void EditLearningElement_CallsPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var content = ViewModelProvider.GetFileContent();
        var element = ViewModelProvider.GetLearningElement();
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.EditLearningElement(element, "g", "g", "g", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, 0, 0, content);

        presentationLogic.Received().EditLearningElement(space, element, "g", "g", "g",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_blackboard_1, 0, 0, content);
    }

    [Test]
    public void EditLearningElementWithSlotIndex_SpaceVmIsNull_SetsError()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        systemUnderTest.EditLearningElement(2);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public void EditLearningElementWithSlotIndex_ElementAtSlotIndexIsNull_SetsError()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(2, null!);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
            errorService: mockErrorService);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.EditLearningElement(2);

        mockErrorService.Received().SetError("Operation failed", "LearningElement to edit not found");
    }

    [Test]
    // ANF-ID: [AWA0015]
    public void EditLearningElementWithSlotIndex_CallsSelectedViewModelsProvider()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var element = ViewModelProvider.GetLearningElement();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(2, element);

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.EditLearningElement(2);

        selectedViewModelsProvider.Received().SetLearningElement(element, null);
    }

    [Test]
    public void DeleteLearningElement_CallsPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var element = ViewModelProvider.GetLearningElement();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.DeleteLearningElement(element);
        presentationLogic.Received().DeleteLearningElementInSpace(space, element);
    }

    [Test]
    // ANF-ID: [AWA0016]
    public void DeleteLearningElement_SetsErrorWhenSelectedSpaceNull()
    {
        var element = ViewModelProvider.GetLearningElement();
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        Assert.That(systemUnderTest.LearningSpaceVm, Is.Null);

        systemUnderTest.DeleteLearningElement(element);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public void ShowSelectedElementContent_SetsErrorWhenSelectedSpaceNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var element = ViewModelProvider.GetLearningElement();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        Assert.That(systemUnderTest.LearningSpaceVm, Is.Null);

        systemUnderTest.ShowElementContent(element);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public void ShowSelectedElementContent_LogsErrorWhenSelectedObjectNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var space = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
            errorService: mockErrorService);
        systemUnderTest.SetLearningSpace(space);
        selectedViewModelsProvider.SetLearningElement(null, null);
        selectedViewModelsProvider.Received(1).SetLearningElement(null, null);
        selectedViewModelsProvider.LearningElement.Returns((ILearningElementViewModel?)null);

        systemUnderTest.ShowElementContent(null!);

        mockErrorService.Received().SetError("Operation failed", "No learning element selected");
    }

    [Test]
    // ANF-ID: [AWA0038]
    public async Task ShowSelectedElementContent_CallsPresentationLogic_WithElement()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = ViewModelProvider.GetLearningSpace();
        var element = ViewModelProvider.GetLearningElement(parent: space);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(0, element);
        selectedViewModelsProvider.SetLearningElement(element, null);
        selectedViewModelsProvider.Received(1).SetLearningElement(element, null);
        selectedViewModelsProvider.LearningElement.Returns(element);

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.ShowElementContent(element);

        await presentationLogic.Received().ShowLearningElementContentAsync(element);
    }

    [Test]
    public void ShowSelectedElementContentAsync_InvalidOperationException_CallsErrorManager()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = ViewModelProvider.GetLearningSpace();
        var element = ViewModelProvider.GetLearningElement(parent: space);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(0, element);
        selectedViewModelsProvider.SetLearningElement(element, null);
        selectedViewModelsProvider.Received(1).SetLearningElement(element, null);
        selectedViewModelsProvider.LearningElement.Returns(element);

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        presentationLogic.ShowLearningElementContentAsync(element)
            .Returns(Task.FromException(new InvalidOperationException()));

        systemUnderTest.ShowElementContent(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public void ShowSelectedElementContentAsync_ArgumentOutOfRangeException_CallsErrorManager()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = ViewModelProvider.GetLearningSpace();
        var element = ViewModelProvider.GetLearningElement(parent: space);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(0, element);
        selectedViewModelsProvider.SetLearningElement(element, null);
        selectedViewModelsProvider.Received(1).SetLearningElement(element, null);
        selectedViewModelsProvider.LearningElement.Returns(element);

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        presentationLogic.ShowLearningElementContentAsync(element)
            .Returns(Task.FromException(new ArgumentOutOfRangeException()));

        systemUnderTest.ShowElementContent(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public void ShowSelectedElementContentAsync_IOException_CallsErrorManager()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = ViewModelProvider.GetLearningSpace();
        var element = ViewModelProvider.GetLearningElement(parent: space);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(0, element);
        selectedViewModelsProvider.SetLearningElement(element, null);
        selectedViewModelsProvider.Received(1).SetLearningElement(element, null);
        selectedViewModelsProvider.LearningElement.Returns(element);

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        presentationLogic.ShowLearningElementContentAsync(element).Returns(Task.FromException(new IOException()));

        systemUnderTest.ShowElementContent(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public void SetLearningSpaceLayout_LearningSpaceVmIsNull_LogsErrorAndReturns()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.SetLearningSpaceLayout(FloorPlanEnum.R_20X30_8L);

        errorService.Received().SetError("Operation failed", "No learning space selected");
        presentationLogic.DidNotReceive().ChangeLearningSpaceLayout(Arg.Any<LearningSpaceViewModel>(),
            Arg.Any<LearningWorldViewModel>(), Arg.Any<FloorPlanEnum>());
    }

    [Test]
    public void SetLearningSpaceLayout_LearningWorldIsNull_LogsErrorAndReturns()
    {
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.SetLearningSpace(learningSpaceVm);
        selectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel)null!);

        systemUnderTest.SetLearningSpaceLayout(FloorPlanEnum.R_20X30_8L);

        errorService.Received().SetError("Operation failed", "No LearningWorld selected");
        presentationLogic.DidNotReceive().ChangeLearningSpaceLayout(Arg.Any<LearningSpaceViewModel>(),
            Arg.Any<LearningWorldViewModel>(), Arg.Any<FloorPlanEnum>());
    }

    [Test]
    // ANF-ID: [AWA0023]
    public void SetLearningSpaceLayout_CallsChangeLearningSpaceLayoutAndSetsActiveElementSlot()
    {
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningWorld = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.SetLearningSpace(learningSpaceVm);
        selectedViewModelsProvider.LearningWorld.Returns(learningWorld);

        systemUnderTest.SetLearningSpaceLayout(FloorPlanEnum.R_20X30_8L);

        presentationLogic.Received()
            .ChangeLearningSpaceLayout(learningSpaceVm, learningWorld, FloorPlanEnum.R_20X30_8L);
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, null);
    }

    [Test]
    public void OnReplaceLearningElementDialogClose_LearningSpaceVmIsNull_LogsErrorAndReturns()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Cancel());
        systemUnderTest.OnReplaceLearningElementDialogClose(dialogReference.Result.Result!);

        errorService.Received().SetError("Operation failed", "No learning space selected");
        presentationLogic.DidNotReceive().DragLearningElementFromUnplaced(Arg.Any<LearningWorldViewModel>(),
            Arg.Any<LearningSpaceViewModel>(), Arg.Any<LearningElementViewModel>(), Arg.Any<int>());
    }

    [Test]
    public void OnReplaceLearningElementDialogClose_CloseResultCanceled_Returns()
    {
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.SetLearningSpace(learningSpaceVm);
        systemUnderTest.OpenReplaceLearningElementDialog(learningWorldVm, ViewModelProvider.GetLearningElement(), 1);

        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Cancel());
        systemUnderTest.OnReplaceLearningElementDialogClose(dialogReference.Result.Result!);

        presentationLogic.DidNotReceive().DragLearningElementFromUnplaced(Arg.Any<LearningWorldViewModel>(),
            Arg.Any<LearningSpaceViewModel>(), Arg.Any<LearningElementViewModel>(), Arg.Any<int>());
    }

    [Test]
    // ANF-ID: [ASN0017]
    public void OnReplaceLearningElementDialogClose_ValidCloseResult_CallsDragLearningElementFromUnplaced()
    {
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.SetLearningSpace(learningSpaceVm);
        systemUnderTest.OpenReplaceLearningElementDialog(learningWorldVm, learningElementVm, 1);

        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        systemUnderTest.OnReplaceLearningElementDialogClose(dialogReference.Result.Result!);

        presentationLogic.Received()
            .DragLearningElementFromUnplaced(learningWorldVm, learningSpaceVm, learningElementVm, 1);
    }

    [Test]
    public void OnReplaceStoryElementDialogClose_LearningSpaceVmIsNull_LogsErrorAndReturns()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Cancel());
        systemUnderTest.OnReplaceStoryElementDialogClose(dialogReference.Result.Result!);

        errorService.Received().SetError("Operation failed", "No learning space selected");
        presentationLogic.DidNotReceive().DragStoryElementFromUnplaced(Arg.Any<LearningWorldViewModel>(),
            Arg.Any<LearningSpaceViewModel>(), Arg.Any<LearningElementViewModel>(), Arg.Any<int>());
    }

    [Test]
    public void OnReplaceStoryElementDialogClose_CloseResultCanceled_Returns()
    {
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.SetLearningSpace(learningSpaceVm);
        systemUnderTest.OpenReplaceStoryElementDialog(learningWorldVm, ViewModelProvider.GetLearningElement(), 1);

        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Cancel());
        systemUnderTest.OnReplaceStoryElementDialogClose(dialogReference.Result.Result!);

        presentationLogic.DidNotReceive().DragStoryElementFromUnplaced(Arg.Any<LearningWorldViewModel>(),
            Arg.Any<LearningSpaceViewModel>(), Arg.Any<LearningElementViewModel>(), Arg.Any<int>());
    }

    [Test]
    // ANF-ID: [ASN0019]
    public void OnReplaceStoryElementDialogClose_ValidCloseResult_CallsDragStoryElementFromUnplaced()
    {
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.SetLearningSpace(learningSpaceVm);
        systemUnderTest.OpenReplaceStoryElementDialog(learningWorldVm, learningElementVm, 1);

        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        systemUnderTest.OnReplaceStoryElementDialogClose(dialogReference.Result.Result!);

        presentationLogic.Received()
            .DragStoryElementFromUnplaced(learningWorldVm, learningSpaceVm, learningElementVm, 1);
    }

    [Test]
    public void DeleteStoryElement_LearningSpaceVmIsNull_LogsErrorAndReturns()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();
        var learningElementViewModel = Substitute.For<ILearningElementViewModel>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.DeleteStoryElement(learningElementViewModel);

        errorService.Received().SetError("Operation failed", "No learning space selected");
        presentationLogic.DidNotReceive()
            .DeleteStoryElementInSpace(Arg.Any<LearningSpaceViewModel>(), Arg.Any<ILearningElementViewModel>());
    }

    [Test]
    // ANF-ID: [ASN0015]
    public void DeleteStoryElement_ValidLearningSpaceVm_CallsDeleteStoryElementInSpace()
    {
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var mediator = Substitute.For<IMediator>();
        var learningElementViewModel = Substitute.For<ILearningElementViewModel>();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
                presentationLogic: presentationLogic,
                errorService: errorService,
                mediator: mediator);

        systemUnderTest.SetLearningSpace(learningSpaceVm);

        systemUnderTest.DeleteStoryElement(learningElementViewModel);

        presentationLogic.Received().DeleteStoryElementInSpace(learningSpaceVm, learningElementViewModel);
    }


    private LearningSpacePresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        IMediator? mediator = null, ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        ILogger<LearningSpacePresenter>? logger = null, IErrorService? errorService = null, IMapper? mapper = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        logger ??= new NullLogger<LearningSpacePresenter>();
        mediator ??= Substitute.For<IMediator>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        errorService ??= Substitute.For<IErrorService>();
        mapper ??= Substitute.For<IMapper>();
        return new LearningSpacePresenter(presentationLogic, mediator, selectedViewModelsProvider, logger,
            errorService, mapper);
    }
}