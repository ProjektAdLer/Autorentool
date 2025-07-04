﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
using BusinessLogic.Commands.LearningOutcomes;
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
using BusinessLogic.Entities.LearningOutcome;
using ElectronWrapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components.Adaptivity.Forms.Models;
using Presentation.Components.Forms.Models;
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
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Adaptivity;
using Shared.Command;
using Shared.Configuration;
using Shared.LearningOutcomes;
using Shared.Theme;
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
        var mockLearningOutcomeCommandFactory = Substitute.For<ILearningOutcomeCommandFactory>();
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var mockBatchCommandFactory = Substitute.For<IBatchCommandFactory>();

        //Act
        var systemUnderTest = CreateTestablePresentationLogic(mockConfiguration, mockBusinessLogic, mockMapper,
            mockCachingMapper, mockSelectedViewModelsProvider, mockServiceProvider, mockLogger,
            mockHybridSupportWrapper, mockShellWrapper, mockQuestionCommandFactory, mockTaskCommandFactory,
            mockConditionCommandFactory, mockElementCommandFactory, mockLayoutCommandFactory, mockPathwayCommandFactory,
            mockSpaceCommandFactory, mockTopicCommandFactory, mockLearningOutcomeCommandFactory,
            mockWorldCommandFactory, mockBatchCommandFactory);
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
            Assert.That(systemUnderTest.LearningOutcomeCommandFactory, Is.EqualTo(mockLearningOutcomeCommandFactory));
            Assert.That(systemUnderTest.WorldCommandFactory, Is.EqualTo(mockWorldCommandFactory));
        });
    }

    [Test]
    // ANF-ID: [AHO22]
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
    // ANF-ID: [ASE6]
    public void SaveLearningWorld_CallsBusinessLogic()
    {
        var mapper = Substitute.For<IMapper>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var worldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var command = Substitute.For<ISaveLearningWorld>();
        var worldVm = ViewModelProvider.GetLearningWorld();
        var worldEntity = EntityProvider.GetLearningWorld();

        mapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(worldEntity);

        worldCommandFactory
            .GetSaveCommand(businessLogic, worldEntity, Arg.Any<string>(), Arg.Any<Action<ILearningWorld>>())
            .Returns(command);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: businessLogic, mapper: mapper,
            worldCommandFactory: worldCommandFactory);

        systemUnderTest.SaveLearningWorld(worldVm);

        worldCommandFactory.Received().GetSaveCommand(businessLogic, worldEntity, Arg.Any<string>(),
            Arg.Any<Action<ILearningWorld>>());
        businessLogic.Received().ExecuteCommand(command);
    }

    [Test]
    // ANF-ID: [ASN0005]
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
    // ANF-ID: [ASN0005]
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
    // ANF-ID: [ASN0003]
    public void CallUndoCommand_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.UndoCommand();

        mockBusinessLogic.Received().UndoCommand();
    }

    [Test]
    // ANF-ID: [ASN0004]
    public void CallRedoCommand_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.RedoCommand();

        mockBusinessLogic.Received().RedoCommand();
    }

    [Test]
    // ANF-ID: [ASE1]
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
        mockWorldCommandFactory.GetCreateCommand(workspaceEntity, "f", "f", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg, "f", "f", "f", "f",
                Arg.Any<Action<BusinessLogic.Entities.AuthoringToolWorkspace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            selectedViewModelsProvider: mockSelectedViewModelsProvider, worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.CreateLearningWorld(workspaceVm, "f", "f", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg, "f", "f", "f", "f");

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetLearningWorld(workspaceVm.LearningWorlds.Last(), mockCommand);
    }

    [Test]
    // ANF-ID: [ASE3]
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
            .GetEditCommand(worldEntity, "f", "f", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg, "f", "f", "f", "f",
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.EditLearningWorld(worldVm, "f", "f", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg, "f", "f", "f", "f");

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [ASE3]
    public void EditLearningWorld_NoChangesInCommand_DoesNotCallBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockCommand = Substitute.For<IEditLearningWorld>();
        mockCommand.AnyChanges().Returns(false);
        var mockWorldCommandFactory = Substitute.For<IWorldCommandFactory>();
        var worldVm = ViewModelProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = EntityProvider.GetLearningWorld();
        Substitute.For<ILogger<WorldCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(worldEntity);
        mockWorldCommandFactory
            .GetEditCommand(worldEntity, "f", "f", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg, "f", "f", "f", "f",
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            worldCommandFactory: mockWorldCommandFactory);

        systemUnderTest.EditLearningWorld(worldVm, "f", "f", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg, "f", "f", "f", "f");

        mockBusinessLogic.DidNotReceive().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [ASE4]
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
    // ANF-ID: [AWA0001]
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
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(learningWorldEntity);
        mockMapper.Map<BusinessLogic.Entities.Topic>(Arg.Any<TopicViewModel>())
            .Returns(topicEntity);
        mockMapper.Map<LearningOutcomeCollection>(Arg.Any<LearningOutcomeCollectionViewModel>())
            .Returns(learningOutcomeCollection);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockSpaceVm = ViewModelProvider.GetLearningSpace();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        learningWorldVm.LearningSpaces.Add(mockSpaceVm);
        mockSpaceCommandFactory.GetCreateCommand(learningWorldEntity, "z", "z", learningOutcomeCollection, 5,
                SpaceTheme.LearningArea,
                6, 7, topicEntity, Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);
        mockCommand.NewSpace.Id.Returns(mockSpaceVm.Id);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            selectedViewModelsProvider: selectedViewModelsProvider, spaceCommandFactory: mockSpaceCommandFactory);

        systemUnderTest.CreateLearningSpace(learningWorldVm, "z", "z", ViewModelProvider.GetLearningOutcomeCollection(),
            5, SpaceTheme.LearningArea, 6, 7,
            topicVm);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0023]
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
        mockSpaceCommandFactory.GetEditCommand(learningSpaceEntity, "z", "z", 5, SpaceTheme.LearningArea, null,
                Arg.Any<Action<ILearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            spaceCommandFactory: mockSpaceCommandFactory);

        systemUnderTest.EditLearningSpace(learningSpaceVm, "z", "z", 5, SpaceTheme.LearningArea, null);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0023]
    public void EditLearningSpace_NoChangesInCommand_DoesNotCallBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockSpaceCommandFactory = Substitute.For<ISpaceCommandFactory>();
        var mockCommand = Substitute.For<IEditLearningSpace>();
        mockCommand.AnyChanges().Returns(false);
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        Substitute.For<ILogger<SpaceCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockSpaceCommandFactory.GetEditCommand(learningSpaceEntity, "z", "z", 5, SpaceTheme.LearningArea, null,
                Arg.Any<Action<ILearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            spaceCommandFactory: mockSpaceCommandFactory);

        systemUnderTest.EditLearningSpace(learningSpaceVm, "z", "z", 5, SpaceTheme.LearningArea, null);

        mockBusinessLogic.DidNotReceive().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0023]
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
    // ANF-ID: [AWA0024]
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
    // ANF-ID: [AWA0002]
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
    // ANF-ID: [AWA0002]
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
    // ANF-ID: [ASN0011]
    public void CreateStoryElementInSlot_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<ICreateStoryElementInSlot>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningContentViewModel = ViewModelProvider.GetStoryContent();
        var learningElementVm =
            ViewModelProvider.GetLearningElement(parent: learningSpaceVm, content: learningContentViewModel);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningContent = EntityProvider.GetStoryContent();
        var learningElementEntity =
            EntityProvider.GetLearningElement(parent: learningSpaceEntity, content: learningContent);
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(learningElementEntity);
        mockMapper.Map<ILearningContent>(learningContentViewModel).Returns(learningContent);
        mockElementCommandFactory
            .GetCreateStoryInSlotCommand(learningSpaceEntity, 0, "name", learningContent, "description", "goals",
                LearningElementDifficultyEnum.Easy, ElementModel.a_npc_defaultdark_female, 123, 10, 5, 7,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);
        mockBusinessLogic
            .When(bl => bl.ExecuteCommand(mockCommand))
            .Do(_ => learningSpaceVm.LearningSpaceLayout.StoryElements.Add(0, learningElementVm));

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.CreateStoryElementInSlot(learningSpaceVm, 0, "name", learningContentViewModel, "description",
            "goals", LearningElementDifficultyEnum.Easy, ElementModel.a_npc_defaultdark_female, 123, 10, 5, 7);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0015]
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
    // ANF-ID: [AWA0015]
    public void EditLearningElement_NoChangesInCommand_DoesNotCallBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<IEditLearningElement>();
        mockCommand.AnyChanges().Returns(false);
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

        mockBusinessLogic.DidNotReceive().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [ASN0017, ASN0018]
    public void PlaceLearningElementFromUnplaced_CallsBusinessLogic()
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
            .GetPlaceLearningElementFromUnplacedCommand(learningWorldEntity, learningSpaceEntity,
                learningElementEntity, 1, Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);
        mockSelectedViewModelsProvider.ActiveElementSlotInSpace.Returns(1);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory, selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.DragLearningElementFromUnplaced(learningWorldVm, learningSpaceVm, learningElementVm, 1);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);

        mockSelectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, mockCommand);
    }

    [Test]
    // ANF-ID: [ASN0019]
    public void PlaceStoryElementFromUnplaced_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockCommand = Substitute.For<IPlaceStoryElementInLayoutFromUnplaced>();
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement(content: ViewModelProvider.GetStoryContent());
        var learningWorldEntity = EntityProvider.GetLearningWorld();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement(content: EntityProvider.GetStoryContent());
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
            .GetPlaceStoryElementFromUnplacedCommand(learningWorldEntity, learningSpaceEntity, learningElementEntity, 1,
                Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory, selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.DragStoryElementFromUnplaced(learningWorldVm, learningSpaceVm, learningElementVm, 1);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);

        mockBusinessLogic.ClearReceivedCalls();
        mockSelectedViewModelsProvider.ActiveStorySlotInSpace.Returns(1);

        systemUnderTest.DragStoryElementFromUnplaced(learningWorldVm, learningSpaceVm, learningElementVm, 1);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);

        mockSelectedViewModelsProvider.Received().SetActiveStorySlotInSpace(-1, mockCommand);
    }

    [Test]
    // ANF-ID: [ASN0022, ASN0018, ASN0021]
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
            .GetRemoveLearningElementCommand(learningWorldEntity, learningSpaceEntity,
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
    // ANF-ID: [ASN0020]
    public void DragStoryElementToUnplaced_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockCommand = Substitute.For<IRemoveStoryElementFromLayout>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement(content: ViewModelProvider.GetStoryContent());
        var learningWorldVm = ViewModelProvider.GetLearningWorld();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement(content: EntityProvider.GetStoryContent());
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
            .GetRemoveStoryElementCommand(learningWorldEntity, learningSpaceEntity,
                learningElementEntity, Arg.Any<Action<BusinessLogic.Entities.LearningWorld>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory, selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.DragStoryElementToUnplaced(learningWorldVm, learningSpaceVm, learningElementVm);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0041, AWA0040]
    public void SwitchLearningElementSlot_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockCommand = Substitute.For<IPlaceLearningElementInLayoutFromLayout>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
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
            .GetPlaceLearningElementFromLayoutCommand(learningSpaceEntity, learningElementEntity, 4,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        mockSelectedViewModelsProvider.ActiveElementSlotInSpace.Returns(4);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory, selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.SwitchLearningElementSlot(learningSpaceVm, learningElementVm, 4);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);

        mockSelectedViewModelsProvider.Received().SetActiveElementSlotInSpace(-1, mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0039]
    public void SwitchStoryElementSlot_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLayoutCommandFactory = Substitute.For<ILayoutCommandFactory>();
        var mockCommand = Substitute.For<IPlaceStoryElementInLayoutFromLayout>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
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
            .GetPlaceStoryElementFromLayoutCommand(learningSpaceEntity, learningElementEntity, 4,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            layoutCommandFactory: mockLayoutCommandFactory, selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.SwitchStoryElementSlot(learningSpaceVm, learningElementVm, 4);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);

        mockBusinessLogic.ClearReceivedCalls();
        mockSelectedViewModelsProvider.ActiveStorySlotInSpace.Returns(4);

        systemUnderTest.SwitchStoryElementSlot(learningSpaceVm, learningElementVm, 4);

        mockBusinessLogic
            .Received()
            .ExecuteCommand(mockCommand);
        mockSelectedViewModelsProvider.Received().SetActiveStorySlotInSpace(-1, mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0016]
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
    // ANF-ID: [ASN0015]
    public void DeleteStoryElementInSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElementCommandFactory = Substitute.For<IElementCommandFactory>();
        var mockCommand = Substitute.For<IDeleteStoryElementInSpace>();
        var learningSpaceVm = ViewModelProvider.GetLearningSpace();
        var learningElementVm = ViewModelProvider.GetLearningElement(content: ViewModelProvider.GetStoryContent());
        var mockMapper = Substitute.For<IMapper>();
        var learningSpaceEntity = EntityProvider.GetLearningSpace();
        var learningElementEntity = EntityProvider.GetLearningElement(content: EntityProvider.GetStoryContent());
        Substitute.For<ILogger<ElementCommandFactory>>();
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>())
            .Returns(learningSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>())
            .Returns(learningElementEntity);
        mockElementCommandFactory.GetDeleteStoryInSpaceCommand(learningElementEntity, learningSpaceEntity,
                Arg.Any<Action<BusinessLogic.Entities.LearningSpace>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            elementCommandFactory: mockElementCommandFactory);

        systemUnderTest.DeleteStoryElementInSpace(learningSpaceVm, learningElementVm);

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0016]
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
    // ANF-ID: [AHO11]
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
    // ANF-ID: [AHO13]
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
    // ANF-ID: [AHO61]
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
    // ANF-ID: [AHO61]
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
    // ANF-ID: [AHO62]
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
    // ANF-ID: [AHO63]
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
            .GetEditCommand(spaceEntity1, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<SpaceTheme>(), null, Arg.Any<Action<ILearningSpace>>())
            .Returns(mockEditSpaceCommand1);
        mockSpaceCommandFactory
            .GetEditCommand(spaceEntity2, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<SpaceTheme>(), null, Arg.Any<Action<ILearningSpace>>())
            .Returns(mockEditSpaceCommand2);
        mockSpaceCommandFactory
            .GetEditCommand(spaceEntity3, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<SpaceTheme>(), null, Arg.Any<Action<ILearningSpace>>())
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
            .GetEditCommand(spaceEntity1, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<SpaceTheme>(), null, Arg.Any<Action<ILearningSpace>>());
        mockSpaceCommandFactory
            .Received()
            .GetEditCommand(spaceEntity2, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<SpaceTheme>(), null, Arg.Any<Action<ILearningSpace>>());
        mockSpaceCommandFactory
            .Received()
            .GetEditCommand(spaceEntity3, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int>(), Arg.Any<SpaceTheme>(), null, Arg.Any<Action<ILearningSpace>>());
        mockBatchCommandFactory
            .Received()
            .GetBatchCommand(Arg.Is<IEnumerable<IUndoCommand>>(i =>
                i.SequenceEqual(new IUndoCommand[]
                    { mockEditSpaceCommand1, mockEditSpaceCommand2, mockEditSpaceCommand3, mockDeleteTopicCommand })
            ));
        mockBusinessLogic.Received().ExecuteCommand(mockBatchCommand);
    }

    [Test]
    // ANF-ID: [ASE2]
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
    // ANF-ID: [ASE2]
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
    // ANF-ID: [ASE2]
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
    // ANF-ID: [ASE2]
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
    // ANF-ID: [ASE2]
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
    // ANF-ID: [ASE2]
    public void GetSavedLearningWorldPaths_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.GetSavedLearningWorldPaths();

        mockBusinessLogic.Received().GetSavedLearningWorldPaths();
    }

    [Test]
    // ANF-ID: [ASE2]
    public void GetSavedLearningWorldPaths_ReturnsResultFromBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var fileInfos = new[]
        {
            Substitute.For<IFileInfo>(),
            Substitute.For<IFileInfo>(),
            Substitute.For<IFileInfo>(),
        };
        mockBusinessLogic.GetSavedLearningWorldPaths()
            .Returns(fileInfos);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var result = systemUnderTest.GetSavedLearningWorldPaths().ToList();

        Assert.That(result, Has.Count.EqualTo(fileInfos.Length));
        //Assert.That(result, Is.EquivalentTo(fileInfos)); 
        // for some reason this assert and this assert only is broken in NUnit 4.0.1 throwing a stack overflow
        // i gave up on debugging after i was 4 levels deep into the same NUnit function over and over again
        result.Should().BeEquivalentTo(fileInfos);
    }

    [Test]
    // ANF-ID: [AWA0036, AWA0047]
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
    // ANF-ID: [AWA0036, AWA0047]
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
    // ANF-ID: [AWA0005]
    public void CreateAdaptivityTask_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockTaskCommandFactory = Substitute.For<ITaskCommandFactory>();
        var mockCommand = Substitute.For<ICreateAdaptivityTask>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityContentFormModel = FormModelProvider.GetAdaptivityContent();
        var mockAdaptivityContentEntity = EntityProvider.GetAdaptivityContent();
        const string name = "name";
        Substitute.For<ILogger<TaskCommandFactory>>();
        mockMapper
            .Map<AdaptivityContent>(mockAdaptivityContentFormModel)
            .Returns(mockAdaptivityContentEntity);
        mockTaskCommandFactory
            .GetCreateCommand(mockAdaptivityContentEntity, name,
                Arg.Any<Action<AdaptivityContent>>())
            .Returns(mockCommand);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
                taskCommandFactory: mockTaskCommandFactory);

        systemUnderTest.CreateAdaptivityTask(mockAdaptivityContentFormModel, name);

        mockMapper.Received().Map<AdaptivityContent>(mockAdaptivityContentFormModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0006]
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
    // ANF-ID: [AWA0007]
    public void DeleteAdaptivityTask_WithFormModel_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockTaskCommandFactory = Substitute.For<ITaskCommandFactory>();
        var mockCommand = Substitute.For<IDeleteAdaptivityTask>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityContentFormModel = FormModelProvider.GetAdaptivityContent();
        var mockAdaptivityContentEntity = EntityProvider.GetAdaptivityContent();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        Substitute.For<ILogger<TaskCommandFactory>>();
        mockMapper
            .Map<AdaptivityContent>(mockAdaptivityContentFormModel)
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

        systemUnderTest.DeleteAdaptivityTask(mockAdaptivityContentFormModel, mockAdaptivityTaskViewModel);

        mockMapper.Received().Map<AdaptivityContent>(mockAdaptivityContentFormModel);
        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0007]
    public void DeleteAdaptivityTask_WithViewModel_CallsBusinessLogic()
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

        systemUnderTest.DeleteAdaptivityTask((AdaptivityContentViewModel)mockAdaptivityContentViewModel,
            mockAdaptivityTaskViewModel);

        mockMapper.Received().Map<AdaptivityContent>(mockAdaptivityContentViewModel);
        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0004]
    public void CreateMultipleChoiceSingleResponseQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<ICreateMultipleChoiceSingleResponseQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var mockChoicesFormModel = new List<ChoiceFormModel>
            { FormModelProvider.GetChoice(), FormModelProvider.GetChoice(), FormModelProvider.GetChoice() };
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoiceFormModel = mockChoicesFormModel.First();
        var mockCorrectChoiceEntity = mockQuestionEntity.CorrectChoice;
        const QuestionDifficulty difficulty = QuestionDifficulty.Easy;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<AdaptivityTask>(mockAdaptivityTaskViewModel)
            .Returns(mockAdaptivityTaskEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesFormModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<Choice>(mockCorrectChoiceFormModel)
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
            questionText, mockChoicesFormModel, mockCorrectChoiceFormModel, expectedCompletionTime);

        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesFormModel);
        mockMapper.Received().Map<Choice>(mockCorrectChoiceFormModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0004]
    public void CreateMultipleChoiceMultipleResponseQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<ICreateMultipleChoiceMultipleResponseQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityTaskViewModel = ViewModelProvider.GetAdaptivityTask();
        var mockAdaptivityTaskEntity = EntityProvider.GetAdaptivityTask();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockChoicesFormModel = new List<ChoiceFormModel>
            { FormModelProvider.GetChoice(), FormModelProvider.GetChoice(), FormModelProvider.GetChoice() };
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoicesFormModel = new List<ChoiceFormModel>
            { mockChoicesFormModel.First(), mockChoicesFormModel.Last() };
        var mockCorrectChoicesEntity = mockQuestionEntity.CorrectChoices;
        const QuestionDifficulty difficulty = QuestionDifficulty.Easy;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<AdaptivityTask>(mockAdaptivityTaskViewModel)
            .Returns(mockAdaptivityTaskEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesFormModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockCorrectChoicesFormModel)
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
            questionText, mockChoicesFormModel, mockCorrectChoicesFormModel, expectedCompletionTime);

        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesFormModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockCorrectChoicesFormModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0008]
    public void EditMultipleChoiceSingleResponseQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<IEditMultipleChoiceSingleResponseQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockQuestionViewModel = ViewModelProvider.GetMultipleChoiceSingleResponseQuestion();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var mockChoicesFormModel = new List<ChoiceFormModel>
            { FormModelProvider.GetChoice(), FormModelProvider.GetChoice(), FormModelProvider.GetChoice() };
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoiceFormModel = mockChoicesFormModel.First();
        var mockCorrectChoiceEntity = mockQuestionEntity.CorrectChoice;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<MultipleChoiceSingleResponseQuestion>(mockQuestionViewModel)
            .Returns(mockQuestionEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesFormModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<Choice>(mockCorrectChoiceFormModel)
            .Returns(mockCorrectChoiceEntity);
        mockQuestionCommandFactory
            .GetEditMultipleChoiceSingleResponseQuestionCommand(mockQuestionEntity,
                questionText, mockChoicesEntity, mockCorrectChoiceEntity, expectedCompletionTime,
                Arg.Any<Action<MultipleChoiceSingleResponseQuestion>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            questionCommandFactory: mockQuestionCommandFactory);

        systemUnderTest.EditMultipleChoiceSingleResponseQuestion(mockQuestionViewModel,
            questionText, mockChoicesFormModel, mockCorrectChoiceFormModel, expectedCompletionTime);

        mockMapper.Received().Map<MultipleChoiceSingleResponseQuestion>(mockQuestionViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesFormModel);
        mockMapper.Received().Map<Choice>(mockCorrectChoiceFormModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0008]
    public void EditMultipleChoiceMultipleResponseQuestion_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockQuestionCommandFactory = Substitute.For<IQuestionCommandFactory>();
        var mockCommand = Substitute.For<IEditMultipleChoiceMultipleResponseQuestion>();
        var mockMapper = Substitute.For<IMapper>();
        var mockQuestionViewModel = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockQuestionEntity = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var mockChoicesFormModel = new List<ChoiceFormModel>
            { FormModelProvider.GetChoice(), FormModelProvider.GetChoice(), FormModelProvider.GetChoice() };
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoicesFormModel = new List<ChoiceFormModel>
            { mockChoicesFormModel.First(), mockChoicesFormModel.Last() };
        var mockCorrectChoicesEntity = mockQuestionEntity.CorrectChoices;
        const string questionText = "questionText";
        const int expectedCompletionTime = 10;
        Substitute.For<ILogger<QuestionCommandFactory>>();
        mockMapper
            .Map<MultipleChoiceMultipleResponseQuestion>(mockQuestionViewModel)
            .Returns(mockQuestionEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockChoicesFormModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockCorrectChoicesFormModel)
            .Returns(mockCorrectChoicesEntity);
        mockQuestionCommandFactory
            .GetEditMultipleChoiceMultipleResponseQuestionCommand(mockQuestionEntity,
                questionText, mockChoicesEntity, mockCorrectChoicesEntity, expectedCompletionTime,
                Arg.Any<Action<MultipleChoiceMultipleResponseQuestion>>())
            .Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            questionCommandFactory: mockQuestionCommandFactory);

        systemUnderTest.EditMultipleChoiceMultipleResponseQuestion(mockQuestionViewModel,
            questionText, mockChoicesFormModel, mockCorrectChoicesFormModel, expectedCompletionTime);

        mockMapper.Received().Map<MultipleChoiceMultipleResponseQuestion>(mockQuestionViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesFormModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockCorrectChoicesFormModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0008]
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
        var mockChoicesFormModel = new List<ChoiceFormModel>
            { FormModelProvider.GetChoice(), FormModelProvider.GetChoice(), FormModelProvider.GetChoice() };
        var mockChoicesEntity = mockQuestionEntity.Choices;
        var mockCorrectChoicesFormModel = new List<ChoiceFormModel>
            { mockChoicesFormModel.First(), mockChoicesFormModel.Last() };
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
            .Map<ICollection<Choice>>(mockChoicesFormModel)
            .Returns(mockChoicesEntity);
        mockMapper
            .Map<ICollection<Choice>>(mockCorrectChoicesFormModel)
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
            questionText, mockChoicesFormModel, mockCorrectChoicesFormModel, expectedCompletionTime);

        mockMapper.Received().Map<AdaptivityTask>(mockAdaptivityTaskViewModel);
        mockMapper.Received().Map<IMultipleChoiceQuestion>(mockQuestionViewModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockChoicesFormModel);
        mockMapper.Received().Map<ICollection<Choice>>(mockCorrectChoicesFormModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0009]
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
    // ANF-ID: [AWA0026, AWA0031]
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
    // ANF-ID: [AWA0028, AWA0033]
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
    // ANF-ID: [AWA0032]
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
    // ANF-ID: [AWA0027]
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
    // ANF-ID: [AWA0027]
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
    // ANF-ID: [AWA0038, AWA0044]
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
    // ANF-ID: [AWA0038]
    public void ShowLearningElementContentAsync_ContentTypeNotSupported_Throws()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockContent = ViewModelProvider.GetAdaptivityContent();
        var mockLearningElement = ViewModelProvider.GetLearningElement(content: mockContent);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            systemUnderTest.ShowLearningElementContentAsync(mockLearningElement));
        Assert.That(ex.Message,
            Is.EqualTo(
                "LearningElementViewModel.LearningContent is not of type FileContentViewModel or LinkContentViewModel (Parameter 'learningElementVm')"));
    }

    [Test]
    // ANF-ID: [AWA0038]
    public void ShowLearningContentAsync_CouldNotOpenFileInOS_ThrowsIOException()
    {
        var mockContent = ViewModelProvider.GetFileContent();
        var mockShellWrapper = Substitute.For<IShellWrapper>();
        mockShellWrapper.OpenPathAsync(Arg.Any<string>()).Returns("error");
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);

        var systemUnderTest =
            CreateTestablePresentationLogic(serviceProvider: mockServiceProvider, shellWrapper: mockShellWrapper);

        systemUnderTest.RunningElectron.Returns(true);

        var ex = Assert.ThrowsAsync<IOException>(() => systemUnderTest.ShowLearningContentAsync(mockContent));
        Assert.That(ex.Message, Is.EqualTo("Could not open file in OS viewererror"));
    }

    [Test]
    // ANF-ID: [AWA0038]
    public async Task ShowLearningContentAsync_FormModel_CallsShellWrapper()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockShellWrapper = Substitute.For<IShellWrapper>();
        mockShellWrapper.OpenPathAsync(Arg.Any<string>()).Returns("");
        var mockContent = new FileContentFormModel();
        mockContent.Filepath = "pathpath";
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport, shellWrapper: mockShellWrapper);

        await systemUnderTest.ShowLearningContentAsync(mockContent);

        await mockShellWrapper.Received().OpenPathAsync("pathpath");
    }

    [Test]
    // ANF-ID: [AWA0038]
    public void ShowLearningContentAsync_FormModel_CouldNotOpenFileInOS_ThrowsIOException()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockShellWrapper = Substitute.For<IShellWrapper>();
        mockShellWrapper.OpenPathAsync(Arg.Any<string>()).Returns("error");
        var mockContent = new FileContentFormModel();
        mockContent.Filepath = "pathpath";
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(serviceProvider: mockServiceProvider,
            hybridSupportWrapper: mockHybridSupport, shellWrapper: mockShellWrapper);

        var ex = Assert.ThrowsAsync<IOException>(() => systemUnderTest.ShowLearningContentAsync(mockContent));
        Assert.That(ex.Message, Is.EqualTo("Could not open file in OS viewererror"));
    }

    [Test]
    // ANF-ID: [AHO01]
    public void AddStructuredLearningOutcome_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningOutcomeCommandFactory = Substitute.For<ILearningOutcomeCommandFactory>();
        var mockCommand = Substitute.For<IAddLearningOutcome>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLearningOutcomeCollectionViewModel = ViewModelProvider.GetLearningOutcomeCollection();
        var mockLearningOutcomeCollectionEntity = EntityProvider.GetLearningOutcomeCollection();
        var taxonomyLevel = TaxonomyLevel.Level1;
        var what = "what";
        var verbOfVisibility = "verbOfVisibility";
        var whereby = "whereby";
        var whatFor = "whatFor";
        var cultureInfo = new CultureInfo("de-DE");

        mockMapper
            .Map<LearningOutcomeCollection>(mockLearningOutcomeCollectionViewModel)
            .Returns(mockLearningOutcomeCollectionEntity);

        mockLearningOutcomeCommandFactory.GetAddLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            taxonomyLevel, what, verbOfVisibility, whereby, whatFor, cultureInfo,
            Arg.Any<Action<LearningOutcomeCollection>>()).Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            learningOutcomeCommandFactory: mockLearningOutcomeCommandFactory);

        systemUnderTest.AddStructuredLearningOutcome(mockLearningOutcomeCollectionViewModel, taxonomyLevel, what,
            verbOfVisibility, whereby, whatFor, cultureInfo);

        mockMapper.Received().Map<LearningOutcomeCollection>(mockLearningOutcomeCollectionViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AHO02]
    public void AddManualLearningOutcome_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningOutcomeCommandFactory = Substitute.For<ILearningOutcomeCommandFactory>();
        var mockCommand = Substitute.For<IAddLearningOutcome>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLearningOutcomeCollectionViewModel = ViewModelProvider.GetLearningOutcomeCollection();
        var mockLearningOutcomeCollectionEntity = EntityProvider.GetLearningOutcomeCollection();
        var manualLearningOutcomeText = "manualLearningOutcomeText";

        mockMapper
            .Map<LearningOutcomeCollection>(mockLearningOutcomeCollectionViewModel)
            .Returns(mockLearningOutcomeCollectionEntity);

        mockLearningOutcomeCommandFactory.GetAddLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            manualLearningOutcomeText,
            Arg.Any<Action<LearningOutcomeCollection>>()).Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, learningOutcomeCommandFactory: mockLearningOutcomeCommandFactory);

        systemUnderTest.AddManualLearningOutcome(mockLearningOutcomeCollectionViewModel, manualLearningOutcomeText);

        mockMapper.Received().Map<LearningOutcomeCollection>(mockLearningOutcomeCollectionViewModel);
        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AHO03]
    public void EditStructuredLearningOutcome_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningOutcomeCommandFactory = Substitute.For<ILearningOutcomeCommandFactory>();
        var mockBatchCommandFactory = Substitute.For<IBatchCommandFactory>();
        var mockBatchCommand = Substitute.For<IBatchCommand>();
        var mockDeleteCommand = Substitute.For<IDeleteLearningOutcome>();
        var mockAddCommand = Substitute.For<IAddLearningOutcome>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLearningOutcomeViewModel = new StructuredLearningOutcomeViewModel(TaxonomyLevel.Level1, "what",
            "whereby", "whatFor", "verbOfVisibility", new CultureInfo("de-DE"));
        var mockLearningOutcomeEntity = new StructuredLearningOutcome(TaxonomyLevel.Level1, "what",
            "whereby", "whatFor", "verbOfVisibility", new CultureInfo("de-DE"));
        var mockLearningOutcomeCollectionViewModel = ViewModelProvider.GetLearningOutcomeCollection();
        var mockLearningOutcomeCollectionEntity = EntityProvider.GetLearningOutcomeCollection();

        mockMapper
            .Map<StructuredLearningOutcome>(mockLearningOutcomeViewModel)
            .Returns(mockLearningOutcomeEntity);

        mockMapper
            .Map<LearningOutcomeCollection>(mockLearningOutcomeCollectionViewModel)
            .Returns(mockLearningOutcomeCollectionEntity);

        mockLearningOutcomeCommandFactory.GetAddLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            TaxonomyLevel.Level1, "what", "verbOfVisibility", "whereby", "whatFor", new CultureInfo("de-DE"),
            Arg.Any<Action<LearningOutcomeCollection>>()).Returns(mockAddCommand);

        mockLearningOutcomeCommandFactory.GetDeleteLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            mockLearningOutcomeEntity, Arg.Any<Action<LearningOutcomeCollection>>()).Returns(mockDeleteCommand);

        mockBatchCommandFactory.GetBatchCommand(Arg.Is<IEnumerable<IUndoCommand>>(i =>
            i.SequenceEqual(new IUndoCommand[]
                { mockDeleteCommand, mockAddCommand }))).Returns(mockBatchCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, learningOutcomeCommandFactory: mockLearningOutcomeCommandFactory,
            batchCommandFactory: mockBatchCommandFactory);

        systemUnderTest.EditStructuredLearningOutcome(mockLearningOutcomeCollectionViewModel,
            mockLearningOutcomeViewModel, TaxonomyLevel.Level1, "what", "verbOfVisibility", "whereby", "whatFor",
            new CultureInfo("de-DE"));


        mockLearningOutcomeCommandFactory.Received().GetDeleteLearningOutcomeCommand(
            mockLearningOutcomeCollectionEntity,
            mockLearningOutcomeEntity, Arg.Any<Action<LearningOutcomeCollection>>());

        mockLearningOutcomeCommandFactory.Received().GetAddLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            TaxonomyLevel.Level1, "what", "verbOfVisibility", "whereby", "whatFor", new CultureInfo("de-DE"),
            Arg.Any<Action<LearningOutcomeCollection>>());

        mockBatchCommandFactory.Received().GetBatchCommand(Arg.Is<IEnumerable<IUndoCommand>>(i =>
            i.SequenceEqual(new IUndoCommand[]
                { mockDeleteCommand, mockAddCommand })));

        mockBusinessLogic.Received().ExecuteCommand(mockBatchCommand);
    }

    [Test]
    // ANF-ID: [AHO04]
    public void EditManualLearningOutcome_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningOutcomeCommandFactory = Substitute.For<ILearningOutcomeCommandFactory>();
        var mockBatchCommandFactory = Substitute.For<IBatchCommandFactory>();
        var mockBatchCommand = Substitute.For<IBatchCommand>();
        var mockDeleteCommand = Substitute.For<IDeleteLearningOutcome>();
        var mockAddCommand = Substitute.For<IAddLearningOutcome>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLearningOutcomeViewModel = new ManualLearningOutcomeViewModel("manualLearningOutcomeText");
        var mockLearningOutcomeEntity = new ManualLearningOutcome("manualLearningOutcomeText");
        var mockLearningOutcomeCollectionViewModel = ViewModelProvider.GetLearningOutcomeCollection();
        var mockLearningOutcomeCollectionEntity = EntityProvider.GetLearningOutcomeCollection();

        mockMapper
            .Map<ManualLearningOutcome>(mockLearningOutcomeViewModel)
            .Returns(mockLearningOutcomeEntity);

        mockMapper
            .Map<LearningOutcomeCollection>(mockLearningOutcomeCollectionViewModel)
            .Returns(mockLearningOutcomeCollectionEntity);

        mockLearningOutcomeCommandFactory.GetAddLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            "manualLearningOutcomeText",
            Arg.Any<Action<LearningOutcomeCollection>>()).Returns(mockAddCommand);

        mockLearningOutcomeCommandFactory.GetDeleteLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            mockLearningOutcomeEntity,
            Arg.Any<Action<LearningOutcomeCollection>>()).Returns(mockDeleteCommand);

        mockBatchCommandFactory.GetBatchCommand(Arg.Is<IEnumerable<IUndoCommand>>(i =>
            i.SequenceEqual(new IUndoCommand[]
                { mockDeleteCommand, mockAddCommand }))).Returns(mockBatchCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, learningOutcomeCommandFactory: mockLearningOutcomeCommandFactory,
            batchCommandFactory: mockBatchCommandFactory);

        systemUnderTest.EditManualLearningOutcome(mockLearningOutcomeCollectionViewModel, mockLearningOutcomeViewModel,
            "manualLearningOutcomeText");

        mockLearningOutcomeCommandFactory.Received().GetDeleteLearningOutcomeCommand(
            mockLearningOutcomeCollectionEntity, mockLearningOutcomeEntity,
            Arg.Any<Action<LearningOutcomeCollection>>());

        mockLearningOutcomeCommandFactory.Received().GetAddLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            "manualLearningOutcomeText", Arg.Any<Action<LearningOutcomeCollection>>());

        mockBatchCommandFactory.Received().GetBatchCommand(Arg.Is<IEnumerable<IUndoCommand>>(i =>
            i.SequenceEqual(new IUndoCommand[]
                { mockDeleteCommand, mockAddCommand })));

        mockBusinessLogic.Received().ExecuteCommand(mockBatchCommand);
    }

    [Test]
    // ANF-ID: [AHO05]
    public void DeleteLearningOutcome_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningOutcomeCommandFactory = Substitute.For<ILearningOutcomeCommandFactory>();
        var mockCommand = Substitute.For<IDeleteLearningOutcome>();
        var mockMapper = Substitute.For<IMapper>();
        var mockLearningOutcomeViewModel = new ManualLearningOutcomeViewModel("manualLearningOutcomeText");
        var mockLearningOutcomeEntity = new ManualLearningOutcome("manualLearningOutcomeText");
        var mockLearningOutcomeCollectionViewModel = ViewModelProvider.GetLearningOutcomeCollection();
        var mockLearningOutcomeCollectionEntity = EntityProvider.GetLearningOutcomeCollection();

        mockMapper
            .Map<ILearningOutcome>(mockLearningOutcomeViewModel)
            .Returns(mockLearningOutcomeEntity);

        mockMapper
            .Map<LearningOutcomeCollection>(mockLearningOutcomeCollectionViewModel)
            .Returns(mockLearningOutcomeCollectionEntity);

        mockLearningOutcomeCommandFactory.GetDeleteLearningOutcomeCommand(mockLearningOutcomeCollectionEntity,
            mockLearningOutcomeEntity,
            Arg.Any<Action<LearningOutcomeCollection>>()).Returns(mockCommand);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            learningOutcomeCommandFactory: mockLearningOutcomeCommandFactory);

        systemUnderTest.DeleteLearningOutcome(mockLearningOutcomeCollectionViewModel, mockLearningOutcomeViewModel);

        mockLearningOutcomeCommandFactory.Received().GetDeleteLearningOutcomeCommand(
            mockLearningOutcomeCollectionEntity, mockLearningOutcomeEntity,
            Arg.Any<Action<LearningOutcomeCollection>>());

        mockBusinessLogic.Received().ExecuteCommand(mockCommand);
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public void RemoveContent_CallsMapperAndBusinessLogic()
    {
        var mockMapper = Substitute.For<IMapper>();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockContentViewModel = Substitute.For<ILearningContentViewModel>();
        var mockContentEntity = Substitute.For<ILearningContent>();
        mockMapper.Map<ILearningContent>(mockContentViewModel).Returns(mockContentEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.RemoveContent(mockContentViewModel);

        mockMapper.Received().Map<ILearningContent>(mockContentViewModel);
        mockBusinessLogic.Received().RemoveContent(mockContentEntity);
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public void RemoveMultipleContents_CallsMapperAndBusinessLogic()
    {
        var mockMapper = Substitute.For<IMapper>();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockContentViewModelEnumerable = Substitute.For<IEnumerable<ILearningContentViewModel>>();
        var mockContentEntityEnumerable = Substitute.For<IEnumerable<ILearningContent>>();
        var mockContentEntityEnumerableList = mockContentEntityEnumerable.ToList();
        mockMapper.Map<IEnumerable<ILearningContent>>(mockContentViewModelEnumerable)
            .Returns(mockContentEntityEnumerableList);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.RemoveMultipleContents(mockContentViewModelEnumerable);

        mockMapper.Received().Map<IEnumerable<ILearningContent>>(mockContentViewModelEnumerable);
        mockBusinessLogic.Received().RemoveMultipleContents(mockContentEntityEnumerableList);
    }

    [Test]
    // ANF-ID: [AHO21, AHO25]
    public void IsLmsConnected_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.IsLmsConnected();

        mockBusinessLogic.Received().IsLmsConnected();
    }

    [Test]
    // ANF-ID: [AHO21, AHO25]
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
    // ANF-ID: [AHO25]
    public void LoginName_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        _ = systemUnderTest.LoginName;

        _ = mockBusinessLogic.Received().LoginName;
    }

    [Test]
    // ANF-ID: [AHO21]
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
    // ANF-ID: [AHO21]
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
    // ANF-ID: [AHO25]
    public void Logout_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.Logout();

        mockBusinessLogic.Received().Logout();
    }

    [Test]
    // ANF-ID: [AHO23]
    public async Task GetLmsWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        await systemUnderTest.GetLmsWorldList();

        await mockBusinessLogic.Received().GetLmsWorldList();
    }

    [Test]
    // ANF-ID: [AHO24]
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

    [Test]
    // ANF-ID: [ASN0001]
    public void ValidateLearningWorldForExport_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        var mockWorld = EntityProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>()).Returns(mockWorld);
        
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.ValidateLearningWorldForExport(mockWorldVm);

        mockBusinessLogic.Received().ValidateLearningWorldForExport(mockWorld);
    }
    
    [Test]
    // ANF-ID: [AHO22]
    public void ValidateLearningWorldForGeneration_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        var mockWorld = EntityProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>()).Returns(mockWorld);
        
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.ValidateLearningWorldForGeneration(mockWorldVm);

        mockBusinessLogic.Received().ValidateLearningWorldForGeneration(mockWorld);
    }

    [Test]
    // ANF-ID: [ASN0001]
    public async Task ExportLearningWorldToZipArchive_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        var mockWorldEntity = EntityProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        var pathToArchive = "pathToArchiveVar";
        mockDialogManager.ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<IEnumerable<FileFilterProxy>?>()).Returns(pathToArchive);
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(mockWorldEntity);
        var serviceProvider = new ServiceCollection();
        serviceProvider.Insert(0, new ServiceDescriptor(typeof(IElectronDialogManager), mockDialogManager));
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            serviceProvider: serviceProvider.BuildServiceProvider());
        systemUnderTest.RunningElectron.Returns(true);
        await systemUnderTest.ExportLearningWorldToZipArchiveAsync(mockWorldVm);
        mockMapper.Received().Map<BusinessLogic.Entities.LearningWorld>(mockWorldVm);
        await mockBusinessLogic.Received().ExportLearningWorldToArchiveAsync(mockWorldEntity, pathToArchive);
    }

    [Test]
    // ANF-ID: [ASN0001]
    public async Task ExportLearningWorldToZipArchive_UserCancels_DoesNotCallBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockDialogManager.ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<IEnumerable<FileFilterProxy>?>()).Throws(new OperationCanceledException("User cancelled"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);
        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);
        systemUnderTest.RunningElectron.Returns(true);

        systemUnderTest.ExportLearningWorldToZipArchiveAsync(mockWorldVm).Throws(new OperationCanceledException("User cancelled"));;

        await mockBusinessLogic.DidNotReceive()
            .ExportLearningWorldToArchiveAsync(Arg.Any<BusinessLogic.Entities.LearningWorld>(), Arg.Any<string>());
    }
    
    [Test]
    // ANF-ID: [AHO22]
    public async Task ExportLearningWorldToMoodleArchive_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        var mockWorldEntity = EntityProvider.GetLearningWorld();
        var mockMapper = Substitute.For<IMapper>();
        var pathToArchive = "pathToArchiveVar";
        mockDialogManager.ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<IEnumerable<FileFilterProxy>?>()).Returns(pathToArchive);
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<LearningWorldViewModel>())
            .Returns(mockWorldEntity);
        var serviceProvider = new ServiceCollection();
        serviceProvider.Insert(0, new ServiceDescriptor(typeof(IElectronDialogManager), mockDialogManager));
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            serviceProvider: serviceProvider.BuildServiceProvider());
        systemUnderTest.RunningElectron.Returns(true);
        await systemUnderTest.ExportLearningWorldToMoodleArchiveAsync(mockWorldVm);
        mockMapper.Received().Map<BusinessLogic.Entities.LearningWorld>(mockWorldVm);
        mockBusinessLogic.Received().ConstructBackup(mockWorldEntity, pathToArchive);
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task ExportLearningWorldToMoodleArchive_UserCancels_DoesNotCallBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockDialogManager.ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<IEnumerable<FileFilterProxy>?>()).Throws(new OperationCanceledException("User cancelled"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);
        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);
        systemUnderTest.RunningElectron.Returns(true);

        systemUnderTest.ExportLearningWorldToMoodleArchiveAsync(mockWorldVm).Throws(new OperationCanceledException("User cancelled"));;

        mockBusinessLogic.DidNotReceive()
            .ConstructBackup(Arg.Any<BusinessLogic.Entities.LearningWorld>(), Arg.Any<string>());
    }

    // ANF-ID: [ASN0002]
    [Test]
    public async Task ImportLearningWorldFromArchive_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        var mockWorldEntity = EntityProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockMapper = Substitute.For<IMapper>();
        var pathToArchive = "pathToArchiveVar";
        var mockViewModel = ViewModelProvider.GetLearningWorld();
        mockDialogManager.ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<IEnumerable<FileFilterProxy>?>()).Returns(pathToArchive);
        mockBusinessLogic.ImportLearningWorldFromArchiveAsync(pathToArchive).Returns(mockWorldEntity);
        mockMapper.Map<LearningWorldViewModel>(mockWorldEntity).Returns(mockViewModel);
        var serviceProvider = new ServiceCollection();
        serviceProvider.Insert(0, new ServiceDescriptor(typeof(IElectronDialogManager), mockDialogManager));
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper,
            serviceProvider: serviceProvider.BuildServiceProvider(),
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.RunningElectron.Returns(true);

        var result = await systemUnderTest.ImportLearningWorldFromArchiveAsync();

        Assert.That(mockViewModel, Is.EqualTo(result));
        mockMapper.Received().Map<LearningWorldViewModel>(mockWorldEntity);
        await mockBusinessLogic.Received().ImportLearningWorldFromArchiveAsync(pathToArchive);
        selectedViewModelsProvider.Received().SetLearningWorld(mockViewModel, null);
    }

    // ANF-ID: [ASN0002]
    [Test]
    public async Task ImportLearningWorldFromArchive_UserCancels_DoesNotCallBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockDialogManager.ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<IEnumerable<FileFilterProxy>?>()).Throws(new OperationCanceledException("User cancelled"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.RunningElectron.Returns(true);

        var result = await systemUnderTest.ImportLearningWorldFromArchiveAsync();

        Assert.That(result, Is.Null);
        await mockBusinessLogic.DidNotReceive().ImportLearningWorldFromArchiveAsync(Arg.Any<string>());
        selectedViewModelsProvider.DidNotReceive()
            .SetLearningWorld(Arg.Any<LearningWorldViewModel>(), Arg.Any<ICommand>());
    }

    [Test]
    // ANF-ID: [ASE5]
    public void GetFileInfoForLearningWorld_IsWhiteSpace_ReturnsNull()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        mockWorldVm.SavePath = "";
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        var fileInfo = systemUnderTest.GetFileInfoForLearningWorld(mockWorldVm);

        Assert.That(fileInfo, Is.Null);

        mockBusinessLogic.DidNotReceive().GetFileInfoForPath(Arg.Any<string>());
    }

    [Test]
    // ANF-ID: [ASE5]
    public void GetFileInfoForLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        mockWorldVm.SavePath = "path";
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        systemUnderTest.GetFileInfoForLearningWorld(mockWorldVm);

        mockBusinessLogic.Received().GetFileInfoForPath("path");
    }

    [Test]
    // ANF-ID: [ASE4]
    public void DeleteLearningWorldByPath_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldVm = ViewModelProvider.GetLearningWorld();
        mockWorldVm.SavePath = "path";
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        systemUnderTest.DeleteLearningWorldByPath(mockWorldVm.SavePath);

        mockBusinessLogic.Received().DeleteFileByPath("path");
    }

    [Test]
    // ANF-ID: [AWA0038]
    public void OpenContentFilesFolder_CallsBusinessLogicAndShellWrapper()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockShellWrapper = Substitute.For<IShellWrapper>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);
        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, shellWrapper: mockShellWrapper);
        systemUnderTest.RunningElectron.Returns(true);

        mockBusinessLogic.GetContentFilesFolderPath().Returns("Path");

        systemUnderTest.OpenContentFilesFolder();

        mockBusinessLogic.Received().GetContentFilesFolderPath();
        mockShellWrapper.Received().OpenPathAsync("Path");
    }

    [Test]
    public void SetSelectedLearningContentViewModel_CallsSelectedViewModelsProvider()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mockContentVm = ViewModelProvider.GetAdaptivityContent();
        var systemUnderTest =
            CreateTestablePresentationLogic(selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.SetSelectedLearningContentViewModel(mockContentVm);

        mockSelectedViewModelsProvider.Received().SetLearningContent(mockContentVm, null);
    }

    [Test]
    // ANF-ID: [AWA0027]
    public void ReplaceContentReferenceActionByElementReferenceAction_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityRuleCommandFactory = Substitute.For<IAdaptivityRuleCommandFactory>();
        var mockBatchCommandFactory = Substitute.For<IBatchCommandFactory>();

        var mockDeleteCommand = Substitute.For<IDeleteAdaptivityRule>();
        var mockCreateCommand = Substitute.For<ICreateAdaptivityRule>();
        var mockBatchCommand = Substitute.For<IBatchCommand>();

        var questionViewModel = Substitute.For<IAdaptivityQuestionViewModel>();
        var ruleViewModel = Substitute.For<IAdaptivityRuleViewModel>();
        var elementReferenceActionViewModel = ViewModelProvider.GetElementReferenceAction();
        var triggerViewModel = Substitute.For<IAdaptivityTriggerViewModel>();

        var questionEntity = Substitute.For<IAdaptivityQuestion>();
        var ruleEntity = Substitute.For<IAdaptivityRule>();
        var elementReferenceActionEntity = EntityProvider.GetElementReferenceAction();
        var triggerEntity = Substitute.For<IAdaptivityTrigger>();

        mockMapper.Map<IAdaptivityQuestion>(questionViewModel).Returns(questionEntity);
        mockMapper.Map<IAdaptivityRule>(ruleViewModel).Returns(ruleEntity);
        mockMapper.Map<ElementReferenceAction>(elementReferenceActionViewModel).Returns(elementReferenceActionEntity);
        mockMapper.Map<IAdaptivityTrigger>(triggerViewModel).Returns(triggerEntity);

        mockAdaptivityRuleCommandFactory.GetDeleteCommand(
                questionEntity,
                ruleEntity,
                Arg.Any<Action<IAdaptivityQuestion>>())
            .Returns(mockDeleteCommand);

        mockAdaptivityRuleCommandFactory.GetCreateCommand(
                questionEntity,
                triggerEntity,
                elementReferenceActionEntity,
                Arg.Any<Action<IAdaptivityQuestion>>())
            .Returns(mockCreateCommand);

        mockBatchCommandFactory.GetBatchCommand(
                Arg.Is<IEnumerable<IUndoCommand>>(cmds =>
                    cmds.SequenceEqual(new IUndoCommand[] { mockDeleteCommand, mockCreateCommand })))
            .Returns(mockBatchCommand);

        var systemUnderTest = CreateTestablePresentationLogic(
            businessLogic: mockBusinessLogic,
            mapper: mockMapper,
            adaptivityRuleCommandFactory: mockAdaptivityRuleCommandFactory,
            batchCommandFactory: mockBatchCommandFactory);

        systemUnderTest.ReplaceContentReferenceActionByElementReferenceAction(
            questionViewModel,
            ruleViewModel,
            elementReferenceActionViewModel,
            triggerViewModel);

        mockAdaptivityRuleCommandFactory.Received().GetDeleteCommand(
            questionEntity,
            ruleEntity,
            Arg.Any<Action<IAdaptivityQuestion>>());

        mockAdaptivityRuleCommandFactory.Received().GetCreateCommand(
            questionEntity,
            triggerEntity,
            elementReferenceActionEntity,
            Arg.Any<Action<IAdaptivityQuestion>>());

        mockBatchCommandFactory.Received().GetBatchCommand(
            Arg.Is<IEnumerable<IUndoCommand>>(cmds =>
                cmds.SequenceEqual(new IUndoCommand[] { mockDeleteCommand, mockCreateCommand })));

        mockBusinessLogic.Received().ExecuteCommand(mockBatchCommand);
    }
    
    [Test]
    // ANF-ID: [AWA0028]
    public void ReplaceElementReferenceActionByContentReferenceAction_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var mockAdaptivityRuleCommandFactory = Substitute.For<IAdaptivityRuleCommandFactory>();
        var mockBatchCommandFactory = Substitute.For<IBatchCommandFactory>();

        var mockDeleteCommand = Substitute.For<IDeleteAdaptivityRule>();
        var mockCreateCommand = Substitute.For<ICreateAdaptivityRule>();
        var mockBatchCommand = Substitute.For<IBatchCommand>();

        var questionViewModel = Substitute.For<IAdaptivityQuestionViewModel>();
        var ruleViewModel = Substitute.For<IAdaptivityRuleViewModel>();
        var contentReferenceActionViewModel = ViewModelProvider.GetContentReferenceAction();
        var triggerViewModel = Substitute.For<IAdaptivityTriggerViewModel>();

        var questionEntity = Substitute.For<IAdaptivityQuestion>();
        var ruleEntity = Substitute.For<IAdaptivityRule>();
        var contentReferenceActionEntity = EntityProvider.GetContentReferenceAction();
        var triggerEntity = Substitute.For<IAdaptivityTrigger>();

        mockMapper.Map<IAdaptivityQuestion>(questionViewModel).Returns(questionEntity);
        mockMapper.Map<IAdaptivityRule>(ruleViewModel).Returns(ruleEntity);
        mockMapper.Map<ContentReferenceAction>(contentReferenceActionViewModel).Returns(contentReferenceActionEntity);
        mockMapper.Map<IAdaptivityTrigger>(triggerViewModel).Returns(triggerEntity);

        mockAdaptivityRuleCommandFactory.GetDeleteCommand(
                questionEntity,
                ruleEntity,
                Arg.Any<Action<IAdaptivityQuestion>>())
            .Returns(mockDeleteCommand);

        mockAdaptivityRuleCommandFactory.GetCreateCommand(
                questionEntity,
                triggerEntity,
                contentReferenceActionEntity,
                Arg.Any<Action<IAdaptivityQuestion>>())
            .Returns(mockCreateCommand);

        mockBatchCommandFactory.GetBatchCommand(
                Arg.Is<IEnumerable<IUndoCommand>>(cmds =>
                    cmds.SequenceEqual(new IUndoCommand[] { mockDeleteCommand, mockCreateCommand })))
            .Returns(mockBatchCommand);

        var systemUnderTest = CreateTestablePresentationLogic(
            businessLogic: mockBusinessLogic,
            mapper: mockMapper,
            adaptivityRuleCommandFactory: mockAdaptivityRuleCommandFactory,
            batchCommandFactory: mockBatchCommandFactory);

        systemUnderTest.ReplaceElementReferenceActionByContentReferenceAction(
            questionViewModel,
            ruleViewModel,
            contentReferenceActionViewModel,
            triggerViewModel);

        mockAdaptivityRuleCommandFactory.Received().GetDeleteCommand(
            questionEntity,
            ruleEntity,
            Arg.Any<Action<IAdaptivityQuestion>>());

        mockAdaptivityRuleCommandFactory.Received().GetCreateCommand(
            questionEntity,
            triggerEntity,
            contentReferenceActionEntity,
            Arg.Any<Action<IAdaptivityQuestion>>());

        mockBatchCommandFactory.Received().GetBatchCommand(
            Arg.Is<IEnumerable<IUndoCommand>>(cmds =>
                cmds.SequenceEqual(new IUndoCommand[] { mockDeleteCommand, mockCreateCommand })));

        mockBusinessLogic.Received().ExecuteCommand(mockBatchCommand);
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
        ILearningOutcomeCommandFactory? learningOutcomeCommandFactory = null,
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
        learningOutcomeCommandFactory ??= Substitute.For<ILearningOutcomeCommandFactory>();
        worldCommandFactory ??= Substitute.For<IWorldCommandFactory>();
        batchCommandFactory ??= Substitute.For<IBatchCommandFactory>();
        adaptivityRuleCommandFactory ??= Substitute.For<IAdaptivityRuleCommandFactory>();
        adaptivityActionCommandFactory ??= Substitute.For<IAdaptivityActionCommandFactory>();

        return new Presentation.PresentationLogic.API.PresentationLogic(configuration, businessLogic, mapper,
            cachingMapper, selectedViewModelsProvider, serviceProvider, logger, hybridSupportWrapper, shellWrapper,
            questionCommandFactory, taskCommandFactory, conditionCommandFactory, elementCommandFactory,
            layoutCommandFactory, pathwayCommandFactory, spaceCommandFactory, topicCommandFactory,
            learningOutcomeCommandFactory,
            worldCommandFactory,
            batchCommandFactory, adaptivityRuleCommandFactory, adaptivityActionCommandFactory, fileSystem);
    }
}