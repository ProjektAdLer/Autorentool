using BusinessLogic.Commands.Adaptivity.Rule;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Adaptivity.Rule;

[TestFixture]
internal class AdaptivityRuleCommandFactoryUt
{
    [Test]
    public void GetCreateCommand_WhenCalled_ReturnsCreateAdaptivityRule()
    {
        // Arrange
        var systemUnderTest = CreateSystemUnderTest();
        var question = Substitute.For<IAdaptivityQuestion>();
        var trigger = Substitute.For<IAdaptivityTrigger>();
        var action = Substitute.For<IAdaptivityAction>();
        Action<IAdaptivityQuestion> mappingAction = _ => { };

        // Act
        var result = systemUnderTest.GetCreateCommand(question, trigger, action, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateAdaptivityRule>());
        var castedResult = (CreateAdaptivityRule)result;
        Assert.Multiple(() =>
        {
            Assert.That(castedResult.Question, Is.EqualTo(question));
            Assert.That(castedResult.Rule.Trigger, Is.EqualTo(trigger));
            Assert.That(castedResult.Rule.Action, Is.EqualTo(action));
            Assert.That(castedResult.MappingAction, Is.EqualTo(mappingAction));
        });

    }

    [Test]
    public void GetDeleteCommand_WhenCalled_ReturnsDeleteAdaptivityRule()
    {
        // Arrange
        var systemUnderTest = CreateSystemUnderTest();
        var question = Substitute.For<IAdaptivityQuestion>();
        var rule = Substitute.For<IAdaptivityRule>();
        Action<IAdaptivityQuestion> mappingAction = _ => { };

        // Act
        var result = systemUnderTest.GetDeleteCommand(question, rule, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteAdaptivityRule>());
        var castedResult = (DeleteAdaptivityRule)result;
        Assert.Multiple(() =>
        {
            Assert.That(castedResult.Question, Is.EqualTo(question));
            Assert.That(castedResult.Rule, Is.EqualTo(rule));
            Assert.That(castedResult.MappingAction, Is.EqualTo(mappingAction));
        });
    }
    
    private AdaptivityRuleCommandFactory CreateSystemUnderTest(ILoggerFactory? loggerFactory = null)
    {
        loggerFactory ??= new NullLoggerFactory();
        return new AdaptivityRuleCommandFactory(loggerFactory);
    }
}