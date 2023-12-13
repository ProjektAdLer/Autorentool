using System;
using System.Collections.Generic;
using Bunit;
using BusinessLogic.Validation;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;
using TestHelpers;

namespace IntegrationTest.Components.Adaptivity;

[TestFixture]
public class AdaptivityQuestionPreviewUt : MudBlazorTestFixture<AdaptivityQuestionPreview>
{
    [SetUp]
    public void Setup()
    {
        _learningElementNamesProvider = Substitute.For<ILearningElementNamesProvider>();
        Context.Services.AddSingleton(_learningElementNamesProvider);
    }

    private ILearningElementNamesProvider _learningElementNamesProvider = null!;

    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var adaptivityQuestion = Substitute.For<IAdaptivityQuestionViewModel>();

        var sut = GetRenderedComponent(adaptivityQuestion);
        Assert.Multiple(() =>
        {
            Assert.That(sut.Instance.Localizer, Is.Not.Null);
            Assert.That(sut.Instance.AdaptivityQuestion, Is.EqualTo(adaptivityQuestion));
            Assert.That(sut.Instance.LearningElementNamesProvider, Is.EqualTo(_learningElementNamesProvider));
        });
    }

    [Test]
    public void Render_Difficulty_RenderPreviewHeader([Values] QuestionDifficulty expectedDifficulty)
    {
        var adaptivityQuestion = Substitute.For<IAdaptivityQuestionViewModel>();
        adaptivityQuestion.Difficulty.Returns(expectedDifficulty);

        var sut = GetRenderedComponent(adaptivityQuestion);

        var header = sut.Find("p");
        Assert.That(header.InnerHtml, Is.EqualTo("AdaptivityQuestionPreview.Header.Question." + expectedDifficulty));
    }

    [Test]
    public void Render_MultipleChoiceQuestions_RenderQuestionText()
    {
        const string expectedQuestionText = "expectedQuestionText";
        var adaptivityQuestion = Substitute.For<IMultipleChoiceQuestionViewModel>();
        adaptivityQuestion.Text = expectedQuestionText;

        var sut = GetRenderedComponent(adaptivityQuestion);

        var questionText = sut.FindAll("p.mud-typography-body1")[1];
        Assert.That(questionText.InnerHtml, Is.EqualTo(expectedQuestionText));
    }

    [Test]
    public void Render_Choices_RenderChoicesText([Values] bool hideChoices)
    {
        var adaptivityQuestion = Substitute.For<IMultipleChoiceQuestionViewModel>();
        var expectedChoice1Text = "choice1";
        var expectedChoice2Text = "choice2";
        var choice1 = ViewModelProvider.GetChoice(expectedChoice1Text);
        var choice2 = ViewModelProvider.GetChoice(expectedChoice2Text);
        adaptivityQuestion.Choices.Returns(new List<ChoiceViewModel> { choice1, choice2 });
        adaptivityQuestion.CorrectChoices.Returns(new List<ChoiceViewModel> { choice1 });

        var sut = GetRenderedComponent(adaptivityQuestion, hideChoices);

        var choices = sut.FindAll("p.mud-typography-body1.choice");
        if (hideChoices)
        {
            Assert.That(choices, Is.Empty);
        }
        else
        {
            Assert.Multiple(() =>
            {
                Assert.That(choices[0].InnerHtml, Is.EqualTo(expectedChoice1Text));
                Assert.That(choices[1].InnerHtml, Is.EqualTo(expectedChoice2Text));
            });
        }
    }

    [Test]
    public void Render_HasCommentAction_RenderCommentActionWithHeader()
    {
        const string expectedComment = "expectedComment";
        var commentAction = ViewModelProvider.GetCommentAction(expectedComment);
        var rule = Substitute.For<IAdaptivityRuleViewModel>();
        rule.Action.Returns(commentAction);
        var adaptivityQuestion = Substitute.For<IAdaptivityQuestionViewModel>();
        adaptivityQuestion.Rules.Returns(new List<IAdaptivityRuleViewModel> { rule });

        var sut = GetRenderedComponent(adaptivityQuestion);

        var commentHeader = sut.FindAll("p")[1];
        var comment = sut.FindAll("p")[2];
        Assert.Multiple(() =>
        {
            Assert.That(commentHeader.InnerHtml, Is.EqualTo("AdaptivityQuestionPreview.Header.Comment"));
            Assert.That(comment.InnerHtml, Is.EqualTo(expectedComment));
        });
    }

    [Test]
    public void Render_HasContentReferenceAction_RenderContentReferenceActionWithHeader()
    {
        var contentReferenceAction = ViewModelProvider.GetContentReferenceAction();
        var rule = Substitute.For<IAdaptivityRuleViewModel>();
        rule.Action.Returns(contentReferenceAction);
        var adaptivityQuestion = Substitute.For<IAdaptivityQuestionViewModel>();
        adaptivityQuestion.Rules.Returns(new List<IAdaptivityRuleViewModel> { rule });

        var sut = GetRenderedComponent(adaptivityQuestion);

        var commentHeader = sut.FindAll("p")[1];
        var contentReference = sut.FindAll("p")[5];
        Assert.Multiple(() =>
        {
            Assert.That(commentHeader.InnerHtml, Is.EqualTo("AdaptivityQuestionPreview.Header.ContentReference"));
            Assert.That(contentReference.InnerHtml, Is.Not.Empty);
        });
    }

    [Test]
    public void Render_HasElementReferenceAction_RenderElementReferenceActionWithHeader([Values] bool doesElementExist)
    {
        var elementReferenceAction = ViewModelProvider.GetElementReferenceAction();
        var elementId = elementReferenceAction.ElementId;
        const string elementName = "expected";
        _learningElementNamesProvider.ElementNames.Returns(doesElementExist
            ? new List<(Guid, string)> { (elementId, elementName) }
            : new List<(Guid, string)>());
        var rule = Substitute.For<IAdaptivityRuleViewModel>();
        rule.Action.Returns(elementReferenceAction);
        var adaptivityQuestion = Substitute.For<IAdaptivityQuestionViewModel>();
        adaptivityQuestion.Rules.Returns(new List<IAdaptivityRuleViewModel> { rule });

        var sut = GetRenderedComponent(adaptivityQuestion);

        if (doesElementExist)
        {
            var commentHeader = sut.FindAll("p")[1];
            var elementReference = sut.FindAll("p")[6];
            Assert.Multiple(() =>
            {
                Assert.That(commentHeader.InnerHtml, Is.EqualTo("AdaptivityQuestionPreview.Header.ElementReference"));
                Assert.That(elementReference.InnerHtml, Is.Not.Empty);
            });
            Assert.That(elementReference.InnerHtml, Is.EqualTo(elementName));
        }
        else
        {
            var commentHeader = sut.FindAll("p")[1];
            var elementReference = sut.FindAll("p")[2];
            Assert.Multiple(() =>
            {
                Assert.That(commentHeader.InnerHtml, Is.EqualTo("AdaptivityQuestionPreview.Header.ElementReference"));
                Assert.That(elementReference.InnerHtml, Is.Not.Empty);
            });
            Assert.That(elementReference.InnerHtml,
                Is.EqualTo("AdaptivityQuestionPreview.ElementReference.NotFound" + elementId));
        }
    }

    private IRenderedComponent<AdaptivityQuestionPreview> GetRenderedComponent(
        IAdaptivityQuestionViewModel? adaptivityQuestion, bool hideChoices = false)
    {
        adaptivityQuestion ??= Substitute.For<IAdaptivityQuestionViewModel>();

        return Context.RenderComponent<AdaptivityQuestionPreview>(p =>
        {
            p.Add(c => c.AdaptivityQuestion, adaptivityQuestion);
            p.Add(c => c.HideChoices, hideChoices);
        });
    }
}