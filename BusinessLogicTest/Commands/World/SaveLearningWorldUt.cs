using BusinessLogic.API;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Shared.Theme;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class SaveLearningWorldUt
{
    [SetUp]
    public void Setup()
    {
        _mockBusinessLogic = Substitute.For<IBusinessLogic>();
        _world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg);
        _unsavedChangesResetHelper = Substitute.For<IUnsavedChangesResetHelper>();
    }

    private IBusinessLogic _mockBusinessLogic;
    private LearningWorld _world;
    private IUnsavedChangesResetHelper _unsavedChangesResetHelper;

    [Test]
    // ANF-ID: [ASE6]
    public void Execute_CallsBusinessLogicAndResetHelper()
    {
        const string filepath = "filepath";
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new SaveLearningWorld(_mockBusinessLogic, _world, filepath, mappingAction,
            new NullLogger<SaveLearningWorld>(), _unsavedChangesResetHelper);

        Assert.That(actionWasInvoked, Is.False);
        command.Execute();
        Assert.That(actionWasInvoked, Is.True);

        _mockBusinessLogic.Received().SaveLearningWorld(_world, filepath);
        _unsavedChangesResetHelper.Received().ResetWorldUnsavedChangesState(_world);
    }

    [Test]
    // ANF-ID: [ASE6]
    public void Execute_NoFilepathGiven_CallsFindSuitableNewSavePath()
    {
        var command = new SaveLearningWorld(_mockBusinessLogic, _world, "", _ => { },
            new NullLogger<SaveLearningWorld>(), Substitute.For<IUnsavedChangesResetHelper>());

        command.Execute();

        _mockBusinessLogic.Received().FindSuitableNewSavePath(Arg.Any<string>(), _world.Name, Arg.Any<string>(), out _);
    }
}