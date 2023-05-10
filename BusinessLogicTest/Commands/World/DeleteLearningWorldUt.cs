using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class DeleteLearningWorldUt
{
    [Test]
    public void Execute_DeletesLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(new List<LearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        workspace.LearningWorlds.Add(world);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction);

        Assert.That(workspace.LearningWorlds, Does.Contain(world));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(workspace.LearningWorlds, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_DeletesLearningWorldAndSetsAnotherLearningWorldAsSelected()
    {
        var workspace = new AuthoringToolWorkspace(new List<LearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var world2 = new LearningWorld("g", "h", "i", "j", "k", "l");
        workspace.LearningWorlds.Add(world);
        workspace.LearningWorlds.Add(world2);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction);

        Assert.That(workspace.LearningWorlds.Count, Is.EqualTo(2));
        Assert.That(workspace.LearningWorlds, Does.Contain(world));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        
        Assert.That(workspace.LearningWorlds.Count, Is.EqualTo(1));
        Assert.That(workspace.LearningWorlds, Does.Not.Contain(world));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var workspace = new AuthoringToolWorkspace(new List<LearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        workspace.LearningWorlds.Add(world);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(new List<LearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var world2 = new LearningWorld("g", "h", "i", "j", "k", "l");
        workspace.LearningWorlds.Add(world);
        workspace.LearningWorlds.Add(world2);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction);

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Undo();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Redo();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
    }
}