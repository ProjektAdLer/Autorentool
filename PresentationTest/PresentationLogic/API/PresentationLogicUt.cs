using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using ElectronWrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
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
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockLogger = Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();

        //Act
        var systemUnderTest = CreateTestablePresentationLogic(mockConfiguration, mockBusinessLogic, mockMapper, mockServiceProvider, mockLogger);
        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(mockBusinessLogic));
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(mockMapper));
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
    public void CreateLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var workspaceVm = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        
        systemUnderTest.CreateLearningWorld(workspaceVm, "a","b","c","d","e", "f");

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }
    
    [Test]
    public void EditLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        
        systemUnderTest.EditLearningWorld(learningWorldVm,"a","b","c","d","e", "f");

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }

    [Test]
    public void DeleteLearningWorld_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        workspaceVm.AddLearningWorld(worldVm);
        var mockMapper = Substitute.For<IMapper>();
        var worldEntity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        var workspaceEntity = new BusinessLogic.Entities.AuthoringToolWorkspace(null,
            new List<BusinessLogic.Entities.LearningWorld>() {worldEntity});
        mockMapper.Map<BusinessLogic.Entities.AuthoringToolWorkspace>(Arg.Any<AuthoringToolWorkspaceViewModel>())
            .Returns(workspaceEntity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        systemUnderTest.DeleteLearningWorld(workspaceVm, worldVm.Name);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }

    [Test]
    public void CreateLearningSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        
        systemUnderTest.CreateLearningSpace(learningWorldVm, "a","b","c","d","e");

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }
    
    [Test]
    public void EditLearningSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var learningSpaceVm = new LearningSpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        
        systemUnderTest.EditLearningSpace(learningSpaceVm,"a","b","c","d","e");

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }

    [Test]
    public void DeleteLearningSpace_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var learningSpaceVm = new LearningSpaceViewModel("g", "g", "g", "g", "g");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        
        systemUnderTest.DeleteLearningSpace(learningWorldVm, learningSpaceVm);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }
    
    [Test]
    public void CreateLearningElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        
        systemUnderTest.CreateLearningElement(learningWorldVm, "a","b", ElementTypeEnum.Activation, ContentTypeEnum.Image, null!,"c","d","e", LearningElementDifficultyEnum.Easy, 0);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }
    
    [Test]
    public void EditLearningElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var elementParentVm = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var learningElementVm = new LearningElementViewModel("f", "f", null!, "f", "f", "f", LearningElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        
        systemUnderTest.EditLearningElement(elementParentVm, learningElementVm, "a","b","c","d","e",LearningElementDifficultyEnum.Easy,0);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }
    
    [Test]
    public void DeleteLearningElement_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var learningWorldVm = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var learningElementVm = new LearningElementViewModel("g", "g", null!, "g", "g", "g",LearningElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);
        
        systemUnderTest.DeleteLearningElement(learningWorldVm, learningElementVm);

        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
    }

    #region Save/Load

    [Test]
    public void SaveLearningWorldAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveLearningWorldAsync(learningWorld));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task SaveLearningWorldAsync_CallsDialogManagerAndWorldMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, hybridSupportWrapper: mockHybridSupport, serviceProvider: mockServiceProvider);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);

        await mockDialogManger.Received().ShowSaveAsDialogAsync("Save Learning World", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.LearningWorld>(learningWorld);
        mockBusinessLogic.Received().SaveLearningWorld(entity, filepath+".awf");
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveLearningWorldAsync(learningWorld));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }
    
    [Test]
    public void SaveLearningSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", "f", "f");

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
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveLearningSpaceAsync(learningSpace));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task SaveLearningSpaceAsync_CallsDialogManagerAndSpaceMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        var entity = new BusinessLogic.Entities.LearningSpace("f", "f", "f", "f", "f");
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<LearningSpaceViewModel>()).Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.SaveLearningSpaceAsync(learningSpace);

        await mockDialogManger.Received().ShowSaveAsDialogAsync("Save Learning Space", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.LearningSpace>(learningSpace);
        mockBusinessLogic.Received().SaveLearningSpace(entity, filepath+".asf");
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
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveLearningSpaceAsync(learningSpace));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }
    
    [Test]
    public void SaveLearningElementAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);
        var learningElement = new LearningElementViewModel("f", "f", null!, "f", "f", "f",LearningElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

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
        var learningElement = new LearningElementViewModel("f", "f", null!,"f", "f", "f",LearningElementDifficultyEnum.Easy);

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
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningElement = new LearningElementViewModel("f", "f",  null!,"f", "f", "f",LearningElementDifficultyEnum.Easy);
        var entity = new BusinessLogic.Entities.LearningElement("f", "f", null!,"f", "f", "f", LearningElementDifficultyEnum.Easy);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>()).Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.SaveLearningElementAsync(learningElement);

        await mockDialogManger.Received().ShowSaveAsDialogAsync("Save Learning Element", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockMapper.Received().Map<BusinessLogic.Entities.LearningElement>(learningElement);
        mockBusinessLogic.Received().SaveLearningElement(entity, filepath+".aef");
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
        var learningElement = new LearningElementViewModel("f", "f", null!, "f", "f", "f",LearningElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveLearningElementAsync(learningElement));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }

    [Test]
    public void LoadLearningWorldAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadLearningWorldAsync());
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

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider,
                hybridSupportWrapper: mockHybridSupport);

        var ex =Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadLearningWorldAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task LoadLearningWorldAsync_CallsDialogManagerAndElementMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var entity = new BusinessLogic.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        mockMapper.Map<LearningWorldViewModel>(Arg.Any<BusinessLogic.Entities.LearningWorld>()).Returns(learningWorld);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningWorld(filepath+".awf").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var actualWorld = await systemUnderTest.LoadLearningWorldAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Learning World", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningWorld(filepath + ".awf");
        mockMapper.Received().Map<LearningWorldViewModel>(entity);
        
        Assert.That(actualWorld, Is.EqualTo(learningWorld));
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadLearningWorldAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
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

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadLearningSpaceAsync(mockLearningWorldViewModel));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task LoadLearningSpaceAsync_CallsDialogManagerAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        var mockLearningWorldViewModel = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.LoadLearningSpaceAsync(mockLearningWorldViewModel);

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Learning Space", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadLearningSpaceAsync(mockLearningWorldViewModel));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    
    [Test]
    public void LoadLearningElementAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("n", "sn", "a", "l", "d");
        mockHybridSupport.IsElectronActive.Returns(false);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadLearningElementAsync(mockLearningSpaceViewModel));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void LoadLearningElementAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("sn", "a", "l", "d", "g");
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadLearningElementAsync(mockLearningSpaceViewModel));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task LoadLearningElementAsync_CallsDialogManagerAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        var mockLearningWorldViewModel = new LearningWorldViewModel("a", "l", "d", "g","h", "i");
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        await systemUnderTest.LoadLearningElementAsync(mockLearningWorldViewModel);
        
        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load Learning Element", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().ExecuteCommand(Arg.Any<ICommand>());
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
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("n", "sn", "a", "l", "d");
        mockElectronDialogManager
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadLearningElementAsync(mockLearningSpaceViewModel));
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
        var learningContent = new LearningContentViewModel("f", ".png", new byte[] { 0x00, 0x00, 0x00, 0x01 });
        var entity = new LearningContent("f", ".png", new byte[] { 0x00, 0x00, 0x00, 0x01 });
        mockMapper.Map<LearningContentViewModel>(Arg.Any<LearningContent>()).Returns(learningContent);
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
        mockMapper.Received().Map<LearningContentViewModel>(entity);
        
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
        var learningContent = new LearningContentViewModel("f", ".mp4", new byte[] { 0x01, 0x00, 0x00, 0x01 });
        var entity = new LearningContent("f", ".mp4", new byte[] { 0x01, 0x00, 0x00, 0x01 });
        mockMapper.Map<LearningContentViewModel>(Arg.Any<LearningContent>()).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialogAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath + ".mp4").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            mapper: mockMapper, serviceProvider: mockServiceProvider, hybridSupportWrapper:mockHybridSupport);

        var loadedContent = await systemUnderTest.LoadVideoAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load video", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath + ".mp4");
        mockMapper.Received().Map<LearningContentViewModel>(entity);
        
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
            await systemUnderTest.LoadH5pAsync());
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

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadH5pAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task LoadH5pAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockHybridSupport = Substitute.For<IHybridSupportWrapper>();
        mockHybridSupport.IsElectronActive.Returns(true);
        var mockMapper = Substitute.For<IMapper>();
        var learningContent = new LearningContentViewModel("f", ".h5p", new byte[] { 0x01, 0x01, 0x00, 0x01 });
        var entity = new LearningContent("f", ".h5p", new byte[] { 0x01, 0x01, 0x00, 0x01 });
        mockMapper.Map<LearningContentViewModel>(Arg.Any<LearningContent>()).Returns(learningContent);
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

        var loadedContent = await systemUnderTest.LoadH5pAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialogAsync("Load h5p", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath + ".h5p");
        mockMapper.Received().Map<LearningContentViewModel>(entity);
        
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadH5pAsync());
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
        var learningContent = new LearningContentViewModel("f", ".pdf", new byte[] { 0x01, 0x01, 0x01, 0x01 });
        var entity = new LearningContent("f", ".pdf", new byte[] { 0x01, 0x01, 0x01, 0x01 });
        mockMapper.Map<LearningContentViewModel>(Arg.Any<LearningContent>()).Returns(learningContent);
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
        mockMapper.Received().Map<LearningContentViewModel>(entity);
        
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

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider, hybridSupportWrapper: mockHybridSupport);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadPdfAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    #endregion
    
    #region Load

    [Test]
    public void PresentationLogic_LoadLearningWorldViewModel_ReturnsLearningWorld()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningWorld = new BusinessLogic.Entities.LearningWorld("n", "sn", "a", "l", "d", "g");
        mockBusinessLogic.LoadLearningWorld(Arg.Any<Stream>()).Returns(mockLearningWorld);
        var mockLearningWorldViewModel = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningWorldViewModel>(Arg.Any<BusinessLogic.Entities.LearningWorld>())
            .Returns(mockLearningWorldViewModel);
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        var result = systemUnderTest.LoadLearningWorldViewModel(stream);

        mockBusinessLogic.Received().LoadLearningWorld(stream);
        mockMapper.Received().Map<LearningWorldViewModel>(mockLearningWorld);
        Assert.That(result, Is.EqualTo(mockLearningWorldViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningWorldViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningWorld(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningWorldViewModel(stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    [Test]
    public void PresentationLogic_LoadLearningSpaceViewModel_ReturnsLearningSpace()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningSpace = new BusinessLogic.Entities.LearningSpace("n", "sn", "a", "d", "g");
        mockBusinessLogic.LoadLearningSpace(Arg.Any<Stream>()).Returns(mockLearningSpace);
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningSpaceViewModel>(Arg.Any<BusinessLogic.Entities.LearningSpace>())
            .Returns(mockLearningSpaceViewModel);
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        var result = systemUnderTest.LoadLearningSpaceViewModel(stream);

        mockBusinessLogic.Received().LoadLearningSpace(stream);
        mockMapper.Received().Map<LearningSpaceViewModel>(mockLearningSpace);
        Assert.That(result, Is.EqualTo(mockLearningSpaceViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningSpaceViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningSpace(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningSpaceViewModel(stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    [Test]
    public void PresentationLogic_LoadLearningElementViewModel_ReturnsLearningElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningElement = new BusinessLogic.Entities.LearningElement("n", "sn",null!, "a", "d", "g", LearningElementDifficultyEnum.Easy);
        mockBusinessLogic.LoadLearningElement(Arg.Any<Stream>()).Returns(mockLearningElement);
        var mockLearningContent = new LearningContentViewModel("n", "t", Array.Empty<byte>());
        var mockLearningElementViewModel = new LearningElementViewModel("n", "sn", mockLearningContent, "a", "d", "g",LearningElementDifficultyEnum.Easy);
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningElementViewModel>(Arg.Any<BusinessLogic.Entities.LearningElement>())
            .Returns(mockLearningElementViewModel);
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        var result = systemUnderTest.LoadLearningElementViewModel(stream);

        mockBusinessLogic.Received().LoadLearningElement(stream);
        mockMapper.Received().Map<LearningElementViewModel>(mockLearningElement);
        Assert.That(result, Is.EqualTo(mockLearningElementViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningElementViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningElement(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningElementViewModel(stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }

    [Test]
    public void PresentationLogic_LoadLearningContentViewModel_ReturnsLearningContent()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningContent = new LearningContent("n", "t", Array.Empty<byte>());
        mockBusinessLogic.LoadLearningContent(Arg.Any<string>(), Arg.Any<Stream>()).Returns(mockLearningContent);
        var mockLearningContentViewModel = new LearningContentViewModel("n", "t", Array.Empty<byte>());
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningContentViewModel>(Arg.Any<LearningContent>())
            .Returns(mockLearningContentViewModel);
        var filename = "test.png";
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, mapper: mockMapper);

        var result = systemUnderTest.LoadLearningContentViewModel(filename, stream);

        mockBusinessLogic.Received().LoadLearningContent(filename, stream);
        mockMapper.Received().Map<LearningContentViewModel>(mockLearningContent);
        Assert.That(result, Is.EqualTo(mockLearningContentViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningContentViewModel_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningContent(Arg.Any<string>(), Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var filename = "test.png";
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningContentViewModel(filename, stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    #endregion

    private static Presentation.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(
        IAuthoringToolConfiguration? configuration = null, IBusinessLogic? businessLogic = null, IMapper? mapper = null,
        IServiceProvider? serviceProvider = null, ILogger<Presentation.PresentationLogic.API.PresentationLogic>? logger = null,
        IHybridSupportWrapper? hybridSupportWrapper = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        mapper ??= Substitute.For<IMapper>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<Presentation.PresentationLogic.API.PresentationLogic>>();
        hybridSupportWrapper ??= Substitute.For<IHybridSupportWrapper>();

        return new Presentation.PresentationLogic.API.PresentationLogic(configuration, businessLogic, mapper,
            serviceProvider, logger, hybridSupportWrapper);
    }
}