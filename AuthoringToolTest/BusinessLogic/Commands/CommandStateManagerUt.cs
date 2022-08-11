using AuthoringTool.BusinessLogic.Commands;
using NUnit.Framework;

namespace AuthoringToolTest.BusinessLogic.Commands;

[TestFixture]
public class CommandStateManagerUt
{
    [Test]
    public void Execute_WithCommand_CallsExecuteOnCommand()
    {
        Assert.Fail("NYI");
    }

    [Test]
    public void Execute_WithCommand_DoesNotPutCommandOnUndoStack()
    {
        Assert.Fail("NYI");
    }
    
    [Test]
    public void Execute_WithCommand_ClearsRedoStack()
    {
        Assert.Fail("NYI");
    }

    [Test]
    public void Execute_WithUndoCommand_DoesPutCommandOnUndoStack()
    {
        Assert.Fail("NYI");
    }

    [Test]
    public void Undo_AfterExecuteUndoCommand_CallsUndoOnCommand()
    {
        Assert.Fail("NYI");
    }

    [Test]
    public void Undo_AfterExecuteUndoCommand_PutsCommandOnRedoStack()
    {
        Assert.Fail("NYI");
    }

    [Test]
    public void Undo_WithCanUndoFalse_ThrowsException()
    {
        Assert.Fail("NYI");
    }
    
    [Test]
    public void Redo_AfterUndoingUndoCommand_CallsRedoOnCommand()
    {
        Assert.Fail("NYI");
    }

    [Test]
    public void Redo_AfterUndoingUndoCommand_PutsCommandOnUndoStack()
    {
        Assert.Fail("NYI");
    }
    
    [Test]
    public void Redo_WithCanRedoFalse_ThrowsException()
    {
        Assert.Fail("NYI");
    }
    
    private CommandStateManager GetCommandStateManagerForTest() => new();
}