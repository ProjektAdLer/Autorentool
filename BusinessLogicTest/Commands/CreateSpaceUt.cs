using BusinessLogic.Commands;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;


[TestFixture]

public class CreateSpaceUt
{
    [Test]
    public void Execute_CreatesSpace()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "good space";
        var goals = "learn";
        var requiredPoints = 10;
        var positionX = 1;
        var positionY = 2;
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateSpace(world, name, shortname, authors, description, goals, requiredPoints, positionX, positionY, mappingAction);
        
        Assert.IsEmpty(world.Spaces);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var space = world.Spaces.First();
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("good space"));
            Assert.That(space.Goals, Is.EqualTo("learn"));
        });
    }
    
    [Test]
    public void Execute_AddsSpaceAndSetAsSelectedObject()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space = new Space("z","y","x","w","v", 5);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateSpace(world, space, mappingAction);
        
        Assert.IsEmpty(world.Spaces);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(world.SelectedObject, Is.Null);

        command.Execute();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.Spaces.First(), Is.EqualTo(space));
        Assert.That(world.SelectedObject, Is.EqualTo(space));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space";
        var goals = "learn";
        var requiredPoints = 10;
        var positionX = 1;
        var positionY = 2;
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateSpace(world, name, shortname, authors, description, goals, requiredPoints, positionX, positionY, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateSpace()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space = new Space("g", "h", "i", "j", "k", 5);
        world.Spaces.Add(space);
        world.SelectedObject = space;
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space";
        var goals = "learn";
        var requiredPoints = 10;
        var positionX = 1;
        var positionY = 2;
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreateSpace(world, name, shortname, authors, description, goals, requiredPoints, positionX, positionY, mappingAction);
        
        Assert.That(world.Spaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedObject, Is.EqualTo(space));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedObject, Is.EqualTo(command.Space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedObject, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedObject, Is.EqualTo(command.Space));
        Assert.IsTrue(actionWasInvoked);
    }
}