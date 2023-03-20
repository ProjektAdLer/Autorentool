using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NSubstitute;
using NUnit.Framework;
using Shared;
using Shared.Command;
using Shared.Configuration;

namespace BusinessLogicTest.API;

[TestFixture]
public class BusinessLogicUt
{
    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        
        var systemUnderTest = CreateStandardBusinessLogic(mockConfiguration, mockDataAccess);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.DataAccess, Is.EqualTo(mockDataAccess));
        });
    }

    [Test]
    public void ConstructBackup_CallsWorldGenerator()
    {
        var mockWorldGenerator = Substitute.For<IWorldGenerator>();

        var systemUnderTest = CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator);

        systemUnderTest.ConstructBackup(null!, "foobar");

        mockWorldGenerator.Received().ConstructBackup(null!, "foobar");
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
    public void ExecuteCommand_InvokesOnUndoRedoPerformed(){
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var mockCommand = Substitute.For<ICommand>();
        var mockOnUndoRedoOrExecutePerformed = Substitute.For<EventHandler<CommandUndoRedoOrExecuteArgs>>();
        
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);
        
        systemUnderTest.OnCommandUndoRedoOrExecute += mockOnUndoRedoOrExecutePerformed;
        systemUnderTest.ExecuteCommand(mockCommand);
        
        mockOnUndoRedoOrExecutePerformed.Received().Invoke(systemUnderTest, Arg.Any<CommandUndoRedoOrExecuteArgs>());
    }
    
    [Test]
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
    public void CallUndoCommand_CallsCommandStateManager()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        systemUnderTest.UndoCommand();

        mockCommandStateManager.Received().Undo();
    }
    
    [Test]
    public void CallUndoCommand_InvokesOnUndoRedoPerformed(){
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var mockOnUndoRedoOrExecutePerformed = Substitute.For<EventHandler<CommandUndoRedoOrExecuteArgs>>();
        
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);
        
        systemUnderTest.OnCommandUndoRedoOrExecute += mockOnUndoRedoOrExecutePerformed;
        systemUnderTest.UndoCommand();
        
        mockOnUndoRedoOrExecutePerformed.Received().Invoke(systemUnderTest, Arg.Any<CommandUndoRedoOrExecuteArgs>());
    }
    
    [Test]
    public void CallRedoCommand_CallsCommandStateManager()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);

        systemUnderTest.RedoCommand();

        mockCommandStateManager.Received().Redo();
    }
    
    [Test]
    public void CallRedoCommand_InvokesOnUndoRedoPerformed(){
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        var mockOnUndoRedoOrExecutePerformed = Substitute.For<EventHandler<CommandUndoRedoOrExecuteArgs>>();
        
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);
        
        systemUnderTest.OnCommandUndoRedoOrExecute += mockOnUndoRedoOrExecutePerformed;
        systemUnderTest.RedoCommand();
        
        mockOnUndoRedoOrExecutePerformed.Received().Invoke(systemUnderTest, Arg.Any<CommandUndoRedoOrExecuteArgs>());
    }

    [Test]
    public void SaveLearningWorld_CallsDataAccess()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");

        mockDataAccess.Received().SaveLearningWorldToFile(learningWorld, "foobar");
    }

    [Test]
    public void LoadLearningWorld_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningWorld("foobar");

        mockDataAccess.Received().LoadLearningWorld("foobar");
    }

    [Test]
    public void LoadLearningWorld_ReturnsLearningWorld()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningWorld("foobar").Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld("foobar");

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void SaveLearningSpace_CallsDataAccess()
    {
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f", 0);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningSpace(learningSpace, "foobar");

        mockDataAccess.Received().SaveLearningSpaceToFile(learningSpace, "foobar");
    }

    [Test]
    public void LoadLearningSpace_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpace("foobar");

        mockDataAccess.Received().LoadLearningSpace("foobar");
    }

    [Test]
    public void LoadLearningSpace_ReturnsLearningSpace()
    {
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f", 0);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningSpace("foobar").Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace("foobar");

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void SaveLearningElement_CallsDataAccess()
    {
        var content = new FileContent("a", "b", "");
        var learningElement = new LearningElement("fa", "f", content,"f",
            "f", "f", LearningElementDifficultyEnum.Easy);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningElement(learningElement, "foobar");

        mockDataAccess.Received().SaveLearningElementToFile(learningElement, "foobar");
    }

    [Test]
    public void LoadLearningElement_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElement("foobar");

        mockDataAccess.Received().LoadLearningElement("foobar");
    }

    [Test]
    public void LoadLearningElement_ReturnsLearningElement()
    {
        var content = new FileContent("a", "b", "");
        var learningElement = new LearningElement("fa", "a", content, "f", "f",
            "f", LearningElementDifficultyEnum.Easy);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningElement("foobar").Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElement("foobar");

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }

    [Test]
    public void LoadLearningContent_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningContent("foobar");

        mockDataAccess.Received().LoadLearningContent("foobar");
    }

    [Test]
    public void LoadLearningContent_ReturnsLearningElement()
    {
        var learningContent = new FileContent("fa", "a", "");
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContent("foobar").Returns(learningContent);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningContent("foobar");

        Assert.That(learningElementActual, Is.EqualTo(learningContent));
    }

    [Test]
    public void LoadLearningWorldFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningWorld(stream);

        mockDataAccess.Received().LoadLearningWorld(stream);
    }

    [Test]
    public void LoadLearningWorldFromStream_ReturnsLearningWorld()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningWorld(stream).Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld(stream);

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void LoadLearningSpaceFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpace(stream);

        mockDataAccess.Received().LoadLearningSpace(stream);
    }

    [Test]
    public void LoadLearningSpaceFromStream_ReturnsLearningSpace()
    {
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f", 0);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningSpace(stream).Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace(stream);

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void LoadLearningElementFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElement(stream);

        mockDataAccess.Received().LoadLearningElement(stream);
    }

    [Test]
    public void LoadLearningElementFromStream_ReturnsLearningElement()
    {
        var content = new FileContent("a", "b", "");
        var learningElement = new LearningElement("fa", "a", content,"f", "f",
            "f", LearningElementDifficultyEnum.Easy);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningElement(stream).Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElement(stream);

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }

    [Test]
    public void LoadLearningContentFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningContent("filename.extension", stream);

        mockDataAccess.Received().LoadLearningContent("filename.extension", stream);
    }

    [Test]
    public void LoadLearningContentFromStream_ReturnsLearningElement()
    {
        var learningContent = new FileContent("filename", "extension", "");
        var stream = Substitute.For<MemoryStream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContent("filename.extension", stream).Returns(learningContent);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningContent("filename.extension", stream);

        Assert.That(learningElementActual, Is.EqualTo(learningContent));
    }

    [Test]
    public void FindSuitableNewSavePath_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        systemUnderTest.FindSuitableNewSavePath("foo", "bar", "baz");

        dataAccess.Received().FindSuitableNewSavePath("foo", "bar", "baz");
    }

    private BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IAuthoringToolConfiguration? fakeConfiguration = null,
        IDataAccess? fakeDataAccess = null,
        IWorldGenerator? worldGenerator = null,
        ICommandStateManager? commandStateManager = null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        worldGenerator ??= Substitute.For<IWorldGenerator>();
        commandStateManager ??= Substitute.For<ICommandStateManager>();
        
        return new BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess, worldGenerator,
            commandStateManager);
    }
}