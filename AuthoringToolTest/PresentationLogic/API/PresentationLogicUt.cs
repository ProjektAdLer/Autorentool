using System;
using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.EntityMapping;
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

        //Act
        var systemUnderTest = CreateTestablePresentationLogic(mockConfiguration, mockBusinessLogic);

        //Assert
        Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
        Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(mockBusinessLogic));
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

    private static AuthoringTool.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(
        IAuthoringToolConfiguration? fakeConfiguration = null, IBusinessLogic? fakeBusinessLogic = null,
        ILearningWorldMapper? worldMapper = null, ILearningSpaceMapper? spaceMapper = null, ILearningElementMapper? elementMapper = null,
        IServiceProvider? serviceProvider = null,
        ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>? logger = null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeBusinessLogic ??= Substitute.For<IBusinessLogic>();
        worldMapper ??= Substitute.For<ILearningWorldMapper>();
        spaceMapper ??= Substitute.For<ILearningSpaceMapper>();
        elementMapper ??= Substitute.For<ILearningElementMapper>();
        serviceProvider ??= Substitute.For<IServiceProvider>();
        logger ??= Substitute.For<ILogger<AuthoringTool.PresentationLogic.API.PresentationLogic>>();
        return new AuthoringTool.PresentationLogic.API.PresentationLogic(fakeConfiguration, fakeBusinessLogic,
            worldMapper, spaceMapper, elementMapper, serviceProvider, logger);
    }
}