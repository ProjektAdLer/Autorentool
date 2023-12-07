using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Adaptivity.Action;
using BusinessLogic.Commands.Adaptivity.Action.Comment;
using BusinessLogic.Commands.Adaptivity.Action.ContentReference;
using BusinessLogic.Commands.Adaptivity.Action.ElementReference;
using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Commands.Adaptivity.Rule;
using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using ElectronWrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Adaptivity;
using Shared.Command;
using Shared.Configuration;
using TestHelpers;

namespace PresentationTest.PresentationLogic.API;

[TestFixture]
public class PresentationLogicUt
{
    [Test]
    public void Standard_AllPropertiesInitialized()
    {
        //Arrange
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockCachingMapper = Substitute.For<ICachingMapper>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        Substitute.For<ILogger<QuestionCommandFactory>>();
        Substitute.For<ILogger<TaskCommandFactory>>();
        Substitute.For<ILogger<ConditionCommandFactory>>();
        Substitute.For<ILogger<ElementCommandFactory>>();
        Substitute.For<ILogger<LayoutCommandFactory>>();
        Substitute.For<ILogger<PathwayCommandFactory>>();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        Substitute.For<ILogger<TopicCommandFactory>>();
        Substitute.For<ILogger<WorldCommandFactory>>();
        Substitute.For<ILogger<BatchCommandFactory>>();
        var mockHybridSupportWrapper = Substitute.For<IHybridSupportWrapper>();
        var mockShellWrapper = Substitute.For<IShellWrapper>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockTaskCommandFactory = Substitute.For<ITaskCommandFactory>();
        var mockConditionCommandFactory = Substitute.For<IConditionCommandFactory>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockPathwayCommandFactory = Substitute.For<IPathwayCommandFactory>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockTopicCommandFactory = Substitute.For<ITopicCommandFactory>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockBatchCommandFactory = Substitute.For<IBatchCommandFactory>();

        //Act
        var systemUnderTest = CreateTestablePresentationLogic(mockConfiguration, mockBusinessLogic, mockMapper,
            mockCachingMapper, mockSelectedViewModelsProvider, mockServiceProvider, mockLogger,
            mockHybridSupportWrapper, mockShellWrapper, mockQuestionCommandFactory, mockTaskCommandFactory,
            mockConditionCommandFactory, mockElementCommandFactory, mockLayoutCommandFactory, mockPathwayCommandFactory,
            mockSpaceCommandFactory, mockTopicCommandFactory, mockWorldCommandFactory, mockBatchCommandFactory);
        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(mockBusinessLogic));
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(mockMapper));
            Assert.That(systemUnderTest.CMapper, Is.EqualTo(mockCachingMapper));
            Assert.That(systemUnderTest.SelectedViewModelsProvider, Is.EqualTo(mockSelectedViewModelsProvider));
            Assert.That(systemUnderTest.Logger, Is.EqualTo(mockLogger));
            Assert.That(systemUnderTest.HybridSupportWrapper, Is.EqualTo(mockHybridSupportWrapper));
            Assert.That(systemUnderTest.ShellWrapper, Is.EqualTo(mockShellWrapper));
            Assert.That(systemUnderTest.ConditionCommandFactory, Is.EqualTo(mockConditionCommandFactory));
            Assert.That(systemUnderTest.ElementCommandFactory, Is.EqualTo(mockElementCommandFactory));
            Assert.That(systemUnderTest.LayoutCommandFactory, Is.EqualTo(mockLayoutCommandFactory));
            Assert.That(systemUnderTest.PathwayCommandFactory, Is.EqualTo(mockPathwayCommandFactory));
            Assert.That(systemUnderTest.SpaceCommandFactory, Is.EqualTo(mockSpaceCommandFactory));
            Assert.That(systemUnderTest.TopicCommandFactory, Is.EqualTo(mockTopicCommandFactory));
            Assert.That(systemUnderTest.WorldCommandFactory, Is.EqualTo(mockWorldCommandFactory));
        });
    }

    [Test]
    public async Task ConstructBackup_CallsDialogManagerAndBusinessLogic()
    {
        //Arrange
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        var mockFileSystem = new MockFileSystem();
        mockDialogManager
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns("supersecretfilepath");
        var viewModel = ViewModelProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        var entity = EntityProvider.GetLearningWorld();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(viewModel).Returns(entity);
        var serviceProvider = new ServiceCollection();
        serviceProvider.Insert(0, new ServiceDescriptor(typeof(IElectronDialogManager), mockDialogManager));
        var mockProgress = Substitute.For<IProgress<int>>();
        var cancellationToken = new CancellationToken();
        var expectedFilepath = Path.Join(ApplicationPaths.SavedWorldsFolder, viewModel.Name + ".mbz");

        mockBusinessLogic
            .When(x => x.ConstructBackup(entity, expectedFilepath))
            .Do(_ => mockFileSystem.AddFile(expectedFilepath, new MockFileData("whatever")));

        var systemUnderTest = CreateTestablePresentationLogic(null, mockBusinessLogic, mockMapper,
            serviceProvider: serviceProvider.BuildServiceProvider(), fileSystem: mockFileSystem);
        //Act
        await systemUnderTest.ConstructAndUploadBackupAsync(viewModel, mockProgress, cancellationToken);

        //Assert
        mockBusinessLogic.Received().ConstructBackup(entity, expectedFilepath);
        await mockBusinessLogic.Received()
            .UploadLearningWorldToBackendAsync(expectedFilepath, mockProgress, cancellationToken);
        Assert.That(mockFileSystem.FileExists(expectedFilepath), Is.False);
    }

    [Test]
    public void AddLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockCommand = Substitute.For<ICreateLearningWorld>();
        var workspaceVm = ViewModelProvider.GetAuthoringToolWorkspace();
        var worldVm = ViewModelProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = EntityProvider.GetLearningWorld();
        var workspaceEntity = EntityProvider.GetAuthoringToolWorkspace(
            new List<ILearningWorld> { worldEntity });
        Substitute.For<ILogger<WorldCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<AuthoringToolWorkspaceViewModel>())
            .Returns(workspaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(worldEntity);
        mockWorldCommandFactory
            .GetCreateCommand(workspaceEntity, worldEntity,
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.AddLearningWorld(workspaceVm, worldVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CanUndoCanRedo_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.CanUndo.Returns(true);
        mockBusinessLogic.CanRedo.Returns(false);
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var canUndo = systemUnderTest.CanUndo;
        var canRedo = systemUnderTest.CanRedo;

        Assert.Multiple(() =>
        {
            Assert.That(canUndo, Is.True);
            Assert.That(canRedo, Is.False);
        });
    }

    [Test]
    public void OnUndoRedoPerformed_SubscribesToBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var wasCalled = false;
        object? sender = null;
        CommandUndoRedoOrExecuteArgs? args = null;
        systemUnderTest.OnCommandUndoRedoOrExecute += (s, e) =>
        {
            wasCalled = true;
            sender = s;
            args = e;
        };
        mockBusinessLogic.OnCommandUndoRedoOrExecute += Raise.Event<EventHandler<CommandUndoRedoOrExecuteArgs>>(
            systemUnderTest, new CommandUndoRedoOrExecuteArgs("testCommand", CommandExecutionState.Executed));
        Assert.Multiple(() =>
        {
            Assert.That(wasCalled, Is.True);
            Assert.That(sender, Is.Not.Null);
            Assert.That(args, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(sender, Is.EqualTo(systemUnderTest));
            Assert.That(args?.CommandName, Is.EqualTo("testCommand"));
            Assert.That(args?.ExecutionState, Is.EqualTo(CommandExecutionState.Executed));
        });
    }

    [Test]
    public void CallUndoCommand_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.UndoCommand();

        mockBusinessLogic.Received().UndoCommand();
    }

    [Test]
    public void CallRedoCommand_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.RedoCommand();

        mockBusinessLogic.Received().RedoCommand();
    }

    [Test]
    public void CreateLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockCommand = Substitute.For<ICreateLearningWorld>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var workspaceVm = ViewModelProvider.GetAuthoringToolWorkspace();
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = EntityProvider.GetLearningWorld();
        var workspaceEntity = EntityProvider.GetAuthoringToolWorkspace(
            new List<ILearningWorld> { worldEntity });
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<AuthoringToolWorkspaceViewModel>())
            .Returns(workspaceEntity);
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        Substitute.For<ILogger<WorldCommandFactory>>();
        workspaceVm.LearningWorlds.Add(mockWorldVm);
        mockWorldCommandFactory.GetCreateCommand(workspaceEntity, "f", "f", "f", "f", "f", "f", "f", "f",
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            selectedViewModelsProvider: mockSelectedViewModelsProvider, worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.CreateLearningWorld(workspaceVm, "f", "f", "f", "f", "f", "f", "f", "f");

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningWorld(workspaceVm.LearningWorlds.Last(), mockCommand);
    }

    [Test]
    public void EditLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockCommand = Substitute.For<IEditLearningWorld>();
        mockCommand.AnyChanges().Returns(true);
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var worldVm = ViewModelProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = EntityProvider.GetLearningWorld();
        Substitute.For<ILogger<WorldCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(worldEntity);
        mockWorldCommandFactory
            .GetEditCommand(worldEntity, "f", "f", "f", "f", "f", "f", "f", "f",
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.EditLearningWorld(worldVm, "f", "f", "f", "f", "f", "f", "f", "f");

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockCommand = Substitute.For<IDeleteLearningWorld>();
        var workspaceVm = ViewModelProvider.GetAuthoringToolWorkspace();
        var worldVm = ViewModelProvider.GetLearningWorld();
        workspaceVm._learningWorlds.Add(worldVm);
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = EntityProvider.GetLearningWorld();
        var workspaceEntity = EntityProvider.GetAuthoringToolWorkspace(
            new List<ILearningWorld> { worldEntity });
        Substitute.For<ILogger<WorldCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<AuthoringToolWorkspaceViewModel>())
            .Returns(workspaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(worldEntity);
        mockWorldCommandFactory
            .GetDeleteCommand(workspaceEntity, worldEntity,
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.DeleteLearningWorld(workspaceVm, worldVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void AddLearningSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<ICreateLearningSpace>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockSpaceCommandFactory.GetCreateCommand(learningWorldEntity, learningSpaceEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            spaceCommandFactory: mockSpaceCommandFactory);

        systemUnderTest.AddLearningSpace(learningWorldVm, learningSpaceVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreateLearningSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<ICreateLearningSpace>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var topicVm = ViewModelProvider.GetTopic();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var topicEntity = EntityProvider.GetTopic();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<BusinessLogic.Entities.Topic>(Arg.Any<TopicViewModel>())
            .Returns(topicEntity);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockSpaceVm = ViewModelProvider.GetLearningSpace();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        learningWorldVm.LearningSpaces.Add(mockSpaceVm);
        mockSpaceCommandFactory.GetCreateCommand(learningWorldEntity, "z", "z", "z", 5, Theme.Campus,
                6, 7, topicEntity, Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            selectedViewModelsProvider: selectedViewModelsProvider, spaceCommandFactory: mockSpaceCommandFactory);

        systemUnderTest.CreateLearningSpace(learningWorldVm, "z", "z", "z", 5, Theme.Campus, 6, 7, topicVm);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void EditLearningSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<IEditLearningSpace>();
        mockCommand.AnyChanges().Returns(true);
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockSpaceCommandFactory.GetEditCommand(learningSpaceEntity, "z", "z", "z", 5, Theme.Campus, null,
                Arg.Any<Action<ILearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            spaceCommandFactory: mockSpaceCommandFactory);

        systemUnderTest.EditLearningSpace(learningSpaceVm, "z", "z", "z", 5, Theme.Campus, null);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void ChangeLearningSpaceLayout_CallsMapperAndCommandFactory()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockCommand = Substitute.For<IChangeLearningSpaceLayout>();
        var topicViewModel = ViewModelProvider.GetTopic();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace(assignedTopic: topicViewModel);
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        Substitute.For<ILogger<LayoutCommandFactory>>();
        const FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L;
        learningWorldEntity.LearningSpaces.Add(learningSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm).Returns(learningWorldEntity);
        mockLayoutCommandFactory
            .GetChangeCommand(learningSpaceEntity, learningWorldEntity, floorPlan,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);
        var spaceSub = Substitute.For<ILearningSpace>();
        spaceSub.Id.Returns(learningSpaceVm.Id);
        learningWorldEntity.LearningSpaces.Add(spaceSub);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory);

        systemUnderTest.ChangeLearningSpaceLayout(learningSpaceVm, learningWorldVm, floorPlan);

        mockBusinessLogic.ExecuteCommand(mockCommand);
    }

    [Test]
    public void DragObjectInPathWay_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockPathwayCommandFactory = Substitute.For<IPathwayCommandFactory>();
        var mockCommand = Substitute.For<IDragObjectInPathWay>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<PathwayCommandFactory>>();
        mockMapper.Map<IObjectInPathWay>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockPathwayCommandFactory.GetDragCommand(learningSpaceEntity, 5, 6,
                learningSpaceEntity.PositionX, learningSpaceEntity.PositionY,
                Arg.Any<Action<IObjectInPathWay>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            pathwayCommandFactory: mockPathwayCommandFactory);

        systemUnderTest.DragObjectInPathWay(learningSpaceVm, 5, 6);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteLearningSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<IDeleteLearningSpace>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockSpaceCommandFactory
            .GetDeleteCommand(learningWorldEntity, learningSpaceEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            spaceCommandFactory: mockSpaceCommandFactory);

        systemUnderTest.DeleteLearningSpace(learningWorldVm, learningSpaceVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void AddLearningElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<ICreateLearningElementInSlot>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement(parent: learningSpaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement(parent: learningSpaceEntity);
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(learningElementEntity);
        mockElementCommandFactory.GetCreateInSlotCommand(learningSpaceEntity, 0, learningElementEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.AddLearningElement(learningSpaceVm, 0, learningElementVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreateUnplacedLearningElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<ICreateUnplacedLearningElement>();
        var mockMapper = Substitute.For<IMapper>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var mockElementVm = Substitute.For<ILearningElementViewModel>();
        learningWorldVm.UnplacedLearningElements.Add(mockElementVm);
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var learningContentEntity = EntityProvider.GetLinkContent();
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper
            .Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm)
            .Returns(learningWorldEntity);
        mockMapper
            .Map<ILearningContent>(null)
            .Returns(learningContentEntity);
        mockElementCommandFactory
            .GetCreateUnplacedCommand(learningWorldEntity, "a", learningContentEntity, "d", "e",
                LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 5, 7, 0, 0,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);
        var learningElementVm = ViewModelProvider.GetLearningElement();
        mockBusinessLogic
            .When(bl => bl.ExecuteCommand(mockCommand))
            .Do(_ => learningWorldVm.UnplacedLearningElements.Add(learningElementVm));

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.CreateUnplacedLearningElement(learningWorldVm, "a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 5, 7);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningElement(learningElementVm, mockCommand);
    }

    [Test]
    public void CreateLearningElementInSlot_CallsBusinessLogic_AndSetsElementInSelectedViewModelsProvider()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<ICreateLearningElementInSlot>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement(parent: learningSpaceVm);
        mockBusinessLogic
            .When(bl => bl.ExecuteCommand(mockCommand))
            .Do(_ => learningSpaceVm.LearningSpaceLayout.PutElement(0, learningElementVm));
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockElementCommandFactory.GetCreateInSlotCommand(learningSpaceEntity, 0, "a", Arg.Any<ILearningContent>(), "d",
                "e", LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 1, 2, positionX: 3,
                positionY: 4,
                mappingAction: Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.CreateLearningElementInSlot(learningSpaceVm, 0, "a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 1, 2, positionX: 3, positionY: 4);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningElement(learningElementVm, mockCommand);
    }

    [Test]
    public void EditLearningElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<IEditLearningElement>();
        mockCommand.AnyChanges().Returns(true);
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement(parent: learningSpaceVm);
        var learningContentVm = ViewModelProvider.GetFileContent();
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement(parent: learningSpaceEntity);
        var learningContentEntity = EntityProvider.GetFileContent();
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(learningElementEntity);
        mockMapper.Map<ILearningContent>(Arg.Any<FileContentViewModel>())
            .Returns(learningContentEntity);
        mockElementCommandFactory.GetEditCommand(learningElementEntity, learningSpaceEntity, "a", "d", "e",
                LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 1, 2, learningContentEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningElement>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.EditLearningElement(learningSpaceVm, learningElementVm, "a", "d",
            "e", LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 1, 2, learningContentVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DragLearningElementFromUnplaced_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockCommand = Substitute.For<IPlaceLearningElementInLayoutFromUnplaced>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        Substitute.For<ILogger<LayoutCommandFactory>>();
        mockMapper
            .Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm)
            .Returns(learningWorldEntity);
        mockMapper
            .Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm)
            .Returns(learningSpaceEntity);
        mockMapper
            .Map<BusinessLogic.Entities.LearningElement>(learningElementVm)
            .Returns(learningElementEntity);
        mockLayoutCommandFactory
            .GetPlaceFromUnplacedCommand(learningWorldEntity, learningSpaceEntity,
                learningElementEntity, 1, Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory, selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.DragLearningElementFromUnplaced(learningWorldVm, learningSpaceVm, learningElementVm, 1);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);
    }

    [Test]
    public void DragLearningElementToUnplaced_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockCommand = Substitute.For<IRemoveLearningElementFromLayout>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        Substitute.For<ILogger<LayoutCommandFactory>>();
        mockMapper
            .Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm)
            .Returns(learningWorldEntity);
        mockMapper
            .Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm)
            .Returns(learningSpaceEntity);
        mockMapper
            .Map<BusinessLogic.Entities.LearningElement>(learningElementVm)
            .Returns(learningElementEntity);
        mockLayoutCommandFactory
            .GetRemoveCommand(learningWorldEntity, learningSpaceEntity,
                learningElementEntity, Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory, selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.DragLearningElementToUnplaced(learningWorldVm, learningSpaceVm, learningElementVm);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);
    }

    [Test]
    public void SwitchLearningElementInSlot_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockCommand = Substitute.For<IPlaceLearningElementInLayoutFromLayout>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement();
        Substitute.For<ILogger<LayoutCommandFactory>>();

        mockMapper
            .Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm)
            .Returns(learningSpaceEntity);
        mockMapper
            .Map<BusinessLogic.Entities.LearningElement>(learningElementVm)
            .Returns(learningElementEntity);
        mockLayoutCommandFactory
            .GetPlaceFromLayoutCommand(learningSpaceEntity, learningElementEntity, 4,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory);

        systemUnderTest.SwitchLearningElementSlot(learningSpaceVm, learningElementVm, 4);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);
    }

    [Test]
    public void DragLearningElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<IDragLearningElement>();
        var learningElementVm = ViewModelProvider.GetLearningElement();
        var mockMapper = Substitute.For<IMapper>();
        var learningElementEntity = EntityProvider.GetLearningElement();
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(learningElementEntity);
        mockElementCommandFactory.GetDragCommand(learningElementEntity, 1, 2,
                learningElementEntity.PositionX, learningElementEntity.PositionY,
                Arg.Any<Action<BusinessLogic.Entities.LearningElement>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.DragLearningElement(learningElementVm, 1, 2);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteLearningElementInSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<IDeleteLearningElementInSpace>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement(parent: learningSpaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement(parent: learningSpaceEntity);
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(learningElementEntity);
        mockElementCommandFactory.GetDeleteInSpaceCommand(learningElementEntity, learningSpaceEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.DeleteLearningElementInSpace(learningSpaceVm, learningElementVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteLearningElementInWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<IDeleteLearningElementInWorld>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningElementVm = ViewModelProvider.GetLearningElement();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var learningElementEntity = EntityProvider.GetLearningElement();
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(learningElementEntity);
        mockElementCommandFactory.GetDeleteInWorldCommand(learningElementEntity, learningWorldEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.DeleteLearningElementInWorld(learningWorldVm, learningElementVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreateLearningPathWay_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockPathwayCommandFactory = Substitute.For<IPathwayCommandFactory>();
        var mockCommand = Substitute.For<ICreateLearningPathWay>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var sourceSpaceVm = ViewModelProvider.GetLearningSpace();
        var targetSpaceVm = ViewModelProvider.GetLearningSpace();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var sourceSpaceEntity = EntityProvider.GetLearningSpace();
        var targetSpaceEntity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<PathwayCommandFactory>>();

        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<IObjectInPathWay>(sourceSpaceVm)
            .Returns(sourceSpaceEntity);
        mockMapper.Map<IObjectInPathWay>(targetSpaceVm)
            .Returns(targetSpaceEntity);
        mockPathwayCommandFactory.GetCreateCommand(learningWorldEntity, sourceSpaceEntity, targetSpaceEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            pathwayCommandFactory: mockPathwayCommandFactory);

        systemUnderTest.CreateLearningPathWay(learningWorldVm, sourceSpaceVm, targetSpaceVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteLearningPathWay_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockPathwayCommandFactory = Substitute.For<IPathwayCommandFactory>();
        var mockCommand = Substitute.For<IDeleteLearningPathWay>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var sourceSpaceVm = ViewModelProvider.GetLearningSpace();
        var targetSpaceVm = ViewModelProvider.GetLearningSpace();
        var pathWayVm = ViewModelProvider.GetLearningPathway(sourceSpaceVm, targetSpaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var pathWayEntity =
            EntityProvider.GetLearningPathway(EntityProvider.GetLearningSpace(), EntityProvider.GetLearningSpace());
        learningWorldEntity.LearningPathways.Add(pathWayEntity);
        Substitute.For<ILogger<PathwayCommandFactory>>();


        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<LearningPathway>(Arg.Any<LearningPathwayViewModel>())
            .Returns(pathWayEntity);
        mockPathwayCommandFactory.GetDeleteCommand(learningWorldEntity, pathWayEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            pathwayCommandFactory: mockPathwayCommandFactory,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.DeleteLearningPathWay(learningWorldVm, pathWayVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningObjectInPathWay(null, mockCommand);
    }

    [Test]
    public void CreatePathWayCondition_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockConditionCommandFactory = Substitute.For<IConditionCommandFactory>();
        var mockCommand = Substitute.For<ICreatePathWayCondition>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockCondition = new PathWayConditionViewModel(ConditionEnum.And, false, 1, 2);
        Substitute.For<ILogger<ConditionCommandFactory>>();
        learningWorldVm.PathWayConditions.Add(mockCondition);
        mockConditionCommandFactory.GetCreateCommand(learningWorldEntity, ConditionEnum.And, 6, 7,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            selectedViewModelsProvider: selectedViewModelsProvider,
            conditionCommandFactory: mockConditionCommandFactory);

        systemUnderTest.CreatePathWayCondition(learningWorldVm, ConditionEnum.And, 6, 7);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreatePathWayConditionBetweenObjects_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockConditionCommandFactory = Substitute.For<IConditionCommandFactory>();
        Substitute.For<ILogger<ConditionCommandFactory>>();
        var mockCommand = Substitute.For<ICreatePathWayCondition>();
        var condition = ConditionEnum.And;
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var sourceSpaceVm = ViewModelProvider.GetLearningSpace();
        var targetSpaceVm = ViewModelProvider.GetLearningSpace();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var sourceSpaceEntity = EntityProvider.GetLearningSpace();
        var targetSpaceEntity = EntityProvider.GetLearningSpace();
        learningWorldEntity.LearningSpaces.Add(sourceSpaceEntity);
        learningWorldEntity.LearningSpaces.Add(targetSpaceEntity);

        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<IObjectInPathWay>(sourceSpaceVm)
            .Returns(sourceSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(targetSpaceVm)
            .Returns(targetSpaceEntity);
        mockConditionCommandFactory.GetCreateCommand(learningWorldEntity, condition, sourceSpaceEntity,
                targetSpaceEntity, Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            conditionCommandFactory: mockConditionCommandFactory);

        systemUnderTest.CreatePathWayConditionBetweenObjects(learningWorldVm, condition, sourceSpaceVm, targetSpaceVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void EditPathWayCondition_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockConditionCommandFactory = Substitute.For<IConditionCommandFactory>();
        var mockCommand = Substitute.For<IEditPathWayCondition>();
        var pathWayConditionVm = ViewModelProvider.GetPathWayCondition();
        var mockMapper = Substitute.For<IMapper>();
        var pathWayConditionEntity = EntityProvider.GetPathWayCondition();
        Substitute.For<ILogger<ConditionCommandFactory>>();
        mockMapper.Map<PathWayCondition>(Arg.Any<PathWayConditionViewModel>())
            .Returns(pathWayConditionEntity);
        mockConditionCommandFactory.GetEditCommand(pathWayConditionEntity, ConditionEnum.Or,
                Arg.Any<Action<PathWayCondition>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            conditionCommandFactory: mockConditionCommandFactory);

        systemUnderTest.EditPathWayCondition(pathWayConditionVm, ConditionEnum.Or);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeletePathWayCondition_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockConditionCommandFactory = Substitute.For<IConditionCommandFactory>();
        var mockCommand = Substitute.For<IDeletePathWayCondition>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var pathWayConditionVm = ViewModelProvider.GetPathWayCondition();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var pathWayConditionEntity = EntityProvider.GetPathWayCondition();
        Substitute.For<ILogger<ConditionCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<PathWayCondition>(Arg.Any<PathWayConditionViewModel>())
            .Returns(pathWayConditionEntity);
        mockConditionCommandFactory.GetDeleteCommand(learningWorldEntity, pathWayConditionEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            conditionCommandFactory: mockConditionCommandFactory);

        systemUnderTest.DeletePathWayCondition(learningWorldVm, pathWayConditionVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreateTopic_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockTopicCommandFactory = Substitute.For<ITopicCommandFactory>();
        var mockCommand = Substitute.For<ICreateTopic>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        Substitute.For<ILogger<TopicCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockTopicCommandFactory
            .GetCreateCommand(learningWorldEntity, "f", Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            topicCommandFactory: mockTopicCommandFactory);

        systemUnderTest.CreateTopic(learningWorldVm, "f");

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void EditTopic_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockTopicCommandFactory = Substitute.For<ITopicCommandFactory>();
        var mockCommand = Substitute.For<IEditTopic>();
        var topicVm = ViewModelProvider.GetTopic();
        var mockMapper = Substitute.For<IMapper>();
        var topicEntity = EntityProvider.GetTopic();
        Substitute.For<ILogger<TopicCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.Topic>(Arg.Any<TopicViewModel>())
            .Returns(topicEntity);
        mockTopicCommandFactory
            .GetEditCommand(topicEntity, "g", Arg.Any<Action<BusinessLogic.Entities.Topic>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            topicCommandFactory: mockTopicCommandFactory);

        systemUnderTest.EditTopic(topicVm, "g");

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteTopic_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockBatchCommandFactory = Substitute.For<IBatchCommandFactory>();
        var mockTopicCommandFactory = Substitute.For<ITopicCommandFactory>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockBatchCommand = Substitute.For<IBatchCommand>();
        var mockEditSpaceCommand1 = Substitute.For<IEditLearningSpace>();
        var mockEditSpaceCommand2 = Substitute.For<IEditLearningSpace>();
        var mockEditSpaceCommand3 = Substitute.For<IEditLearningSpace>();
        var mockDeleteTopicCommand = Substitute.For<IDeleteTopic>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var topicVm = ViewModelProvider.GetTopic();
        var spaceVm1 = ViewModelProvider.GetLearningSpace(assignedTopic: topicVm);
        var spaceVm2 = ViewModelProvider.GetLearningSpace(assignedTopic: topicVm);
        var spaceVm3 = ViewModelProvider.GetLearningSpace(assignedTopic: topicVm);
        learningWorldVm.LearningSpaces.Add(spaceVm1);
        learningWorldVm.LearningSpaces.Add(spaceVm2);
        learningWorldVm.LearningSpaces.Add(spaceVm3);
        learningWorldVm.Topics.Add(topicVm);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var topicEntity = EntityProvider.GetTopic();
        var spaceEntity1 = EntityProvider.GetLearningSpace(assignedTopic: topicEntity);
        var spaceEntity2 = EntityProvider.GetLearningSpace(assignedTopic: topicEntity);
        var spaceEntity3 = EntityProvider.GetLearningSpace(assignedTopic: topicEntity);
        learningWorldEntity.LearningSpaces.Add(spaceEntity1);
        learningWorldEntity.LearningSpaces.Add(spaceEntity2);
        learningWorldEntity.LearningSpaces.Add(spaceEntity3);
        learningWorldEntity.Topics.Add(topicEntity);
        Substitute.For<ILogger<SpaceCommandFactory>>();
        Substitute.For<ILogger<TopicCommandFactory>>();

        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<BusinessLogic.Entities.Topic>(Arg.Any<TopicViewModel>())
            .Returns(topicEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(spaceVm1)
            .Returns(spaceEntity1);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(spaceVm2)
            .Returns(spaceEntity2);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(spaceVm3)
            .Returns(spaceEntity3);

        mockTopicCommandFactory
            .GetDeleteCommand(learningWorldEntity, topicEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockDeleteTopicCommand);
        mockSpaceCommandFactory
            .GetEditCommand(spaceEntity1, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), null, Arg.Any<Action<ILearningSpace>>())
            .Returns(mockEditSpaceCommand1);
        mockSpaceCommandFactory
            .GetEditCommand(spaceEntity2, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), null, Arg.Any<Action<ILearningSpace>>())
            .Returns(mockEditSpaceCommand2);
        mockSpaceCommandFactory
            .GetEditCommand(spaceEntity3, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), null, Arg.Any<Action<ILearningSpace>>())
            .Returns(mockEditSpaceCommand3);
        mockBatchCommandFactory
            .GetBatchCommand(Arg.Is<IEnumerable<IUndoCommand>>(i =>
                i.SequenceEqual(new IUndoCommand[]
                    { mockEditSpaceCommand1, mockEditSpaceCommand2, mockEditSpaceCommand3, mockDeleteTopicCommand })
            ))
            .Returns(mockBatchCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            spaceCommandFactory: mockSpaceCommandFactory, topicCommandFactory: mockTopicCommandFactory,
            batchCommandFactory: mockBatchCommandFactory);

        systemUnderTest.DeleteTopic(learningWorldVm, topicVm);

        mockTopicCommandFactory
            .Received()
            .GetDeleteCommand(learningWorldEntity, topicEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>());
        mockSpaceCommandFactory
            .Received()
            .GetEditCommand(spaceEntity1, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), null, Arg.Any<Action<ILearningSpace>>());
        mockSpaceCommandFactory
            .Received()
            .GetEditCommand(spaceEntity2, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), null, Arg.Any<Action<ILearningSpace>>());
        mockSpaceCommandFactory
            .Received()
            .GetEditCommand(spaceEntity3, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<Theme>(), null, Arg.Any<Action<ILearningSpace>>());
        mockBatchCommandFactory
            .Received()
            .GetBatchCommand(Arg.Is<IEnumerable<IUndoCommand>>(i =>
                i.SequenceEqual(new IUndoCommand[]
                    { mockEditSpaceCommand1, mockEditSpaceCommand2, mockEditSpaceCommand3, mockDeleteTopicCommand })
            ));
        mockBusinessLogic.Received().ExecuteCommand(mockBatchCommand);
    }

    [Test]
    public async Task SaveLearningWorldAsync_NoPreviousPathInWorld_GeneratesNewWorldFilepath()
    {
        var resultId = Guid.Empty;
        var resultName = string.Empty;
        var resultPath = string.Empty;
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.When(sub => sub.AddSavedLearningWorldPath(Arg.Any<SavedLearningWorldPath>())).Do(sub =>
        {
            resultId = sub.Arg<SavedLearningWorldPath>().Id;
            resultName = sub.Arg<SavedLearningWorldPath>().Name;
            resultPath = sub.Arg<SavedLearningWorldPath>().Path;
        });
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorld = ViewModelProvider.GetLearningWorld();
        var entity = EntityProvider.GetLearningWorld();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorld).Returns(entity);
        var filepathPart1 = Path.Join(ApplicationPaths.SavedWorldsFolder, $"{learningWorld.Name}-");
        var filepathPart2 = learningWorld.FileEnding;

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            hybridSupportWrapper: mockHybridSupport);

        systemUnderTest.SaveLearningWorld(learningWorld);

        mockBusinessLogic.Received().AddSavedLearningWorldPath(Arg.Any<SavedLearningWorldPath>());
        Assert.Multiple(() =>
        {
            Assert.That(resultId, Is.EqualTo(entity.Id));
            Assert.That(resultName, Is.EqualTo(entity.Name));
            Assert.That(resultPath, Contains.Substring(filepathPart1));
            Assert.That(resultPath, Contains.Substring(filepathPart2));
        });
    }

    [Test]
    public async Task SaveLearningWorldAsync_PreviousPathInWorld_UsesPreviousPath()
    {
        var resultId = Guid.Empty;
        var resultName = string.Empty;
        var resultPath = string.Empty;
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.When(sub => sub.AddSavedLearningWorldPath(Arg.Any<SavedLearningWorldPath>())).Do(sub =>
        {
            resultId = sub.Arg<SavedLearningWorldPath>().Id;
            resultName = sub.Arg<SavedLearningWorldPath>().Name;
            resultPath = sub.Arg<SavedLearningWorldPath>().Path;
        });
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorld = ViewModelProvider.GetLearningWorld();
        var learningWorldSavePath = "foobar";
        learningWorld.SavePath = learningWorldSavePath;
        var entity = EntityProvider.GetLearningWorld();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorld).Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            hybridSupportWrapper: mockHybridSupport);

        systemUnderTest.SaveLearningWorld(learningWorld);

        mockBusinessLogic.Received().AddSavedLearningWorldPath(Arg.Any<SavedLearningWorldPath>());
        Assert.Multiple(() =>
        {
            Assert.That(resultId, Is.EqualTo(entity.Id));
            Assert.That(resultName, Is.EqualTo(entity.Name));
            Assert.That(resultPath, Is.EqualTo(learningWorldSavePath));
        });
    }

    [Test]
    public void SaveLearningSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var learningSpace = ViewModelProvider.GetLearningSpace();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveLearningSpaceAsync(learningSpace));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void SaveLearningSpaceAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var learningSpace = ViewModelProvider.GetLearningSpace();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveLearningSpaceAsync(learningSpace));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task SaveLearningSpaceAsync_CallsDialogManagerAndSpaceMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<ISaveLearningSpace>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpace = ViewModelProvider.GetLearningSpace();
        var entity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>()).Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        mockSpaceCommandFactory
            .GetSaveCommand(mockBusinessLogic, entity, Arg.Any<string>())
            .Returns(mockCommand);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport,
            spaceCommandFactory: mockSpaceCommandFactory);

        await systemUnderTest.SaveLearningSpaceAsync(learningSpace);

        await mockDialogManger.Received()
            .ShowSaveAsDialogAsync("Save Learning Space", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.LearningSpace>(learningSpace);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void SaveLearningSpaceAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);
        var learningSpace = ViewModelProvider.GetLearningSpace();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await systemUnderTest.SaveLearningSpaceAsync(learningSpace));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }

    [Test]
    public void SaveLearningElementAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var learningElement = ViewModelProvider.GetLearningElement();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveLearningElementAsync(learningElement));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void SaveLearningElementAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var learningElement = ViewModelProvider.GetLearningElement();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveLearningElementAsync(learningElement));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task SaveLearningElementAsync_CallsDialogManagerAndElementMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<ISaveLearningElement>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockHybridSupport
            .IsElectronActive
            .Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningElement = ViewModelProvider.GetLearningElement();
        var entity = EntityProvider.GetLearningElement();
        mockMapper
            .Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider
            .GetService(typeof(IElectronDialogManager))
            .Returns(mockDialogManger);
        mockElementCommandFactory
            .GetSaveCommand(mockBusinessLogic, entity, Arg.Any<string>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport,
            elementCommandFactory: mockElementCommandFactory);

        await systemUnderTest.SaveLearningElementAsync(learningElement);

        await mockDialogManger.Received()
            .ShowSaveAsDialogAsync("Save Learning Element", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.LearningElement>(learningElement);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void SaveLearningElementAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);
        var learningElement = ViewModelProvider.GetLearningElement();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await systemUnderTest.SaveLearningElementAsync(learningElement));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }

    [Test]
    public void LoadLearningWorldAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadLearningWorldAsync(authoringToolWorkspaceVm));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadLearningWorldAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.LoadLearningWorldAsync(authoringToolWorkspaceVm));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task LoadLearningWorldAsync_CallsDialogManagerAndElementMapper()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();

        var workspaceEntity = EntityProvider.GetAuthoringToolWorkspace();
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<LearningWorldViewModel>())
            .Returns(workspaceEntity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest = CreateTestablePresentationLogic(mapper: mockMapper, serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.LoadLearningWorldAsync(authoringToolWorkspaceVm);

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Learning World", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockMapper.Received().Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
    }

    [Test]
    public void LoadLearningWorldAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await systemUnderTest.LoadLearningWorldAsync(authoringToolWorkspaceVm));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }

    [Test]
    public void GetWorldSavePath_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(() =>
            systemUnderTest.GetWorldSavePath());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void GetWorldSavePath_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.GetWorldSavePath());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public void GetWorldSavePath_CallsDialogManager()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        const string filepath = "foobar.awf";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport);

        var result = systemUnderTest.GetWorldSavePath();

        Assert.That(result.Result, Is.EqualTo(filepath));
        mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Learning World", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
    }

    [Test]
    public void LoadLearningWorldFromPath_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.Throws<NotImplementedException>(() =>
            systemUnderTest.LoadLearningWorldFromPath(authoringToolWorkspaceVm, "foobar"));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadLearningWorldFromPath_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            systemUnderTest.LoadLearningWorldFromPath(authoringToolWorkspaceVm, "foobar"));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public void LoadLearningWorldFromPath_CallsBusinessLogic()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var workspaceEntity =
            new BusinessLogic.Entities.AuthoringToolWorkspace(new List<ILearningWorld>());
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockServiceProvider
            .GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockCommand = Substitute.For<ILoadLearningWorld>();
        Substitute.For<ILogger<WorldCommandFactory>>();
        mockWorldCommandFactory
            .GetLoadCommand(workspaceEntity, "foobar", mockBusinessLogic,
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>())
            .Returns(mockCommand);
        mockMapper
            .Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm)
            .Returns(workspaceEntity);
        var guid = new Guid();
        mockCommand.LearningWorld!.Id.Returns(guid);
        var learningWorldViewModel = Substitute.For<ILearningWorldViewModel>();
        learningWorldViewModel.Id.Returns(guid);
        authoringToolWorkspaceVm.LearningWorlds
            .Returns(new List<ILearningWorldViewModel> { learningWorldViewModel });

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport,
            worldCommandFactory: mockWorldCommandFactory, selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.LoadLearningWorldFromPath(authoringToolWorkspaceVm, "foobar");

        mockWorldCommandFactory
            .Received()
            .GetLoadCommand(workspaceEntity, "foobar", mockBusinessLogic,
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>());
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningWorld(learningWorldViewModel, mockCommand);
    }

    [Test]
    public void GetSavedLearningWorldPaths_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.GetSavedLearningWorldPaths();

        mockBusinessLogic.Received().GetSavedLearningWorldPaths();
    }

    [Test]
    public void GetSavedLearningWorldPaths_ReturnsResultFromBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var savedLearningWorldPath = EntityProvider.GetSavedLearningWorldPath();
        mockBusinessLogic.GetSavedLearningWorldPaths()
            .Returns(new List<SavedLearningWorldPath> { savedLearningWorldPath });

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var result = systemUnderTest.GetSavedLearningWorldPaths().ToList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result, Contains.Item(savedLearningWorldPath));
    }

    [Test]
    public void AddSavedLearningWorldPath_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        var savedLearningWorldPath = EntityProvider.GetSavedLearningWorldPath();

        systemUnderTest.AddSavedLearningWorldPath(savedLearningWorldPath);

        mockBusinessLogic.Received().AddSavedLearningWorldPath(savedLearningWorldPath);
    }

    [Test]
    public void AddSavedLearningWorldPathByPathOnly_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.AddSavedLearningWorldPathByPathOnly("foobar");

        mockBusinessLogic.Received().AddSavedLearningWorldPathByPathOnly("foobar");
    }

    [Test]
    public void AddSavedLearningWorldPathByPathOnly_ReturnsResultFromBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var savedLearningWorldPath = EntityProvider.GetSavedLearningWorldPath();
        mockBusinessLogic.AddSavedLearningWorldPathByPathOnly("foobar").Returns(savedLearningWorldPath);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var result = systemUnderTest.AddSavedLearningWorldPathByPathOnly("foobar");

        Assert.That(result, Is.EqualTo(savedLearningWorldPath));
    }

    [Test]
    public void UpdateIdOfSavedLearningWorldPath_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        var savedLearningWorldPath = EntityProvider.GetSavedLearningWorldPath();
        var changedId = Guid.ParseExact("00000000-0000-0000-0000-000000000002", "D");

        systemUnderTest.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, changedId);

        mockBusinessLogic.Received().UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, changedId);
    }

    [Test]
    public void RemoveSavedLearningWorldPath_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        var savedLearningWorldPath = EntityProvider.GetSavedLearningWorldPath();

        systemUnderTest.RemoveSavedLearningWorldPath(savedLearningWorldPath);

        mockBusinessLogic.Received().RemoveSavedLearningWorldPath(savedLearningWorldPath);
    }

    [Test]
    public void LoadLearningSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockLearningWorldViewModel = ViewModelProvider.GetLearningWorld();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadLearningSpaceAsync(mockLearningWorldViewModel));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadLearningSpaceAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockLearningWorldViewModel = ViewModelProvider.GetLearningWorld();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.LoadLearningSpaceAsync(mockLearningWorldViewModel));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task LoadLearningSpaceAsync_CallsDialogManagerAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<ILoadLearningSpace>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLearningWorldViewModel = ViewModelProvider.GetLearningWorld();
        var mockLearningWorldEntity = EntityProvider.GetLearningWorld();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockMapper
            .Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(mockLearningWorldEntity);
        mockSpaceCommandFactory
            .GetLoadCommand(mockLearningWorldEntity, filepath + ".asf", mockBusinessLogic,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport,
            spaceCommandFactory: mockSpaceCommandFactory);

        await systemUnderTest.LoadLearningSpaceAsync(mockLearningWorldViewModel);

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Learning Space", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void LoadLearningSpaceAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        var mockLearningWorldViewModel = ViewModelProvider.GetLearningWorld();
        mockElectronDialogManager
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await systemUnderTest.LoadLearningSpaceAsync(mockLearningWorldViewModel));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }

    [Test]
    public void LoadLearningElementAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockLearningSpaceViewModel = ViewModelProvider.GetLearningSpace();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadLearningElementAsync(mockLearningSpaceViewModel, 0));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadLearningElementAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockLearningSpaceViewModel = ViewModelProvider.GetLearningSpace();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.LoadLearningElementAsync(mockLearningSpaceViewModel, 0));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task LoadLearningElementAsync_CallsDialogManagerAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<ILoadLearningElement>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLearningSpaceViewModel = ViewModelProvider.GetLearningSpace();
        var mockLearningSpaceEntity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider
            .GetService(typeof(IElectronDialogManager))
            .Returns(mockDialogManger);
        mockMapper
            .Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(mockLearningSpaceEntity);
        mockElementCommandFactory
            .GetLoadCommand(mockLearningSpaceEntity, 0, Arg.Any<string>(), mockBusinessLogic,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockLearningElementViewModel = ViewModelProvider.GetLearningElement();
        mockLearningSpaceViewModel.LearningSpaceLayout.LearningElements.Add(0, mockLearningElementViewModel);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            selectedViewModelsProvider: selectedViewModelsProvider, serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport, elementCommandFactory: mockElementCommandFactory);

        await systemUnderTest.LoadLearningElementAsync(mockLearningSpaceViewModel, 0);

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Learning Element", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void LoadLearningElementAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        var mockLearningSpaceViewModel = ViewModelProvider.GetLearningSpace();
        mockElectronDialogManager
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await systemUnderTest.LoadLearningElementAsync(mockLearningSpaceViewModel, 0));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }

    [Test]
    public void PresentationLogic_LoadLearningWorldViewModel_ReturnsLearningWorld()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockCommand = Substitute.For<ILoadLearningWorld>();
        var mockMapper = Substitute.For<IMapper>();
        var workspaceVm = ViewModelProvider.GetAuthoringToolWorkspace();
        var workspaceEntity = EntityProvider.GetAuthoringToolWorkspace();
        var stream = Substitute.For<Stream>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockLearningWorldVm = Substitute.For<ILearningWorldViewModel>();
        workspaceVm.LearningWorlds.Add(mockLearningWorldVm);
        Substitute.For<ILogger<WorldCommandFactory>>();
        mockMapper
            .Map<BusinessLogic.Entities.AuthoringToolWorkspace>(workspaceVm)
            .Returns(workspaceEntity);
        mockWorldCommandFactory
            .GetLoadCommand(workspaceEntity, stream, mockBusinessLogic,
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
                selectedViewModelsProvider: mockSelectedViewModelsProvider,
                worldCommandFactory: mockWorldCommandFactory, mapper: mockMapper);

        systemUnderTest.LoadLearningWorldViewModel(workspaceVm, stream);

        mockWorldCommandFactory
            .Received()
            .GetLoadCommand(workspaceEntity, stream, mockBusinessLogic,
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>());
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningWorld(mockLearningWorldVm, mockCommand);
    }

    [Test]
    public void PresentationLogic_LoadLearningSpaceViewModel_ReturnsLearningSpace()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<ILoadLearningSpace>();
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        mockMapper
            .Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm)
            .Returns(learningWorldEntity);
        var stream = Substitute.For<Stream>();
        mockSpaceCommandFactory
            .GetLoadCommand(learningWorldEntity, stream, mockBusinessLogic,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
                spaceCommandFactory: mockSpaceCommandFactory);

        systemUnderTest.LoadLearningSpaceViewModel(learningWorldVm, stream);

        mockMapper.Received().Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void PresentationLogic_LoadLearningElementViewModel_ReturnsLearningElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<ILoadLearningElement>();
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper
            .Map<BusinessLogic.Entities.LearningSpace>(learningSpaceVm)
            .Returns(learningSpaceEntity);
        var stream = Substitute.For<Stream>();
        mockElementCommandFactory
            .GetLoadCommand(learningSpaceEntity, 0, stream, mockBusinessLogic,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
                elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.LoadLearningElementViewModel(learningSpaceVm, 0, stream);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public async Task PresentationLogic_LoadLearningContentViewModel_ReturnsLearningContent()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningContent = EntityProvider.GetFileContent();
        mockBusinessLogic.LoadLearningContentAsync(Arg.Any<string>(), Arg.Any<MemoryStream>())
            .Returns(mockLearningContent);
        var mockLearningContentViewModel = ViewModelProvider.GetFileContent();
        var mockMapper = Substitute.For<IMapper>();
        mockMapper
            .Map<ILearningContentViewModel>(Arg.Any<ILearningContent>())
            .Returns(mockLearningContentViewModel);
        const string filename = "test.png";
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        var result = await systemUnderTest.LoadLearningContentViewModelAsync(filename, stream);

        await mockBusinessLogic.Received().LoadLearningContentAsync(filename, stream);
        mockMapper.Received().Map<ILearningContentViewModel>(mockLearningContent);
        Assert.That(result, Is.EqualTo(mockLearningContentViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningContentViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningContentAsync(Arg.Any<string>(), Arg.Any<MemoryStream>())
            .Throws(new Exception("Exception"));
        const string filename = "test.png";
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await systemUnderTest.LoadLearningContentViewModelAsync(filename, stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }

    [Test]
    public void CreateAdaptivityTask_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockTaskCommandFactory = Substitute.For<ITaskCommandFactory>();
        var mockCommand = Substitute.For<ICreateAdaptivityTask>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityContentViewModel = ViewModelProvider.GetAdaptivityContent();
        var mockAdaptivityContentEntity = EntityProvider.GetAdaptivityContent();
        const string name = "name";
        Substitute.For<ILogger<TaskCommandFactory>>();
        mockMapper
            .Map<AdaptivityContent>(mockAdaptivityContentViewModel)
            .Returns(mockAdaptivityContentEntity);
        mockTaskCommandFactory
            .GetCreateCommand(mockAdaptivityContentEntity, name,
                Arg.Any<Action<AdaptivityContent>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
                taskCommandFactory: mockTaskCommandFactory);

        systemUnderTest.CreateAdaptivityTask(mockAdaptivityContentViewModel, name);

        mockMapper.Received().Map<AdaptivityContent>(mockAdaptivityContentViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void EditAdaptivityTask_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockTaskCommandFactory = Substitute.For<ITaskCommandFactory>();
        var mockCommand = Substitute.For<IEditAdaptivityTask>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        const string name = "name";
        const QuestionDifficulty minimumRequiredDifficulty = QuestionDifficulty.Medium;
        Substitute.For<ILogger<TaskCommandFactory>>();
        mockMapper
            .Map<AdaptivityTask>(mockAdaptivityTaskViewModel)
            .Returns(mockAdaptivityTaskEntity);
        mockTaskCommandFactory
            .GetEditCommand(mockAdaptivityTaskEntity, name, minimumRequiredDifficulty,
                Arg.Any<Action<AdaptivityTask>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
                taskCommandFactory: mockTaskCommandFactory);

        systemUnderTest.EditAdaptivityTask(mockAdaptivityTaskViewModel, name, minimumRequiredDifficulty);

        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteAdaptivityTask_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockTaskCommandFactory = Substitute.For<ITaskCommandFactory>();
        var mockCommand = Substitute.For<IDeleteAdaptivityTask>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityContentViewModel = ViewModelProvider.GetAdaptivityContent();
        var mockAdaptivityContentEntity = EntityProvider.GetAdaptivityContent();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        Substitute.For<ILogger<TaskCommandFactory>>();
        mockMapper
            .Map<AdaptivityContent>(mockAdaptivityContentViewModel)
            .Returns(mockAdaptivityContentEntity);
        mockMapper
            .Map<AdaptivityTask>(mockAdaptivityTaskViewModel)
            .Returns(mockAdaptivityTaskEntity);
        mockTaskCommandFactory
            .GetDeleteCommand(mockAdaptivityContentEntity, mockAdaptivityTaskEntity,
                Arg.Any<Action<AdaptivityContent>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
                taskCommandFactory: mockTaskCommandFactory);

        systemUnderTest.DeleteAdaptivityTask(mockAdaptivityContentViewModel, mockAdaptivityTaskViewModel);

        mockMapper.Received().Map<AdaptivityContent>(mockAdaptivityContentViewModel);
        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreateMultipleChoiceSingleResponseQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<ICreateMultipleChoiceSingleResponseQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        var mockQuestionViewModel = ViewModelProvider.GetMultipleChoiceSingleResponseQuestion();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var mockChoicesViewModel = mockQuestionViewModel.Choices;
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoiceViewModel = mockQuestionViewModel.CorrectChoice;
        var mockCorrectChoiceEntity = mockQuestionEntity.CorrectChoice;
        const QuestionDifficulty difficulty = QuestionDifficulty.Easy;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<AdaptivityTask>(mockAdaptivityTaskViewModel)
            .Returns(mockAdaptivityTaskEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesViewModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<Choice>(mockCorrectChoiceViewModel)
            .Returns(mockCorrectChoiceEntity);
        mockQuestionCommandFactory
            .GetCreateMultipleChoiceSingleResponseQuestionCommand(mockAdaptivityTaskEntity, difficulty,
                questionText, mockChoicesEntity, mockCorrectChoiceEntity, expectedCompletionTime,
                Arg.Any<Action<AdaptivityTask>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
                questionCommandFactory: mockQuestionCommandFactory);

        systemUnderTest.CreateMultipleChoiceSingleResponseQuestion(mockAdaptivityTaskViewModel, difficulty,
            questionText, mockChoicesViewModel, mockCorrectChoiceViewModel, expectedCompletionTime);

        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesViewModel);
        mockMapper.Received().Map<Choice>(mockCorrectChoiceViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreateMultipleChoiceMultipleResponseQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<ICreateMultipleChoiceMultipleResponseQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        var mockQuestionViewModel = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockChoicesViewModel = mockQuestionViewModel.Choices;
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoicesViewModel = mockQuestionViewModel.CorrectChoices;
        var mockCorrectChoicesEntity = mockQuestionEntity.CorrectChoices;
        const QuestionDifficulty difficulty = QuestionDifficulty.Easy;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<AdaptivityTask>(mockAdaptivityTaskViewModel)
            .Returns(mockAdaptivityTaskEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesViewModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockCorrectChoicesViewModel)
            .Returns(mockCorrectChoicesEntity);
        mockQuestionCommandFactory
            .GetCreateMultipleChoiceMultipleResponseQuestionCommand(mockAdaptivityTaskEntity, difficulty,
                questionText, mockChoicesEntity, mockCorrectChoicesEntity, expectedCompletionTime,
                Arg.Any<Action<AdaptivityTask>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
                questionCommandFactory: mockQuestionCommandFactory);

        systemUnderTest.CreateMultipleChoiceMultipleResponseQuestion(mockAdaptivityTaskViewModel, difficulty,
            questionText, mockChoicesViewModel, mockCorrectChoicesViewModel, expectedCompletionTime);

        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockCorrectChoicesViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void EditMultipleChoiceSingleResponseQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<IEditMultipleChoiceSingleResponseQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockQuestionViewModel = ViewModelProvider.GetMultipleChoiceSingleResponseQuestion();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var mockChoicesViewModel = mockQuestionViewModel.Choices;
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoiceViewModel = mockQuestionViewModel.CorrectChoice;
        var mockCorrectChoiceEntity = mockQuestionEntity.CorrectChoice;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<MultipleChoiceSingleResponseQuestion>(mockQuestionViewModel)
            .Returns(mockQuestionEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesViewModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<Choice>(mockCorrectChoiceViewModel)
            .Returns(mockCorrectChoiceEntity);
        mockQuestionCommandFactory
            .GetEditMultipleChoiceSingleResponseQuestionCommand(mockQuestionEntity,
                questionText, mockChoicesEntity, mockCorrectChoiceEntity, expectedCompletionTime,
                Arg.Any<Action<MultipleChoiceSingleResponseQuestion>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            questionCommandFactory: mockQuestionCommandFactory);

        systemUnderTest.EditMultipleChoiceSingleResponseQuestion(mockQuestionViewModel,
            questionText, mockChoicesViewModel, mockCorrectChoiceViewModel, expectedCompletionTime);

        mockMapper.Received().Map<MultipleChoiceSingleResponseQuestion>(mockQuestionViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesViewModel);
        mockMapper.Received().Map<Choice>(mockCorrectChoiceViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void EditMultipleChoiceMultipleResponseQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<IEditMultipleChoiceMultipleResponseQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockQuestionViewModel = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockChoicesViewModel = mockQuestionViewModel.Choices;
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoicesViewModel = mockQuestionViewModel.CorrectChoices;
        var mockCorrectChoicesEntity = mockQuestionEntity.CorrectChoices;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<MultipleChoiceMultipleResponseQuestion>(mockQuestionViewModel)
            .Returns(mockQuestionEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesViewModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockCorrectChoicesViewModel)
            .Returns(mockCorrectChoicesEntity);
        mockQuestionCommandFactory
            .GetEditMultipleChoiceMultipleResponseQuestionCommand(mockQuestionEntity,
                questionText, mockChoicesEntity, mockCorrectChoicesEntity, expectedCompletionTime,
                Arg.Any<Action<MultipleChoiceMultipleResponseQuestion>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            questionCommandFactory: mockQuestionCommandFactory);

        systemUnderTest.EditMultipleChoiceMultipleResponseQuestion(mockQuestionViewModel,
            questionText, mockChoicesViewModel, mockCorrectChoicesViewModel, expectedCompletionTime);

        mockMapper.Received().Map<MultipleChoiceMultipleResponseQuestion>(mockQuestionViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockCorrectChoicesViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void EditMultipleChoiceQuestionWithTypeChange_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<IEditMultipleChoiceQuestionWithTypeChange>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        var mockQuestionViewModel = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockChoicesViewModel = mockQuestionViewModel.Choices;
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoicesViewModel = mockQuestionViewModel.CorrectChoices;
        var mockCorrectChoicesEntity = mockQuestionEntity.CorrectChoices;
        const bool isSingleResponse = true;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<AdaptivityTask>(mockAdaptivityTaskViewModel)
            .Returns(mockAdaptivityTaskEntity);
        mockMapper
            .Map<IMultipleChoiceQuestion>(mockQuestionViewModel)
            .Returns(mockQuestionEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesViewModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockCorrectChoicesViewModel)
            .Returns(mockCorrectChoicesEntity);
        mockQuestionCommandFactory
            .GetEditMultipleChoiceQuestionWithTypeChangeCommand(mockAdaptivityTaskEntity, mockQuestionEntity,
                isSingleResponse,
                questionText, mockChoicesEntity, mockCorrectChoicesEntity, expectedCompletionTime,
                Arg.Any<Action<AdaptivityTask>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            questionCommandFactory: mockQuestionCommandFactory);

        systemUnderTest.EditMultipleChoiceQuestionWithTypeChange(mockAdaptivityTaskViewModel, mockQuestionViewModel,
            isSingleResponse,
            questionText, mockChoicesViewModel, mockCorrectChoicesViewModel, expectedCompletionTime);

        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockMapper.Received().Map<IMultipleChoiceQuestion>(mockQuestionViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockCorrectChoicesViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteAdaptivityQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<IDeleteAdaptivityQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        var mockQuestionViewModel = ViewModelProvider.GetMultipleChoiceSingleResponseQuestion();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<AdaptivityTask>(mockAdaptivityTaskViewModel)
            .Returns(mockAdaptivityTaskEntity);
        mockMapper
            .Map<IAdaptivityQuestion>(mockQuestionViewModel)
            .Returns(mockQuestionEntity);
        mockQuestionCommandFactory
            .GetDeleteCommand(mockAdaptivityTaskEntity, mockQuestionEntity,
                Arg.Any<Action<AdaptivityTask>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            questionCommandFactory: mockQuestionCommandFactory);

        systemUnderTest.DeleteAdaptivityQuestion(mockAdaptivityTaskViewModel, mockQuestionViewModel);

        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockMapper.Received().Map<IAdaptivityQuestion>(mockQuestionViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreateAdaptivityRule_WhenCalled_ShouldExecuteCreateCommand()
    {
        var mapper = Substitute.For<IMapper>();
        var adaptivityRuleCommandFactory = Substitute.For<IAdaptivityRuleCommandFactory>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var questionViewModel = Substitute.For<IAdaptivityQuestionViewModel>();
        var triggerViewModel = Substitute.For<IAdaptivityTriggerViewModel>();
        var actionViewModel = Substitute.For<IAdaptivityActionViewModel>();
        var questionEntity = Substitute.For<IAdaptivityQuestion>();
        var triggerEntity = Substitute.For<IAdaptivityTrigger>();
        var actionEntity = Substitute.For<IAdaptivityAction>();

        mapper.Map<IAdaptivityQuestion>(questionViewModel).Returns(questionEntity);
        mapper.Map<IAdaptivityTrigger>(triggerViewModel).Returns(triggerEntity);
        mapper.Map<IAdaptivityAction>(actionViewModel).Returns(actionEntity);

        var createCommand = Substitute.For<ICreateAdaptivityRule>();
        adaptivityRuleCommandFactory
            .GetCreateCommand(questionEntity, triggerEntity, actionEntity, Arg.Any<Action<IAdaptivityQuestion>>())
            .Returns(createCommand);

        var systemUnderTest = CreateTestablePresentationLogic(mapper: mapper,
            adaptivityRuleCommandFactory: adaptivityRuleCommandFactory, businessLogic: businessLogic);

        systemUnderTest.CreateAdaptivityRule(questionViewModel, triggerViewModel, actionViewModel);

        adaptivityRuleCommandFactory.Received().GetCreateCommand(questionEntity, triggerEntity, actionEntity,
            Arg.Any<Action<IAdaptivityQuestion>>());
        businessLogic.Received().ExecuteCommand(createCommand);
    }

    [Test]
    public void DeleteAdaptivityRule_WhenCalled_ShouldExecuteDeleteCommand()
    {
        var mapper = Substitute.For<IMapper>();
        var adaptivityRuleCommandFactory = Substitute.For<IAdaptivityRuleCommandFactory>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var questionViewModel = Substitute.For<IAdaptivityQuestionViewModel>();
        var ruleViewMOdel = Substitute.For<IAdaptivityRuleViewModel>();
        var questionEntity = Substitute.For<IAdaptivityQuestion>();
        var ruleEntity = Substitute.For<IAdaptivityRule>();

        mapper.Map<IAdaptivityQuestion>(questionViewModel).Returns(questionEntity);
        mapper.Map<IAdaptivityRule>(ruleViewMOdel).Returns(ruleEntity);

        var deleteCommand = Substitute.For<IDeleteAdaptivityRule>();
        adaptivityRuleCommandFactory
            .GetDeleteCommand(questionEntity, ruleEntity, Arg.Any<Action<IAdaptivityQuestion>>())
            .Returns(deleteCommand);

        var systemUnderTest = CreateTestablePresentationLogic(mapper: mapper,
            adaptivityRuleCommandFactory: adaptivityRuleCommandFactory, businessLogic: businessLogic);

        systemUnderTest.DeleteAdaptivityRule(questionViewModel, ruleViewMOdel);

        adaptivityRuleCommandFactory.Received()
            .GetDeleteCommand(questionEntity, ruleEntity, Arg.Any<Action<IAdaptivityQuestion>>());
        businessLogic.Received().ExecuteCommand(deleteCommand);
    }

    [Test]
    public void EditCommentAction_WhenCalled_ShouldExecuteEditCommand()
    {
        var mapper = Substitute.For<IMapper>();
        var adaptivityActionCommandFactory = Substitute.For<IAdaptivityActionCommandFactory>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var commentActionViewModel = ViewModelProvider.GetCommentAction();
        var commentActionEntity = EntityProvider.GetCommentAction();
        const string comment = "foobar";

        mapper.Map<CommentAction>(commentActionViewModel).Returns(commentActionEntity);

        var editCommand = Substitute.For<IEditCommentAction>();
        adaptivityActionCommandFactory
            .GetEditCommentAction(commentActionEntity, comment, Arg.Any<Action<CommentAction>>()).Returns(editCommand);

        var systemUnderTest = CreateTestablePresentationLogic(mapper: mapper,
            adaptivityActionCommandFactory: adaptivityActionCommandFactory, businessLogic: businessLogic);

        systemUnderTest.EditCommentAction(commentActionViewModel, comment);

        adaptivityActionCommandFactory.Received()
            .GetEditCommentAction(commentActionEntity, comment, Arg.Any<Action<CommentAction>>());
        businessLogic.Received().ExecuteCommand(editCommand);
    }

    [Test]
    public void EditElementReferenceAction_WhenCalled_ShouldExecuteEditCommand()
    {
        var mapper = Substitute.For<IMapper>();
        var adaptivityActionCommandFactory = Substitute.For<IAdaptivityActionCommandFactory>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var elementReferenceActionViewModel = ViewModelProvider.GetElementReferenceAction();
        var elementReferenceActionEntity = EntityProvider.GetElementReferenceAction();
        var guid = Guid.NewGuid();
        var comment = "somecomment";

        mapper.Map<ElementReferenceAction>(elementReferenceActionViewModel).Returns(elementReferenceActionEntity);

        var editCommand = Substitute.For<IEditElementReferenceAction>();
        adaptivityActionCommandFactory
            .GetEditElementReferenceAction(elementReferenceActionEntity, guid, comment,
                Arg.Any<Action<ElementReferenceAction>>()).Returns(editCommand);

        var systemUnderTest = CreateTestablePresentationLogic(mapper: mapper,
            adaptivityActionCommandFactory: adaptivityActionCommandFactory, businessLogic: businessLogic);

        systemUnderTest.EditElementReferenceAction(elementReferenceActionViewModel, guid, comment);

        adaptivityActionCommandFactory.Received().GetEditElementReferenceAction(elementReferenceActionEntity, guid,
            comment, Arg.Any<Action<ElementReferenceAction>>());
        businessLogic.Received().ExecuteCommand(editCommand);
    }

    [Test]
    public void EditContentReferenceAction_WhenCalled_ShouldExecuteEditCommand()
    {
        var mapper = Substitute.For<IMapper>();
        var adaptivityActionCommandFactory = Substitute.For<IAdaptivityActionCommandFactory>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var commentActionViewModel = ViewModelProvider.GetContentReferenceAction();
        var commentActionEntity = EntityProvider.GetContentReferenceAction();
        var contentViewModel = Substitute.For<ILearningContentViewModel>();
        var contentEntity = Substitute.For<ILearningContent>();
        var comment = "somestring";

        mapper.Map<ContentReferenceAction>(commentActionViewModel).Returns(commentActionEntity);
        mapper.Map<ILearningContent>(contentViewModel).Returns(contentEntity);

        var editCommand = Substitute.For<IEditContentReferenceAction>();
        adaptivityActionCommandFactory
            .GetEditContentReferenceAction(commentActionEntity, contentEntity, comment,
                Arg.Any<Action<ContentReferenceAction>>()).Returns(editCommand);

        var systemUnderTest = CreateTestablePresentationLogic(mapper: mapper,
            adaptivityActionCommandFactory: adaptivityActionCommandFactory, businessLogic: businessLogic);

        systemUnderTest.EditContentReferenceAction(commentActionViewModel, contentViewModel, comment);

        adaptivityActionCommandFactory.Received().GetEditContentReferenceAction(commentActionEntity, contentEntity,
            comment, Arg.Any<Action<ContentReferenceAction>>());
        businessLogic.Received().ExecuteCommand(editCommand);
    }


    [Test]
    public async Task ShowLearningElementContentAsync_CallsShellWrapper()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockShellWrapper = Substitute.For<IShellWrapper>();
        mockShellWrapper.OpenPathAsync(Arg.Any<string>()).Returns("");
        var mockContent = ViewModelProvider.GetFileContent(filepath: "pathpath");
        var mockLearningElement = ViewModelProvider.GetLearningElement(content: mockContent);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport, shellWrapper: mockShellWrapper);

        await systemUnderTest.ShowLearningElementContentAsync(mockLearningElement);

        await mockShellWrapper.Received().OpenPathAsync("pathpath");
    }

    [Test]
    public void IsLmsConnected_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.IsLmsConnected();

        mockBusinessLogic.Received().IsLmsConnected();
    }

    [Test]
    public void IsLmsConnected_ReturnsTask()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var expectedResult = new Task<bool>(() => true);
        mockBusinessLogic.IsLmsConnected().Returns(expectedResult);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var result = systemUnderTest.IsLmsConnected();

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void LoginName_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        _ = systemUnderTest.LoginName;

        _ = mockBusinessLogic.Received().LoginName;
    }

    [Test]
    public void Login_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var username = "username";
        var password = "password";

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.Login(username, password);

        mockBusinessLogic.Received().Login(username, password);
    }

    [Test]
    public void Login_ReturnsTask()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var username = "username";
        var password = "password";
        var expectedResult = new Task<bool>(() => true);
        mockBusinessLogic.Login(username, password).Returns(expectedResult);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var result = systemUnderTest.Login(username, password);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void Logout_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.Logout();

        mockBusinessLogic.Received().Logout();
    }

    [Test]
    public async Task GetLmsWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        await systemUnderTest.GetLmsWorldList();

        await mockBusinessLogic.Received().GetLmsWorldList();
    }

    [Test]
    public async Task DeleteLmsWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldVm = Substitute.For<LmsWorldViewModel>();
        var mockWorld = Substitute.For<LmsWorld>();
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LmsWorld>(Arg.Any<LmsWorldViewModel>()).Returns(mockWorld);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        await systemUnderTest.DeleteLmsWorld(mockWorldVm);

        await mockBusinessLogic.Received().DeleteLmsWorld(mockWorld);
    }

    private static Presentation.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(
        IApplicationConfiguration? configuration = null, IBusinessLogic? businessLogic = null, IMapper? mapper = null,
        ICachingMapper? cachingMapper = null, ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        IServiceProvider? serviceProvider = null,
        ILogger<Presentation.PresentationLogic.API.PresentationLogic>? logger = null,
        IHybridSupportWrapper? hybridSupportWrapper = null, IShellWrapper? shellWrapper = null,
        IQuestionCommandFactory? questionCommandFactory = null,
        ITaskCommandFactory? taskCommandFactory = null,
        IConditionCommandFactory? conditionCommandFactory = null,
        IElementCommandFactory? elementCommandFactory = null,
        ILayoutCommandFactory? layoutCommandFactory = null,
        IPathwayCommandFactory? pathwayCommandFactory = null,
        ISpaceCommandFactory? spaceCommandFactory = null,
        ITopicCommandFactory? topicCommandFactory = null,
        IWorldCommandFactory? worldCommandFactory = null,
        IBatchCommandFactory? batchCommandFactory = null,
        IAdaptivityRuleCommandFactory? adaptivityRuleCommandFactory = null,
        IAdaptivityActionCommandFactory? adaptivityActionCommandFactory = null,
        IFileSystem? fileSystem = null)
    {
        configuration ??= Substitute.For<IApplicationConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        mapper ??= Substitute.For<IMapper>();
        cachingMapper ??= Substitute.For<ICachingMapper>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        hybridSupportWrapper ??= Substitute.For<IHybridSupportWrapper>();
        shellWrapper ??= Substitute.For<IShellWrapper>();
        fileSystem ??= new MockFileSystem();

        questionCommandFactory ??= Substitute.For<IQuestionCommandFactory>();
        taskCommandFactory ??= Substitute.For<ITaskCommandFactory>();
        conditionCommandFactory ??= Substitute.For<IConditionCommandFactory>();
        elementCommandFactory ??= Substitute.For<IElementCommandFactory>();
        layoutCommandFactory ??= Substitute.For<ILayoutCommandFactory>();
        pathwayCommandFactory ??= Substitute.For<IPathwayCommandFactory>();
        spaceCommandFactory ??= Substitute.For<ISpaceCommandFactory>();
        topicCommandFactory ??= Substitute.For<ITopicCommandFactory>();
        worldCommandFactory ??= Substitute.For<IWorldCommandFactory>();
        batchCommandFactory ??= Substitute.For<IBatchCommandFactory>();
        adaptivityRuleCommandFactory ??= Substitute.For<IAdaptivityRuleCommandFactory>();
        adaptivityActionCommandFactory ??= Substitute.For<IAdaptivityActionCommandFactory>();

        return new Presentation.PresentationLogic.API.PresentationLogic(configuration, businessLogic, mapper,
            cachingMapper, selectedViewModelsProvider, serviceProvider, logger, hybridSupportWrapper, shellWrapper,
            questionCommandFactory, taskCommandFactory, conditionCommandFactory, elementCommandFactory,
            layoutCommandFactory, pathwayCommandFactory, spaceCommandFactory, topicCommandFactory, worldCommandFactory,
            batchCommandFactory, adaptivityRuleCommandFactory, adaptivityActionCommandFactory, fileSystem);
    }
}