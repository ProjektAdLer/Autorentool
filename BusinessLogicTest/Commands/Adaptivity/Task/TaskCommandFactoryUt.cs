using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Adaptivity;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Task;

[TestFixture]
public class TaskCommandFactoryUt
{
    [SetUp]
    public void SetUp()
    {
        _factory = new TaskCommandFactory(new NullLoggerFactory());
    }

    private TaskCommandFactory _factory = null!;

    [Test]
    // ANF-ID: [AWA0005]
    public void GetCreateCommand_ReturnsCreateAdaptivityTask()
    {
        // Arrange
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var name = "Task1";
        Action<AdaptivityContent> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateCommand(adaptivityContent, name, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateAdaptivityTask>());
        var resultCasted = result as CreateAdaptivityTask;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.AdaptivityContent, Is.EqualTo(adaptivityContent));
            Assert.That(resultCasted.AdaptivityTask.Name, Is.EqualTo(name));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    // ANF-ID: [AWA0006]
    public void GetEditCommand_ReturnsEditAdaptivityTask()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var name = "NewTaskName";
        var minimumRequiredDifficulty = QuestionDifficulty.Medium;
        Action<AdaptivityTask> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditCommand(adaptivityTask, name, minimumRequiredDifficulty, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditAdaptivityTask>());
        var resultCasted = result as EditAdaptivityTask;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.AdaptivityTask, Is.EqualTo(adaptivityTask));
            Assert.That(resultCasted.AdaptivityTaskName, Is.EqualTo(name));
            Assert.That(resultCasted.MinimumRequiredDifficulty, Is.EqualTo(minimumRequiredDifficulty));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    // ANF-ID: [AWA0007]
    public void GetDeleteCommand_ReturnsDeleteAdaptivityTask()
    {
        // Arrange
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        Action<AdaptivityContent> mappingAction = _ => { };

        // Act
        var result = _factory.GetDeleteCommand(adaptivityContent, adaptivityTask, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteAdaptivityTask>());
        var resultCasted = result as DeleteAdaptivityTask;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.AdaptivityContent, Is.EqualTo(adaptivityContent));
            Assert.That(resultCasted.AdaptivityTask, Is.EqualTo(adaptivityTask));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }
}