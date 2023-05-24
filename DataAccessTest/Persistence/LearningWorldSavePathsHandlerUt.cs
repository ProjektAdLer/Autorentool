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
    private static string LearningWorldSavePathsFolderPath => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AdLerAuthoring", "SavedWorlds");

    private static string SavedWorldPathsFilePath => Path.Join(LearningWorldSavePathsFolderPath, "SavedWorlds.xml");

    private static string XmlContentStart =>
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<ArrayOfSavedLearningWorldPath " +
        "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
        "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";

    private static string XmlContentEnd => "</ArrayOfSavedLearningWorldPath>";

    #region Constructor

    [Test]
    public void Standard_FolderAndFileExist_EmptyFile()
    {
        var fileSystem = new MockFileSystem();

        CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);
        Assert.Multiple(() =>
        {
            Assert.That(fileSystem.Directory.Exists(LearningWorldSavePathsFolderPath), Is.True);
            Assert.That(fileSystem.File.Exists(SavedWorldPathsFilePath), Is.True);
        });
        var reader = new StreamReader(fileSystem.File.OpenRead(SavedWorldPathsFilePath));
        var content = reader.ReadToEnd();
        Assert.That(content, Is.EqualTo(XmlContentStart.Remove(XmlContentStart.Length - 1) + " />"));
    }

    [Test]
    public void Standard_FolderAndFileExist_WithContent()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(SavedWorldPathsFilePath, new MockFileData(XmlContentStart + XmlContentEnd));

        CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);
        Assert.Multiple(() =>
        {
            Assert.That(fileSystem.Directory.Exists(LearningWorldSavePathsFolderPath), Is.True);
            Assert.That(fileSystem.File.Exists(SavedWorldPathsFilePath), Is.True);
        });
        var reader = new StreamReader(fileSystem.File.OpenRead(SavedWorldPathsFilePath));
        var content = reader.ReadToEnd();
        Assert.That(content, Is.EqualTo(XmlContentStart + XmlContentEnd));
    }

    [Test]
    public void Standard_FolderAndFileExist_WithContentAndOneEntry()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(SavedWorldPathsFilePath,
            new MockFileData(XmlContentStart +
                             "<SavedLearningWorldPath><Name>TestName</Name><Path>TestPath</Path></SavedLearningWorldPath>" +
                             XmlContentEnd));

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        Assert.That(fileSystem.Directory.Exists(LearningWorldSavePathsFolderPath), Is.True);
        Assert.That(fileSystem.File.Exists(SavedWorldPathsFilePath), Is.True);
        var reader = new StreamReader(fileSystem.File.OpenRead(SavedWorldPathsFilePath));
        var content = reader.ReadToEnd();
        Assert.That(content,
            Is.EqualTo(XmlContentStart +
                       "<SavedLearningWorldPath><Name>TestName</Name><Path>TestPath</Path></SavedLearningWorldPath>" +
                       XmlContentEnd));
        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath"));
        });
    }

    [Test]
    public void Standard_FolderAndFileExists_WithPrefilledContent()
    {
        var fileSystem = CreatePrefilledMockFileSystem();

        CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);
        Assert.Multiple(() =>
        {
            Assert.That(fileSystem.Directory.Exists(LearningWorldSavePathsFolderPath), Is.True);
            Assert.That(fileSystem.File.Exists(SavedWorldPathsFilePath), Is.True);
        });
        var reader = new StreamReader(fileSystem.File.OpenRead(SavedWorldPathsFilePath));
        var content = reader.ReadToEnd();
        Assert.That(content, Is.EqualTo(
            XmlContentStart
            + "<SavedLearningWorldPath><Name>name1</Name><Path>path1</Path></SavedLearningWorldPath>"
            + "<SavedLearningWorldPath><Name>name2</Name><Path>path2</Path></SavedLearningWorldPath>"
            + "<SavedLearningWorldPath><Name>name3</Name><Path>path3</Path></SavedLearningWorldPath>"
            + XmlContentEnd
        ));
    }

    #endregion

    #region GetSavedLearningWorldPaths

    [Test]
    public void GetSavedLearningWorldPaths_WithPrefilledContent_ReturnsEnumerable()
    {
        var fileSystem = CreatePrefilledMockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths[0].Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths[0].Name, Is.EqualTo("name1"));
            Assert.That(savedLearningWorldPaths[0].Path, Is.EqualTo("path1"));
            Assert.That(savedLearningWorldPaths[1].Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths[1].Name, Is.EqualTo("name2"));
            Assert.That(savedLearningWorldPaths[1].Path, Is.EqualTo("path2"));
            Assert.That(savedLearningWorldPaths[2].Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths[2].Name, Is.EqualTo("name3"));
            Assert.That(savedLearningWorldPaths[2].Path, Is.EqualTo("path3"));
        });
    }

    [Test]
    public void GetSavedLearningWorldPaths_ReturnsEmptyEnumerable_IfNoSavedLearningWorldPathExists()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Is.Empty);
    }

    [Test]
    public void GetSavedLearningWorldPaths_ReturnsEnumerable_IfSavedLearningWorldPathExists()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(SavedWorldPathsFilePath,
            new MockFileData(XmlContentStart +
                             "<SavedLearningWorldPath><Name>TestName</Name><Path>TestPath</Path></SavedLearningWorldPath>" +
                             XmlContentEnd));

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath"));
        });
    }

    [Test]
    public void GetSavedLearningWorldPaths_ReturnsEnumerable_WithMultipleEntries()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(SavedWorldPathsFilePath,
            new MockFileData(XmlContentStart +
                             "<SavedLearningWorldPath><Name>TestName</Name><Path>TestPath</Path></SavedLearningWorldPath>" +
                             "<SavedLearningWorldPath><Name>TestName2</Name><Path>TestPath2</Path></SavedLearningWorldPath>" +
                             XmlContentEnd));

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath"));
            Assert.That(savedLearningWorldPaths.Last().Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths.Last().Name, Is.EqualTo("TestName2"));
            Assert.That(savedLearningWorldPaths.Last().Path, Is.EqualTo("TestPath2"));
        });
    }

    #endregion

    #region AddSavedLearningWorldPath

    [Test]
    public void AddSavedLearningWorldPath_AddsSavedLearningWorldPathToFile()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "TestName", Path = "TestPath"});
        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath"));
        });
    }

    [Test]
    public void AddSavedLearningWorldPath_AddsSavedLearningWorldPathToFile_WithMultipleEntries()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "TestName1", Path = "TestPath1"});

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });
        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "TestName2", Path = "TestPath2"});

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName1"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
            Assert.That(savedLearningWorldPaths.Last().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000002")));
            Assert.That(savedLearningWorldPaths.Last().Name, Is.EqualTo("TestName2"));
            Assert.That(savedLearningWorldPaths.Last().Path, Is.EqualTo("TestPath2"));
        });
    }

    [Test]
    public void AddSavedLearningWorldPath_AddsSavedLearningWorldPathToFile_WithExistingEntries_WithSameId()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "TestName1", Path = "TestPath1"});

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName1"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });
        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "TestName2", Path = "TestPath2"});

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName1"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
            Assert.That(savedLearningWorldPaths.Last().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.Last().Name, Is.EqualTo("TestName2"));
            Assert.That(savedLearningWorldPaths.Last().Path, Is.EqualTo("TestPath2"));
        });
    }

    [Test]
    public void AddSavedLearningWorldPath_AddsSavedLearningWorldPathToFile_WithExistingEntries_WithSamePath()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Path = "TestPath1"});

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Path = "TestPath1"});

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000002")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });
    }

    [Test]
    public void AddSavedLearningWorldPath_AddsSavedLearningWorldPathToFile_WithExistingEntries_WithSameIdAndPath()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Path = "TestPath1"});

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Path = "TestPath1"});

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });
    }

    [Test]
    public void
        AddSavedLearningWorldPath_AddsSavedLearningWorldPathToFile_WithExistingEntries_WithSameIdAsAnEntryAndSamePathAsAnotherEntry()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "TestName1", Path = "TestPath1"});

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName1"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "TestName2", Path = "TestPath2"});

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName1"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
            Assert.That(savedLearningWorldPaths.Last().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000002")));
            Assert.That(savedLearningWorldPaths.Last().Name, Is.EqualTo("TestName2"));
            Assert.That(savedLearningWorldPaths.Last().Path, Is.EqualTo("TestPath2"));
        });

        systemUnderTest.AddSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "TestName3", Path = "TestPath2"});

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestName1"));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
            Assert.That(savedLearningWorldPaths.Last().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.Last().Name, Is.EqualTo("TestName3"));
            Assert.That(savedLearningWorldPaths.Last().Path, Is.EqualTo("TestPath2"));
        });
    }

    #endregion

    #region AddSavedLearningWorldPathByPathOnly

    [Test]
    public void AddSavedLearningWorldPathByPathOnly_AddsSavedLearningWorldPathToFile()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        systemUnderTest.AddSavedLearningWorldPathByPathOnly("C:/TestPath1");

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("C:/TestPath1"));
            Assert.That(savedLearningWorldPaths.First().Name, Is.EqualTo("TestPath1"));
        });
    }

    [Test]
    public void AddSavedLearningWorldPathByPathOnly_ReturnsSavedLearningWorldPath()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var savedLearningWorldPath = systemUnderTest.AddSavedLearningWorldPathByPathOnly("C:/TestPath1");

        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPath.Id, Is.EqualTo(Guid.Empty));
            Assert.That(savedLearningWorldPath.Path, Is.EqualTo("C:/TestPath1"));
            Assert.That(savedLearningWorldPath.Name, Is.EqualTo("TestPath1"));
        });
    }

    #endregion

    #region UpdateIdOfSavedLearningWorldPath

    [Test]
    public void UpdateIdOfSavedLearningWorldPath_UpdatesIdOfSavedLearningWorldPathInFile()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var savedLearningWorldPath = new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Path = "TestPath1"};
        systemUnderTest.AddSavedLearningWorldPath(savedLearningWorldPath);

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });

        systemUnderTest.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath,
            Guid.Parse("00000000-0000-0000-0000-000000000002"));

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000002")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });
    }

    #endregion

    #region RemoveSavedLearningWorldPath

    [Test]
    public void RemoveSavedLearningWorldPath_RemovesSavedLearningWorldPathFromFile()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var savedLearningWorldPath = new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Path = "TestPath1"};
        systemUnderTest.AddSavedLearningWorldPath(savedLearningWorldPath);

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });

        systemUnderTest.RemoveSavedLearningWorldPath(savedLearningWorldPath);

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(0));
    }

    [Test]
    public void RemoveSavedLearningWorldPath_PathDoesNotExistInFile_NothingHappens()
    {
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableLearningWorldSavePathsHandler(fileSystem: fileSystem);

        var savedLearningWorldPath = new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Path = "TestPath1"};
        systemUnderTest.AddSavedLearningWorldPath(savedLearningWorldPath);

        var savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });

        systemUnderTest.RemoveSavedLearningWorldPath(new SavedLearningWorldPath
            {Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Path = "TestPath2"});

        savedLearningWorldPaths = systemUnderTest.GetSavedLearningWorldPaths().ToList();
        Assert.That(savedLearningWorldPaths, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(savedLearningWorldPaths.First().Id,
                Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000001")));
            Assert.That(savedLearningWorldPaths.First().Path, Is.EqualTo("TestPath1"));
        });
    }

    #endregion

    private static IFileSystem CreatePrefilledMockFileSystem()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(LearningWorldSavePathsFolderPath);
        fileSystem.AddFile(SavedWorldPathsFilePath,
            new MockFileData(
                XmlContentStart
                + "<SavedLearningWorldPath><Name>name1</Name><Path>path1</Path></SavedLearningWorldPath>"
                + "<SavedLearningWorldPath><Name>name2</Name><Path>path2</Path></SavedLearningWorldPath>"
                + "<SavedLearningWorldPath><Name>name3</Name><Path>path3</Path></SavedLearningWorldPath>"
                + XmlContentEnd
            )
        );
        return fileSystem;
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