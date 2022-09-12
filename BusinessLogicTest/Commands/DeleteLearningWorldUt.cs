using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class DeleteLearningWorldUt
{
    [Test]
    public void Execute_DeletesLearningSpace()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
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
    public void Undo_MementoIsNull_ThrowsException()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
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
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        workspace.LearningWorlds.Add(world);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction);

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(0));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Undo();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Redo();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(0));
        Assert.IsTrue(actionWasInvoked);
    }
}