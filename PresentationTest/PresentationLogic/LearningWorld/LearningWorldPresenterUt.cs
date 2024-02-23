using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using TestHelpers;

namespace PresentationTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldPresenterUt
{
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;

    [Test]
    public void EditLearningWorld_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.EditLearningWorld("n", "s", "a", "l", "d", "g", "h", "f");
        presentationLogic.Received().EditLearningWorld(world, "n", "s", "a", "l", "d", "g", "h", "f");
    }

    [Test]
    public void EditLearningWorld_LogsErrorAndSetsErrorServiceIfLearningWorldIsNull()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockErrorService = Substitute.For<IErrorService>();

        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);

        var systemUnderTest = CreatePresenterForTesting(
            selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: mockErrorService);

        systemUnderTest.EditLearningWorld("n", "s", "a", "l", "d", "g", "h", "f");

        mockErrorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public async Task SaveLearningWorldAsync_CallsSaveLearningWorldAsyncIfLearningWorldIsNotNull()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockPresentationLogic = Substitute.For<IPresentationLogic>();
        var mockLogger = Substitute.For<ILogger<LearningWorldPresenter>>();
        var mockErrorService = Substitute.For<IErrorService>();

        var world = ViewModelProvider.GetLearningWorld();

        mockSelectedViewModelsProvider.LearningWorld.Returns(world);

        var systemUnderTest = CreatePresenterForTesting(
            selectedViewModelsProvider: mockSelectedViewModelsProvider,
            presentationLogic: mockPresentationLogic,
            errorService: mockErrorService,
            logger: mockLogger);

        systemUnderTest.SaveLearningWorld();

        mockPresentationLogic.Received().SaveLearningWorld(Arg.Is<LearningWorldViewModel>(x => x == world));
    }


    [Test]
    public async Task SaveLearningWorldAsync_LogsErrorAndSetsErrorServiceIfLearningWorldIsNull()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockErrorService = Substitute.For<IErrorService>();

        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);

        var systemUnderTest = CreatePresenterForTesting(
            selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: mockErrorService);

        systemUnderTest.SaveLearningWorld();

        mockErrorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void SetSelectedLearningObject_SelectedLearningWorldIsNull_LogsErrorAndSetsErrorService()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockErrorService = Substitute.For<IErrorService>();

        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: mockErrorService);
        var space = ViewModelProvider.GetLearningSpace();

        systemUnderTest.SetSelectedLearningObject(space);

        mockErrorService.Received().SetError("Operation failed", "No learning world selected");
    }


    [Test]
    public void SetSelectedLearningObject_CallsSpacePresenter()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);
        systemUnderTest.SetSelectedLearningObject(space);

        learningSpacePresenter.Received().SetLearningSpace(space);
    }

    [Test]
    public void DragObjectInPathWay_CallsPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace();
        double oldPositionX = 5;
        double oldPositionY = 7;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.DragObjectInPathWay(space,
            new DraggedEventArgs<IObjectInPathWayViewModel>(space, oldPositionX, oldPositionY));

        presentationLogic.Received().DragObjectInPathWay(space, oldPositionX, oldPositionY);
    }

    [Test]
    public void SetSelectedLearningSpace_SetsSelectedLearningSpaceAndSetsEditLearningSpaceDialogOpenToTrue()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningSpace(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(space, null);
        mediator.Received().RequestOpenSpaceDialog();
    }

    [Test]
    public void EditSelectedLearningSpace_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.EditSelectedLearningSpace();

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void EditSelectedLearningSpace_SelectedObjectInPathWayIsNotLearningSpace_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(condition);

        systemUnderTest.EditSelectedLearningSpace();

        errorService.Received().SetError("Operation failed", "Selected object in pathway is not a learning space");
    }

    [Test]
    public void EditSelectedLearningSpace_CallsMediator()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        systemUnderTest.EditSelectedLearningSpace();
        mediator.Received().RequestOpenSpaceDialog();
    }

    [Test]
    public void DeleteLearningSpace_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DeleteLearningSpace(space);

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    [Test]
    public void DeleteLearningSpace_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var space = ViewModelProvider.GetLearningSpace();

        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.DeleteLearningSpace(space);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void RightClickedLearningSpace_SetsRightClickedLearningObjectToSpace()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(space);

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(space));
    }

    [Test]
    public void HideRightClickMenu_SetsRightClickedLearningObjectToNull()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreatePresenterForTesting();

        systemUnderTest.RightClickOnObjectInPathWay(space);
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(space));

        systemUnderTest.HideRightClickMenu();

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }

    [Test]
    public void ClickedLearningSpace_SetsSelectedLearningObjectToSpace()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningWorld.Returns(world);
        systemUnderTest.ClickOnObjectInWorld(space);

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(space, null);
    }

    [Test]
    public void DoubleClickedLearningSpace_SetsSelectedLearningObjectAndShowsLearningSpaceView()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DoubleClickOnObjectInPathway(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        Assert.That(selectedViewModelsProvider.LearningObjectInPathWay, Is.EqualTo(space));
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }

    [Test]
    public void AddNewLearningSpace_LogsErrorWhenWorldIsNull()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);

        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: mockErrorService);

        systemUnderTest.AddNewLearningSpace();

        mockErrorService.Received().SetError(Arg.Any<string>(), Arg.Any<string>());
    }


    [Test]
    public void AddNewLearningSpace_CallsSelectedViewModelsProviderAndMediator()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.LearningWorldVm = world;


        systemUnderTest.AddNewLearningSpace();

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(null, null);
        mediator.Received().RequestOpenSpaceDialog();
    }

    [Test]
    public void CreateNewLearningSpace_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm?.LearningSpaces.Add(space);

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);

        presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<int>(), Arg.Any<Theme>(), Arg.Any<double>(),
            Arg.Any<double>(), Arg.Any<TopicViewModel>());
    }

    [Test]
    public void CreateNewLearningSpace_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void CreateMultipleLearningObjects_PositionIsCorrect()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);
        systemUnderTest.LearningWorldVm.LearningSpaces.Add(new LearningSpaceViewModel("aa", "bb", "cc", Theme.Campus));

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);

        var space = new LearningSpaceViewModel("aa", "bb", "cc", Theme.Campus, 0,
            null, 0, 85);
        systemUnderTest.LearningWorldVm.LearningSpaces.Add(space);

        //Drag latest learning space into offset
        space.PositionX = 15;
        space.PositionY = 100;

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);
        systemUnderTest.LearningWorldVm.LearningSpaces.Add(new LearningSpaceViewModel("aa", "bb", "cc", Theme.Campus, 0,
            null, 0, 185));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);

        var condition = new PathWayConditionViewModel(ConditionEnum.And, false, 0, 270);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(condition);

        //Drag latest condition into offset
        condition.PositionX = 15;
        condition.PositionY = 280;

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);
        systemUnderTest.LearningWorldVm.LearningSpaces.Add(new LearningSpaceViewModel("aa", "bb", "cc", Theme.Campus, 0,
            null, 0, 335));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 420));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 475));

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);
        systemUnderTest.LearningWorldVm.LearningSpaces.Add(new LearningSpaceViewModel("aa", "bb", "cc", Theme.Campus, 0,
            null, 0, 530));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 615));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 670));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 725));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 780));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 835));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 890));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 945));

        systemUnderTest.CreatePathWayCondition(ConditionEnum.And);
        systemUnderTest.LearningWorldVm.PathWayConditions.Add(
            new PathWayConditionViewModel(ConditionEnum.And, false, 0, 950));

        Received.InOrder(() =>
        {
            presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), 0, 0, Arg.Any<TopicViewModel>());

            presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), 0, 85, Arg.Any<TopicViewModel>());

            presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), 0, 185, Arg.Any<TopicViewModel>());

            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 270);

            presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), 0, 335, Arg.Any<TopicViewModel>());

            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 420);

            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 475);

            presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), 0, 530, Arg.Any<TopicViewModel>());

            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 615);
            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 670);
            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 725);
            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 780);
            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 835);
            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 890);
            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 945);
            presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 0, 950);
        });
    }

    [Test]
    public void DeleteSelectedLearningObject_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService);
        systemUnderTest.LearningWorldVm = null;

        systemUnderTest.DeleteSelectedLearningObject();

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void DeleteSelectedLearningObject_DoesNotThrowWhenSelectedSpaceNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningWorld.Returns(world);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns((LearningSpaceViewModel)null!);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(selectedViewModelsProvider.LearningObjectInPathWay, Is.Null);
            Assert.That(systemUnderTest.LearningWorldVm.LearningSpaces, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesSpaceFromViewModel_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningSpaces.Add(space);
        selectedViewModelsProvider.SetLearningObjectInPathWay(space, null);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesConditionFromViewModel_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.PathWayConditions.Add(condition);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(condition);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesPathWayFromViewModel_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var space = ViewModelProvider.GetLearningSpace();
        var pathWay = ViewModelProvider.GetLearningPathway(source: space, target: condition);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningPathWays.Add(pathWay);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(pathWay);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeleteLearningPathWay(world, pathWay);
    }

    [Test]
    public async Task SaveSelectedLearningSpaceAsync_SelectedWorldNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService);
        systemUnderTest.LearningWorldVm = null;

        await systemUnderTest.SaveSelectedLearningSpaceAsync();

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public async Task SaveSelectedLearningSpaceAsync_SelectedSpaceNull_CallsErrorService()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();

        var systemUnderTest = CreatePresenterForTesting(
            selectedViewModelsProvider: selectedViewModelsProvider,
            errorService: errorService);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns((LearningSpaceViewModel)null!);

        await systemUnderTest.SaveSelectedLearningSpaceAsync();

        errorService.Received().SetError("Operation failed", "No object in pathway is selected");
    }

    [Test]
    public async Task SaveSelectedLearningSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningSpaces.Add(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningSpaceAsync();

        await presentationLogic.Received().SaveLearningSpaceAsync(space);
    }

    [Test]
    public async Task SaveSelectedLearningSpace_SerializationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningSpaces.Add(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);
        presentationLogic.When(x => x.SaveLearningSpaceAsync(space))
            .Do(_ => throw new SerializationException());

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, errorService: errorService,
                selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningSpaceAsync();
        errorService.Received().SetError("Error while saving learning space", Arg.Any<string>());
    }

    [Test]
    public async Task SaveSelectedLearningSpace_InvalidOperationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningSpaces.Add(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);
        presentationLogic.When(x => x.SaveLearningSpaceAsync(space))
            .Do(_ => throw new InvalidOperationException());

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, errorService: errorService,
                selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningSpaceAsync();
        errorService.Received().SetError("Error while saving learning space", Arg.Any<string>());
    }

    [Test]
    public void LoadLearningSpaceAsync_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService);
        systemUnderTest.LearningWorldVm = null;

        Assert.DoesNotThrowAsync(async () => await systemUnderTest.LoadLearningSpaceAsync());

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public async Task LoadLearningSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.LoadLearningSpaceAsync();

        await presentationLogic.Received().LoadLearningSpaceAsync(world);
    }

    [Test]
    public async Task LoadLearningSpace_SerializationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        presentationLogic.LoadLearningSpaceAsync(world).Throws(new SerializationException());

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.LoadLearningSpaceAsync();
        errorService.Received().SetError("Error while loading learning space", Arg.Any<string>());
    }

    [Test]
    public async Task LoadLearningSpace_InvalidOperationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        presentationLogic.LoadLearningSpaceAsync(world).Throws(new InvalidOperationException());

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.LoadLearningSpaceAsync();
        errorService.Received().SetError("Error while loading learning space", Arg.Any<string>());
    }

    [Test]
    public void SetSelectedLearningObject_SetsPathWayToSelectedObject()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var space = ViewModelProvider.GetLearningSpace();
        var pathWay = ViewModelProvider.GetLearningPathway(source: space, target: condition);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(pathWay);

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(pathWay, null);
    }

    [Test]
    public void DeleteLearningObject_SelectedLearningWorldIsNull_LogsErrorAndSetsErrorService()
    {
        var condition = ViewModelProvider.GetPathWayCondition();

        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockErrorService = Substitute.For<IErrorService>();

        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: mockErrorService);

        systemUnderTest.DeleteLearningObject(condition);

        mockErrorService.Received().SetError("Operation failed", "No learning world selected");
    }


    [Test]
    public void DeleteLearningObject_CallsPresentationLogic_WithPathWayCondition()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(condition);

        systemUnderTest.DeleteLearningObject(condition);

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }

    [Test]
    public void DeleteLearningObject_CallsPresentationLogic_WithLearningSpace()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(space);

        systemUnderTest.DeleteLearningObject(space);

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    [Test]
    public void SetSelectedLearningObject_SetsConditionToSelectedObject()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(condition);

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(condition, null);
    }

    [Test]
    public void CreatePathWayCondition_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.CreatePathWayCondition();

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void CreatePathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.CreatePathWayCondition();

        presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.Or, 0, 0);
    }

    [Test]
    public void CreatePathWayCondition_ApplicationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, errorService: errorService,
                selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.LearningWorldVm = world;
        presentationLogic.When(x => x.CreatePathWayCondition(world, ConditionEnum.Or, 0, 0))
            .Do(_ => throw new ApplicationException("Test"));

        systemUnderTest.CreatePathWayCondition();
        errorService.Received().SetError("Error while creating PathWayCondition", "Test");
    }

    [Test]
    public void DeletePathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DeletePathWayCondition(condition);

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }

    [Test]
    public void DeletePathWayCondition_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var condition = ViewModelProvider.GetPathWayCondition();

        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.DeletePathWayCondition(condition);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void RightClickedPathWayCondition_SetsRightClickedLearningObjectToSpace()
    {
        var condition = ViewModelProvider.GetPathWayCondition();
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(condition);

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(condition));
    }

    [Test]
    public void HideRightClickMenuFromCondition_SetsRightClickedLearningObjectToNull()
    {
        var conditionViewModel = ViewModelProvider.GetPathWayCondition();
        var systemUnderTest = CreatePresenterForTesting();

        systemUnderTest.RightClickOnObjectInPathWay(conditionViewModel);
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(conditionViewModel));

        systemUnderTest.HideRightClickMenu();

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }

    [Test]
    public void ClickedPathWayCondition_SetsSelectedLearningObjectToCondition()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var conditionViewModel = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.ClickOnObjectInWorld(conditionViewModel);
        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(conditionViewModel, null);
    }

    [Test]
    public void DoubleClickedPathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DoubleClickOnObjectInPathway(condition);

        presentationLogic.Received().EditPathWayCondition(condition, ConditionEnum.Or);
    }

    [Test]
    public void SetOnHoveredPathWayObject_ObjectAtPositionIsNull_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace();
        var targetSpace = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay = targetSpace;

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace));

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 400, 400);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceSpace_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(sourceSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 30, 30);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceCondition_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceCondition = ViewModelProvider.GetPathWayCondition();
        world.PathWayConditions.Add(sourceCondition);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceCondition, 30, 30);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_SetsCorrectObjectAtPosition()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace(positionX: 25, positionY: 25);
        var targetSpace1 = ViewModelProvider.GetLearningSpace(positionX: 250, positionY: 250);
        var targetSpace2 = ViewModelProvider.GetLearningSpace(positionX: 500, positionY: 500);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace1);
        world.LearningSpaces.Add(targetSpace2);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay = targetSpace1;

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace1));

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 510, 510);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace2));
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: errorService);
        var space = ViewModelProvider.GetLearningSpace();

        systemUnderTest.SetOnHoveredObjectInPathWay(space, 3, 3);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void CreateLearningPathWay_WithoutTargetSpace_DoesNotCallPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace();
        var targetSpace = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningPathWay(sourceSpace, 25, 25);

        presentationLogic.DidNotReceive().CreateLearningPathWay(world, sourceSpace, targetSpace);
    }

    [Test]
    public void CreateLearningPathWay_TargetSpaceIsSourceSpace_DoesNotCallPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace();
        var targetSpace = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningPathWay(sourceSpace, 60, 60);

        presentationLogic.DidNotReceive().CreateLearningPathWay(world, sourceSpace, sourceSpace);
    }

    [Test]
    public void CreateLearningPathWay_WithTargetSpace_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace(positionX: 25, positionY: 25);
        var targetSpace = ViewModelProvider.GetLearningSpace(positionX: 250, positionY: 250);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningPathWay(sourceSpace, 260, 260);

        presentationLogic.Received().CreateLearningPathWay(world, sourceSpace, targetSpace);
    }

    [Test]
    public void CreateLearningPathWay_LogsErrorWhenWorldIsNull()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);

        var mockErrorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: mockErrorService);

        var space = ViewModelProvider.GetLearningSpace();

        systemUnderTest.CreateLearningPathWay(space, 3, 3);

        mockErrorService.Received().SetError(Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void DeleteLearningPathWay_CallsPresentationLogic_NoPathWaySelected()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var space1 = ViewModelProvider.GetLearningSpace();
        var space2 = ViewModelProvider.GetLearningSpace();
        var learningPathWay1 = ViewModelProvider.GetLearningPathway(source: space1, target: condition);
        var learningPathWay2 = ViewModelProvider.GetLearningPathway(source: space2, target: condition);
        world.LearningPathWays.Add(learningPathWay1);
        world.LearningPathWays.Add(learningPathWay2);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteLearningPathWay(condition);

        presentationLogic.Received().DeleteLearningPathWay(world, learningPathWay2);
    }

    [Test]
    public void DeleteLearningPathWay_CallsPresentationLogic_TargetObjectPathWaySelected()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var space1 = ViewModelProvider.GetLearningSpace();
        var space2 = ViewModelProvider.GetLearningSpace();
        var learningPathWay1 = ViewModelProvider.GetLearningPathway(source: space1, target: condition);
        var learningPathWay2 = ViewModelProvider.GetLearningPathway(source: space2, target: condition);
        world.LearningPathWays.Add(learningPathWay1);
        world.LearningPathWays.Add(learningPathWay2);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        mockSelectedViewModelsProvider.LearningObjectInPathWay.Returns(learningPathWay1);

        systemUnderTest.DeleteLearningPathWay(condition);

        presentationLogic.Received().DeleteLearningPathWay(world, learningPathWay1);
    }

    [Test]
    public void DeleteLearningPathWay_CallsPresentationLogic_ExternalPathWaySelected()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var space1 = ViewModelProvider.GetLearningSpace();
        var space2 = ViewModelProvider.GetLearningSpace();
        var space3 = ViewModelProvider.GetLearningSpace();
        var learningPathWay1 = ViewModelProvider.GetLearningPathway(source: space1, target: condition);
        var learningPathWay2 = ViewModelProvider.GetLearningPathway(source: space2, target: condition);
        var learningPathWay3 = ViewModelProvider.GetLearningPathway(source: space3, target: space1);
        world.LearningPathWays.Add(learningPathWay1);
        world.LearningPathWays.Add(learningPathWay2);
        world.LearningPathWays.Add(learningPathWay3);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        mockSelectedViewModelsProvider.LearningObjectInPathWay.Returns(learningPathWay3);

        systemUnderTest.DeleteLearningPathWay(condition);

        presentationLogic.Received().DeleteLearningPathWay(world, learningPathWay2);
    }

    [Test]
    public void DeleteLearningPathWay_SelectedLearningWorld_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns(null as ILearningWorldViewModel);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var objectInPathway = ViewModelProvider.GetLearningSpace();

        systemUnderTest.DeleteLearningPathWay(objectInPathway);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void DeleteLearningPathWay_PathWayIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns(ViewModelProvider.GetLearningWorld());
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var objectInPathway = ViewModelProvider.GetLearningSpace();

        systemUnderTest.DeleteLearningPathWay(objectInPathway);

        errorService.Received().SetError("Operation failed", "No LearningPathWay found");
    }

    [Test]
    public void SetSelectedLearningElement_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: errorService);
        var element = ViewModelProvider.GetLearningElement();

        systemUnderTest.SetSelectedLearningElement(element);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void SetSelectedLearningElement_SetsSelectedLearningElement()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        var element = ViewModelProvider.GetLearningElement();

        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningElement(element);
        selectedViewModelsProvider.Received(1).SetLearningElement(element, null);
        mediator.Received(1).RequestOpenElementDialog();
    }

    [Test]
    public void SetSelectedLearningElement_SetsSelectedAdaptivityElement()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        var element = ViewModelProvider.GetAdaptivityElement();

        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningElement(element);
        selectedViewModelsProvider.Received(1).SetLearningElement(element, null);
        mediator.Received(1).RequestOpenAdaptivityElementDialog();
    }

    [Test]
    public void EditLearningElement_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var element = ViewModelProvider.GetLearningElement();

        systemUnderTest.EditLearningElement(null, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0, null!);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void EditLearningElement_UnplacedHasASpaceParent_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content, parent: space);

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);

        systemUnderTest.EditLearningElement(null, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0, null!);

        errorService.Received().SetError("Operation failed", "LearningElement is unplaced but has a space parent");
    }

    [Test]
    public void EditLearningElement_UnplacedDoesNotContainElement_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var world = ViewModelProvider.GetLearningWorld();
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.EditLearningElement(null, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0, null!);

        errorService.Received()
            .SetError("Operation failed", "LearningElement is placed but has a different or null parent");
    }


    [Test]
    public void EditLearningElement_ElementParentIsNotNull_CallsSpacePresenter()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);
        var space = ViewModelProvider.GetLearningSpace();
        element.Parent = space;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.EditLearningElement(space, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0, null!);

        learningSpacePresenter.Received(1).EditLearningElement(element, "a", "b", "c",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 0, 0, null!);
    }

    [Test]
    public void EditLearningElement_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        systemUnderTest.EditLearningElement(null, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0, content);
        presentationLogic.Received().EditLearningElement(null, element, "a", "b", "c",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 0, 0, content);
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider,
            errorService: errorService);
        var element = ViewModelProvider.GetLearningElement();

        await systemUnderTest.ShowSelectedElementContentAsync(element);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        await systemUnderTest.ShowSelectedElementContentAsync(element);
        await presentationLogic.Received().ShowLearningElementContentAsync(element);
        selectedViewModelsProvider.Received().SetLearningElement(element, null);
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_ArgumentOutOfRangeException_CallsErrorService()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        presentationLogic.When(x => x.ShowLearningElementContentAsync(element))
            .Do(_ => throw new ArgumentOutOfRangeException());

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        await systemUnderTest.ShowSelectedElementContentAsync(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_IOException_CallsErrorService()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        presentationLogic.When(x => x.ShowLearningElementContentAsync(element))
            .Do(_ => throw new IOException());

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        await systemUnderTest.ShowSelectedElementContentAsync(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_InvalidOperationException_CallsErrorService()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        presentationLogic.When(x => x.ShowLearningElementContentAsync(element))
            .Do(_ => throw new InvalidOperationException());

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        await systemUnderTest.ShowSelectedElementContentAsync(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public void DeleteLearningElement_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var element = ViewModelProvider.GetLearningElement();

        systemUnderTest.DeleteLearningElement(element);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void DeleteLearningElement_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        systemUnderTest.DeleteLearningElement(element);
        presentationLogic.Received().DeleteLearningElementInWorld(world, element);
    }

    [Test]
    public void GetAllContent_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);

        systemUnderTest.GetAllContent();
        presentationLogic.Received().GetAllContent();
    }

    [Test]
    public void CreateUnplacedLearningElement_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.CreateUnplacedLearningElement("a", null!, "c", "d", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0);

        errorService.Received().SetError("Operation failed", "No learning world selected");
    }

    [Test]
    public void CreateUnplacedLearningElement_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        var content = ViewModelProvider.GetLinkContent();

        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateUnplacedLearningElement("abc", content, "a", "b", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0);
        presentationLogic.Received().CreateUnplacedLearningElement(world, "abc", content, "a", "b",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 0, 0);
    }

    private LearningWorldPresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null,
        ILogger<LearningWorldPresenter>? logger = null, IMediator? mediator = null,
        ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        IErrorService? errorService = null, IMapper? mapper = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        mediator ??= Substitute.For<IMediator>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        errorService ??= Substitute.For<IErrorService>();
        mapper ??= Substitute.For<IMapper>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, logger,
            mediator, selectedViewModelsProvider, errorService, mapper);
    }
}