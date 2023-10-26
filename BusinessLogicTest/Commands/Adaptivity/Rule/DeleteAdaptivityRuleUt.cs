using BusinessLogic.Commands.Adaptivity.Rule;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Rule;

[TestFixture]
public class DeleteAdaptivityRuleUt
{
    [Test]
    public void Constructor_InitializesProperties()
    {
        var question = Substitute.For<IAdaptivityQuestion>();
        var rule = Substitute.For<IAdaptivityRule>();
        var mappingAction = new Action<IAdaptivityQuestion>(_ => { });
        var logger = new NullLogger<DeleteAdaptivityRule>();

        var sut = GetSystemUnderTest(question, rule, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Question, Is.EqualTo(question));
            Assert.That(sut.Rule, Is.EqualTo(rule));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
        });
    }
    
    [Test]
    public void Execute_RemovesRule_CallsMappingActionWithUpdatedQuestion()
    {
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var rule = question.Rules.ElementAt(0);
        var mappingActionCalled = false;
        var mappingAction = new Action<IAdaptivityQuestion>(q =>
        {
            Assert.That(q, Is.EqualTo(question));
            Assert.That(q.Rules, Has.Count.EqualTo(0));
            mappingActionCalled = true;
        });
        
        var sut = GetSystemUnderTest(question, rule, mappingAction);
        sut.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(mappingActionCalled, Is.True);
            Assert.That(question.Rules, Has.Count.EqualTo(0));
        });
    }

    [Test]
    public void Undo_UndoesRemovalOfRule_CallsMappingActionWithRestoredQuestion()
    {
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var rule = question.Rules.ElementAt(0);
        var mappingActionCallCounter = 0;
        var mappingAction = new Action<IAdaptivityQuestion>(q =>
        {
            mappingActionCallCounter++;
            Assert.That(q, Is.EqualTo(question));
            switch (mappingActionCallCounter)
            {
                case 1:
                    Assert.That(q.Rules, Has.Count.EqualTo(0));
                    break;
                case 2:
                    Assert.That(q.Rules, Has.Count.EqualTo(1));
                    Assert.That(q.Rules.ElementAt(0), Is.EqualTo(rule));
                    break;
            }
        });
        
        var sut = GetSystemUnderTest(question, rule, mappingAction);

        sut.Execute();
        sut.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(mappingActionCallCounter, Is.EqualTo(2));
            Assert.That(question.Rules, Has.Count.EqualTo(1));
            Assert.That(question.Rules.ElementAt(0), Is.EqualTo(rule));
        });
    }
    
    [Test]
    public void Redo_ExecutesCommandAgain()
    {
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var rule = question.Rules.ElementAt(0);
        var mappingActionCallCounter = 0;
        var mappingAction = new Action<IAdaptivityQuestion>(q =>
        {
            mappingActionCallCounter++;
            Assert.That(q, Is.EqualTo(question));
            switch (mappingActionCallCounter)
            {
                case 1:
                case 3:
                    Assert.That(q.Rules, Has.Count.EqualTo(0));
                    break;
                case 2:
                    Assert.That(q.Rules, Has.Count.EqualTo(1));
                    Assert.That(q.Rules.ElementAt(0), Is.EqualTo(rule));
                    break;
            }
        });
        
        var sut = GetSystemUnderTest(question, rule, mappingAction);
        sut.Execute();
        sut.Undo();
        sut.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(mappingActionCallCounter, Is.EqualTo(3));
            Assert.That(question.Rules, Has.Count.EqualTo(0));
        });
    }
    
    private DeleteAdaptivityRule GetSystemUnderTest(IAdaptivityQuestion? question = null, IAdaptivityRule? rule = null,
        Action<IAdaptivityQuestion>? mappingAction = null, ILogger<DeleteAdaptivityRule>? logger = null)
    {
        question ??= Substitute.For<IAdaptivityQuestion>();
        rule ??= Substitute.For<IAdaptivityRule>();
        mappingAction ??= _ => { };
        logger ??= NullLogger<DeleteAdaptivityRule>.Instance;
        return new DeleteAdaptivityRule(question, rule, mappingAction, logger);
    }
}