using BusinessLogic.API;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class LoadLearningWorldUt
{
    [Test]
    // ANF-ID: [ASE2]
    public void Execute_LoadsLearningWorld()
    {
        var authoringToolWorkspace = EntityProvider.GetAuthoringToolWorkspace();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = EntityProvider.GetLearningWorld();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningWorld(filepath).Returns(world);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningWorld(authoringToolWorkspace, filepath, mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningWorld>());
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        mockBusinessLogic.Received().LoadLearningWorld(filepath);
        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds[0], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}