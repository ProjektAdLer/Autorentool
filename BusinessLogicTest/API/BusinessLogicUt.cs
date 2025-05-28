using System.Runtime.Serialization;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.ErrorManagement;
using BusinessLogic.ErrorManagement.BackendAccess;
using BusinessLogic.Validation.Validators;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Shared.Command;
using Shared.Configuration;
using Shared.Theme;
using TestHelpers;

namespace BusinessLogicTest.API;

[TestFixture]
public class BusinessLogicUt
{
    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(mockConfiguration, mockDataAccess);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.DataAccess, Is.EqualTo(mockDataAccess));
        });
    }

    [Test]
    public void GetAllContent_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        systemUnderTest.GetAllContent();
        dataAccess.Received().GetAllContent();
    }

    [Test]
    public void GetContentFilesFolderPath_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        systemUnderTest.GetContentFilesFolderPath();
        dataAccess.Received().GetContentFilesFolderPath();
    }

    [Test]
    // ANF-ID: [ASN0001]
    public async Task ExportLearningWorldToArchiveAsync_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        var mockWorld = EntityProvider.GetLearningWorld();
        var mockFilePath = "foo";

        await systemUnderTest.ExportLearningWorldToArchiveAsync(mockWorld, mockFilePath);
        await dataAccess.Received().ExportLearningWorldToArchiveAsync(mockWorld, mockFilePath);
    }

    [Test]
    // ANF-ID: [ASN0002]
    public async Task ImportLearningWorldFromArchiveAsync_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        var mockFilePath = "foo";

        await systemUnderTest.ImportLearningWorldFromArchiveAsync(mockFilePath);
        await dataAccess.Received().ImportLearningWorldFromArchiveAsync(mockFilePath);
    }

    [Test]
    public void GetFileInfoForPath_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        var mockPath = "foo";

        systemUnderTest.GetFileInfoForPath(mockPath);
        dataAccess.Received().GetFileInfoForPath(mockPath);
    }

    [Test]
    public void DeleteFileByPath_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        var mockPath = "foo";

        systemUnderTest.DeleteFileByPath(mockPath);
        dataAccess.Received().DeleteFileByPath(mockPath);
    }

    [Test]
    public async Task IsLmsConnected_ReturnsFalse()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        var result = await systemUnderTest.IsLmsConnected();

        Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveContent_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);
        var content = new FileContent("foo", "bar", "baz");

        systemUnderTest.RemoveContent(content);
        dataAccess.Received().RemoveContent(content);
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveContent_ArgumentOutOfRangeException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(_ => throw new ArgumentOutOfRangeException());
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess, errorManager: errorManager);
        var content = new FileContent("foo", "bar", "baz");

        systemUnderTest.RemoveContent(content);
        errorManager.Received(1).LogAndRethrowError(Arg.Any<ArgumentOutOfRangeException>());
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveContent_FileNotFoundException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(_ => throw new FileNotFoundException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess, errorManager: errorManager);
        var content = new FileContent("foo", "bar", "baz");

        systemUnderTest.RemoveContent(content);
        errorManager.Received(1).LogAndRethrowError(Arg.Any<FileNotFoundException>());
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveContent_SerializationException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(_ => throw new SerializationException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess, errorManager: errorManager);
        var content = new FileContent("foo", "bar", "baz");

        systemUnderTest.RemoveContent(content);
        errorManager.Received(1).LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveMultipleContents_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);
        var content1 = Substitute.For<IFileContent>();
        var content2 = Substitute.For<ILinkContent>();
        var content3 = Substitute.For<IAdaptivityContent>();
        var contents = new List<ILearningContent>() { content1, content2, content3 };

        systemUnderTest.RemoveMultipleContents(contents);
        foreach (var content in contents)
        {
            dataAccess.Received().RemoveContent(content);
        }
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveMultipleContents_ArgumentOutOfRangeException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(_ => throw new ArgumentOutOfRangeException());
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess, errorManager: errorManager);
        var content1 = Substitute.For<IFileContent>();
        var content2 = Substitute.For<ILinkContent>();
        var content3 = Substitute.For<IAdaptivityContent>();
        var contents = new List<ILearningContent>() { content1, content2, content3 };

        systemUnderTest.RemoveMultipleContents(contents);
        dataAccess.Received(1).RemoveContent(Arg.Any<ILearningContent>());
        errorManager.Received(1).LogAndRethrowError(Arg.Any<ArgumentOutOfRangeException>());
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveMultipleContents_FileNotFoundException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(_ => throw new FileNotFoundException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess, errorManager: errorManager);
        var content1 = Substitute.For<IFileContent>();
        var content2 = Substitute.For<ILinkContent>();
        var content3 = Substitute.For<IAdaptivityContent>();
        var contents = new List<ILearningContent>() { content1, content2, content3 };

        systemUnderTest.RemoveMultipleContents(contents);
        dataAccess.Received(1).RemoveContent(Arg.Any<ILearningContent>());
        errorManager.Received(1).LogAndRethrowError(Arg.Any<FileNotFoundException>());
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveMultipleContents_SerializationException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(_ => throw new SerializationException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess, errorManager: errorManager);
        var content1 = Substitute.For<IFileContent>();
        var content2 = Substitute.For<ILinkContent>();
        var content3 = Substitute.For<IAdaptivityContent>();
        var contents = new List<ILearningContent>() { content1, content2, content3 };

        systemUnderTest.RemoveMultipleContents(contents);
        dataAccess.Received(1).RemoveContent(Arg.Any<ILearningContent>());
        errorManager.Received(1).LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    // ANF-ID: [AWA0042]
    public void SaveLink_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);
        var content = new LinkContent("foo", "bar");

        systemUnderTest.SaveLink(content);
        dataAccess.Received().SaveLink(content);
    }

    [Test]
    // ANF-ID: [AWA0042]
    public void SaveLink_SerializationException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.SaveLink(Arg.Any<LinkContent>()))
            .Do(_ => throw new SerializationException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess, errorManager: errorManager);
        var content = new LinkContent("foo", "bar");

        systemUnderTest.SaveLink(content);
        errorManager.Received(1).LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    public void ConstructBackup_CallsWorldGenerator()
    {
        var mockWorldGenerator = Substitute.For<IWorldGenerator>();
        var world = EntityProvider.GetLearningWorld();

        var systemUnderTest = CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator);

        systemUnderTest.ConstructBackup(world, "foobar");

        mockWorldGenerator.Received().ConstructBackup(world, "foobar");
    }

    [Test]
    public void ConstructBackup_ThrowsArgumentOutOfRangeException_LogAndRethrowErrorCalled()
    {
        var mockWorldGenerator = Substitute.For<IWorldGenerator>();
        var mockErrorManager = Substitute.For<IErrorManager>();
        mockWorldGenerator.When(wg => wg.ConstructBackup(null!, "foobar"))
            .Do(_ => { throw new ArgumentOutOfRangeException(); });

        var systemUnderTest =
            CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator, errorManager: mockErrorManager);

        systemUnderTest.ConstructBackup(null!, "foobar");
        mockErrorManager.Received().LogAndRethrowGeneratorError(Arg.Any<ArgumentOutOfRangeException>());
    }

    [Test]
    public void ConstructBackup_ThrowsInvalidOperationException_LogAndRethrowErrorCalled()
    {
        var mockWorldGenerator = Substitute.For<IWorldGenerator>();
        var mockErrorManager = Substitute.For<IErrorManager>();
        mockWorldGenerator.When(wg => wg.ConstructBackup(null!, "foobar"))
            .Do(_ => { throw new InvalidOperationException(); });

        var systemUnderTest =
            CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator, errorManager: mockErrorManager);

        systemUnderTest.ConstructBackup(null!, "foobar");

        mockErrorManager.Received().LogAndRethrowGeneratorError(Arg.Any<InvalidOperationException>());
    }

    [Test]
    public void ConstructBackup_ThrowsFileNotFoundException_LogAndRethrowErrorCalled()
    {
        var mockWorldGenerator = Substitute.For<IWorldGenerator>();
        var mockErrorManager = Substitute.For<IErrorManager>();
        mockWorldGenerator.When(wg => wg.ConstructBackup(null!, "foobar"))
            .Do(_ => { throw new FileNotFoundException(); });

        var systemUnderTest =
            CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator, errorManager: mockErrorManager);

        systemUnderTest.ConstructBackup(null!, "foobar");

        mockErrorManager.Received().LogAndRethrowGeneratorError(Arg.Any<FileNotFoundException>());
    }

    [Test]
    public void ExecuteCommand_CallsCommandStateManager()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var mockCommand = Substitute.For<ICommand>();

        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        systemUnderTest.ExecuteCommand(mockCommand);

        mockCommandStateManager.Received().Execute(mockCommand);
    }

    [Test]
    // ANF-ID: [ASN0003, ASN0005]
    public void ExecuteCommand_InvokesOnUndoRedoPerformed()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var mockCommand = Substitute.For<ICommand>();
        var mockOnUndoRedoOrExecutePerformed = Substitute.For<EventHandler<CommandUndoRedoOrExecuteArgs>>();

        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        systemUnderTest.OnCommandUndoRedoOrExecute += mockOnUndoRedoOrExecutePerformed;
        systemUnderTest.ExecuteCommand(mockCommand);

        mockOnUndoRedoOrExecutePerformed.Received().Invoke(systemUnderTest, Arg.Any<CommandUndoRedoOrExecuteArgs>());
    }

    [Test]
    // ANF-ID: [ASN0003, ASN0005]
    public void CanUndoCanRedo_CallsCommandStateManager()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.CanUndo.Returns(true);
        mockCommandStateManager.CanRedo.Returns(false);
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        var canUndo = systemUnderTest.CanUndo;
        var canRedo = systemUnderTest.CanRedo;

        Assert.Multiple(() =>
        {
            Assert.That(canUndo, Is.True);
            Assert.That(canRedo, Is.False);
        });
    }

    [Test]
    // ANF-ID: [ASN0003]
    public void CallUndoCommand_CallsCommandStateManager()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        systemUnderTest.UndoCommand();

        mockCommandStateManager.Received().Undo();
    }

    [Test]
    // ANF-ID: [ASN0003]
    public void CallUndoCommand_InvokesOnUndoRedoPerformed()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var mockOnUndoRedoOrExecutePerformed = Substitute.For<EventHandler<CommandUndoRedoOrExecuteArgs>>();

        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        systemUnderTest.OnCommandUndoRedoOrExecute += mockOnUndoRedoOrExecutePerformed;
        systemUnderTest.UndoCommand();

        mockOnUndoRedoOrExecutePerformed.Received().Invoke(systemUnderTest, Arg.Any<CommandUndoRedoOrExecuteArgs>());
    }

    [Test]
    // ANF-ID: [ASN0003]
    public void CallUndoCommand_UndoThrowsInvalidOperationException_ErrorManagerCalled()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.When(x => x.Undo()).Do(_ => throw new InvalidOperationException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(
            commandStateManager: mockCommandStateManager,
            errorManager: mockErrorManager
        );

        systemUnderTest.UndoCommand();

        mockErrorManager.Received().LogAndRethrowUndoError(Arg.Any<InvalidOperationException>());
    }

    [Test]
    // ANF-ID: [ASN0003]
    public void CallUndoCommand_UndoThrowsArgumentOutOfRangeException_ErrorManagerCalled()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.When(x => x.Undo()).Do(_ => throw new ArgumentOutOfRangeException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(
            commandStateManager: mockCommandStateManager,
            errorManager: mockErrorManager
        );

        systemUnderTest.UndoCommand();

        mockErrorManager.Received().LogAndRethrowUndoError(Arg.Any<ArgumentOutOfRangeException>());
    }

    [Test]
    // ANF-ID: [ASN0005]
    public void CallRedoCommand_CallsCommandStateManager()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        systemUnderTest.RedoCommand();

        mockCommandStateManager.Received().Redo();
    }

    [Test]
    // ANF-ID: [ASN0005]
    public void CallRedoCommand_InvokesOnUndoRedoPerformed()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var mockOnUndoRedoOrExecutePerformed = Substitute.For<EventHandler<CommandUndoRedoOrExecuteArgs>>();

        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        systemUnderTest.OnCommandUndoRedoOrExecute += mockOnUndoRedoOrExecutePerformed;
        systemUnderTest.RedoCommand();

        mockOnUndoRedoOrExecutePerformed.Received().Invoke(systemUnderTest, Arg.Any<CommandUndoRedoOrExecuteArgs>());
    }

    [Test]
    // ANF-ID: [ASN0005]
    public void CallRedoCommand_RedoThrowsInvalidOperationException_ErrorManagerCalled()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.When(x => x.Redo()).Do(_ => throw new InvalidOperationException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(
            commandStateManager: mockCommandStateManager,
            errorManager: mockErrorManager
        );

        systemUnderTest.RedoCommand();

        mockErrorManager.Received().LogAndRethrowRedoError(Arg.Any<InvalidOperationException>());
    }

    [Test]
    // ANF-ID: [ASN0005]
    public void CallRedoCommand_RedoThrowsApplicationException_ErrorManagerCalled()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.When(x => x.Redo()).Do(_ => throw new ApplicationException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(
            commandStateManager: mockCommandStateManager,
            errorManager: mockErrorManager
        );

        systemUnderTest.RedoCommand();

        mockErrorManager.Received().LogAndRethrowRedoError(Arg.Any<ApplicationException>());
    }


    [Test]
    // ANF-ID: [ASE6]
    public void SaveLearningWorld_CallsDataAccess()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");

        mockDataAccess.Received().SaveLearningWorldToFile(learningWorld, "foobar");
    }

    [Test]
    // ANF-ID: [ASE6]
    public void SaveLearningWorld_SerializationException_CallsErrorManager()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.When(x => x.SaveLearningWorldToFile(Arg.Any<LearningWorld>(), Arg.Any<string>()))
            .Do(_ => throw new SerializationException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    // ANF-ID: [ASE2]
    public void LoadLearningWorld_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var world = EntityProvider.GetLearningWorld();
        mockDataAccess.LoadLearningWorld("foobar").Returns(world);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningWorld("foobar");

        mockDataAccess.Received().LoadLearningWorld("foobar");
    }

    [Test]
    // ANF-ID: [ASE2]
    public void LoadLearningWorld_ReturnsLearningWorld()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f", WorldTheme.CampusAschaffenburg);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningWorld("foobar").Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld("foobar");

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    // ANF-ID: [ASE2]
    public void LoadLearningWorld_SerializationException_CallsErrorManager()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.When(x => x.LoadLearningWorld(Arg.Any<string>()))
            .Do(_ => throw new SerializationException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.LoadLearningWorld("foobar");

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    // ANF-ID: [AWA0036]
    public  async Task LoadLearningContent_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        await systemUnderTest.LoadLearningContentAsync("foobar");

        await mockDataAccess.Received().LoadLearningContentAsync("foobar");
    }

    [Test]
    // ANF-ID: [AWA0036]
    public async Task LoadLearningContent_ReturnsLearningElement()
    {
        var learningContent = new FileContent("fa", "a", "");
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContentAsync("foobar").Returns(learningContent);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = await systemUnderTest.LoadLearningContentAsync("foobar");

        Assert.That(learningElementActual, Is.EqualTo(learningContent));
    }

    [Test]
    public void GetSavedLearningWorldPaths_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.GetSavedLearningWorldPaths();

        mockDataAccess.Received().GetSavedLearningWorldPaths();
    }

    [Test]
    // ANF-ID: [AWA0036]
    public async Task LoadLearningContentFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        await systemUnderTest.LoadLearningContentAsync("filename.extension", stream);

        await mockDataAccess.Received().LoadLearningContentAsync("filename.extension", stream);
    }

    [Test]
    // ANF-ID: [AWA0036]
    public async Task LoadLearningContentFromStream_ReturnsLearningElement()
    {
        var learningContent = new FileContent("filename", "extension", "");
        var stream = Substitute.For<MemoryStream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContentAsync("filename.extension", stream).Returns(learningContent);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = await systemUnderTest.LoadLearningContentAsync("filename.extension", stream);

        Assert.That(learningElementActual, Is.EqualTo(learningContent));
    }

    [Test]
    // ANF-ID: [AWA0036]
    public async Task LoadLearningContentFromStream_IOException_CallsErrorManager()
    {
        var stream = Substitute.For<MemoryStream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContentAsync("filename.extension", stream).ThrowsAsync<IOException>();

        var errorManager = Substitute.For<IErrorManager>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: mockDataAccess, errorManager: errorManager);

        await systemUnderTest.LoadLearningContentAsync("filename.extension", stream);

        errorManager.Received().LogAndRethrowError(Arg.Any<IOException>());
    }

    [Test]
    public void FindSuitableNewSavePath_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        systemUnderTest.FindSuitableNewSavePath("foo", "bar", "baz", out _);

        dataAccess.Received().FindSuitableNewSavePath("foo", "bar", "baz", out _);
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task Login_WritesTokenToConfiguration()
    {
        var backendAccess = Substitute.For<IBackendAccess>();
        const string username = "username";
        const string password = "password";
        var tokenString = "token";
        var token = new UserToken(tokenString);
        var userInformation = new UserInformation(username, false, 0, "");
        backendAccess.GetUserTokenAsync(username, password).Returns(Task.FromResult(token));
        backendAccess.GetUserInformationAsync(Arg.Is<UserToken>(t => t.Token == tokenString))
            .Returns(Task.FromResult(userInformation));

        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(tokenString);
        var systemUnderTest =
            CreateStandardBusinessLogic(apiAccess: backendAccess, fakeConfiguration: mockConfiguration);

        await systemUnderTest.Login(username, password);

        mockConfiguration.Received()[IApplicationConfiguration.BackendToken] = tokenString;
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task Login_CallsBackendAccess()
    {
        var backendAccess = Substitute.For<IBackendAccess>();
        const string username = "username";
        const string password = "password";
        var tokenString = "token";
        var token = new UserToken(tokenString);
        var userInformation = new UserInformation(username, false, 0, "");
        backendAccess.GetUserTokenAsync(username, password).Returns(Task.FromResult(token));
        backendAccess.GetUserInformationAsync(Arg.Is<UserToken>(t => t.Token == tokenString))
            .Returns(Task.FromResult(userInformation));
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(tokenString);
        var systemUnderTest =
            CreateStandardBusinessLogic(apiAccess: backendAccess, fakeConfiguration: mockConfiguration);

        await systemUnderTest.Login(username, password);

        await backendAccess.Received().GetUserTokenAsync(username, password);
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task Login_IfLoginSuccess_CallsBackendForUserInformation() //This is not a unit test
    {
        var backendAccess = Substitute.For<IBackendAccess>();
        const string username = "username";
        const string password = "password";
        var tokenString = "token";
        var token = new UserToken(tokenString);
        backendAccess.GetUserTokenAsync(username, password).Returns(Task.FromResult(token));
        var userInformation = new UserInformation(username, false, 0, "");
        backendAccess.GetUserInformationAsync(Arg.Is<UserToken>(t => t.Token == tokenString))
            .Returns(Task.FromResult(userInformation));
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        mockConfiguration[IApplicationConfiguration.BackendBaseUrl] = "some url";

        var systemUnderTest =
            CreateStandardBusinessLogic(fakeConfiguration: mockConfiguration, apiAccess: backendAccess);

        await systemUnderTest.Login(username, password);

        await backendAccess.Received().GetUserInformationAsync(Arg.Is<UserToken>(t => t.Token == tokenString));
    }

    [Test]
    // ANF-ID: [AHO21]
    public Task Login_ThrowsBackendInvalidLoginException_Logout()
    {
        var mockBackendAccess = Substitute.For<IBackendAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(apiAccess: mockBackendAccess);

        mockBackendAccess.When(x => x.GetUserTokenAsync(Arg.Any<string>(), Arg.Any<string>()))
            .Do(_ => throw new BackendInvalidLoginException());

        Assert.ThrowsAsync<BackendInvalidLoginException>(async () =>
            await systemUnderTest.Login("user", "pw"));
        return Task.CompletedTask;
    }

    [Test]
    // ANF-ID: [AHO21]
    public Task Login_BackendInvalidUrlException_Logout()
    {
        var mockBackendAccess = Substitute.For<IBackendAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(apiAccess: mockBackendAccess);

        mockBackendAccess.When(x => x.GetUserTokenAsync(Arg.Any<string>(), Arg.Any<string>()))
            .Do(_ => throw new BackendInvalidUrlException());

        Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await systemUnderTest.Login("user", "pw"));
        return Task.CompletedTask;
    }

    [Test]
    // ANF-ID: [AHO022]
    public  async Task UploadLearningWorldToBackend_CallsWorldGenerator()
    {
        var worldGenerator = Substitute.For<IWorldGenerator>();
        const string filepath = "filepath";
        var systemUnderTest = CreateStandardBusinessLogic(worldGenerator: worldGenerator);

        await systemUnderTest.UploadLearningWorldToBackendAsync(filepath);

        worldGenerator.Received().ExtractAtfFromBackup(filepath);
    }

    [Test]
    // ANF-ID: [AHO022]
    public  async Task UploadLearningWorldToBackend_CallsBackendAccess()
    {
        const string filepath = "filepath";
        const string atfPath = "atfPath";
        var token = "token";
        var worldGenerator = Substitute.For<IWorldGenerator>();
        worldGenerator.ExtractAtfFromBackup(filepath).Returns(atfPath);
        var backendAccess = Substitute.For<IBackendAccess>();
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        var systemUnderTest = CreateStandardBusinessLogic(apiAccess: backendAccess, worldGenerator: worldGenerator,
            fakeConfiguration: mockConfiguration);
        var mockProgress = Substitute.For<IProgress<int>>();
        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(token);

        await systemUnderTest.UploadLearningWorldToBackendAsync(filepath, mockProgress);

        await backendAccess.Received()
            .UploadLearningWorldAsync(Arg.Is<UserToken>(c => c.Token == "token"), filepath, atfPath, mockProgress);
    }

    [Test]
    // ANF-ID: [AHO022]
    public void UploadLearningWorldToBackend_HttpRequestException_LogAndRethrow()
    {
        var mockBackendAccess = Substitute.For<IBackendAccess>();
        var mockErrorManager = Substitute.For<IErrorManager>();
        var systemUnderTest = CreateStandardBusinessLogic(apiAccess: mockBackendAccess, errorManager: mockErrorManager);

        mockBackendAccess.When(x => x.UploadLearningWorldAsync(Arg.Any<UserToken>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<IProgress<int>>()))
            .Do(_ => throw new HttpRequestException());

        Assert.ThrowsAsync<HttpRequestException>(async () =>
            await systemUnderTest.UploadLearningWorldToBackendAsync("path"));

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<HttpRequestException>());
    }

    [Test]
    // ANF-ID: [AHO23]
    public async Task GetLmsWorldList_CallsBackendAccess()
    {
        var backendAccess = Substitute.For<IBackendAccess>();
        var token = "token";
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        var systemUnderTest =
            CreateStandardBusinessLogic(apiAccess: backendAccess, fakeConfiguration: mockConfiguration);

        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(token);

        await systemUnderTest.GetLmsWorldList();

        await backendAccess.Received().GetLmsWorldList(Arg.Is<UserToken>(c => c.Token == "token"), Arg.Any<int>());
    }

    [Test]
    // ANF-ID: [AHO23]
    public void GetLmsWorldList_BackendThrowsHttpRequestException_ErrorManagerCalled()
    {
        var backendAccess = Substitute.For<IBackendAccess>();
        var errorManager = Substitute.For<IErrorManager>();
        var token = "token";
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();

        backendAccess.When(x => x.GetLmsWorldList(Arg.Any<UserToken>(), Arg.Any<int>()))
            .Do(_ => throw new HttpRequestException());


        var systemUnderTest = CreateStandardBusinessLogic(apiAccess: backendAccess,
            fakeConfiguration: mockConfiguration, errorManager: errorManager);

        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(token);

        Assert.ThrowsAsync<HttpRequestException>(async () =>
            await systemUnderTest.GetLmsWorldList());

        errorManager.Received().LogAndRethrowBackendAccessError(Arg.Any<HttpRequestException>());
    }

    [Test]
    // ANF-ID: [AHO24]
    public async Task DeleteLmsWorld_CallsBackendAccess()
    {
        var backendAccess = Substitute.For<IBackendAccess>();
        backendAccess.When(x => x.DeleteLmsWorld(Arg.Any<UserToken>(), Arg.Any<LmsWorld>()).Returns(true));
        var token = "token";
        var mockLmsWorld = Substitute.For<LmsWorld>();
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        var systemUnderTest =
            CreateStandardBusinessLogic(apiAccess: backendAccess, fakeConfiguration: mockConfiguration);

        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(token);

        await systemUnderTest.DeleteLmsWorld(mockLmsWorld);

        await backendAccess.Received().DeleteLmsWorld(Arg.Is<UserToken>(c => c.Token == "token"), mockLmsWorld);
    }

    [Test]
    // ANF-ID: [AHO24]
    public async Task DeleteLmsWorld_ResultIsFalse_ErrorManagerCalled()
    {
        var backendAccess = Substitute.For<IBackendAccess>();
        backendAccess.When(x => x.DeleteLmsWorld(Arg.Any<UserToken>(), Arg.Any<LmsWorld>()).Returns(false));
        var errorManager = Substitute.For<IErrorManager>();
        var mockLmsWorld = Substitute.For<LmsWorld>();
        var token = "token";
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();

        var systemUnderTest = CreateStandardBusinessLogic(apiAccess: backendAccess,
            fakeConfiguration: mockConfiguration, errorManager: errorManager);

        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(token);

        await systemUnderTest.DeleteLmsWorld(mockLmsWorld);

        errorManager.Received().LogAndRethrowBackendAccessError(Arg.Any<BackendWorldDeletionException>());
    }

    [Test]
    // ANF-ID: [AHO24]
    public async Task DeleteLmsWorld_BackendThrowsHttpRequestException_ErrorManagerCalled()
    {
        var backendAccess = Substitute.For<IBackendAccess>();
        var errorManager = Substitute.For<IErrorManager>();
        var mockLmsWorld = Substitute.For<LmsWorld>();
        var token = "token";
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();

        backendAccess.When(x => x.DeleteLmsWorld(Arg.Any<UserToken>(), Arg.Any<LmsWorld>()))
            .Do(_ => throw new HttpRequestException());

        var systemUnderTest = CreateStandardBusinessLogic(apiAccess: backendAccess,
            fakeConfiguration: mockConfiguration, errorManager: errorManager);

        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(token);


        await systemUnderTest.DeleteLmsWorld(mockLmsWorld);

        errorManager.Received().LogAndRethrowBackendAccessError(Arg.Any<HttpRequestException>());
    }

    private BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IApplicationConfiguration? fakeConfiguration = null,
        IDataAccess? fakeDataAccess = null,
        IWorldGenerator? worldGenerator = null,
        ICommandStateManager? commandStateManager = null,
        IBackendAccess? apiAccess = null,
        IErrorManager? errorManager = null,
        ILogger<BusinessLogic.API.BusinessLogic>? logger = null,
        ILearningWorldStructureValidator? learningWorldStructureValidator = null)
    {
        fakeConfiguration ??= Substitute.For<IApplicationConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        worldGenerator ??= Substitute.For<IWorldGenerator>();
        commandStateManager ??= Substitute.For<ICommandStateManager>();
        apiAccess ??= Substitute.For<IBackendAccess>();
        errorManager ??= Substitute.For<IErrorManager>();
        logger ??= Substitute.For<ILogger<BusinessLogic.API.BusinessLogic>>();
        learningWorldStructureValidator ??= Substitute.For<ILearningWorldStructureValidator>();

        return new BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess, worldGenerator,
            commandStateManager, apiAccess, errorManager, logger, learningWorldStructureValidator);
    }
}