using BusinessLogic.Commands.Adaptivity.Rule;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;
using NullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger;

namespace BusinessLogicTest.Commands.Adaptivity.Rule;

[TestFixture]
public class CreateAdaptivityRuleUt
{
    [Test]
    public void Constructor_InitializesProperties()
    {
        var question = Substitute.For<IAdaptivityQuestion>();
        var trigger = Substitute.For<IAdaptivityTrigger>();
        var action = Substitute.For<IAdaptivityAction>();
        var mappingAction = new Action<IAdaptivityQuestion>(_ => { });
        var logger = new NullLogger<CreateAdaptivityRule>();

        var sut = GetSystemUnderTest(question, trigger, action, mappingAction, logger);
        
        Assert.Multiple(() =>
        {
            Assert.That(sut.Question, Is.EqualTo(question));
            Assert.That(sut.Rule.Trigger, Is.EqualTo(trigger));
            Assert.That(sut.Rule.Action, Is.EqualTo(action));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void Execute_AddsRule_CallsMappingActionWithUpdatedQuestion()
    {
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        question.Rules.Clear();
        var trigger = Substitute.For<IAdaptivityTrigger>();
        var action = Substitute.For<IAdaptivityAction>();
        var mappingActionCalled = false;
        var mappingAction = new Action<IAdaptivityQuestion>(q =>
        {
            Assert.That(q, Is.EqualTo(question));
            Assert.That(q.Rules, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(q.Rules.ElementAt(0).Trigger, Is.EqualTo(trigger));
                Assert.That(q.Rules.ElementAt(0).Action, Is.EqualTo(action));
            });
            mappingActionCalled = true;
        });
        
        var sut = GetSystemUnderTest(question, trigger, action, mappingAction);
        sut.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(mappingActionCalled, Is.True);
            Assert.That(question.Rules, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(question.Rules.ElementAt(0).Trigger, Is.EqualTo(trigger));
                Assert.That(question.Rules.ElementAt(0).Action, Is.EqualTo(action));
            });
        });
    }

    [Test]
    public void Undo_RemovesRule_CallsMappingActionWithResetQuestion()
    {
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        question.Rules.Clear();
        var trigger = Substitute.For<IAdaptivityTrigger>();
        var action = Substitute.For<IAdaptivityAction>();
        var mappingActionCallCounter = 0;
        var mappingAction = new Action<IAdaptivityQuestion>(q =>
        {
            mappingActionCallCounter++;
            Assert.That(q, Is.EqualTo(question));
            switch (mappingActionCallCounter)
            {
                case 1:
                    Assert.That(q.Rules, Has.Count.EqualTo(1));
                    Assert.Multiple(() =>
                    {
                        Assert.That(q.Rules.ElementAt(0).Trigger, Is.EqualTo(trigger));
                        Assert.That(q.Rules.ElementAt(0).Action, Is.EqualTo(action));
                    });
                    break;
                case 2:
                    Assert.That(q.Rules, Is.Empty);
                    break;
                default:
                    Assert.Fail("Callback called more than twice");
                    break;
            }
        });
        
        var sut = GetSystemUnderTest(question, trigger, action, mappingAction);
        
        sut.Execute();
        sut.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(mappingActionCallCounter, Is.EqualTo(2));
            Assert.That(question.Rules, Is.Empty);
        });
    }

    [Test]
    public void Redo_ExecutesCommandAgain()
    {
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        question.Rules.Clear();
        var trigger = Substitute.For<IAdaptivityTrigger>();
        var action = Substitute.For<IAdaptivityAction>();
        var mappingActionCallCounter = 0;
        var mappingAction = new Action<IAdaptivityQuestion>(q =>
        {
            mappingActionCallCounter++;
            Assert.That(q, Is.EqualTo(question));
            switch (mappingActionCallCounter)
            {
                case 1:
                    Assert.That(q.Rules, Has.Count.EqualTo(1));
                    Assert.Multiple(() =>
                    {
                        Assert.That(q.Rules.ElementAt(0).Trigger, Is.EqualTo(trigger));
                        Assert.That(q.Rules.ElementAt(0).Action, Is.EqualTo(action));
                    });
                    break;
                case 2:
                    Assert.That(q.Rules, Is.Empty);
                    break;
                case 3:
                    Assert.That(q.Rules, Has.Count.EqualTo(1));
                    Assert.Multiple(() =>
                    {
                        Assert.That(q.Rules.ElementAt(0).Trigger, Is.EqualTo(trigger));
                        Assert.That(q.Rules.ElementAt(0).Action, Is.EqualTo(action));
                    });
                    break;
                default:
                    Assert.Fail("Callback called more than three times");
                    break;
            }
        });
        
        var sut = GetSystemUnderTest(question, trigger, action, mappingAction);
        
        sut.Execute();
        sut.Undo();
        sut.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(mappingActionCallCounter, Is.EqualTo(3));
            Assert.That(question.Rules, Has.Count.EqualTo(1));
            Assert.That(question.Rules.ElementAt(0).Trigger, Is.EqualTo(trigger));
            Assert.That(question.Rules.ElementAt(0).Action, Is.EqualTo(action));
        });
    }
    
    private CreateAdaptivityRule GetSystemUnderTest(IAdaptivityQuestion? question = null, IAdaptivityTrigger? trigger = null,
        IAdaptivityAction? action = null, Action<IAdaptivityQuestion>? mappingAction = null, ILogger<CreateAdaptivityRule>? logger = null)
    {
        question ??= Substitute.For<IAdaptivityQuestion>();
        trigger ??= Substitute.For<IAdaptivityTrigger>();
        action ??= Substitute.For<IAdaptivityAction>();
        mappingAction ??= _ => { };
        logger ??= NullLogger<CreateAdaptivityRule>.Instance;
        
        return new CreateAdaptivityRule(question, trigger, action, mappingAction, logger);
    }
}