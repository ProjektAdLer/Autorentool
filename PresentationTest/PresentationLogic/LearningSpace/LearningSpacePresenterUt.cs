using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using BusinessLogic.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
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
    public void AddLearningElement_SelectedLearningSpaceIsNull_SetsError()
    {
        var element = ViewModelProvider.GetLearningElement();
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        systemUnderTest.AddLearningElement(element, 0);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public void AddLearningElement_CallsPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X30_8L);
        var element = ViewModelProvider.GetLearningElement(parent: space);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(0, element);
        selectedViewModelsProvider.SetLearningElement(element, null);

        var mockPresentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: mockPresentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.AddLearningElement(element, 1);

        mockPresentationLogic.Received().AddLearningElement(space, 1, element);
    }

    [Test]
    public void DragLearningElement_CallsPresentationLogic()
    {
        var element = ViewModelProvider.GetLearningElement();
        double oldPositionX = 5;
        double oldPositionY = 7;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.DragLearningElement(element,
            new DraggedEventArgs<ILearningElementViewModel>(element, oldPositionX, oldPositionY));

        presentationLogic.Received().DragLearningElement(element, oldPositionX, oldPositionY);
    }

    [Test]
    public void SetSelectedLearningElement_CallsSelectedViewModelsProvider()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var element = ViewModelProvider.GetLearningElement();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.SetSelectedLearningElement(element);

        selectedViewModelsProvider.Received().SetLearningElement(element, null);
    }

    [Test]
    public void SetSelectedLearningElement_SelectedLearningSpaceIsNull_SetsError()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);
        var element = ViewModelProvider.GetLearningElement();

        systemUnderTest.SetSelectedLearningElement(element);

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
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
        selectedViewModelsProvider.Received().SetLearningElement(element, null);
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

        systemUnderTest.ClickOnSlot(1);

        mediator.DidNotReceive().RequestOpenElementDialog();
        selectedViewModelsProvider.DidNotReceive().SetActiveSlotInSpace(1, null);
    }

    [Test]
    public void ClickOnSlot_SlotAlreadyActive_SetsToInactive()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();

        selectedViewModelsProvider.ActiveSlotInSpace.Returns(1);

        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.ClickOnSlot(1);

        mediator.DidNotReceive().RequestOpenElementDialog();
        selectedViewModelsProvider.Received().SetActiveSlotInSpace(-1, null);
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

        systemUnderTest.ClickOnSlot(1);

        mediator.Received().RequestOpenElementDialog();
        selectedViewModelsProvider.Received().SetActiveSlotInSpace(1, null);
    }

    [Test]
    public void CreateLearningElementInSlot_SpaceVmIsNull_SetsError()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.ActiveSlotInSpace.Returns(1);
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
        selectedViewModelsProvider.ActiveSlotInSpace.Returns(1);
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
        selectedViewModelsProvider.Received().SetActiveSlotInSpace(-1, null);
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
    public void DeleteSelectedLearningElement_SetsErrorWhenSelectedSpaceNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        Assert.That(systemUnderTest.LearningSpaceVm, Is.Null);

        systemUnderTest.DeleteSelectedLearningElement();

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }


    [Test]
    public void DeleteSelectedLearningElement_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = new SelectedViewModelsProvider(Substitute.For<IOnUndoRedo>(),
            Substitute.For<ILogger<SelectedViewModelsProvider>>());
        selectedViewModelsProvider.SetLearningElement(null, null);

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceVm, Is.Not.Null);
            Assert.That(selectedViewModelsProvider.LearningElement, Is.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.ContainedLearningElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningElement());
        });
    }

    [Test]
    public void DeleteSelectedLearningElement_CallsPresentationLogic_WithElement()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var element = ViewModelProvider.GetLearningElement(parent: space);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(0, element);
        selectedViewModelsProvider.SetLearningElement(element, null);
        selectedViewModelsProvider.Received(1).SetLearningElement(element, null);
        selectedViewModelsProvider.LearningElement.Returns(element);

        var mockPresentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: mockPresentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        systemUnderTest.DeleteSelectedLearningElement();

        mockPresentationLogic.Received().DeleteLearningElementInSpace(space, element);
    }

    [Test]
    public async Task SaveSelectedLearningElementAsync_SetsErrorWhenSelectedSpaceNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        Assert.That(systemUnderTest.LearningSpaceVm, Is.Null);

        await systemUnderTest.SaveSelectedLearningElementAsync();

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public async Task SaveSelectedLearningElementAsync_LogsErrorWhenSelectedObjectNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var space = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider,
            errorService: mockErrorService);
        systemUnderTest.SetLearningSpace(space);
        selectedViewModelsProvider.LearningElement.Returns((ILearningElementViewModel?)null);

        await systemUnderTest.SaveSelectedLearningElementAsync();

        mockErrorService.Received().SetError("Operation failed", "No learning element selected");
    }

    [Test]
    public async Task SaveSelectedLearningElement_CallsPresentationLogic_WithElement()
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
        await systemUnderTest.SaveSelectedLearningElementAsync();

        await presentationLogic.Received().SaveLearningElementAsync(element);
    }

    [Test]
    public async Task SaveSelectedLearningElementAsync_SerializationException_CallsErrorManager()
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
        presentationLogic.SaveLearningElementAsync(element).Returns(Task.FromException(new SerializationException()));

        await systemUnderTest.SaveSelectedLearningElementAsync();

        errorService.Received().SetError("Error while loading learning element", Arg.Any<string>());
    }

    [Test]
    public async Task SaveSelectedLearningElementAsync_InvalidOperationException_CallsErrorManager()
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
        presentationLogic.SaveLearningElementAsync(element)
            .Returns(Task.FromException(new InvalidOperationException()));

        await systemUnderTest.SaveSelectedLearningElementAsync();

        errorService.Received().SetError("Error while loading learning element", Arg.Any<string>());
    }

    [Test]
    public async Task ShowSelectedElementContent_SetsErrorWhenSelectedSpaceNull()
    {
        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: mockErrorService);

        Assert.That(systemUnderTest.LearningSpaceVm, Is.Null);

        await systemUnderTest.ShowSelectedElementContentAsync();

        mockErrorService.Received().SetError("Operation failed", "No learning space selected");
    }

    [Test]
    public async Task ShowSelectedElementContent_LogsErrorWhenSelectedObjectNull()
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

        await systemUnderTest.ShowSelectedElementContentAsync();

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
    public async Task ShowSelectedElementContentAsync_InvalidOperationException_CallsErrorManager()
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

        await systemUnderTest.ShowSelectedElementContentAsync();
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_ArgumentOutOfRangeException_CallsErrorManager()
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

        await systemUnderTest.ShowSelectedElementContentAsync();
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_IOException_CallsErrorManager()
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

        await systemUnderTest.ShowSelectedElementContentAsync();
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