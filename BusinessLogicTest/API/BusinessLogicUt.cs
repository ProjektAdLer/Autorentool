using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Shared;
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
        var mockOnUndoRedoPerformed = Substitute.For<Action>();
        
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);
        
        systemUnderTest.OnUndoRedoPerformed += mockOnUndoRedoPerformed;
        systemUnderTest.ExecuteCommand(mockCommand);
        
        mockOnUndoRedoPerformed.Received().Invoke();
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
        var mockOnUndoRedoPerformed = Substitute.For<Action>();
        
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);
        
        systemUnderTest.OnUndoRedoPerformed += mockOnUndoRedoPerformed;
        systemUnderTest.UndoCommand();
        
        mockOnUndoRedoPerformed.Received().Invoke();
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
        var mockOnUndoRedoPerformed = Substitute.For<Action>();
        
        var systemUnderTest = CreateStandardBusinessLogic(commandStateManager: mockCommandStateManager);
        
        systemUnderTest.OnUndoRedoPerformed += mockOnUndoRedoPerformed;
        systemUnderTest.RedoCommand();
        
        mockOnUndoRedoPerformed.Received().Invoke();
    }

    [Test]
    public void SaveWorld_CallsDataAccess()
    {
        var world = new World("fa", "a", "f", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveWorld(world, "foobar");

        mockDataAccess.Received().SaveWorldToFile(world, "foobar");
    }

    [Test]
    public void LoadWorld_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadWorld("foobar");

        mockDataAccess.Received().LoadWorld("foobar");
    }

    [Test]
    public void LoadWorld_ReturnsWorld()
    {
        var world = new World("fa", "a", "f", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadWorld("foobar").Returns(world);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var worldActual = systemUnderTest.LoadWorld("foobar");

        Assert.That(worldActual, Is.EqualTo(world));
    }

    [Test]
    public void SaveSpace_CallsDataAccess()
    {
        var space = new Space("fa", "a", "f", "f", "f", 0);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveSpace(space, "foobar");

        mockDataAccess.Received().SaveSpaceToFile(space, "foobar");
    }

    [Test]
    public void LoadSpace_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadSpace("foobar");

        mockDataAccess.Received().LoadSpace("foobar");
    }

    [Test]
    public void LoadSpace_ReturnsSpace()
    {
        var space = new Space("fa", "a", "f", "f", "f", 0);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadSpace("foobar").Returns(space);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var spaceActual = systemUnderTest.LoadSpace("foobar");

        Assert.That(spaceActual, Is.EqualTo(space));
    }

    [Test]
    public void SaveElement_CallsDataAccess()
    {
        var content = new Content("a", "b", "");
        var element = new Element("fa", "f", content, "","f",
            "f", "f", ElementDifficultyEnum.Easy);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveElement(element, "foobar");

        mockDataAccess.Received().SaveElementToFile(element, "foobar");
    }

    [Test]
    public void LoadElement_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadElement("foobar");

        mockDataAccess.Received().LoadElement("foobar");
    }

    [Test]
    public void LoadElement_ReturnsElement()
    {
        var content = new Content("a", "b", "");
        var element = new Element("fa", "a", content, "", "f", "f",
            "f", ElementDifficultyEnum.Easy);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadElement("foobar").Returns(element);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var elementActual = systemUnderTest.LoadElement("foobar");

        Assert.That(elementActual, Is.EqualTo(element));
    }

    [Test]
    public void LoadContent_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadContent("foobar");

        mockDataAccess.Received().LoadContent("foobar");
    }

    [Test]
    public void LoadContent_ReturnsElement()
    {
        var content = new Content("fa", "a", "");
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadContent("foobar").Returns(content);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var elementActual = systemUnderTest.LoadContent("foobar");

        Assert.That(elementActual, Is.EqualTo(content));
    }

    [Test]
    public void LoadWorldFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadWorld(stream);

        mockDataAccess.Received().LoadWorld(stream);
    }

    [Test]
    public void LoadWorldFromStream_ReturnsWorld()
    {
        var world = new World("fa", "a", "f", "f", "f", "f");
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadWorld(stream).Returns(world);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var worldActual = systemUnderTest.LoadWorld(stream);

        Assert.That(worldActual, Is.EqualTo(world));
    }

    [Test]
    public void LoadSpaceFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadSpace(stream);

        mockDataAccess.Received().LoadSpace(stream);
    }

    [Test]
    public void LoadSpaceFromStream_ReturnsSpace()
    {
        var space = new Space("fa", "a", "f", "f", "f", 0);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadSpace(stream).Returns(space);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var spaceActual = systemUnderTest.LoadSpace(stream);

        Assert.That(spaceActual, Is.EqualTo(space));
    }

    [Test]
    public void LoadElementFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadElement(stream);

        mockDataAccess.Received().LoadElement(stream);
    }

    [Test]
    public void LoadElementFromStream_ReturnsElement()
    {
        var content = new Content("a", "b", "");
        var element = new Element("fa", "a", content, "","f", "f",
            "f", ElementDifficultyEnum.Easy);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadElement(stream).Returns(element);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var elementActual = systemUnderTest.LoadElement(stream);

        Assert.That(elementActual, Is.EqualTo(element));
    }

    [Test]
    public void LoadContentFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<MemoryStream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadContent("filename.extension", stream);

        mockDataAccess.Received().LoadContent("filename.extension", stream);
    }

    [Test]
    public void LoadContentFromStream_ReturnsElement()
    {
        var content = new Content("filename", "extension", "");
        var stream = Substitute.For<MemoryStream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadContent("filename.extension", stream).Returns(content);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var elementActual = systemUnderTest.LoadContent("filename.extension", stream);

        Assert.That(elementActual, Is.EqualTo(content));
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