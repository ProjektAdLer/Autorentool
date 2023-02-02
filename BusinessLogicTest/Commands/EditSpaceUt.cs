using BusinessLogic.Commands;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class EditSpaceUt
{
    [Test]
    public void Execute_EditsSpace()
    {
        var space = new Space("a", "b", "c", "d", "e", 5);
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learn";
        var goals = "learn";
        var requiredPoints = 10;
        bool actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new EditSpace(space, name, shortname, authors, description, goals, requiredPoints, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("a"));
            Assert.That(space.Shortname, Is.EqualTo("b"));
            Assert.That(space.Authors, Is.EqualTo("c"));
            Assert.That(space.Description, Is.EqualTo("d"));
            Assert.That(space.Goals, Is.EqualTo("e"));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learn"));
            Assert.That(space.Goals, Is.EqualTo("learn"));
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var space = new Space("a", "b", "c", "d", "e", 5);
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learn";
        var goals = "learn";
        var requiredPoints = 10;
        bool actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new EditSpace(space, name, shortname, authors, description, goals, requiredPoints, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesEditSpace()
    {
        var space = new Space("g", "h", "i", "j", "k", 5);
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learn";
        var goals = "learn";
        var requiredPoints = 10;
        bool actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;
        
        var command = new EditSpace(space, name, shortname, authors, description, goals, requiredPoints, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Shortname, Is.EqualTo("h"));
            Assert.That(space.Authors, Is.EqualTo("i"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.Goals, Is.EqualTo("k"));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learn"));
            Assert.That(space.Goals, Is.EqualTo("learn"));
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Shortname, Is.EqualTo("h"));
            Assert.That(space.Authors, Is.EqualTo("i"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.Goals, Is.EqualTo("k"));
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learn"));
            Assert.That(space.Goals, Is.EqualTo("learn"));
        });
    }
}