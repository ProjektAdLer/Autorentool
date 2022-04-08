using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningWorld;
using Microsoft.Extensions.Logging;
using NSubstitute;
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
        var mockServiceProvider = Substitute.For<IServiceProvider>();
        var mockLogger = Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();

        //Act
        var systemUnderTest = CreateTestablePresentationLogic(mockConfiguration, mockBusinessLogic, mockWorldMapper,
            mockSpaceMapper, mockElementMapper, mockServiceProvider, mockLogger);
        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(mockBusinessLogic));
            Assert.That(systemUnderTest.WorldMapper, Is.EqualTo(mockWorldMapper));
            Assert.That(systemUnderTest.SpaceMapper, Is.EqualTo(mockSpaceMapper));
            Assert.That(systemUnderTest.ElementMapper, Is.EqualTo(mockElementMapper));
        });
    }

    [Test]
    public void PresentationLogic_ConstructBackup_BackupFile()
    {
        //Arrange
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var systemUnderTest = CreateTestablePresentationLogic(null, mockBusinessLogic);

        //Act
        systemUnderTest.ConstructBackup();

        //Assert
        mockBusinessLogic.Received().ConstructBackup();
    }

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

        var ex = Assert.ThrowsAsync<Exception>(async () =>
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

        await mockDialogManger.Received().ShowSaveAsDialog("Save learning world", null, Arg.Any<IEnumerable<FileFilterProxy>>());
        mockWorldMapper.Received().ToEntity(learningWorld);
        mockBusinessLogic.Received().SaveLearningWorld(entity, filepath+".awf");
    }

    private static AuthoringTool.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(
        IAuthoringToolConfiguration? configuration = null, IBusinessLogic? businessLogic = null,
        ILearningWorldMapper? worldMapper = null, ILearningSpaceMapper? spaceMapper = null,
        ILearningElementMapper? elementMapper = null, IServiceProvider? serviceProvider = null,
        ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>? logger = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        worldMapper ??= Substitute.For<ILearningWorldMapper>();
        spaceMapper ??= Substitute.For<ILearningSpaceMapper>();
        elementMapper ??= Substitute.For<ILearningElementMapper>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        return new AuthoringTool.PresentationLogic.API.PresentationLogic(configuration, businessLogic,
            worldMapper, spaceMapper, elementMapper, serviceProvider, logger);
    }
}