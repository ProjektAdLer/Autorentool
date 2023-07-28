using System.Runtime.Serialization;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.ErrorManagement;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Shared;
using Shared.Command;
using Shared.Configuration;
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
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess:dataAccess);
        
        systemUnderTest.GetAllContent();
        dataAccess.Received().GetAllContent();
    }

    [Test]
    public void RemoveContent_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess:dataAccess);
        var content = new FileContent("foo", "bar", "baz");
        
        systemUnderTest.RemoveContent(content);
        dataAccess.Received().RemoveContent(content);
    }
    
    [Test]
    public void RemoveContent_ArgumentOutOfRangeException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(x => throw new ArgumentOutOfRangeException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess:dataAccess, errorManager:errorManager);
        var content = new FileContent("foo", "bar", "baz");
        
        systemUnderTest.RemoveContent(content);
        errorManager.Received(1).LogAndRethrowError(Arg.Any<ArgumentOutOfRangeException>());  
    }
    
    [Test]
    public void RemoveContent_FileNotFoundException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(x => throw new FileNotFoundException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess:dataAccess, errorManager:errorManager);
        var content = new FileContent("foo", "bar", "baz");
        
        systemUnderTest.RemoveContent(content);
        errorManager.Received(1).LogAndRethrowError(Arg.Any<FileNotFoundException>());  
    }
    
    [Test]
    public void RemoveContent_SerializationException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.RemoveContent(Arg.Any<ILearningContent>()))
            .Do(x => throw new SerializationException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess:dataAccess, errorManager:errorManager);
        var content = new FileContent("foo", "bar", "baz");
        
        systemUnderTest.RemoveContent(content);
        errorManager.Received(1).LogAndRethrowError(Arg.Any<SerializationException>());  
    }
    
    [Test]
    public void SaveLink_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess:dataAccess);
        var content = new LinkContent("foo", "bar");
        
        systemUnderTest.SaveLink(content);
        dataAccess.Received().SaveLink(content);
    }
    
    [Test]
    public void SaveLink_SerializationException_CallsErrorManager()
    {
        var errorManager = Substitute.For<IErrorManager>();
        var dataAccess = Substitute.For<IDataAccess>();
        dataAccess.When(x => x.SaveLink(Arg.Any<LinkContent>()))
            .Do(x => throw new SerializationException("test"));
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess:dataAccess, errorManager:errorManager);
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
        mockWorldGenerator.When(wg => wg.ConstructBackup(null!, "foobar")).Do(x => { throw new ArgumentOutOfRangeException(); });

        var systemUnderTest = CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator, errorManager: mockErrorManager);
        
        systemUnderTest.ConstructBackup(null!, "foobar");
        mockErrorManager.Received().LogAndRethrowGeneratorError(Arg.Any<ArgumentOutOfRangeException>());
    }

    [Test]
    public void ConstructBackup_ThrowsInvalidOperationException_LogAndRethrowErrorCalled()
    {
        var mockWorldGenerator = Substitute.For<IWorldGenerator>();
        var mockErrorManager = Substitute.For<IErrorManager>();
        mockWorldGenerator.When(wg => wg.ConstructBackup(null!, "foobar")).Do(x => { throw new InvalidOperationException(); });

        var systemUnderTest = CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator, errorManager: mockErrorManager);
        
        systemUnderTest.ConstructBackup(null!, "foobar");
        
        mockErrorManager.Received().LogAndRethrowGeneratorError(Arg.Any<InvalidOperationException>());
    }

    [Test]
    public void ConstructBackup_ThrowsFileNotFoundException_LogAndRethrowErrorCalled()
    {
        var mockWorldGenerator = Substitute.For<IWorldGenerator>();
        var mockErrorManager = Substitute.For<IErrorManager>();
        mockWorldGenerator.When(wg => wg.ConstructBackup(null!, "foobar")).Do(x => { throw new FileNotFoundException(); });

        var systemUnderTest = CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator, errorManager: mockErrorManager);
        
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
    public void CallUndoCommand_UndoThrowsInvalidOperationException_ErrorManagerCalled()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.When(x => x.Undo()).Do(x => throw new InvalidOperationException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(
            commandStateManager: mockCommandStateManager,
            errorManager: mockErrorManager
        );

        systemUnderTest.UndoCommand();
        
        mockErrorManager.Received().LogAndRethrowUndoError(Arg.Any<InvalidOperationException>());
    }

    [Test]
    public void CallUndoCommand_UndoThrowsArgumentOutOfRangeException_ErrorManagerCalled()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.When(x => x.Undo()).Do(x => throw new ArgumentOutOfRangeException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(
            commandStateManager: mockCommandStateManager,
            errorManager: mockErrorManager
        );

        systemUnderTest.UndoCommand();
        
        mockErrorManager.Received().LogAndRethrowUndoError(Arg.Any<ArgumentOutOfRangeException>());
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
    public void CallRedoCommand_RedoThrowsInvalidOperationException_ErrorManagerCalled()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.When(x => x.Redo()).Do(x => throw new InvalidOperationException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(
            commandStateManager: mockCommandStateManager,
            errorManager: mockErrorManager
        );

        systemUnderTest.RedoCommand();
        
        mockErrorManager.Received().LogAndRethrowRedoError(Arg.Any<InvalidOperationException>());
    }

    [Test]
    public void CallRedoCommand_RedoThrowsApplicationException_ErrorManagerCalled()
    {
        var mockCommandStateManager = Substitute.For<ICommandStateManager>();
        mockCommandStateManager.When(x => x.Redo()).Do(x => throw new ApplicationException());
        var mockErrorManager = Substitute.For<IErrorManager>();

        var systemUnderTest = CreateStandardBusinessLogic(
            commandStateManager: mockCommandStateManager,
            errorManager: mockErrorManager
        );

        systemUnderTest.RedoCommand();
        
        mockErrorManager.Received().LogAndRethrowRedoError(Arg.Any<ApplicationException>());
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
    public void SaveLearningWorld_SerializationException_CallsErrorManager()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.When(x => x.SaveLearningWorldToFile(Arg.Any<LearningWorld>(), Arg.Any<string>()))
            .Do(x => throw new SerializationException());
        var mockErrorManager = Substitute.For<IErrorManager>();
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
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
    public void LoadLearningWorld_SerializationException_CallsErrorManager()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.When(x => x.LoadLearningWorld(Arg.Any<string>()))
            .Do(x => throw new SerializationException());
        var mockErrorManager = Substitute.For<IErrorManager>();
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.LoadLearningWorld("foobar");

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    public void SaveLearningSpace_CallsDataAccess()
    {
        var learningSpace = new LearningSpace("fa", "f", "f", 0, Theme.Campus);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningSpace(learningSpace, "foobar");

        mockDataAccess.Received().SaveLearningSpaceToFile(learningSpace, "foobar");
    }
    
    [Test]
    public void SaveLearningSpace_SerializationException_CallsErrorManager()
    {
        var learningSpace = new LearningSpace("fa", "f", "f", 0, Theme.Campus);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.When(x => x.SaveLearningSpaceToFile(Arg.Any<LearningSpace>(), Arg.Any<string>()))
            .Do(x => throw new SerializationException());
        var mockErrorManager = Substitute.For<IErrorManager>();
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.SaveLearningSpace(learningSpace, "foobar");

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    public void LoadLearningSpace_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var space = EntityProvider.GetLearningSpace();
        mockDataAccess.LoadLearningSpace("foobar").Returns(space);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpace("foobar");

        mockDataAccess.Received().LoadLearningSpace("foobar");
    }

    [Test]
    public void LoadLearningSpace_ReturnsLearningSpace()
    {
        var learningSpace = new LearningSpace("fa", "f", "f", 0, Theme.Campus);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningSpace("foobar").Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace("foobar");

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }
    
    [Test]
    public void LoadLearningSpace_SerializationException_CallsErrorManager()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.When(x => x.LoadLearningSpace(Arg.Any<string>()))
            .Do(x => throw new SerializationException());
        var mockErrorManager = Substitute.For<IErrorManager>();
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.LoadLearningSpace("foobar");

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    public void SaveLearningElement_CallsDataAccess()
    {
        var learningElement = EntityProvider.GetLearningElement();
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningElement(learningElement, "foobar");

        mockDataAccess.Received().SaveLearningElementToFile(learningElement, "foobar");
    }
    
    [Test]
    public void SaveLearningElement_SerializationException_CallsErrorManager()
    {
        var learningElement = EntityProvider.GetLearningElement();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.When(x => x.SaveLearningElementToFile(Arg.Any<LearningElement>(), Arg.Any<string>()))
            .Do(x => throw new SerializationException());
        var mockErrorManager = Substitute.For<IErrorManager>();
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.SaveLearningElement(learningElement, "foobar");

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    public void LoadLearningElement_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var element = EntityProvider.GetLearningElement();
        mockDataAccess.LoadLearningElement("foobar").Returns(element);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElement("foobar");

        mockDataAccess.Received().LoadLearningElement("foobar");
    }

    [Test]
    public void LoadLearningElement_ReturnsLearningElement()
    {
        var learningElement = EntityProvider.GetLearningElement();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningElement("foobar").Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElement("foobar");

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }
    
    [Test]
    public void LoadLearningElement_SerializationException_CallsErrorManager()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.When(x => x.LoadLearningElement(Arg.Any<string>()))
            .Do(x => throw new SerializationException());
        var mockErrorManager = Substitute.For<IErrorManager>();
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.LoadLearningElement("foobar");

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
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
    public void GetSavedLearningWorldPaths_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.GetSavedLearningWorldPaths();

        mockDataAccess.Received().GetSavedLearningWorldPaths();
    }

    [Test]
    public void AddSavedLearningWorldPath_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var savedLearningWorldPath = new SavedLearningWorldPath();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.AddSavedLearningWorldPath(savedLearningWorldPath);

        mockDataAccess.Received().AddSavedLearningWorldPath(savedLearningWorldPath);
    }

    [Test]
    public void AddSavedLearningWorldPathByPathOnly_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var path = "path";

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.AddSavedLearningWorldPathByPathOnly(path);

        mockDataAccess.Received().AddSavedLearningWorldPathByPathOnly(path);
    }

    [Test]
    public void AddSavedLearningWorldPathByPathOnly_ReturnsSavedLearningWorldPath()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var path = "path";
        var savedLearningWorldPath = new SavedLearningWorldPath();
        mockDataAccess.AddSavedLearningWorldPathByPathOnly(path).Returns(savedLearningWorldPath);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var savedLearningWorldPathActual = systemUnderTest.AddSavedLearningWorldPathByPathOnly(path);

        Assert.That(savedLearningWorldPathActual, Is.EqualTo(savedLearningWorldPath));
    }

    [Test]
    public void UpdateIdOfSavedLearningWorldPath_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var savedLearningWorldPath = new SavedLearningWorldPath();
        var changedId = Guid.NewGuid();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, changedId);

        mockDataAccess.Received().UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, changedId);
    }

    [Test]
    public void RemoveSavedLearningWorldPath_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var savedLearningWorldPath = new SavedLearningWorldPath();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.RemoveSavedLearningWorldPath(savedLearningWorldPath);

        mockDataAccess.Received().RemoveSavedLearningWorldPath(savedLearningWorldPath);
    }

    [Test]
    public void LoadLearningWorldFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();
        var learningWorld = EntityProvider.GetLearningWorld();
        mockDataAccess.LoadLearningWorld(stream).Returns(learningWorld);

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
    public void LoadLearningWorldFromStream_SerializationException_CallsErrorManager()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();
        mockDataAccess.When(x => x.LoadLearningWorld(Arg.Any<Stream>()))
            .Do(x => { throw new SerializationException(); });
        
        var mockErrorManager = Substitute.For<IErrorManager>();
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);

        systemUnderTest.LoadLearningWorld(stream);

        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
    }

    [Test]
    public void LoadLearningSpaceFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();
        var space = EntityProvider.GetLearningSpace();
        mockDataAccess.LoadLearningSpace(stream).Returns(space);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpace(stream);

        mockDataAccess.Received().LoadLearningSpace(stream);
    }
    
    [Test]
    public void LoadLearningSpaceFromStream_SerializationException_CallsErrorManager()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();
        mockDataAccess.When(x => x.LoadLearningSpace(Arg.Any<Stream>()))
            .Do(x => { throw new SerializationException(); });
        
        var mockErrorManager = Substitute.For<IErrorManager>();
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);
        
        systemUnderTest.LoadLearningSpace(stream);
        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
        
    }

    [Test]
    public void LoadLearningSpaceFromStream_ReturnsLearningSpace()
    {
        var learningSpace = new LearningSpace("fa", "f", "f", 0, Theme.Campus);
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
        var element = EntityProvider.GetLearningElement();
        mockDataAccess.LoadLearningElement(stream).Returns(element);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElement(stream);

        mockDataAccess.Received().LoadLearningElement(stream);
    }

    [Test]
    public void LoadLearningElementFromStream_ReturnsLearningElement()
    {
        var learningElement = EntityProvider.GetLearningElement();
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningElement(stream).Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElement(stream);

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }
    
    [Test]
    public void LoadLearningElementFromStream_SerializationException_CallsErrorManager()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();
        mockDataAccess.When(x => x.LoadLearningElement(Arg.Any<Stream>()))
            .Do(x => { throw new SerializationException(); });
        
        var mockErrorManager = Substitute.For<IErrorManager>();
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, errorManager: mockErrorManager);
        
        systemUnderTest.LoadLearningElement(stream);
        mockErrorManager.Received().LogAndRethrowError(Arg.Any<SerializationException>());
        
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
    public void LoadLearningContentFromStream_IOException_CallsErrorManager()
    {
        var stream = Substitute.For<MemoryStream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContent("filename.extension", stream).Returns(x => { throw new IOException(); });
        
        var errorManager = Substitute.For<IErrorManager>();
        var systemUnderTest = CreateStandardBusinessLogic(null,fakeDataAccess: mockDataAccess, errorManager: errorManager);

        systemUnderTest.LoadLearningContent("filename.extension", stream);

        errorManager.Received().LogAndRethrowError(Arg.Any<IOException>());
    }

    [Test]
    public void FindSuitableNewSavePath_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        systemUnderTest.FindSuitableNewSavePath("foo", "bar", "baz");

        dataAccess.Received().FindSuitableNewSavePath("foo", "bar", "baz");
    }

    #region BackendAccess

    [Test]
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
        mockConfiguration[IApplicationConfiguration.BackendToken].Returns(tokenString);

        var systemUnderTest = CreateStandardBusinessLogic(apiAccess: backendAccess);

        await systemUnderTest.Login(username, password);

        await backendAccess.Received().GetUserInformationAsync(Arg.Is<UserToken>(t => t.Token == tokenString));
    }

    [Test]
    public void UploadLearningWorldToBackend_CallsWorldGenerator()
    {
        var worldGenerator = Substitute.For<IWorldGenerator>();
        const string filepath = "filepath";
        var systemUnderTest = CreateStandardBusinessLogic(worldGenerator: worldGenerator);

        systemUnderTest.UploadLearningWorldToBackend(filepath);

        worldGenerator.Received().ExtractAtfFromBackup(filepath);
    }

    [Test]
    public void UploadLearningWorldToBackend_CallsBackendAccess()
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

        systemUnderTest.UploadLearningWorldToBackend(filepath, mockProgress);

        backendAccess.Received()
            .UploadLearningWorldAsync(Arg.Is<UserToken>(c => c.Token == "token"), filepath, atfPath, mockProgress);
    }

    #endregion

    private BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IApplicationConfiguration? fakeConfiguration = null,
        IDataAccess? fakeDataAccess = null,
        IWorldGenerator? worldGenerator = null,
        ICommandStateManager? commandStateManager = null,
        IBackendAccess? apiAccess = null,
        IErrorManager? errorManager = null,
        ILogger<BusinessLogic.API.BusinessLogic>? logger = null)
    {
        fakeConfiguration ??= Substitute.For<IApplicationConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        worldGenerator ??= Substitute.For<IWorldGenerator>();
        commandStateManager ??= Substitute.For<ICommandStateManager>();
        apiAccess ??= Substitute.For<IBackendAccess>();
        errorManager ??= Substitute.For<IErrorManager>();
        logger ??= Substitute.For<ILogger<BusinessLogic.API.BusinessLogic>>();

        return new BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess, worldGenerator,
            commandStateManager, apiAccess, errorManager, logger);
    }
}