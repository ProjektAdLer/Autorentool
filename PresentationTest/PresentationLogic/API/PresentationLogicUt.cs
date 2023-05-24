using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using ElectronWrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
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
        var mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockCachingMapper = Substitute.For<ICachingMapper>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        var mockHybridSupportWrapper = Substitute.For<IHybridSupportWrapper>();
        var mockShellWrapper = Substitute.For<IShellWrapper>();
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
            mockCachingMapper, mockSelectedViewModelsProvider, mockServiceProvider, mockLogger, mockHybridSupportWrapper,
            mockShellWrapper, mockConditionCommandFactory, mockElementCommandFactory, mockLayoutCommandFactory,
            mockPathwayCommandFactory, mockSpaceCommandFactory, mockTopicCommandFactory, mockWorldCommandFactory,
            mockBatchCommandFactory);
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
        mockDialogManager
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns("supersecretfilepath");
        var viewModel = new LearningWorldViewModel("fo", "fo", "fo", "fo", "fo", "fo");
        var mockMapper = Substitute.For<IMapper>();
        var entity = new BusinessLogic.Entities.LearningWorld("baba", "baba", "baba", "baba", "baba", "baba");
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(viewModel).Returns(entity);
        var serviceProvider = new ServiceCollection();
        serviceProvider.Insert(0, new ServiceDescriptor(typeof(IElectronDialogManager), mockDialogManager));

        var systemUnderTest = CreateTestablePresentationLogic(null, mockBusinessLogic, mockMapper,
            serviceProvider: serviceProvider.BuildServiceProvider());
        //Act
        await systemUnderTest.ConstructBackupAsync(viewModel);

        //Assert
        mockBusinessLogic.Received().ConstructBackup(entity, "supersecretfilepath.mbz");
    }

    [Test]
    public void AddLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockCommand = Substitute.For<ICreateLearningWorld>();
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var workspaceEntity = new BusinessLogic.Entities.AuthoringToolWorkspace(
            new List<ILearningWorld> {worldEntity});
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
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var workspaceEntity = new BusinessLogic.Entities.AuthoringToolWorkspace(
            new List<ILearningWorld> {worldEntity});
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<AuthoringToolWorkspaceViewModel>())
            .Returns(workspaceEntity);
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        workspaceVm.LearningWorlds.Add(mockWorldVm);
        mockWorldCommandFactory.GetCreateCommand(workspaceEntity, "f", "f", "f", "f", "f", "f",
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            selectedViewModelsProvider: mockSelectedViewModelsProvider, worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.CreateLearningWorld(workspaceVm, "f", "f", "f", "f", "f", "f");

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningWorld(workspaceVm.LearningWorlds.Last(), mockCommand);
    }

    [Test]
    public void EditLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockCommand = Substitute.For<IEditLearningWorld>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var worldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(worldEntity);
        mockWorldCommandFactory
            .GetEditCommand(worldEntity, "f", "f", "f", "f", "f", "f",
            Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.EditLearningWorld(worldVm, "f", "f", "f", "f", "f", "f");

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void DeleteLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockCommand = Substitute.For<IDeleteLearningWorld>();
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        workspaceVm._learningWorlds.Add(worldVm);
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var workspaceEntity = new BusinessLogic.Entities.AuthoringToolWorkspace(
            new List<ILearningWorld> {worldEntity});
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
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var learningSpaceVm = new LearningSpaceViewModel("z", "z", "z", Theme.Campus);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("a", "d", "e", 5, Theme.Campus);
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
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var topicVm = new TopicViewModel("topic1", false);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var topicEntity = new BusinessLogic.Entities.Topic("topic1");
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<BusinessLogic.Entities.Topic>(Arg.Any<TopicViewModel>())
            .Returns(topicEntity);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockSpaceVm = new LearningSpaceViewModel("z", "z", "z", Theme.Campus);
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
        var learningSpaceVm = new LearningSpaceViewModel("z", "z", "z", Theme.Campus);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("a", "d", "e", 5, Theme.Campus);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockSpaceCommandFactory.GetEditCommand(learningSpaceEntity, "z", "z", "z", 5, Theme.Campus, null,
            Arg.Any<Action<BusinessLogic.Entities.ILearningSpace>>())
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
        var topicViewModel = new TopicViewModel("topic1", false);
        var learningSpaceVm = new LearningSpaceViewModel("z", "z", "z", Theme.Campus)
        {
            AssignedTopic = topicViewModel
        };
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("a", "d", "e" , 5, Theme.Campus);
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        const FloorPlanEnum floorPlan = FloorPlanEnum.Rectangle2X2;
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
        var learningSpaceVm = new LearningSpaceViewModel("z", "z", "z", Theme.Campus);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("a", "d", "e", 5, Theme.Campus);
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
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var learningSpaceVm = new LearningSpaceViewModel("z", "z", "z", Theme.Campus);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("a", "d", "e", 5, Theme.Campus);
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
        var learningSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 4);
        var learningElementVm = new LearningElementViewModel("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, learningSpaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 4, Theme.Campus);
        var learningElementEntity = new BusinessLogic.Entities.LearningElement("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, learningSpaceEntity);
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
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var mockElementVm = Substitute.For<ILearningElementViewModel>();
        learningWorldVm.UnplacedLearningElements.Add(mockElementVm);
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var learningContentEntity = new LinkContent("foobar", "baba");
        mockMapper
            .Map<BusinessLogic.Entities.LearningWorld>(learningWorldVm)
            .Returns(learningWorldEntity);
        mockMapper
            .Map<ILearningContent>(null)
            .Returns(learningContentEntity);
        mockElementCommandFactory
            .GetCreateUnplacedCommand(learningWorldEntity, "a", learningContentEntity, "d", "e",
                LearningElementDifficultyEnum.Easy, 5, 7, 0, 0,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.CreateUnplacedLearningElement(learningWorldVm, "a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, 5, 7);
        
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningElement(mockElementVm, mockCommand);
    }

    [Test]
    public void CreateLearningElementInSlot_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<ICreateLearningElementInSlot>();
        var learningSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 4);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 5, Theme.Campus);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockElementCommandFactory.GetCreateInSlotCommand(learningSpaceEntity, 0, "a", Arg.Any<ILearningContent>(), "d",
                "e", LearningElementDifficultyEnum.Easy, 1, 2, positionX: 3, positionY: 4,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.CreateLearningElementInSlot(learningSpaceVm, 0, "a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, 1, 2, positionX: 3, positionY: 4);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void EditLearningElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<IEditLearningElement>();
        var learningSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 5);
        var learningElementVm = new LearningElementViewModel("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, learningSpaceVm);
        var learningContentVm = new FileContentViewModel("a", "h5p", "/user/marvin/learningcontent.h5p");
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 5, Theme.Campus);
        var learningElementEntity = new BusinessLogic.Entities.LearningElement("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, learningSpaceEntity);
        var learningContentEntity = new FileContent("a", "h5p", "/user/marvin/learningcontent.h5p");
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(learningElementEntity);
        mockMapper.Map<ILearningContent>(Arg.Any<FileContentViewModel>())
            .Returns(learningContentEntity);
        mockElementCommandFactory.GetEditCommand(learningElementEntity, learningSpaceEntity, "a", "d", "e",
            LearningElementDifficultyEnum.Easy, 1, 2, learningContentEntity,
            Arg.Any<Action<BusinessLogic.Entities.LearningElement>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.EditLearningElement(learningSpaceVm, learningElementVm, "a", "d",
            "e", LearningElementDifficultyEnum.Easy, 1, 2, learningContentVm);

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
        mockSelectedViewModelsProvider
            .Received()
            .SetLearningElement(learningElementVm, mockCommand);
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
        mockSelectedViewModelsProvider
            .Received()
            .SetLearningElement(learningElementVm, mockCommand);
        
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
        var learningElementVm = new LearningElementViewModel("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy);
        var mockMapper = Substitute.For<IMapper>();
        var learningElementEntity = new BusinessLogic.Entities.LearningElement("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy);
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
        var learningSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 4);
        var learningElementVm = new LearningElementViewModel("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, learningSpaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 3, Theme.Campus);
        var learningElementEntity = new BusinessLogic.Entities.LearningElement("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy, learningSpaceEntity);
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
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "t");
        var learningElementVm = new LearningElementViewModel("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "g");
        var learningElementEntity = new BusinessLogic.Entities.LearningElement("a", null!, "d", "e",
            LearningElementDifficultyEnum.Easy);
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
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var sourceSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 4);
        var targetSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 4);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var sourceSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 3, Theme.Campus);
        var targetSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 3, Theme.Campus);

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
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var sourceSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 4);
        var targetSpaceVm = new LearningSpaceViewModel("z", "z", "z", Theme.Campus);
        var pathWayVm = new LearningPathwayViewModel(sourceSpaceVm, targetSpaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var pathWayEntity = new LearningPathway(
            new BusinessLogic.Entities.LearningSpace("f", "f", "f", 3, Theme.Campus),
            new BusinessLogic.Entities.LearningSpace("z", "z", "z", 5, Theme.Campus));
        learningWorldEntity.LearningPathways.Add(pathWayEntity);


        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<LearningPathway>(Arg.Any<LearningPathwayViewModel>())
            .Returns(pathWayEntity);
        mockPathwayCommandFactory.GetDeleteCommand(learningWorldEntity, pathWayEntity,
            Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            pathwayCommandFactory: mockPathwayCommandFactory);

        systemUnderTest.DeleteLearningPathWay(learningWorldVm, pathWayVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void CreatePathWayCondition_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockConditionCommandFactory = Substitute.For<IConditionCommandFactory>();
        var mockCommand = Substitute.For<ICreatePathWayCondition>();
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockCondition = new PathWayConditionViewModel(ConditionEnum.And, false, 1, 2);
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
        var mockCommand = Substitute.For<ICreatePathWayCondition>();
        var condition = ConditionEnum.And;
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var sourceSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 4);
        var targetSpaceVm = new LearningSpaceViewModel("f", "f", "f", Theme.Campus, 4);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var sourceSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 3, Theme.Campus);
        var targetSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 3, Theme.Campus);
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
        var pathWayConditionVm = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 1);
        var mockMapper = Substitute.For<IMapper>();
        var pathWayConditionEntity = new PathWayCondition(ConditionEnum.And, 2, 1);
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
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var pathWayConditionVm = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 1);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var pathWayConditionEntity = new PathWayCondition(ConditionEnum.And, 2, 1);
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
        var learningWorldVm = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("a", "b", "c", "d", "e", "f");
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
        var topicVm = new TopicViewModel("f", false);
        var mockMapper = Substitute.For<IMapper>();
        var topicEntity = new BusinessLogic.Entities.Topic("f");
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
        var learningWorldVm = new LearningWorldViewModel("a","b","c","d","e","f");
        var topicVm = new TopicViewModel("a", false);
        var spaceVm1 = new LearningSpaceViewModel("a", "b", "c", Theme.Campus, 2, assignedTopic: topicVm);
        var spaceVm2 = new LearningSpaceViewModel("a", "b", "c", Theme.Campus, 2, assignedTopic: topicVm);
        var spaceVm3 = new LearningSpaceViewModel("a", "b", "c", Theme.Campus, 2, assignedTopic: topicVm);
        learningWorldVm.LearningSpaces.Add(spaceVm1);
        learningWorldVm.LearningSpaces.Add(spaceVm2);
        learningWorldVm.LearningSpaces.Add(spaceVm3);
        learningWorldVm.Topics.Add(topicVm);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorldEntity = new BusinessLogic.Entities.LearningWorld("a", "b", "c", "d", "e", "f");
        var topicEntity = new BusinessLogic.Entities.Topic("a");
        var spaceEntity1 =
            new BusinessLogic.Entities.LearningSpace("a", "b", "c", 2, Theme.Campus, assignedTopic: topicEntity);
        var spaceEntity2 =
            new BusinessLogic.Entities.LearningSpace("a", "b", "c", 2, Theme.Campus, assignedTopic: topicEntity);
        var spaceEntity3 =
            new BusinessLogic.Entities.LearningSpace("a", "b", "c", 2, Theme.Campus, assignedTopic: topicEntity);
        learningWorldEntity.LearningSpaces.Add(spaceEntity1);
        learningWorldEntity.LearningSpaces.Add(spaceEntity2);
        learningWorldEntity.LearningSpaces.Add(spaceEntity3);
        learningWorldEntity.Topics.Add(topicEntity);

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

    #region Save/Load

    [Test]
    public void SaveLearningWorldAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveLearningWorldAsync(learningWorld));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void SaveLearningWorldAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveLearningWorldAsync(learningWorld));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task SaveLearningWorldAsync_CallsDialogManagerAndWorldMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockCommand = Substitute.For<ISaveLearningWorld>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var entity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorld).Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockWorldCommandFactory.GetSaveCommand(mockBusinessLogic, entity, filepath+".awf").Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport, worldCommandFactory: mockWorldCommandFactory);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);

        await mockDialogManger.Received()
            .ShowSaveAsDialogAsync("Save Learning World", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.LearningWorld>(learningWorld);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    public void SaveLearningWorldAsync_LogsAndRethrowsDialogCancelledException()
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
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await systemUnderTest.SaveLearningWorldAsync(learningWorld));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }

    [Test]
    public async Task SaveLearningWorldAsync_CallsSavedLearningWorldsManager()
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
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var entity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(learningWorld).Returns(entity);
        const string filepath = "foobar.awf";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper, serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);

        mockBusinessLogic.Received().AddSavedLearningWorldPath(Arg.Any<SavedLearningWorldPath>());
        Assert.Multiple(() =>
        {
            Assert.That(resultId, Is.EqualTo(entity.Id));
            Assert.That(resultName, Is.EqualTo(entity.Name));
            Assert.That(resultPath, Is.EqualTo(filepath));
        });
    }

    [Test]
    public void SaveLearningSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", Theme.Campus);

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
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", Theme.Campus);

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
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", Theme.Campus);
        var entity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 5, Theme.Campus);
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
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", Theme.Campus);

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
        var learningElement = new LearningElementViewModel("f", null!, "f", "f", LearningElementDifficultyEnum.Easy);

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
        var learningElement = new LearningElementViewModel("f", null!, "f", "f", LearningElementDifficultyEnum.Easy);

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
        mockHybridSupport
            .IsElectronActive
            .Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningElement = new LearningElementViewModel("f", null!, "f", "f", LearningElementDifficultyEnum.Easy);
        var entity =
            new BusinessLogic.Entities.LearningElement("f", null!, "f", "f", LearningElementDifficultyEnum.Easy);
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
        var learningElement = new LearningElementViewModel("f", null!, "f", "f", LearningElementDifficultyEnum.Easy);

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

        var workspaceEntity =
            new BusinessLogic.Entities.AuthoringToolWorkspace(new List<ILearningWorld>());
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

    #region LearningWorldSavePaths

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
        mockWorldCommandFactory
            .GetLoadCommand(workspaceEntity, "foobar", mockBusinessLogic,
            Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>())
            .Returns(mockCommand);
        mockMapper
            .Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm)
            .Returns(workspaceEntity);
        var guid = new Guid();
        mockCommand.LearningWorld.Id.Returns(guid);
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
        var savedLearningWorldPath = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "n1", Path = "p1"};
        mockBusinessLogic.GetSavedLearningWorldPaths()
            .Returns(new List<SavedLearningWorldPath> {savedLearningWorldPath});

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
        var savedLearningWorldPath = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "n1", Path = "p1"};

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
        var savedLearningWorldPath = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "n1", Path = "p1"};
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
        var savedLearningWorldPath = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "n1", Path = "p1"};
        var changedId = Guid.ParseExact("00000000-0000-0000-0000-000000000002", "D");

        systemUnderTest.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, changedId);

        mockBusinessLogic.Received().UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, changedId);
    }

    [Test]
    public void RemoveSavedLearningWorldPath_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        var savedLearningWorldPath = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "n1", Path = "p1"};

        systemUnderTest.RemoveSavedLearningWorldPath(savedLearningWorldPath);

        mockBusinessLogic.Received().RemoveSavedLearningWorldPath(savedLearningWorldPath);
    }

    #endregion

    [Test]
    public void LoadLearningSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockLearningWorldViewModel = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
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
        var mockLearningWorldViewModel = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
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
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<ILoadLearningSpace>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLearningWorldViewModel = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        var mockLearningWorldEntity = new BusinessLogic.Entities.LearningWorld("a", "b", "c", "d", "e", "f");
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
        var mockLearningWorldViewModel = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
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
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("n", "l", "d", Theme.Campus);
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
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("sn", "d", "g", Theme.Campus);
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
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("a", "g", "h", Theme.Campus, 1);
        var mockLearningSpaceEntity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", 1, Theme.Campus);
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
        var mockLearningElementViewModel =
            new LearningElementViewModel("n", null!, "d", "t", LearningElementDifficultyEnum.Easy, null, 3);
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
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("n", "l", "d", Theme.Campus);
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
    public void LoadImageAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadImageAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadImageAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadImageAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task LoadImageAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningContent = new FileContentViewModel("f", ".png", "");
        var entity = new FileContent("f", ".png", "");
        mockMapper.Map<ILearningContentViewModel>(Arg.Any<ILearningContent>()).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath).Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadImageAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load image", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath);
        mockMapper.Received().Map<ILearningContentViewModel>(entity);

        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }

    [Test]
    public void LoadImageAsync_LogsAndRethrowsDialogCancelledException()
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadImageAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }


    [Test]
    public void LoadVideoAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadVideoAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadVideoAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadVideoAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task LoadVideoAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningContent = new FileContentViewModel("f", ".mp4", "");
        var entity = new FileContent("f", ".mp4", "");
        mockMapper.Map<ILearningContentViewModel>(Arg.Any<ILearningContent>()).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath + ".mp4").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadVideoAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load video", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath + ".mp4");
        mockMapper.Received().Map<ILearningContentViewModel>(entity);

        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }

    [Test]
    public void LoadVideoAsync_LogsAndRethrowsDialogCancelledException()
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadVideoAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }

    [Test]
    public void LoadH5pAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadH5PAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadH5pAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadH5PAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task LoadH5pAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningContent = new FileContentViewModel("f", ".h5p", "");
        var entity = new FileContent("f", ".h5p", "");
        mockMapper.Map<ILearningContentViewModel>(Arg.Any<ILearningContent>()).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath + ".h5p").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadH5PAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load h5p", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath + ".h5p");
        mockMapper.Received().Map<ILearningContentViewModel>(entity);

        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }

    [Test]
    public void LoadH5pAsync_LogsAndRethrowsDialogCancelledException()
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadH5PAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }

    [Test]
    public void LoadPdfAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadPdfAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadPdfAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadPdfAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task LoadPdfAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningContent = new FileContentViewModel("f", ".pdf", "");
        var entity = new FileContent("f", ".pdf", "");
        mockMapper.Map<ILearningContentViewModel>(Arg.Any<ILearningContent>()).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath + ".pdf").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadPdfAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load pdf", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath + ".pdf");
        mockMapper.Received().Map<ILearningContentViewModel>(entity);

        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }

    [Test]
    public void LoadPdfAsync_LogsAndRethrowsDialogCancelledException()
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadPdfAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }

    [Test]
    public void LoadTextAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadTextAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadTextAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadTextAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task LoadTextAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningContent = new FileContentViewModel("f", ".txt", "");
        var entity = new FileContent("f", ".txt", "");
        mockMapper.Map<ILearningContentViewModel>(Arg.Any<ILearningContent>()).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath).Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadTextAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load text", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath);
        mockMapper.Received().Map<ILearningContentViewModel>(entity);

        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }

    [Test]
    public void LoadTextAsync_LogsAndRethrowsDialogCancelledException()
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, logger: mockLogger, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadTextAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }

    #endregion

    #region Load

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
    public void PresentationLogic_LoadLearningContentViewModel_ReturnsLearningContent()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningContent = new FileContent("n", "t", "");
        mockBusinessLogic.LoadLearningContent(Arg.Any<string>(), Arg.Any<MemoryStream>()).Returns(mockLearningContent);
        var mockLearningContentViewModel = new FileContentViewModel("n", "t", "");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper
            .Map<ILearningContentViewModel>(Arg.Any<ILearningContent>())
            .Returns(mockLearningContentViewModel);
        const string filename = "test.png";
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        var result = systemUnderTest.LoadLearningContentViewModel(filename, stream);

        mockBusinessLogic.Received().LoadLearningContent(filename, stream);
        mockMapper.Received().Map<ILearningContentViewModel>(mockLearningContent);
        Assert.That(result, Is.EqualTo(mockLearningContentViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningContentViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningContent(Arg.Any<string>(), Arg.Any<MemoryStream>())
            .Throws(new Exception("Exception"));
        const string filename = "test.png";
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningContentViewModel(filename, stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }

    #endregion

    [Test]
    public void ShowLearningElementContentAsync_CallsShellWrapper()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockShellWrapper = Substitute.For<IShellWrapper>();
        mockShellWrapper.OpenPathAsync(Arg.Any<string>()).Returns("");
        var mockContent = new FileContentViewModel("a", "r", "pathpath");
        var mockLearningElement =
            new LearningElementViewModel("n", mockContent, "d", "g", LearningElementDifficultyEnum.Easy);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport, shellWrapper: mockShellWrapper);

        systemUnderTest.ShowLearningElementContentAsync(mockLearningElement);

        mockShellWrapper.Received().OpenPathAsync("pathpath");
    }

    #region BackendAccess

    [Test]
    public void IsLmsConnected_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var result = systemUnderTest.IsLmsConnected();

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

    #endregion

    private static Presentation.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(
        IAuthoringToolConfiguration? configuration = null, IBusinessLogic? businessLogic = null, IMapper? mapper = null,
        ICachingMapper? cachingMapper = null, ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        IServiceProvider? serviceProvider = null,
        ILogger<Presentation.PresentationLogic.API.PresentationLogic>? logger = null,
        IHybridSupportWrapper? hybridSupportWrapper = null, IShellWrapper? shellWrapper = null,
        IConditionCommandFactory? conditionCommandFactory = null,
        IElementCommandFactory? elementCommandFactory = null,
        ILayoutCommandFactory? layoutCommandFactory = null,
        IPathwayCommandFactory? pathwayCommandFactory = null,
        ISpaceCommandFactory? spaceCommandFactory = null,
        ITopicCommandFactory? topicCommandFactory = null,
        IWorldCommandFactory? worldCommandFactory = null,
        IBatchCommandFactory? batchCommandFactory = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        mapper ??= Substitute.For<IMapper>();
        cachingMapper ??= Substitute.For<ICachingMapper>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        hybridSupportWrapper ??= Substitute.For<IHybridSupportWrapper>();
        shellWrapper ??= Substitute.For<IShellWrapper>();
        conditionCommandFactory ??= Substitute.For<IConditionCommandFactory>();
        elementCommandFactory ??= Substitute.For<IElementCommandFactory>();
        layoutCommandFactory ??= Substitute.For<ILayoutCommandFactory>();
        pathwayCommandFactory ??= Substitute.For<IPathwayCommandFactory>();
        spaceCommandFactory ??= Substitute.For<ISpaceCommandFactory>();
        topicCommandFactory ??= Substitute.For<ITopicCommandFactory>();
        worldCommandFactory ??= Substitute.For<IWorldCommandFactory>();
        batchCommandFactory ??= Substitute.For<IBatchCommandFactory>();

        return new Presentation.PresentationLogic.API.PresentationLogic(configuration, businessLogic, mapper,
            cachingMapper, selectedViewModelsProvider, serviceProvider, logger, hybridSupportWrapper, shellWrapper,
            conditionCommandFactory, elementCommandFactory, layoutCommandFactory, pathwayCommandFactory,
            spaceCommandFactory, topicCommandFactory, worldCommandFactory, batchCommandFactory);
    }
}