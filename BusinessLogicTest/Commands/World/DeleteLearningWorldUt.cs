using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Theme;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class DeleteLearningWorldUt
{
    [Test]
    // ANF-ID: [ASE4]
    public void Execute_DeletesLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg);
        workspace.LearningWorlds.Add(world);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction, new NullLogger<DeleteLearningWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Does.Contain(world));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    // ANF-ID: [ASE4]
    public void Execute_DeletesLearningWorldAndSetsAnotherLearningWorldAsSelected()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg);
        var world2 = new LearningWorld("g", "h", "i", "j", "k", "l", WorldTheme.CampusAschaffenburg);
        workspace.LearningWorlds.Add(world);
        workspace.LearningWorlds.Add(world2);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction, new NullLogger<DeleteLearningWorld>());

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Does.Contain(world));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();


        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Does.Not.Contain(world));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}