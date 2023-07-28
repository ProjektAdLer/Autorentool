using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using BusinessLogic.Commands;
using BusinessLogic.ErrorManagement;
using Microsoft.Extensions.Logging;
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
    #region LearningSpace

    #region EditLearningSpace

    [Test]
    public void EditLearningSpace_LearningSpaceVmIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.EditLearningSpace("a", "d", "e", 5, Theme.Campus, null));
        Assert.That(ex!.Message, Is.EqualTo("LearningSpaceVm is null"));
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

    #endregion

    #endregion

    #region LearningElement

    [Test]
    public void AddLearningElement_SelectedLearningSpaceIsNull_ThrowsException()
    {
        var element = ViewModelProvider.GetLearningElement();

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddLearningElement(element, 0));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
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
    public void SetSelectedLearningElement_SelectedLearningSpaceIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var element = ViewModelProvider.GetLearningElement();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
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
    public void CreateLearningElementInSlot_SpaceVmIsNull_Throws()
    {
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

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.CreateLearningElementInSlot(name, content, description, goals, difficulty, elementModel,
                workload, points));
        Assert.That(ex!.Message, Is.EqualTo("LearningSpaceVm is null"));
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

    #region EditLearningElement

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
    public void EditLearningElementWithSlotIndex_SpaceVmIsNull_Throws()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.EditLearningElement(2));
        Assert.That(ex!.Message, Is.EqualTo("LearningSpaceVm is null"));
    }

    [Test]
    public void EditLearningElementWithSlotIndex_ElementAtSlotIndexIsNull_Throws()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        space.LearningSpaceLayout.PutElement(2, null!);

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.EditLearningElement(2));
        Assert.That(ex!.Message, Is.EqualTo("LearningElement at slotIndex 2 is null"));
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

    #endregion


    #region DeleteLearningElement

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
    public void DeleteLearningElement_ThrowsWhenSelectedSpaceNull()
    {
        var element = ViewModelProvider.GetLearningElement();
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void DeleteSelectedLearningElement_ThrowsWhenSelectedSpaceNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningElement());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void DeleteSelectedLearningElement_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = new SelectedViewModelsProvider(Substitute.For<IOnUndoRedo>(), Substitute.For<ILogger<SelectedViewModelsProvider>>());
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

    #endregion

    #region SaveSelectedLearningElement

    [Test]
    public void SaveSelectedLearningElementAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void SaveSelectedLearningElementAsync_DoesNotThrowWhenSelectedObjectNull()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var space = ViewModelProvider.GetLearningSpace();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        selectedViewModelsProvider.LearningElement.Returns((ILearningElementViewModel?) null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningElement is null"));
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
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService, selectedViewModelsProvider: selectedViewModelsProvider);
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
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        presentationLogic.SaveLearningElementAsync(element).Returns(Task.FromException(new InvalidOperationException()));

        await systemUnderTest.SaveSelectedLearningElementAsync();

        errorService.Received().SetError("Error while loading learning element", Arg.Any<string>());
    }

    #endregion

    #region ShowSelectedElementContent

    [Test]
    public void ShowSelectedElementContent_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.ShowSelectedElementContentAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void ShowSelectedElementContent_DoesNotThrowWhenSelectedObjectNull()
    {
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var space = ViewModelProvider.GetLearningSpace();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        selectedViewModelsProvider.SetLearningElement(null, null);
        selectedViewModelsProvider.Received(1).SetLearningElement(null, null);
        selectedViewModelsProvider.LearningElement.Returns((ILearningElementViewModel?) null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.ShowSelectedElementContentAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningElement is null"));
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
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        presentationLogic.ShowLearningElementContentAsync(element).Returns(Task.FromException(new InvalidOperationException()));
        
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
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        presentationLogic.ShowLearningElementContentAsync(element).Returns(Task.FromException(new ArgumentOutOfRangeException()));
        
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
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.SetLearningSpace(space);
        presentationLogic.ShowLearningElementContentAsync(element).Returns(Task.FromException(new IOException()));
        
        await systemUnderTest.ShowSelectedElementContentAsync();
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    #endregion

    #region LoadLearningElement

    [Test]
    public void LoadLearningElementAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadLearningElementAsync(1));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
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
            .Do(x => throw new SerializationException("test"));
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
            .Do(x => throw new InvalidOperationException("test"));
        var space = ViewModelProvider.GetLearningSpace();

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService);
        systemUnderTest.SetLearningSpace(space);
        
        await systemUnderTest.LoadLearningElementAsync(1);
        errorService.Received(1).SetError("Error while loading learning element", "test");
    }

    #endregion

    #endregion


    private LearningSpacePresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        IMediator? mediator = null, ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        ILogger<LearningSpacePresenter>? logger = null, IErrorService? errorService = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        logger ??= Substitute.For<ILogger<LearningSpacePresenter>>();
        mediator ??= Substitute.For<IMediator>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        errorService ??= Substitute.For<IErrorService>();
        return new LearningSpacePresenter(presentationLogic, mediator, selectedViewModelsProvider, logger,
            errorService);
    }
}