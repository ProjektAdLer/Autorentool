using System;
using NSubstitute;
using NUnit.Framework;

namespace PresentationTest.BusinessLogic.Commands;

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
    
    private CommandStateManager GetCommandStateManagerForTest() => new();
}