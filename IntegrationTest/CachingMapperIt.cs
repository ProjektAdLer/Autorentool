using System;
using System.Linq;
using AuthoringTool;
using AuthoringTool.Mapping;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands;
using ElectronWrapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
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
        var businessLogic = new BusinessLogic.API.BusinessLogic(null!, null!, null!, commandStateManager);
        var config = new MapperConfiguration(ViewModelEntityMappingProfile.Configure);
        var mapper = config.CreateMapper();
        var logger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, logger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper);
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        systemUnderTest.CreateWorld(workspaceVm, "a", "b", "c", "d", "e", "f");
        Assert.That(workspaceVm.Worlds, Has.Count.EqualTo(1));
        var worldVm = workspaceVm.Worlds[0];

        Assert.That(workspaceVm.SelectedWorld, Is.EqualTo(worldVm));
        systemUnderTest.EditWorld(workspaceVm.SelectedWorld!, "a1", "b1", "c1", "d1", "e1", "f1");
        systemUnderTest.UndoCommand();
        Assert.That(workspaceVm.SelectedWorld, Is.EqualTo(worldVm));

        systemUnderTest.UndoCommand();

        Assert.That(workspaceVm.Worlds, Has.Count.EqualTo(0));

        systemUnderTest.RedoCommand();

        Assert.That(workspaceVm.Worlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.Worlds[0], Is.EqualTo(worldVm));
            Assert.That(workspaceVm.Worlds[0].Name, Is.EqualTo(worldVm.Name));
            Assert.That(workspaceVm.Worlds[0].Description, Is.EqualTo(worldVm.Description));
        });
    }

    [Test]
    public void CreateWorldAndSpaceThenUndoAndRedo_CheckIfWorldViewModelStaysTheSame()
    {
        var commandStateManager = new CommandStateManager();
        var businessLogic = new BusinessLogic.API.BusinessLogic(null!, null!, null!, commandStateManager);
        var config = new MapperConfiguration(ViewModelEntityMappingProfile.Configure);
        var mapper = config.CreateMapper();
        var logger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, logger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper);

        var workspaceVm = new AuthoringToolWorkspaceViewModel();

        systemUnderTest.CreateWorld(workspaceVm, "a", "b", "c", "d", "e", "f");

        Assert.That(workspaceVm.Worlds, Has.Count.EqualTo(1));

        var worldVm = workspaceVm.Worlds[0];

        systemUnderTest.CreateSpace(worldVm, "g", "h", "i", "j", "k", 1, 2, 3);

        Assert.That(worldVm.Spaces, Has.Count.EqualTo(1));

        var spaceVm = worldVm.Spaces.First();

        Assert.That(workspaceVm.Worlds, Has.Count.EqualTo(1));
        Assert.That(worldVm.Name, Is.EqualTo("a"));
        Assert.That(spaceVm.Name, Is.EqualTo("g"));

        Assert.That(worldVm.Spaces.First(), Is.EqualTo(spaceVm));

        //Undo Redo CreateSpaceCommand
        systemUnderTest.UndoCommand();
        systemUnderTest.RedoCommand();

        Assert.That(worldVm.Spaces.First(), Is.EqualTo(spaceVm));

        //Undo Redo CreateSpaceCommand and CreateWorldCommand
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.RedoCommand();
        systemUnderTest.RedoCommand();

        Assert.That(workspaceVm.Worlds[0], Is.EqualTo(worldVm));
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.Worlds, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.Worlds[0], Is.EqualTo(worldVm));
            Assert.That(workspaceVm.Worlds[0].Spaces, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.Worlds[0].Spaces.First(), Is.EqualTo(spaceVm));
            Assert.That(worldVm.Spaces.First(), Is.EqualTo(spaceVm));
            Assert.That(worldVm.Spaces.First().Name, Is.EqualTo(spaceVm.Name));
        });
    }

    [Test]
    public void CreateWorldAndSpaceAndElementThenUndoAndRedo_CheckIfAllViewModelsStayTheSame()
    {
        var commandStateManager = new CommandStateManager();
        var businessLogic = new BusinessLogic.API.BusinessLogic(null!, null!, null!, commandStateManager);
        var config = new MapperConfiguration(ViewModelEntityMappingProfile.Configure);
        var mapper = config.CreateMapper();
        var logger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, logger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper);

        var workspaceVm = new AuthoringToolWorkspaceViewModel();

        systemUnderTest.CreateWorld(workspaceVm, "a", "b", "c", "d", "e", "f");

        Assert.That(workspaceVm.Worlds, Has.Count.EqualTo(1));

        var worldVm = workspaceVm.Worlds[0];

        systemUnderTest.CreateSpace(worldVm, "g", "h", "i", "j", "k", 1, 2, 3);
        systemUnderTest.ChangeSpaceLayout(worldVm.Spaces.First(), FloorPlanEnum.Rectangle2X3);

        Assert.That(worldVm.Spaces, Has.Count.EqualTo(1));

        var spaceVm = worldVm.Spaces.First();

        systemUnderTest.CreateElement(spaceVm, 0, "l", "m", ElementTypeEnum.Transfer, ContentTypeEnum.PDF,
            null!, "url", "n", "o", "p", ElementDifficultyEnum.Easy, 2, 3);

        Assert.That(spaceVm.ContainedElements.Count(), Is.EqualTo(1));

        var elementVm = spaceVm.ContainedElements.First();

        Assert.That(elementVm.Name, Is.EqualTo("l"));

        //Undo Redo CreateElementCommand and ChangeSpaceLayoutCommand and CreateSpaceCommand and CreateWorldCommand
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.RedoCommand();
        systemUnderTest.RedoCommand();
        systemUnderTest.RedoCommand();
        systemUnderTest.RedoCommand();

        Assert.That(workspaceVm.Worlds[0], Is.EqualTo(worldVm));
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.Worlds, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.Worlds[0], Is.EqualTo(worldVm));
        });
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.Worlds[0].Spaces, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.Worlds[0].Spaces.First(), Is.EqualTo(spaceVm));
        });
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.Worlds[0].Spaces.First().ContainedElements.Count(),
                Is.EqualTo(1));
            Assert.That(workspaceVm.Worlds[0].Spaces.First().ContainedElements.First(),
                Is.EqualTo(elementVm));
        });
    }

    private static PresentationLogic CreateTestablePresentationLogic(
        IAuthoringToolConfiguration? configuration = null, IBusinessLogic? businessLogic = null, IMapper? mapper = null,
        ICachingMapper? cachingMapper = null, IServiceProvider? serviceProvider = null,
        ILogger<PresentationLogic>? logger = null,
        IHybridSupportWrapper? hybridSupportWrapper = null, IShellWrapper? shellWrapper = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        mapper ??= Substitute.For<IMapper>();
        cachingMapper ??= Substitute.For<ICachingMapper>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<PresentationLogic>>();
        hybridSupportWrapper ??= Substitute.For<IHybridSupportWrapper>();
        shellWrapper ??= Substitute.For<IShellWrapper>();

        return new PresentationLogic(configuration, businessLogic, mapper,
            cachingMapper,
            serviceProvider, logger, hybridSupportWrapper, shellWrapper);
    }
}