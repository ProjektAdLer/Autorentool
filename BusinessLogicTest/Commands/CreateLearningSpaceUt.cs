using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;


[TestFixture]

public class CreateLearningSpaceUt
{
    [Test]
    public void Execute_CreatesLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var requiredPoints = 10;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningSpace(world, name, shortname, authors, description, goals, requiredPoints, mappingAction);
        
        Assert.IsEmpty(world.LearningSpaces);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var space = world.LearningSpaces.First();
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
        });
    }
    
    [Test]
    public void Execute_AddsLearningSpaceAndSetAsSelectedLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("z","y","x","w","v", 5);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningSpace(world, space, mappingAction);
        
        Assert.IsEmpty(world.LearningSpaces);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(world.SelectedLearningSpace, Is.Null);

        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.LearningSpaces.First(), Is.EqualTo(space));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var requiredPoints = 10;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningSpace(world, name, shortname, authors, description, goals, requiredPoints, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "h", "i", "j", "k", 5);
        world.LearningSpaces.Add(space);
        world.SelectedLearningSpace = space;
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var requiredPoints = 10;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreateLearningSpace(world, name, shortname, authors, description, goals, requiredPoints, mappingAction);
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(command.LearningSpace));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(command.LearningSpace));
        Assert.IsTrue(actionWasInvoked);
    }
}