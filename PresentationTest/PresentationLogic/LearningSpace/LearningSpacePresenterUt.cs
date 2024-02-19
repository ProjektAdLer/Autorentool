using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;
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

        systemUnderTest.EditLearningSpace("a", "d", "e", 5, Theme.Campus, null);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public void EditLearningSpace_CallsPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var topic = ViewModelProvider.GetTopic();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.EditLearningSpace("space", "d", "e", 5, Theme.Campus, topic);

        presentationLogic.Received().EditLearningSpace(space, "space", "d", "e", 5, Theme.Campus, topic);
    }

    [Test]
    public void ClickedLearningElement_CallsMediatorAndSelectedViewModelsProvider()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var element = ViewModelProvider.GetLearningElement();

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.ClickedLearningElement(element);

        mediator.Received().RequestOpenElementDialog();
        selectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, null);
        selectedViewModelsProvider.Received().SetLearningElement(element, null);
    }

    [Test]
    public void ClickedLearningElement_CallsErrorServiceWhenSpaceIsNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var element = ViewModelProvider.GetLearningElement();

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
    public async Task LoadLearningElementAsync_SetsErrorWhenSelectedSpaceNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        Assert.That(systemUnderTest.LearningSpaceVm, Is.Null);

        await systemUnderTest.LoadLearningElementAsync(1);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public async Task LoadLearningElementAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = ViewModelProvider.GetLearningSpace();
        var element = ViewModelProvider.GetLearningElement(parent: space);
        space.LearningSpaceLayout.PutElement(0, element);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        await systemUnderTest.LoadLearningElementAsync(1);

        await presentationLogic.Received().LoadLearningElementAsync(space, 1);
    }

    [Test]
    public async Task LoadLearningElementAsync_SerializationException_CallsErrorManager()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.When(x => x.LoadLearningElementAsync(Arg.Any<ILearningSpaceViewModel>(), Arg.Any<int>()))
            .Do(_ => throw new SerializationException("test"));
        var space = ViewModelProvider.GetLearningSpace();

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService);
        systemUnderTest.SetLearningSpace(space);

        await systemUnderTest.LoadLearningElementAsync(1);
        errorService.Received(1).SetError("Error while loading learning element", "test");
    }

    [Test]
    public async Task LoadLearningElementAsync_InvalidOperationException_CallsErrorManager()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.When(x => x.LoadLearningElementAsync(Arg.Any<ILearningSpaceViewModel>(), Arg.Any<int>()))
            .Do(_ => throw new InvalidOperationException("test"));
        var space = ViewModelProvider.GetLearningSpace();

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService);
        systemUnderTest.SetLearningSpace(space);

        await systemUnderTest.LoadLearningElementAsync(1);
        errorService.Received(1).SetError("Error while loading learning element", "test");
    }


    private LearningSpacePresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        IMediator? mediator = null, ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        ILogger<LearningSpacePresenter>? logger = null, IErrorService? errorService = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        logger ??= new NullLogger<LearningSpacePresenter>();
        mediator ??= Substitute.For<IMediator>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        errorService ??= Substitute.For<IErrorService>();
        return new LearningSpacePresenter(presentationLogic, mediator, selectedViewModelsProvider, logger,
            errorService);
    }
}