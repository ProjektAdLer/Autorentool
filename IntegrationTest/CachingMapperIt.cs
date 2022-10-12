﻿using System;
using System.Linq;
using AuthoringTool;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands;
using ElectronWrapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
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
        var config = new MapperConfiguration(MappingProfile.Configure);
        var mapper = config.CreateMapper();
        var logger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, logger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper);
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        systemUnderTest.CreateLearningWorld(workspaceVm, "a","b","c","d","e","f");
        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));
        var worldVm = workspaceVm.LearningWorlds[0];
        
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(worldVm));
        systemUnderTest.EditLearningWorld(workspaceVm.SelectedLearningWorld!, "a1","b1","c1","d1","e1","f1");
        systemUnderTest.UndoCommand();
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(worldVm));
        
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
        var businessLogic = new BusinessLogic.API.BusinessLogic(null!, null!, null!, commandStateManager);
        var config = new MapperConfiguration(MappingProfile.Configure);
        var mapper = config.CreateMapper();
        var logger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, logger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper);
        
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        
        systemUnderTest.CreateLearningWorld(workspaceVm, "a","b","c","d","e","f");
        
        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));

        var worldVm = workspaceVm.LearningWorlds[0];
        
        systemUnderTest.CreateLearningSpace(worldVm, "g","h","i","j","k",1, 2, 3);
        
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
        var businessLogic = new BusinessLogic.API.BusinessLogic(null!, null!, null!, commandStateManager);
        var config = new MapperConfiguration(MappingProfile.Configure);
        var mapper = config.CreateMapper();
        var logger = Substitute.For<ILogger<CachingMapper>>();
        var cachingMapper = new CachingMapper(mapper, commandStateManager, logger);
        var systemUnderTest = CreateTestablePresentationLogic(null, businessLogic, mapper, cachingMapper);

        var workspaceVm = new AuthoringToolWorkspaceViewModel();

        systemUnderTest.CreateLearningWorld(workspaceVm, "a", "b", "c", "d", "e", "f");

        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));

        var worldVm = workspaceVm.LearningWorlds[0];

        systemUnderTest.CreateLearningSpace(worldVm, "g", "h", "i", "j", "k", 1, 2, 3);

        Assert.That(worldVm.LearningSpaces, Has.Count.EqualTo(1));

        var spaceVm = worldVm.LearningSpaces.First();

        systemUnderTest.CreateLearningElement(spaceVm, "l", "m", ElementTypeEnum.Transfer,ContentTypeEnum.PDF, null!, "url", "n", "o","p", LearningElementDifficultyEnum.Easy, 2, 3);

        Assert.That(spaceVm.LearningElements, Has.Count.EqualTo(1));

        var elementVm = spaceVm.LearningElements.First();

        Assert.That(elementVm.Name, Is.EqualTo("l"));

        //Undo Redo CreateLearningElementCommand and CreateLearningSpaceCommand and CreateLearningWorldCommand
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
        systemUnderTest.UndoCommand();
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
            Assert.That(workspaceVm.LearningWorlds[0].LearningSpaces.First().LearningElements, Has.Count.EqualTo(1));
            Assert.That(workspaceVm.LearningWorlds[0].LearningSpaces.First().LearningElements.First(), Is.EqualTo(elementVm));
        });
    }

    private static Presentation.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(
        IAuthoringToolConfiguration? configuration = null, IBusinessLogic? businessLogic = null, IMapper? mapper = null, 
        ICachingMapper? cachingMapper = null, IServiceProvider? serviceProvider = null, 
        ILogger<Presentation.PresentationLogic.API.PresentationLogic>? logger = null, 
        IHybridSupportWrapper? hybridSupportWrapper = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        mapper ??= Substitute.For<IMapper>();
        cachingMapper ??= Substitute.For<ICachingMapper>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        hybridSupportWrapper ??= Substitute.For<IHybridSupportWrapper>();

        return new Presentation.PresentationLogic.API.PresentationLogic(configuration, businessLogic, mapper, cachingMapper,
            serviceProvider, logger, hybridSupportWrapper);
    }
}