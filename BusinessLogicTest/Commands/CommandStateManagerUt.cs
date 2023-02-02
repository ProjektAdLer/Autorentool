using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.World;
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
        var element =
            new Element("n", "s", null!, "u", "a", "d", "g", ElementDifficultyEnum.Easy);
        var space = new Space("n", "s", "a", "d", "g", 5, new SpaceLayout(new IElement?[6],FloorPlanEnum.Rectangle2X3));
        var world = new World("n", "s", "a", "l","d", "g");
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var createElementCommand = new CreateElement(space, 0, element, _ => { });
        var createSpaceCommand = new CreateSpace(world, space, _ => { });
        var createWorldCommand = new CreateWorld(workspace, world, _ => { });
        var deleteElementCommand = new DeleteElement(element, space, _ => { });
        var deleteSpaceCommand = new DeleteSpace(world, space, _ => { });
        var deleteWorldCommand = new DeleteWorld(workspace, world, _ => { });
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadElement(Arg.Any<string>()).Returns(element);
        mockBusinessLogic.LoadSpace(Arg.Any<string>()).Returns(space);
        mockBusinessLogic.LoadWorld(Arg.Any<string>()).Returns(world);
        var loadElementCommand = new LoadElement(space, 0, "e", mockBusinessLogic, _ => { });
        var loadSpaceCommand = new LoadSpace(world, "s", mockBusinessLogic, _ => { });
        var loadWorldCommand = new LoadWorld(workspace, "w", mockBusinessLogic, _ => { });

        var secondWorld = new World("n2", "s2", "a2", "l2","d2", "g2");
        var thirdWorld = new World("n3", "s3", "a3", "l3","d3", "g3");
        var createSecondWorldCommand = new CreateWorld(workspace, secondWorld, _ => { });
        var createThirdWorldCommand = new CreateWorld(workspace, thirdWorld, _ => { });
        
        var systemUnderTest = GetCommandStateManagerForTest();
        var wasCalled = false;
        var eventObjects = new List<object>();
        systemUnderTest.RemovedCommandsFromStacks += (sender, args) =>
        {
            wasCalled = true;
            eventObjects = args.ObjectsInStacks.ToList();
        };
        
        systemUnderTest.Execute(createWorldCommand);
        systemUnderTest.Execute(createSpaceCommand);
        systemUnderTest.Execute(createElementCommand);
        systemUnderTest.Execute(deleteElementCommand);
        systemUnderTest.Execute(deleteSpaceCommand);
        systemUnderTest.Execute(deleteWorldCommand);
        systemUnderTest.Execute(loadWorldCommand);
        systemUnderTest.Execute(loadSpaceCommand);
        systemUnderTest.Execute(loadElementCommand);
        // command to remove
        systemUnderTest.Execute(createSecondWorldCommand);
        systemUnderTest.Undo();
        systemUnderTest.Execute(createThirdWorldCommand);
        
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
        Assert.That(eventObjects, Has.Member(element));
        Assert.That(eventObjects, Has.Member(space));
        Assert.That(eventObjects, Has.Member(world));
        Assert.That(eventObjects, Has.Member(thirdWorld));
        });
    }

    private CommandStateManager GetCommandStateManagerForTest() => new();
}