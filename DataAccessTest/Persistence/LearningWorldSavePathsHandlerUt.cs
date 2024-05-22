using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using DataAccess.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared;
using Shared.Configuration;

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

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
            Assert.That(systemUnderTest.FileSystem, Is.EqualTo(fileSystem));
        });
    }

    [Test]
    // ANF-ID: [ASE5]
    public void GetSavedLearningWorldPaths_ReturnsAllWorldFiles()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { ApplicationPaths.SavedWorldsFolder + "/world1.awf", new MockFileData("world1") },
            { ApplicationPaths.SavedWorldsFolder + "/world2.awf", new MockFileData("world2") },
            { ApplicationPaths.SavedWorldsFolder + "/world3.txt", new MockFileData("world3") },
        });
        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var result = systemUnderTest.GetSavedLearningWorldPaths();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result, Has.All.Matches<IFileInfo>(file => file.Name.EndsWith(FileEndings.WorldFileEndingWithDot)));
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