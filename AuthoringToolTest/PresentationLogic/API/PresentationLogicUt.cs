using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.API;

[TestFixture]
public class PresentationLogicUt
{
    [Test]
    public void PresentationLogic_Standard_AllPropertiesInitialized()
    {
        //Arrange
        var mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockWorldMapper = Substitute.For<ILearningWorldMapper>();
        var mockSpaceMapper = Substitute.For<ILearningSpaceMapper>();
        var mockElementMapper = Substitute.For<ILearningElementMapper>();
        var mockContentMapper = Substitute.For<ILearningContentMapper>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();

        //Act
        var systemUnderTest = CreateTestablePresentationLogic(mockConfiguration, mockBusinessLogic, mockWorldMapper,
            mockSpaceMapper, mockElementMapper,mockContentMapper, mockServiceProvider, mockLogger);
        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(mockBusinessLogic));
            Assert.That(systemUnderTest.WorldMapper, Is.EqualTo(mockWorldMapper));
            Assert.That(systemUnderTest.SpaceMapper, Is.EqualTo(mockSpaceMapper));
            Assert.That(systemUnderTest.ElementMapper, Is.EqualTo(mockElementMapper));
            Assert.That(systemUnderTest.ContentMapper, Is.EqualTo(mockContentMapper));
        });
    }

    [Test]
    public async Task PresentationLogic_ConstructBackup_CallsDialogManagerAndBusinessLogic()
    {
        //Arrange
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockDialogManager = Substitute.For<IElectronDialogManager>();
        mockDialogManager
            .ShowSaveAsDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns("supersecretfilepath");
        var viewModel = new LearningWorldViewModel("fo", "fo", "fo", "fo", "fo", "fo");
        var mockWorldMapper = Substitute.For<ILearningWorldMapper>();
        var entity = new AuthoringTool.Entities.LearningWorld("baba", "baba", "baba", "baba", "baba", "baba");
        mockWorldMapper.ToEntity(viewModel)
            .Returns(entity);
        var serviceProvider = new ServiceCollection();
        serviceProvider.Insert(0, new ServiceDescriptor(typeof(IElectronDialogManager), mockDialogManager));
        
        var systemUnderTest = CreateTestablePresentationLogic(null, mockBusinessLogic, mockWorldMapper,
            serviceProvider: serviceProvider.BuildServiceProvider());
        //Act
        await systemUnderTest.ConstructBackupAsync(viewModel);

        //Assert
        mockBusinessLogic.Received().ConstructBackup(entity, "supersecretfilepath.mbz");
    }

    #region Save/Load

    [Test]
    public void PresentationLogic_SaveLearningWorldAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveLearningWorldAsync(learningWorld));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void PresentationLogic_SaveLearningWorldAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveLearningWorldAsync(learningWorld));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task PresentationLogic_SaveLearningWorldAsync_CallsDialogManagerAndWorldMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockWorldMapper = Substitute.For<ILearningWorldMapper>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var entity = new AuthoringTool.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        mockWorldMapper.ToEntity(Arg.Any<LearningWorldViewModel>())
            .Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            worldMapper: mockWorldMapper, serviceProvider: mockServiceProvider);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);

        await mockDialogManger.Received().ShowSaveAsDialog("Save Learning World", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockWorldMapper.Received().ToEntity(learningWorld);
        mockBusinessLogic.Received().SaveLearningWorld(entity, filepath+".awf");
    }

    [Test]
    public void PresentationLogic_SaveLearningWorldAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowSaveAsDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveLearningWorldAsync(learningWorld));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }
    
    [Test]
    public void PresentationLogic_SaveLearningSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveLearningSpaceAsync(learningSpace));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void PresentationLogic_SaveLearningSpaceAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveLearningSpaceAsync(learningSpace));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task PresentationLogic_SaveLearningSpaceAsync_CallsDialogManagerAndSpaceMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockSpaceMapper = Substitute.For<ILearningSpaceMapper>();
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        var entity = new AuthoringTool.Entities.LearningSpace("f", "f", "f", "f", "f");
        mockSpaceMapper.ToEntity(Arg.Any<LearningSpaceViewModel>())
            .Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            spaceMapper: mockSpaceMapper, serviceProvider: mockServiceProvider);

        await systemUnderTest.SaveLearningSpaceAsync(learningSpace);

        await mockDialogManger.Received().ShowSaveAsDialog("Save Learning Space", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockSpaceMapper.Received().ToEntity(learningSpace);
        mockBusinessLogic.Received().SaveLearningSpace(entity, filepath+".asf");
    }

    [Test]
    public void PresentationLogic_SaveLearningSpaceAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowSaveAsDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);
        var learningSpace = new LearningSpaceViewModel("f", "f", "f", "f", "f");

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveLearningSpaceAsync(learningSpace));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }
    
    [Test]
    public void PresentationLogic_SaveLearningElementAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);
        var learningElement = new LearningElementViewModel("f", "f", null, null, "f", "f", "f",LearningElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveLearningElementAsync(learningElement));
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void PresentationLogic_SaveLearningElementAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var learningElement = new LearningElementViewModel("f", "f", null, null,"f", "f", "f",LearningElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await systemUnderTest.SaveLearningElementAsync(learningElement));
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }

    [Test]
    public async Task PresentationLogic_SaveLearningElementAsync_CallsDialogManagerAndElementMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockElementMapper = Substitute.For<ILearningElementMapper>();
        var learningElement = new LearningElementViewModel("f", "f", null,  null,"f", "f", "f",LearningElementDifficultyEnum.Easy);
        var entity = new AuthoringTool.Entities.LearningElement("f", "f", "f", null,"f", "f", "f",LearningElementDifficultyEnum.Easy);
        mockElementMapper.ToEntity(Arg.Any<LearningElementViewModel>())
            .Returns(entity);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowSaveAsDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            elementMapper: mockElementMapper, serviceProvider: mockServiceProvider);

        await systemUnderTest.SaveLearningElementAsync(learningElement);

        await mockDialogManger.Received().ShowSaveAsDialog("Save Learning Element", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockElementMapper.Received().ToEntity(learningElement);
        mockBusinessLogic.Received().SaveLearningElement(entity, filepath+".aef");
    }

    [Test]
    public void PresentationLogic_SaveLearningElementAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowSaveAsDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);
        var learningElement = new LearningElementViewModel("f", "f", null, null, "f", "f", "f",LearningElementDifficultyEnum.Easy);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.SaveLearningElementAsync(learningElement));
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Save as dialog cancelled by user");
    }

    [Test]
    public void PresentationLogic_LoadLearningWorldAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadLearningWorldAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void PresentationLogic_LoadLearningWorldAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);

        var ex =Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadLearningWorldAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task PresentationLogic_LoadLearningWorldAsync_CallsDialogManagerAndElementMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockWorldMapper = Substitute.For<ILearningWorldMapper>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var entity = new AuthoringTool.Entities.LearningWorld("f", "f", "f", "f", "f", "f");
        mockWorldMapper.ToViewModel(entity).Returns(learningWorld);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningWorld(filepath+".awf").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            worldMapper: mockWorldMapper, serviceProvider: mockServiceProvider);

        var actualWorld = await systemUnderTest.LoadLearningWorldAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialog("Load Learning World", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningWorld(filepath + ".awf");
        mockWorldMapper.Received().ToViewModel(entity);
        
        Assert.That(actualWorld, Is.EqualTo(learningWorld));
    }

    [Test]
    public void PresentationLogic_LoadLearningWorldAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadLearningWorldAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    [Test]
    public void PresentationLogic_LoadLearningSpaceAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadLearningSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void PresentationLogic_LoadLearningSpaceAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadLearningSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task PresentationLogic_LoadLearningSpaceAsync_CallsDialogManagerAndSpaceMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockSpaceMapper = Substitute.For<ILearningSpaceMapper>();
        var learningSpace = new LearningSpaceViewModel("f", "f", "", "f", "f" );
        var entity = new AuthoringTool.Entities.LearningSpace("f", "f", "f", "f", "f");
        mockSpaceMapper.ToViewModel(entity).Returns(learningSpace);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningSpace(filepath+".asf").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            spaceMapper: mockSpaceMapper, serviceProvider: mockServiceProvider);

        var actualSpace = await systemUnderTest.LoadLearningSpaceAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialog("Load Learning Space", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningSpace(filepath + ".asf");
        mockSpaceMapper.Received().ToViewModel(entity);
        
        Assert.That(actualSpace, Is.EqualTo(learningSpace));
    }

    [Test]
    public void PresentationLogic_LoadLearningSpaceAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadLearningSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    
    [Test]
    public void PresentationLogic_LoadLearningElementAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }

    [Test]
    public void PresentationLogic_LoadLearningElementAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task PresentationLogic_LoadLearningElementAsync_CallsDialogManagerAndElementMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockElementMapper = Substitute.For<ILearningElementMapper>();
        var learningElement = new LearningElementViewModel("f", "f", null, null, "f", "f", "f",LearningElementDifficultyEnum.Easy );
        var entity = new AuthoringTool.Entities.LearningElement("f", "f", "f", null, "f", "f", "f",LearningElementDifficultyEnum.Easy);
        mockElementMapper.ToViewModel(entity).Returns(learningElement);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningElement(filepath+".aef").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            elementMapper: mockElementMapper, serviceProvider: mockServiceProvider);

        var actualElement = await systemUnderTest.LoadLearningElementAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialog("Load Learning Element", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningElement(filepath + ".aef");
        mockElementMapper.Received().ToViewModel(entity);
        
        Assert.That(actualElement, Is.EqualTo(learningElement));
    }

    [Test]
    public void PresentationLogic_LoadLearningElementAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    [Test]
    public void PresentationLogic_LoadImageAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadImageAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }
    
    [Test]
    public void PresentationLogic_LoadImageAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadImageAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task PresentationLogic_LoadImageAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockContentMapper = Substitute.For<ILearningContentMapper>();
        var learningContent = new LearningContentViewModel("f", ".png", new byte[] { 0x00, 0x00, 0x00, 0x01 });
        var entity = new LearningContent("f", ".png", new byte[] { 0x00, 0x00, 0x00, 0x01 });
        mockContentMapper.ToViewModel(entity).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath).Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            contentMapper: mockContentMapper, serviceProvider: mockServiceProvider);

        var loadedContent = await systemUnderTest.LoadImageAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialog("Load image", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath);
        mockContentMapper.Received().ToViewModel(entity);
        
        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }
    
    [Test]
    public void PresentationLogic_LoadImageAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadImageAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }


    [Test]
    public void PresentationLogic_LoadVideoAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadVideoAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }
    
    [Test]
    public void PresentationLogic_LoadVideoAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadVideoAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task PresentationLogic_LoadVideoAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockContentMapper = Substitute.For<ILearningContentMapper>();
        var learningContent = new LearningContentViewModel("f", ".mp4", new byte[] { 0x01, 0x00, 0x00, 0x01 });
        var entity = new LearningContent("f", ".mp4", new byte[] { 0x01, 0x00, 0x00, 0x01 });
        mockContentMapper.ToViewModel(entity).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath + ".mp4").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            contentMapper: mockContentMapper, serviceProvider: mockServiceProvider);

        var loadedContent = await systemUnderTest.LoadVideoAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialog("Load video", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath + ".mp4");
        mockContentMapper.Received().ToViewModel(entity);
        
        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }
    
    [Test]
    public void PresentationLogic_LoadVideoAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadVideoAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    [Test]
    public void PresentationLogic_LoadH5pAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadH5pAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }
    
    [Test]
    public void PresentationLogic_LoadH5pAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadH5pAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task PresentationLogic_LoadH5pAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockContentMapper = Substitute.For<ILearningContentMapper>();
        var learningContent = new LearningContentViewModel("f", ".h5p", new byte[] { 0x01, 0x01, 0x00, 0x01 });
        var entity = new LearningContent("f", ".h5p", new byte[] { 0x01, 0x01, 0x00, 0x01 });
        mockContentMapper.ToViewModel(entity).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath + ".h5p").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            contentMapper: mockContentMapper, serviceProvider: mockServiceProvider);

        var loadedContent = await systemUnderTest.LoadH5pAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialog("Load h5p", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath + ".h5p");
        mockContentMapper.Received().ToViewModel(entity);
        
        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }
    
    [Test]
    public void PresentationLogic_LoadH5pAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadH5pAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    
    
    [Test]
    public void PresentationLogic_LoadPdfAsync_ThrowsNYIExceptionWhenNotRunningInElectron()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(false);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.LoadPdfAsync());
        Assert.That(ex!.Message, Is.EqualTo("Browser upload/download not yet implemented"));
    }
    
    [Test]
    public void PresentationLogic_LoadPdfAsync_ThrowsExceptionWhenNoDialogManagerInServiceProvider()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(null);

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.LoadPdfAsync());
        Assert.That(ex!.Message, Is.EqualTo("dialogManager received from DI unexpectedly null"));
    }
    
    [Test]
    public async Task PresentationLogic_LoadPdfAsync_CallsDialogManagerAndContentMapperAndBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockContentMapper = Substitute.For<ILearningContentMapper>();
        var learningContent = new LearningContentViewModel("f", ".pdf", new byte[] { 0x01, 0x01, 0x01, 0x01 });
        var entity = new LearningContent("f", ".pdf", new byte[] { 0x01, 0x01, 0x01, 0x01 });
        mockContentMapper.ToViewModel(entity).Returns(learningContent);
        const string filepath = "foobar";
        var mockDialogManger = Substitute.For<IElectronDialogManager>();
        mockDialogManger
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Returns(filepath);
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        mockServiceProvider.GetService(typeof(IElectronDialogManager)).Returns(mockDialogManger);
        mockBusinessLogic.LoadLearningContent(filepath + ".pdf").Returns(entity);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic,
            contentMapper: mockContentMapper, serviceProvider: mockServiceProvider);

        var loadedContent = await systemUnderTest.LoadPdfAsync();

        await mockDialogManger.Received()
            .ShowOpenFileDialog("Load pdf", null, Arg.Any<IEnumerable<FileFilterProxy>?>());
        mockBusinessLogic.Received().LoadLearningContent(filepath + ".pdf");
        mockContentMapper.Received().ToViewModel(entity);
        
        Assert.That(loadedContent, Is.EqualTo(learningContent));
    }
    
    [Test]
    public void PresentationLogic_LoadPdfAsync_LogsAndRethrowsDialogCancelledException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.RunningElectron.Returns(true);
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockElectronDialogManager = Substitute.For<IElectronDialogManager>();
        mockElectronDialogManager
            .ShowOpenFileDialog(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<IEnumerable<FileFilterProxy>?>())
            .Throws(new OperationCanceledException("bububaba"));
        mockServiceProvider.GetService(typeof(IElectronDialogManager))
            .Returns(mockElectronDialogManager);

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, logger: mockLogger,
            serviceProvider: mockServiceProvider);

        var ex = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.LoadPdfAsync());
        Assert.That(ex!.Message, Is.EqualTo("bububaba"));
        mockLogger.Received().LogInformation("Load dialog cancelled by user");
    }
    #endregion
    
    #region LoadFromStream

    [Test]
    public void PresentationLogic_LoadLearningWorldViewModelFromStream_ReturnsLearningWorld()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningWorld = new AuthoringTool.Entities.LearningWorld("n", "sn", "a", "l", "d", "g");
        mockBusinessLogic.LoadLearningWorldFromStream(Arg.Any<Stream>()).Returns(mockLearningWorld);
        var mockLearningWorldViewModel = new LearningWorldViewModel("n", "sn", "a", "l", "d", "g");
        var mockWorldMapper = Substitute.For<ILearningWorldMapper>();
        mockWorldMapper.ToViewModel(Arg.Any<AuthoringTool.Entities.LearningWorld>())
            .Returns(mockLearningWorldViewModel);
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, worldMapper: mockWorldMapper);

        var result = systemUnderTest.LoadLearningWorldViewModelFromStream(stream);

        mockBusinessLogic.Received().LoadLearningWorldFromStream(stream);
        mockWorldMapper.Received().ToViewModel(mockLearningWorld);
        Assert.That(result, Is.EqualTo(mockLearningWorldViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningWorldViewModelFromStream_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningWorldFromStream(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningWorldViewModelFromStream(stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    [Test]
    public void PresentationLogic_LoadLearningSpaceViewModelFromStream_ReturnsLearningSpace()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningSpace = new AuthoringTool.Entities.LearningSpace("n", "sn", "a", "d", "g");
        mockBusinessLogic.LoadLearningSpaceFromStream(Arg.Any<Stream>()).Returns(mockLearningSpace);
        var mockLearningSpaceViewModel = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        var mockSpaceMapper = Substitute.For<ILearningSpaceMapper>();
        mockSpaceMapper.ToViewModel(Arg.Any<AuthoringTool.Entities.LearningSpace>())
            .Returns(mockLearningSpaceViewModel);
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, spaceMapper: mockSpaceMapper);

        var result = systemUnderTest.LoadLearningSpaceViewModelFromStream(stream);

        mockBusinessLogic.Received().LoadLearningSpaceFromStream(stream);
        mockSpaceMapper.Received().ToViewModel(mockLearningSpace);
        Assert.That(result, Is.EqualTo(mockLearningSpaceViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningSpaceViewModelFromStream_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningSpaceFromStream(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningSpaceViewModelFromStream(stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    [Test]
    public void PresentationLogic_LoadLearningElementViewModelFromStream_ReturnsLearningElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningElement = new AuthoringTool.Entities.LearningElement("n", "sn", "pn",null, "a", "d", "g",LearningElementDifficultyEnum.Easy);
        mockBusinessLogic.LoadLearningElementFromStream(Arg.Any<Stream>()).Returns(mockLearningElement);
        var mockLearningContent = new LearningContentViewModel("n", "t", Array.Empty<byte>());
        var mockLearningElementViewModel = new LearningElementViewModel("n", "sn", null, mockLearningContent, "a", "d", "g",LearningElementDifficultyEnum.Easy);
        var mockElementMapper = Substitute.For<ILearningElementMapper>();
        mockElementMapper.ToViewModel(Arg.Any<AuthoringTool.Entities.LearningElement>())
            .Returns(mockLearningElementViewModel);
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, elementMapper: mockElementMapper);

        var result = systemUnderTest.LoadLearningElementViewModelFromStream(stream);

        mockBusinessLogic.Received().LoadLearningElementFromStream(stream);
        mockElementMapper.Received().ToViewModel(mockLearningElement);
        Assert.That(result, Is.EqualTo(mockLearningElementViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningElementViewModelFromStream_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningElementFromStream(Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningElementViewModelFromStream(stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }

    [Test]
    public void PresentationLogic_LoadLearningContentViewModelFromStream_ReturnsLearningContent()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var mockLearningContent = new LearningContent("n", "t", Array.Empty<byte>());
        mockBusinessLogic.LoadLearningContentFromStream(Arg.Any<string>(), Arg.Any<Stream>()).Returns(mockLearningContent);
        var mockLearningContentViewModel = new LearningContentViewModel("n", "t", Array.Empty<byte>());
        var mockContentMapper = Substitute.For<ILearningContentMapper>();
        mockContentMapper.ToViewModel(Arg.Any<LearningContent>())
            .Returns(mockLearningContentViewModel);
        var filename = "test.png";
        var stream = Substitute.For<Stream>();

        var systemUnderTest =
            CreateTestablePresentationLogic(businessLogic: mockBusinessLogic, contentMapper: mockContentMapper);

        var result = systemUnderTest.LoadLearningContentViewModelFromStream(filename, stream);

        mockBusinessLogic.Received().LoadLearningContentFromStream(filename, stream);
        mockContentMapper.Received().ToViewModel(mockLearningContent);
        Assert.That(result, Is.EqualTo(mockLearningContentViewModel));
    }

    [Test]
    public void PresentationLogic_LoadLearningContentViewModelFromStream_CatchesException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningContentFromStream(Arg.Any<string>(), Arg.Any<Stream>()).Throws(new Exception("Exception"));
        var filename = "test.png";
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateTestablePresentationLogic(businessLogic: mockBusinessLogic);

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LoadLearningContentViewModelFromStream(filename, stream));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex?.Message, Is.EqualTo("Exception"));
    }
    
    #endregion

    private static AuthoringTool.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(
        IAuthoringToolConfiguration? configuration = null, IBusinessLogic? businessLogic = null,
        ILearningWorldMapper? worldMapper = null, ILearningSpaceMapper? spaceMapper = null,
        ILearningElementMapper? elementMapper = null, ILearningContentMapper? contentMapper = null,
        IServiceProvider? serviceProvider = null, ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>? logger = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        worldMapper ??= Substitute.For<ILearningWorldMapper>();
        spaceMapper ??= Substitute.For<ILearningSpaceMapper>();
        elementMapper ??= Substitute.For<ILearningElementMapper>();
        contentMapper ??= Substitute.For<ILearningContentMapper>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        return new AuthoringTool.PresentationLogic.API.PresentationLogic(configuration, businessLogic,
            worldMapper, spaceMapper, elementMapper, contentMapper, serviceProvider, logger);
    }
}