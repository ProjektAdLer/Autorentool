using System;
using System.Linq;
using AuthoringTool.Mapping;
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
using ElectronWrapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;
using Shared.Configuration;

namespace IntegrationTest;

[TestFixture]
public class CachingMapperIt
{
    [Test]
    public void CreateWorldThenUndoAndRedo_ViewModelShouldStayTheSame()
    {
        var commandStateManager = new CommandStateManager();
        var businessLogger = Substitute.For<ILogger<BusinessLogic.API.BusinessLogic>>();
        var businessLogic =
            new BusinessLogic.API.BusinessLogic(null!, null!, null!, commandStateManager, null!, null!, businessLogger);
        var config = new MapperConfiguration(ViewModelEntityMappingProfile.Configure);
        var mapper = config.CreateMapper();
        var cachingLogger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, cachingLogger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper,
            worldCommandFactory: new WorldCommandFactory(new NullLoggerFactory()));
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        systemUnderTest.CreateLearningWorld(workspaceVm, "a", "b", "c", "d", "e", "f");
        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));
        var worldVm = workspaceVm.LearningWorlds[0];

        systemUnderTest.EditLearningWorld(worldVm, "a1", "b1", "c1", "d1", "e1", "f1");
        systemUnderTest.UndoCommand();

        systemUnderTest.UndoCommand();

        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(0));

        systemUnderTest.RedoCommand();

        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.LearningWorlds[0], Is.EqualTo(worldVm));
            Assert.That(workspaceVm.LearningWorlds[0].Name, Is.EqualTo(worldVm.Name));
            Assert.That(workspaceVm.LearningWorlds[0].Description, Is.EqualTo(worldVm.Description));
        });
    }

    [Test]
    public void CreateWorldAndSpaceThenUndoAndRedo_CheckIfWorldViewModelStaysTheSame()
    {
        var commandStateManager = new CommandStateManager();
        var businessLogger = Substitute.For<ILogger<BusinessLogic.API.BusinessLogic>>();
        var businessLogic =
            new BusinessLogic.API.BusinessLogic(null!, null!, null!, commandStateManager, null!, null!, businessLogger);
        var config = new MapperConfiguration(ViewModelEntityMappingProfile.Configure);
        var mapper = config.CreateMapper();
        var cachingLogger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, cachingLogger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper,
            worldCommandFactory: new WorldCommandFactory(new NullLoggerFactory()),
            spaceCommandFactory: new SpaceCommandFactory(new NullLoggerFactory()));

        var workspaceVm = new AuthoringToolWorkspaceViewModel();

        systemUnderTest.CreateLearningWorld(workspaceVm, "a", "b", "c", "d", "e", "f");

        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));

        var worldVm = workspaceVm.LearningWorlds[0];

        systemUnderTest.CreateLearningSpace(worldVm, "g", "j", "k", 1, Theme.Campus, 2, 3, null!);
        Assert.That(worldVm.LearningSpaces, Has.Count.EqualTo(1));

        var spaceVm = worldVm.LearningSpaces.First();

        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));
        Assert.That(worldVm.Name, Is.EqualTo("a"));
        Assert.That(spaceVm.Name, Is.EqualTo("g"));

        Assert.That(worldVm.LearningSpaces.First(), Is.EqualTo(spaceVm));

        //Undo Redo CreateLearningSpaceCommand
        systemUnderTest.UndoCommand();
        systemUnderTest.RedoCommand();

        Assert.That(worldVm.LearningSpaces.First(), Is.EqualTo(spaceVm));

        //Undo Redo CreateLearningSpaceCommand and CreateLearningWorldCommand
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.RedoCommand();
        systemUnderTest.RedoCommand();

        Assert.That(workspaceVm.LearningWorlds[0], Is.EqualTo(worldVm));
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.LearningWorlds[0], Is.EqualTo(worldVm));
            Assert.That(workspaceVm.LearningWorlds[0].LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.LearningWorlds[0].LearningSpaces.First(), Is.EqualTo(spaceVm));
            Assert.That(worldVm.LearningSpaces.First(), Is.EqualTo(spaceVm));
            Assert.That(worldVm.LearningSpaces.First().Name, Is.EqualTo(spaceVm.Name));
        });
    }

    [Test]
    public void CreateWorldAndSpaceAndElementThenUndoAndRedo_CheckIfAllViewModelsStayTheSame()
    {
        var commandStateManager = new CommandStateManager();
        var businessLogger = Substitute.For<ILogger<BusinessLogic.API.BusinessLogic>>();
        var businessLogic =
            new BusinessLogic.API.BusinessLogic(null!, null!, null!, commandStateManager, null!, null!, businessLogger);
        var config = new MapperConfiguration(ViewModelEntityMappingProfile.Configure);
        var mapper = config.CreateMapper();
        var cachingLogger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, cachingLogger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper,
            worldCommandFactory: new WorldCommandFactory(new NullLoggerFactory()),
            spaceCommandFactory: new SpaceCommandFactory(new NullLoggerFactory()),
            elementCommandFactory: new ElementCommandFactory(new NullLoggerFactory()));

        var workspaceVm = new AuthoringToolWorkspaceViewModel();

        systemUnderTest.CreateLearningWorld(workspaceVm, "a", "b", "c", "d", "e", "f");

        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));

        var worldVm = workspaceVm.LearningWorlds[0];

        systemUnderTest.CreateLearningSpace(worldVm, "g", "j", "k", 1, Theme.Campus, 2, 3, null!);
        systemUnderTest.ChangeLearningSpaceLayout(worldVm.LearningSpaces.First(), worldVm, FloorPlanEnum.R_20X30_8L);

        Assert.That(worldVm.LearningSpaces, Has.Count.EqualTo(1));

        var spaceVm = worldVm.LearningSpaces.First();

        systemUnderTest.CreateLearningElementInSlot(spaceVm, 0, "l",
            null!, "o", "p", LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 2, 3);

        Assert.That(spaceVm.ContainedLearningElements.Count(), Is.EqualTo(1));

        var elementVm = spaceVm.ContainedLearningElements.First();

        Assert.That(elementVm.Name, Is.EqualTo("l"));

        //Undo Redo CreateLearningElementCommand and ChangeLearningSpaceLayoutCommand and CreateLearningSpaceCommand and CreateLearningWorldCommand
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.RedoCommand();
        systemUnderTest.RedoCommand();
        systemUnderTest.RedoCommand();
        systemUnderTest.RedoCommand();

        Assert.That(workspaceVm.LearningWorlds[0], Is.EqualTo(worldVm));
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.LearningWorlds[0], Is.EqualTo(worldVm));
        });
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.LearningWorlds[0].LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.LearningWorlds[0].LearningSpaces.First(), Is.EqualTo(spaceVm));
        });
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.LearningWorlds[0].LearningSpaces.First().ContainedLearningElements.Count(),
                Is.EqualTo(1));
            Assert.That(workspaceVm.LearningWorlds[0].LearningSpaces.First().ContainedLearningElements.First(),
                Is.EqualTo(elementVm));
        });
    }

    private static PresentationLogic CreateTestablePresentationLogic(
        IApplicationConfiguration? configuration = null, IBusinessLogic? businessLogic = null, IMapper? mapper = null,
        ICachingMapper? cachingMapper = null, ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        IServiceProvider? serviceProvider = null,
        ILogger<PresentationLogic>? logger = null,
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
        configuration ??= Substitute.For<IApplicationConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        mapper ??= Substitute.For<IMapper>();
        cachingMapper ??= Substitute.For<ICachingMapper>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<PresentationLogic>>();
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

        return new PresentationLogic(configuration, businessLogic, mapper,
            cachingMapper, selectedViewModelsProvider, serviceProvider, logger, hybridSupportWrapper,
            shellWrapper,
            conditionCommandFactory, elementCommandFactory, layoutCommandFactory, pathwayCommandFactory,
            spaceCommandFactory, topicCommandFactory, worldCommandFactory, batchCommandFactory);
    }
}