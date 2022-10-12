using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class CommandStateManagerUt
{
    [Test]
    public void Execute_WithCommand_CallsExecuteOnCommand()
    {
        var commandMock = Substitute.For<ICommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        
        systemUnderTest.Execute(commandMock);
        
        commandMock.Received().Execute();
    }

    [Test]
    public void Execute_WithCommand_DoesNotPutCommandOnUndoStack()
    {
        var commandMock = Substitute.For<ICommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        
        systemUnderTest.Execute(commandMock);
     
        Assert.That(systemUnderTest.CanUndo, Is.False);
    }
    
    [Test]
    public void Execute_WithCommand_ClearsRedoStack()
    {
        var commandMock = Substitute.For<ICommand>();
        var undoCommandMock = Substitute.For<IUndoCommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        
        systemUnderTest.Execute(undoCommandMock);
        
        Assert.That(systemUnderTest.CanUndo);
        systemUnderTest.Undo();
        Assert.That(systemUnderTest.CanRedo);
        systemUnderTest.Execute(commandMock);
        Assert.That(systemUnderTest.CanRedo, Is.False);
    }

    [Test]
    public void Execute_WithUndoCommand_DoesPutCommandOnUndoStack()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        
        systemUnderTest.Execute(undoCommandMock);
        
        Assert.That(systemUnderTest.CanUndo);
    }

    [Test]
    public void Undo_AfterExecuteUndoCommand_CallsUndoOnCommand()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        
        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();
     
        undoCommandMock.Received().Undo();
    }

    [Test]
    public void Undo_AfterExecuteUndoCommand_PutsCommandOnRedoStack()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        
        Assert.That(systemUnderTest.CanRedo, Is.False);
        
        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();
        
        Assert.That(systemUnderTest.CanRedo, Is.True);
    }

    [Test]
    public void Undo_WithCanUndoFalse_ThrowsInvalidOperationException()
    {
        var systemUnderTest = GetCommandStateManagerForTest();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CanUndo, Is.False);
            Assert.That(() => systemUnderTest.Undo(),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("no command to undo"));
        });
    }

    [Test]
    public void Redo_AfterUndoingUndoCommand_CallsRedoOnCommand()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        
        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();
        systemUnderTest.Redo();
        undoCommandMock.Received().Redo();
    }

    [Test]
    public void Redo_AfterUndoingUndoCommand_PutsCommandOnUndoStack()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        
        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();
        
        Assert.That(systemUnderTest.CanUndo, Is.False);
        
        systemUnderTest.Redo();
        
        Assert.That(systemUnderTest.CanUndo, Is.True);
    }
    
    [Test]
    public void Redo_WithCanRedoFalse_ThrowsInvalidOperationException()
    {
        var systemUnderTest = GetCommandStateManagerForTest();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CanRedo, Is.False);
            Assert.That(() => systemUnderTest.Redo(),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("no command to redo"));
        });
    }
    
    [Test]
    public void ExecuteCommandAfterRedoingAnCommand_TriggersRemovedCommandsFromStacksWithRemainingCommandsInStacks(){
        var undoCommandMock = Substitute.For<IUndoCommand>();
        var undoCommandMock2 = Substitute.For<IUndoCommand>();
        var undoCommandMock3 = Substitute.For<IUndoCommand>();
        
        var systemUnderTest = GetCommandStateManagerForTest();
        var wasCalled = false;
        var eventObjects = new List<object>();
        systemUnderTest.RemovedCommandsFromStacks += (sender, args) =>
        {
            wasCalled = true;
            eventObjects = args.ObjectsInStacks.ToList();
        };
        
        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();
        systemUnderTest.Redo();
        systemUnderTest.Execute(undoCommandMock2);
        systemUnderTest.Undo();
        systemUnderTest.Execute(undoCommandMock3);
        
        Assert.That(systemUnderTest.CanUndo, Is.True);
        Assert.That(systemUnderTest.CanRedo, Is.False);
        Assert.That(wasCalled, Is.True);
        // eventObjects has count 0 because the mocked commands do not return an object in GetObjectFromCommand
        Assert.That(eventObjects, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void ExecuteCommandAfterRedoingAnCommand_TriggersRemovedCommandsFromStacksWithRemainingCommandsInStacks_AllCommandTypes()
    {
        var learningElement =
            new LearningElement("n", "s", null!, "u", "a", "d", "g", LearningElementDifficultyEnum.Easy);
        var learningSpace = new LearningSpace("n", "s", "a", "d", "g", 5);
        var learningWorld = new LearningWorld("n", "s", "a", "l","d", "g");
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var createLearningElementCommand = new CreateLearningElement(learningSpace, learningElement, _ => { });
        var createLearningSpaceCommand = new CreateLearningSpace(learningWorld, learningSpace, _ => { });
        var createLearningWorldCommand = new CreateLearningWorld(workspace, learningWorld, _ => { });
        var deleteLearningElementCommand = new DeleteLearningElement(learningElement, learningSpace, _ => { });
        var deleteLearningSpaceCommand = new DeleteLearningSpace(learningWorld, learningSpace, _ => { });
        var deleteLearningWorldCommand = new DeleteLearningWorld(workspace, learningWorld, _ => { });
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningElement(Arg.Any<string>()).Returns(learningElement);
        mockBusinessLogic.LoadLearningSpace(Arg.Any<string>()).Returns(learningSpace);
        mockBusinessLogic.LoadLearningWorld(Arg.Any<string>()).Returns(learningWorld);
        var loadLearningElementCommand = new LoadLearningElement(learningSpace, "e", mockBusinessLogic, _ => { });
        var loadLearningSpaceCommand = new LoadLearningSpace(learningWorld, "s", mockBusinessLogic, _ => { });
        var loadLearningWorldCommand = new LoadLearningWorld(workspace, "w", mockBusinessLogic, _ => { });

        var secondLearningWorld = new LearningWorld("n2", "s2", "a2", "l2","d2", "g2");
        var thirdLearningWorld = new LearningWorld("n3", "s3", "a3", "l3","d3", "g3");
        var createSecondLearningWorldCommand = new CreateLearningWorld(workspace, secondLearningWorld, _ => { });
        var createThirdLearningWorldCommand = new CreateLearningWorld(workspace, thirdLearningWorld, _ => { });
        
        var systemUnderTest = GetCommandStateManagerForTest();
        var wasCalled = false;
        var eventObjects = new List<object>();
        systemUnderTest.RemovedCommandsFromStacks += (sender, args) =>
        {
            wasCalled = true;
            eventObjects = args.ObjectsInStacks.ToList();
        };
        
        systemUnderTest.Execute(createLearningWorldCommand);
        systemUnderTest.Execute(createLearningSpaceCommand);
        systemUnderTest.Execute(createLearningElementCommand);
        systemUnderTest.Execute(deleteLearningElementCommand);
        systemUnderTest.Execute(deleteLearningSpaceCommand);
        systemUnderTest.Execute(deleteLearningWorldCommand);
        systemUnderTest.Execute(loadLearningWorldCommand);
        systemUnderTest.Execute(loadLearningSpaceCommand);
        systemUnderTest.Execute(loadLearningElementCommand);
        // command to remove
        systemUnderTest.Execute(createSecondLearningWorldCommand);
        systemUnderTest.Undo();
        systemUnderTest.Execute(createThirdLearningWorldCommand);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CanUndo, Is.True);
            Assert.That(systemUnderTest.CanRedo, Is.False);
        });
        Assert.Multiple(() =>
        {
            Assert.That(wasCalled, Is.True);
            Assert.That(eventObjects, Has.Count.EqualTo(4));
        });
        Assert.Multiple(() =>
        {
        Assert.That(eventObjects, Has.Member(learningElement));
        Assert.That(eventObjects, Has.Member(learningSpace));
        Assert.That(eventObjects, Has.Member(learningWorld));
        Assert.That(eventObjects, Has.Member(thirdLearningWorld));
        });
    }

    private CommandStateManager GetCommandStateManagerForTest() => new();
}