using BusinessLogic.API;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class SaveLearningWorldUt
{
    [Test]
    public void Execute_CallsBusinessLogic()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        const string filepath = "filepath";

        var command = new SaveLearningWorld(mockBusinessLogic, world, filepath, new NullLogger<SaveLearningWorld>());

        Assert.That(world.UnsavedChanges);
        command.Execute();
        Assert.That(world.UnsavedChanges, Is.False);

        mockBusinessLogic.Received().SaveLearningWorld(world, filepath);
    }
}