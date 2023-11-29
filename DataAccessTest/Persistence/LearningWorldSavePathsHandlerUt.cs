using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using DataAccess.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace DataAccessTest.Persistence;

[TestFixture]
public class LearningWorldSavePathsHandlerUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var logger = Substitute.For<ILogger<LearningWorldSavePathsHandler>>();
        var fileSystem = Substitute.For<IFileSystem>();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(logger, fileSystem);

        Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
        Assert.That(systemUnderTest.FileSystem, Is.EqualTo(fileSystem));
    }


    private static LearningWorldSavePathsHandler CreateTestableLearningWorldSavePathsHandler(
        ILogger<LearningWorldSavePathsHandler>? logger = null,
        IFileSystem? fileSystem = null)
    {
        logger ??= Substitute.For<ILogger<LearningWorldSavePathsHandler>>();
        fileSystem ??= new MockFileSystem();
        return new LearningWorldSavePathsHandler(logger, fileSystem);
    }
}