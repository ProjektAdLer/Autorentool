using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class EditLearningSpaceUt
{
    [Test]
    public void Execute_EditsLearningSpace()
    {
        var space = new LearningSpace("a", "b", "c", "d", "e");
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var mappingAction = Substitute.For<Action<LearningSpace>>();

        var command = new EditLearningSpace(space, name, shortname, authors, description, goals, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("a"));
            Assert.That(space.Shortname, Is.EqualTo("b"));
            Assert.That(space.Authors, Is.EqualTo("c"));
            Assert.That(space.Description, Is.EqualTo("d"));
            Assert.That(space.Goals, Is.EqualTo("e"));
        });
        
        command.Execute();
        
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
    public void Undo_MementoIsNull_ThrowsException()
    {
        var space = new LearningSpace("a", "b", "c", "d", "e");
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var mappingAction = Substitute.For<Action<LearningSpace>>();

        var command = new EditLearningSpace(space, name, shortname, authors, description, goals, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningSpace()
    {
        var space = new LearningSpace("g", "h", "i", "j", "k");
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        
        var command = new EditLearningSpace(space, name, shortname, authors, description, goals, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Shortname, Is.EqualTo("h"));
            Assert.That(space.Authors, Is.EqualTo("i"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.Goals, Is.EqualTo("k"));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
        });
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Shortname, Is.EqualTo("h"));
            Assert.That(space.Authors, Is.EqualTo("i"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.Goals, Is.EqualTo("k"));
        });
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
        });
    }
}