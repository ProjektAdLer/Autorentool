using System.Collections.Generic;
using Bunit;
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
    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var adaptivityQuestion = Substitute.For<IAdaptivityQuestionViewModel>();

        var sut = GetRenderedComponent(adaptivityQuestion);
        Assert.Multiple(() =>
        {
            Assert.That(sut.Instance.Localizer, Is.Not.Null);
            Assert.That(sut.Instance.AdaptivityQuestion, Is.EqualTo(adaptivityQuestion));
        });
    }

    [Test]
    public void Render_Difficulty_RenderPreviewHeader([Values] QuestionDifficulty expectedDifficulty)
    {
        var adaptivityQuestion = Substitute.For<IAdaptivityQuestionViewModel>();
        adaptivityQuestion.Difficulty.Returns(expectedDifficulty);

        var sut = GetRenderedComponent(adaptivityQuestion);

        var header = sut.Find("h5");
        Assert.That(header.InnerHtml, Is.EqualTo("AdaptivityQuestionPreview.Header.Question." + expectedDifficulty));
    }

    [Test]
    public void Render_MultipleChoiceQuestions_RenderQuestionText()
    {
        const string expectedQuestionText = "expectedQuestionText";
        var adaptivityQuestion = Substitute.For<IMultipleChoiceQuestionViewModel>();
        adaptivityQuestion.Text = expectedQuestionText;

        var sut = GetRenderedComponent(adaptivityQuestion);

        var questionText = sut.Find("p.mud-typography-body1");
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
        adaptivityQuestion.Choices.Returns(new List<ChoiceViewModel> {choice1, choice2});
        adaptivityQuestion.CorrectChoices.Returns(new List<ChoiceViewModel> {choice1});

        var sut = GetRenderedComponent(adaptivityQuestion, hideChoices);

        var choices = sut.FindAll("p.mud-typography-body2");
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
        adaptivityQuestion.Rules.Returns(new List<IAdaptivityRuleViewModel> {rule});

        var sut = GetRenderedComponent(adaptivityQuestion);

        var commentHeader = sut.Find("h6");
        var comment = sut.Find("p.mud-typography-body1");
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
        adaptivityQuestion.Rules.Returns(new List<IAdaptivityRuleViewModel> {rule});

        var sut = GetRenderedComponent(adaptivityQuestion);

        var commentHeader = sut.Find("h6");
        var contentReference = sut.Find("p.mud-typography-body1");
        Assert.Multiple(() =>
        {
            Assert.That(commentHeader.InnerHtml, Is.EqualTo("AdaptivityQuestionPreview.Header.ContentReference"));
            Assert.That(contentReference.InnerHtml, Is.Not.Empty);
        });
    }

    [Test]
    public void Render_HasElementReferenceAction_RenderElementReferenceActionWithHeader()
    {
        var elementReferenceAction = ViewModelProvider.GetElementReferenceAction();
        var rule = Substitute.For<IAdaptivityRuleViewModel>();
        rule.Action.Returns(elementReferenceAction);
        var adaptivityQuestion = Substitute.For<IAdaptivityQuestionViewModel>();
        adaptivityQuestion.Rules.Returns(new List<IAdaptivityRuleViewModel> {rule});

        var sut = GetRenderedComponent(adaptivityQuestion);

        var commentHeader = sut.Find("h6");
        var elementReference = sut.Find("p.mud-typography-body1");
        Assert.Multiple(() =>
        {
            Assert.That(commentHeader.InnerHtml, Is.EqualTo("AdaptivityQuestionPreview.Header.ElementReference"));
            Assert.That(elementReference.InnerHtml, Is.Not.Empty);
        });
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