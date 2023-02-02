using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using ElectronWrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Shared;
using Shared.Configuration;

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
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();

        //Act
        var systemUnderTest = CreateTestablePresentationLogic(mockConfiguration, mockBusinessLogic, mockMapper, mockCachingMapper, mockServiceProvider, mockLogger);
        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(mockBusinessLogic));
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(mockMapper));
            Assert.That(systemUnderTest.CMapper, Is.EqualTo(mockCachingMapper));
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
        var viewModel = new WorldViewModel("fo", "fo", "fo", "fo", "fo", "fo");
        var mockMapper = Substitute.For<IMapper>();
        var entity = new BusinessLogic.Entities.World("baba", "baba", "baba", "baba", "baba", "baba");
        mockMapper.Map<BusinessLogic.Entities.World>(viewModel).Returns(entity);
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
    public void AddWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreateWorld? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>()))
            .Do(sub => command = sub.Arg<ICommand>() as CreateWorld);
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var workspaceEntity = new BusinessLogic.Entities.AuthoringToolWorkspace(null,
            new List<BusinessLogic.Entities.World>{worldEntity});
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<AuthoringToolWorkspaceViewModel>())
            .Returns(workspaceEntity);
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.AddWorld(workspaceVm, worldVm);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.AuthoringToolWorkspace, Is.EqualTo(workspaceEntity));
            Assert.That(command.World, Is.EqualTo(worldEntity));
        });
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
        systemUnderTest.OnUndoRedoPerformed += () => wasCalled = true;
        mockBusinessLogic.OnUndoRedoPerformed += Raise.Event<Action>();

        Assert.That(wasCalled, Is.True);
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
    public void CreateWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreateWorld? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>()))
            .Do(sub => command = sub.Arg<ICommand>() as CreateWorld);
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var workspaceEntity = new BusinessLogic.Entities.AuthoringToolWorkspace(null,
            new List<BusinessLogic.Entities.World>{worldEntity});
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<AuthoringToolWorkspaceViewModel>())
            .Returns(workspaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.CreateWorld(workspaceVm, "f", "f", "f", "f", "f", "f");

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.AuthoringToolWorkspace, Is.EqualTo(workspaceEntity));
    }
    
    [Test]
    public void EditWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        EditWorld? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>()))
            .Do(sub => command = sub.Arg<ICommand>() as EditWorld);
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.EditWorld(worldVm, "f", "f", "f", "f", "f", "f");

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.World, Is.EqualTo(worldEntity));
    }

    [Test]
    public void DeleteWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        DeleteWorld? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>()))
            .Do(sub => command = sub.Arg<ICommand>() as DeleteWorld);
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        workspaceVm._worlds.Add(worldVm);
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var workspaceEntity = new BusinessLogic.Entities.AuthoringToolWorkspace(null,
            new List<BusinessLogic.Entities.World>{worldEntity});
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<AuthoringToolWorkspaceViewModel>())
            .Returns(workspaceEntity);
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.DeleteWorld(workspaceVm, worldVm);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.AuthoringToolWorkspace, Is.EqualTo(workspaceEntity));
            Assert.That(command.World, Is.EqualTo(worldEntity));
        });
    }
    
    [Test]
    public void AddSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreateSpace? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as CreateSpace);
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var spaceVm = new SpaceViewModel("z", "z", "z", "z", "z");
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var spaceEntity = new BusinessLogic.Entities.Space("a", "b", "c", "d", "e", 5);
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>())
            .Returns(spaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.AddSpace(worldVm, spaceVm);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.World, Is.EqualTo(worldEntity));
            Assert.That(command!.Space, Is.EqualTo(spaceEntity));
        });
    }

    [Test]
    public void CreateSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreateSpace? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as CreateSpace);
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.CreateSpace(worldVm, "z", "z", "z", "z", "z", 5, 6, 7);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.World, Is.EqualTo(worldEntity));
    }
    
    [Test]
    public void EditSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        EditSpace? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as EditSpace);
        var spaceVm = new SpaceViewModel("z", "z", "z", "z", "z");
        var mockMapper = Substitute.For<IMapper>();
        var spaceEntity = new BusinessLogic.Entities.Space("a", "b", "c", "d", "e" , 5);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>())
            .Returns(spaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.EditSpace(spaceVm, "z", "z", "z", "z", "z", 5);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.Space, Is.EqualTo(spaceEntity));
    }
    
    [Test]
    public void DragObjectInPathWay_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        DragObjectInPathWay? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as DragObjectInPathWay);
        var spaceVm = new SpaceViewModel("z", "z", "z", "z", "z");
        var mockMapper = Substitute.For<IMapper>();
        var spaceEntity = new BusinessLogic.Entities.Space("a", "b", "c", "d", "e" , 5);
        mockMapper.Map<BusinessLogic.Entities.IObjectInPathWay>(Arg.Any<SpaceViewModel>())
            .Returns(spaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.DragObjectInPathWay(spaceVm, 5, 6);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.DraggableObject, Is.EqualTo(spaceEntity));
    }

    [Test]
    public void DeleteSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        DeleteSpace? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as DeleteSpace);
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var spaceVm = new SpaceViewModel("z", "z", "z", "z", "z");
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var spaceEntity = new BusinessLogic.Entities.Space("a", "b", "c", "d", "e", 5);
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>())
            .Returns(spaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.DeleteSpace(worldVm, spaceVm);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.World, Is.EqualTo(worldEntity));
            Assert.That(command!.Space, Is.EqualTo(spaceEntity));
        });
    }
    
    [Test]
    public void AddElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreateElement? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as CreateElement);
        var spaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 4);
        var elementVm = new ElementViewModel("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, spaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var spaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 4);
        var elementEntity = new BusinessLogic.Entities.Element("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, spaceEntity);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>())
            .Returns(spaceEntity);
        mockMapper.Map<BusinessLogic.Entities.Element>(Arg.Any<ElementViewModel>())
            .Returns(elementEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.AddElement(spaceVm, 0, elementVm);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.ParentSpace, Is.EqualTo(spaceEntity));
            Assert.That(command!.Element, Is.EqualTo(elementEntity));
        });
    }
    
    [Test]
    public void CreateElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreateElement? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as CreateElement);
        var spaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 4);
        var mockMapper = Substitute.For<IMapper>();
        var spaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 5);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>())
            .Returns(spaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.CreateElement(spaceVm, 0, "a", "b", ElementTypeEnum.Activation,
            ContentTypeEnum.H5P, null!, "url", "c", "d", "e", ElementDifficultyEnum.Easy, 1, 2, 3, 4);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.ParentSpace, Is.EqualTo(spaceEntity));
    }
    
    [Test]
    public void EditElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        EditElement? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as EditElement);
        var spaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 5);
        var elementVm = new ElementViewModel("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, spaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var spaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 5);
        var elementEntity = new BusinessLogic.Entities.Element("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, spaceEntity);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>())
            .Returns(spaceEntity);
        mockMapper.Map<BusinessLogic.Entities.Element>(Arg.Any<ElementViewModel>())
            .Returns(elementEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.EditElement(spaceVm, elementVm, "a","b","c", "Google.com", "d",
            "e",ElementDifficultyEnum.Easy,1,2);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.ParentSpace, Is.EqualTo(spaceEntity));
            Assert.That(command!.Element, Is.EqualTo(elementEntity));
        });
    }
    
    [Test]
    public void DragElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        DragElement? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as DragElement);
        var elementVm = new ElementViewModel("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy);
        var mockMapper = Substitute.For<IMapper>();
        var elementEntity = new BusinessLogic.Entities.Element("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy);
        mockMapper.Map<BusinessLogic.Entities.Element>(Arg.Any<ElementViewModel>())
            .Returns(elementEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.DragElement(elementVm, 1, 2);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.Element, Is.EqualTo(elementEntity));
        
    }
    
    [Test]
    public void DeleteElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        DeleteElement? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as DeleteElement);
        var spaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 4);
        var elementVm = new ElementViewModel("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, spaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var spaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 3);
        var elementEntity = new BusinessLogic.Entities.Element("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, spaceEntity);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>())
            .Returns(spaceEntity);
        mockMapper.Map<BusinessLogic.Entities.Element>(Arg.Any<ElementViewModel>())
            .Returns(elementEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.DeleteElement(spaceVm, elementVm);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.ParentSpace, Is.EqualTo(spaceEntity));
            Assert.That(command!.Element, Is.EqualTo(elementEntity));
        });
    }

    [Test]
    public void CreatePathWay_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreatePathWay? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as CreatePathWay);
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var sourceSpaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 4);
        var targetSpaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 4);
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var sourceSpaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 3);
        var targetSpaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 3);

        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);
        mockMapper.Map<BusinessLogic.Entities.IObjectInPathWay>(sourceSpaceVm)
            .Returns(sourceSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.IObjectInPathWay>(targetSpaceVm)
            .Returns(targetSpaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.CreatePathWay(worldVm, sourceSpaceVm, targetSpaceVm);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.World.Id, Is.EqualTo(worldEntity.Id));
            Assert.That(command!.Pathway.SourceObject.Id, Is.EqualTo(sourceSpaceEntity.Id));
            Assert.That(command!.Pathway.TargetObject.Id, Is.EqualTo(targetSpaceEntity.Id));
        });
    }

    [Test]
    public void DeletePathWay_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        DeletePathWay? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as DeletePathWay);
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var sourceSpaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 4);
        var targetSpaceVm = new SpaceViewModel("z", "z", "z", "z", "z");
        var pathWayVm = new PathwayViewModel(sourceSpaceVm, targetSpaceVm);
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var pathWayEntity = new BusinessLogic.Entities.Pathway(
            new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 3),
            new BusinessLogic.Entities.Space("z", "z", "z", "z", "z",5));
        worldEntity.Pathways.Add(pathWayEntity);
        
        
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);
        mockMapper.Map<BusinessLogic.Entities.Pathway>(Arg.Any<PathwayViewModel>())
            .Returns(pathWayEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.DeletePathWay(worldVm, pathWayVm);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.World, Is.EqualTo(worldEntity));
            Assert.That(command!.Pathway, Is.EqualTo(pathWayEntity));
        });
    }
    
    [Test]
    public void CreatePathWayCondition_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreatePathWayCondition? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as CreatePathWayCondition);
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.CreatePathWayCondition(worldVm, ConditionEnum.And, 6, 7);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.World, Is.EqualTo(worldEntity));
    }
    
    [Test]
    public void CreatePathWayConditionBetweenObjects_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        CreatePathWayCondition? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as CreatePathWayCondition);
        var condition = ConditionEnum.And;
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var sourceSpaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 4);
        var targetSpaceVm = new SpaceViewModel("f", "f", "f", "f", "f", 4);
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var sourceSpaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 3);
        var targetSpaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 3);
        worldEntity.Spaces.Add(sourceSpaceEntity);
        worldEntity.Spaces.Add(targetSpaceEntity);
        
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);
        mockMapper.Map<BusinessLogic.Entities.IObjectInPathWay>(sourceSpaceVm)
            .Returns(sourceSpaceEntity);
        mockMapper.Map<BusinessLogic.Entities.Space>(targetSpaceVm)
            .Returns(targetSpaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.CreatePathWayConditionBetweenObjects(worldVm, condition, sourceSpaceVm, targetSpaceVm);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.World.Id, Is.EqualTo(worldEntity.Id));
            Assert.That(command!.SourceObject!.Id, Is.EqualTo(sourceSpaceEntity.Id));
            Assert.That(command!.TargetObject!.Id, Is.EqualTo(targetSpaceEntity.Id));
        });
    }
    
    [Test]
    public void EditPathWayCondition_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        EditPathWayCondition? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as EditPathWayCondition);
        var pathWayConditionVm = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var mockMapper = Substitute.For<IMapper>();
        var pathWayConditionEntity = new BusinessLogic.Entities.PathWayCondition(ConditionEnum.And,2,1);
        mockMapper.Map<BusinessLogic.Entities.PathWayCondition>(Arg.Any<PathWayConditionViewModel>())
            .Returns(pathWayConditionEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.EditPathWayCondition(pathWayConditionVm, ConditionEnum.Or);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.PathWayCondition, Is.EqualTo(pathWayConditionEntity));
    }
    
    [Test]
    public void DeletePathWayCondition_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        DeletePathWayCondition? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as DeletePathWayCondition);
        var worldVm = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var pathWayConditionVm = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        var pathWayConditionEntity = new BusinessLogic.Entities.PathWayCondition(ConditionEnum.And,2,1);
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(worldEntity);
        mockMapper.Map<BusinessLogic.Entities.PathWayCondition>(Arg.Any<PathWayConditionViewModel>())
            .Returns(pathWayConditionEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);
        
        systemUnderTest.DeletePathWayCondition(worldVm, pathWayConditionVm);
        
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(command!.World, Is.EqualTo(worldEntity));
            Assert.That(command!.PathWayCondition, Is.EqualTo(pathWayConditionEntity));
        });
    }

    #region Save/Load

    [Test]
    public void SaveWorldAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveWorldAsync(world));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void SaveWorldAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveWorldAsync(world));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task SaveWorldAsync_CallsDialogManagerAndWorldMapperAndBusinessLogic()
    {
        SaveWorld? command = null;
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as SaveWorld);
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");
        var entity = new BusinessLogic.Entities.World("f", "f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.World>(world).Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, hybridSupportWrapper: mockHybridSupport, serviceProvider: mockServiceProvider);

        await systemUnderTest.SaveWorldAsync(world);

        await mockDialogManger.Received().ShowSaveAsDialogAsync("Save World", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.World>(world);
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
    }

    [Test]
    public void SaveWorldAsync_LogsAndRethrowsDialogCancelledException()
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
        var world = new WorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveWorldAsync(world));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }
    
    [Test]
    public void SaveSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var space = new SpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveSpaceAsync(space));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void SaveSpaceAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var space = new SpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveSpaceAsync(space));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task SaveSpaceAsync_CallsDialogManagerAndSpaceMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        SaveSpace? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as SaveSpace);
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var space = new SpaceViewModel("f", "f", "f", "f", "f");
        var entity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 5);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>()).Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.SaveSpaceAsync(space);

        await mockDialogManger.Received().ShowSaveAsDialogAsync("Save Space", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.Space>(space);
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
    }

    [Test]
    public void SaveSpaceAsync_LogsAndRethrowsDialogCancelledException()
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
        var space = new SpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveSpaceAsync(space));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }
    
    [Test]
    public void SaveElementAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var element = new ElementViewModel("f", "f", null!, "url","f", "f", "f",ElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveElementAsync(element));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void SaveElementAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var element = new ElementViewModel("f", "f", null!,"url","f", "f", "f",ElementDifficultyEnum.Easy);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveElementAsync(element));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task SaveElementAsync_CallsDialogManagerAndElementMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        SaveElement? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as SaveElement);
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var element = new ElementViewModel("f", "f",  null!,"url","f", "f", "f",ElementDifficultyEnum.Easy);
        var entity = new BusinessLogic.Entities.Element("f", "f", null!,"url","f", "f", "f", ElementDifficultyEnum.Easy);
        mockMapper.Map<BusinessLogic.Entities.Element>(Arg.Any<ElementViewModel>()).Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.SaveElementAsync(element);

        await mockDialogManger.Received().ShowSaveAsDialogAsync("Save Element", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.Element>(element);
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
    }

    [Test]
    public void SaveElementAsync_LogsAndRethrowsDialogCancelledException()
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
        var element = new ElementViewModel("f", "f", null!, "url", "f", "f", "f",ElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveElementAsync(element));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }

    [Test]
    public void LoadWorldAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadWorldAsync(authoringToolWorkspaceVm));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadWorldAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
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

        var ex =Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadWorldAsync(authoringToolWorkspaceVm));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task LoadWorldAsync_CallsDialogManagerAndElementMapper()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        
        var workspaceEntity = new BusinessLogic.Entities.AuthoringToolWorkspace(null, new List<BusinessLogic.Entities.World>());
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<WorldViewModel>()).Returns(workspaceEntity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        var authoringToolWorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest = CreateTestablePresentationLogic(mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.LoadWorldAsync(authoringToolWorkspaceVm);

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load World", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockMapper.Received().Map<BusinessLogic.Entities.AuthoringToolWorkspace>(authoringToolWorkspaceVm);
    }

    [Test]
    public void LoadWorldAsync_LogsAndRethrowsDialogCancelledException()
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadWorldAsync(authoringToolWorkspaceVm));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    [Test]
    public void LoadSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockWorldViewModel = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadSpaceAsync(mockWorldViewModel));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadSpaceAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockWorldViewModel = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadSpaceAsync(mockWorldViewModel));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task LoadSpaceAsync_CallsDialogManagerAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        LoadSpace? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as LoadSpace);
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        var mockMapper = Substitute.For<IMapper>();
        var mockWorldViewModel = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        var mockWorldEntity = new BusinessLogic.Entities.World("a", "b", "c", "d", "e", "f");
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockMapper.Map<BusinessLogic.Entities.World>(Arg.Any<WorldViewModel>())
            .Returns(mockWorldEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport, mapper: mockMapper);

        await systemUnderTest.LoadSpaceAsync(mockWorldViewModel);

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Space", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.World, Is.EqualTo(mockWorldEntity));
    }

    [Test]
    public void LoadSpaceAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        var mockWorldViewModel = new WorldViewModel("n", "sn", "a", "l", "d", "g");
        mockElectronDialogManager
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadSpaceAsync(mockWorldViewModel));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    
    [Test]
    public void LoadElementAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockSpaceViewModel = new SpaceViewModel("n", "sn", "a", "l", "d");
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadElementAsync(mockSpaceViewModel, 0));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadElementAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockSpaceViewModel = new SpaceViewModel("sn", "a", "l", "d", "g");
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadElementAsync(mockSpaceViewModel, 0));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task LoadElementAsync_CallsDialogManagerAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        LoadElement? command = null;
        mockBusinessLogic.When(sub => sub.ExecuteCommand(Arg.Any<ICommand>())).
            Do(sub => command = sub.Arg<ICommand>() as LoadElement);
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        var mockMapper = Substitute.For<IMapper>();
        var mockSpaceViewModel = new SpaceViewModel("a", "l", "d", "g","h", 1);
        var mockSpaceEntity = new BusinessLogic.Entities.Space("f", "f", "f", "f", "f", 1);
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockMapper.Map<BusinessLogic.Entities.Space>(Arg.Any<SpaceViewModel>())
            .Returns(mockSpaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport, mapper: mockMapper);

        await systemUnderTest.LoadElementAsync(mockSpaceViewModel, 0);
        
        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Element", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
        Assert.That(command, Is.Not.Null);
        Assert.That(command!.ParentSpace, Is.EqualTo(mockSpaceEntity));
    }

    [Test]
    public void LoadElementAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        var mockSpaceViewModel = new SpaceViewModel("n", "sn", "a", "l", "d");
        mockElectronDialogManager
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadElementAsync(mockSpaceViewModel, 0));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    [Test]
    public void LoadImageAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

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
        var content = new ContentViewModel("f", ".png", "");
        var entity = new Content("f", ".png", "");
        mockMapper.Map<ContentViewModel>(Arg.Any<Content>()).Returns(content);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadContent(filepath).Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadImageAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load image", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadContent(filepath);
        mockMapper.Received().Map<ContentViewModel>(entity);
        
        Assert.That(loadedContent, Is.EqualTo(content));
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

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
        var content = new ContentViewModel("f", ".mp4", "");
        var entity = new Content("f", ".mp4", "");
        mockMapper.Map<ContentViewModel>(Arg.Any<Content>()).Returns(content);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadContent(filepath + ".mp4").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper:mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadVideoAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load video", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadContent(filepath + ".mp4");
        mockMapper.Received().Map<ContentViewModel>(entity);
        
        Assert.That(loadedContent, Is.EqualTo(content));
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

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
        var content = new ContentViewModel("f", ".h5p", "");
        var entity = new Content("f", ".h5p", "");
        mockMapper.Map<ContentViewModel>(Arg.Any<Content>()).Returns(content);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadContent(filepath + ".h5p").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadH5PAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load h5p", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadContent(filepath + ".h5p");
        mockMapper.Received().Map<ContentViewModel>(entity);
        
        Assert.That(loadedContent, Is.EqualTo(content));
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

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
        var content = new ContentViewModel("f", ".pdf", "");
        var entity = new Content("f", ".pdf", "");
        mockMapper.Map<ContentViewModel>(Arg.Any<Content>()).Returns(content);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadContent(filepath + ".pdf").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadPdfAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load pdf", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadContent(filepath + ".pdf");
        mockMapper.Received().Map<ContentViewModel>(entity);
        
        Assert.That(loadedContent, Is.EqualTo(content));
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

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
        var content = new ContentViewModel("f", ".txt", "");
        var entity = new Content("f", ".txt", "");
        mockMapper.Map<ContentViewModel>(Arg.Any<Content>()).Returns(content);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadContent(filepath).Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadTextAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load text", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadContent(filepath);
        mockMapper.Received().Map<ContentViewModel>(entity);
        
        Assert.That(loadedContent, Is.EqualTo(content));
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadTextAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    #endregion
    
    #region Load

    [Test]
    public void PresentationLogic_LoadWorldViewModel_ReturnsWorld()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorld = new BusinessLogic.Entities.World("n", "sn", "a", "l", "d", "g");
        mockBusinessLogic.LoadWorld(Arg.Any<Stream>()).Returns(mockWorld);
        var workspace = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        systemUnderTest.LoadWorldViewModel(workspace, stream);

        mockBusinessLogic.Received().LoadWorld(stream);
    }

    [Test]
    public void PresentationLogic_LoadWorldViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadWorld(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var workspace = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadWorldViewModel(workspace, stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    [Test]
    public void PresentationLogic_LoadSpaceViewModel_ReturnsSpace()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockSpace = new BusinessLogic.Entities.Space("n", "sn", "a", "d", "g", 5);
        mockBusinessLogic.LoadSpace(Arg.Any<Stream>()).Returns(mockSpace);
        var mockSpaceViewModel = new SpaceViewModel("n", "sn", "a", "d", "g");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<SpaceViewModel>(Arg.Any<BusinessLogic.Entities.Space>())
            .Returns(mockSpaceViewModel);
        var worldVm = Substitute.For<IWorldViewModel>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

       systemUnderTest.LoadSpaceViewModel(worldVm, stream);

        mockBusinessLogic.Received().LoadSpace(stream);
        mockMapper.Received().Map<BusinessLogic.Entities.World>(worldVm);
    }

    [Test]
    public void PresentationLogic_LoadSpaceViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadSpace(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var worldVm = Substitute.For<IWorldViewModel>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadSpaceViewModel(worldVm, stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    [Test]
    public void PresentationLogic_LoadElementViewModel_ReturnsElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockElement = new BusinessLogic.Entities.Element("n", "sn",null!, "url","a", "d", "g", ElementDifficultyEnum.Easy);
        mockBusinessLogic.LoadElement(Arg.Any<Stream>()).Returns(mockElement);
        var mockContent = new ContentViewModel("n", "t", "");
        var mockElementViewModel = new ElementViewModel("n", "sn", mockContent, "url","a", "d", "g",ElementDifficultyEnum.Easy);
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<ElementViewModel>(Arg.Any<BusinessLogic.Entities.Element>())
            .Returns(mockElementViewModel);
        var spaceVm = Substitute.For<ISpaceViewModel>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.LoadElementViewModel(spaceVm, 0, stream);

        mockBusinessLogic.Received().LoadElement(stream);
        mockMapper.Received().Map<BusinessLogic.Entities.Space>(spaceVm);
    }

    [Test]
    public void PresentationLogic_LoadElementViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadElement(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var spaceVm = Substitute.For<ISpaceViewModel>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadElementViewModel(spaceVm, 0, stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }

    [Test]
    public void PresentationLogic_LoadContentViewModel_ReturnsContent()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockContent = new Content("n", "t", "");
        mockBusinessLogic.LoadContent(Arg.Any<string>(), Arg.Any<MemoryStream>()).Returns(mockContent);
        var mockContentViewModel = new ContentViewModel("n", "t", "");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<ContentViewModel>(Arg.Any<Content>())
            .Returns(mockContentViewModel);
        var filename = "test.png";
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        var result = systemUnderTest.LoadContentViewModel(filename, stream);

        mockBusinessLogic.Received().LoadContent(filename, stream);
        mockMapper.Received().Map<ContentViewModel>(mockContent);
        Assert.That(result, Is.EqualTo(mockContentViewModel));
    }

    [Test]
    public void PresentationLogic_LoadContentViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadContent(Arg.Any<string>(), Arg.Any<MemoryStream>()).Throws(new Exception("Exception"));
        var filename = "test.png";
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadContentViewModel(filename, stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    #endregion

    [Test]
    public void ShowElementContentAsync_CallsShellWrapper()
    {
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockShellWrapper = Substitute.For<IShellWrapper>();
        mockShellWrapper.OpenPathAsync(Arg.Any<string>()).Returns("");
        var mockContent = new ContentViewModel("a", "r", "pathpath");
        var mockElement = new ElementViewModel("n", "sn",mockContent, "url","a", "d", "g", ElementDifficultyEnum.Easy);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(shellWrapper:mockShellWrapper, hybridSupportWrapper: mockHybridSupport, serviceProvider: mockServiceProvider);

        systemUnderTest.ShowElementContentAsync(mockElement);
        
        mockShellWrapper.Received().OpenPathAsync("pathpath");
    }
    private static Presentation.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(
        IAuthoringToolConfiguration? configuration = null, IBusinessLogic? businessLogic = null, IMapper? mapper = null, 
        ICachingMapper? cachingMapper = null, IServiceProvider? serviceProvider = null, 
        ILogger<Presentation.PresentationLogic.API.PresentationLogic>? logger = null, 
        IHybridSupportWrapper? hybridSupportWrapper = null, IShellWrapper? shellWrapper = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        mapper ??= Substitute.For<IMapper>();
        cachingMapper ??= Substitute.For<ICachingMapper>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        hybridSupportWrapper ??= Substitute.For<IHybridSupportWrapper>();
        shellWrapper ??= Substitute.For<IShellWrapper>();

        return new Presentation.PresentationLogic.API.PresentationLogic(configuration, businessLogic, mapper, cachingMapper,
            serviceProvider, logger, hybridSupportWrapper,shellWrapper);
    }
}