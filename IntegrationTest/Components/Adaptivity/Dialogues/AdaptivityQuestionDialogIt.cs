using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity;
using Presentation.Components.Adaptivity.Dialogues;
using Presentation.Components.Adaptivity.Forms.AdaptivityQuestion;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace IntegrationTest.Components.Adaptivity.Dialogues;

[TestFixture]
public class AdaptivityQuestionDialogIt : MudDialogTestFixture<AdaptivityQuestionDialog>
{
    [SetUp]
    public void Setup()
    {
        Task = Substitute.For<IAdaptivityTaskViewModel>();
        Difficulty = QuestionDifficulty.Medium;
        Context.ComponentFactories.AddStub<AdaptivityQuestionPreview>();
        Context.ComponentFactories.AddStub<MultipleChoiceQuestionForm>();
    }

    [TearDown]
    public void Teardown()
    {
        DialogProvider.Dispose();
    }

    private IDialogReference Dialog { get; set; } = null!;
    private IAdaptivityTaskViewModel Task { get; set; }
    private QuestionDifficulty Difficulty { get; set; }

    private async Task GetDialogAsync()
    {
        var dialogParameters = new DialogParameters
        {
            { nameof(AdaptivityQuestionDialog.Task), Task },
            { nameof(AdaptivityQuestionDialog.Difficulty), Difficulty }
        };
        Dialog = await OpenDialogAndGetDialogReferenceAsync("title", new DialogOptions(),
            dialogParameters);
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public async Task Render_InjectsDependenciesAndParameters()
    {
        await GetDialogAsync();
        var sut = DialogProvider.FindComponent<AdaptivityQuestionDialog>();
        Assert.Multiple(() =>
        {
            Assert.That(Dialog, Is.Not.Null);
            Assert.That(sut.Instance.Localizer, Is.Not.Null);
            Assert.That(sut.Instance.Task, Is.EqualTo(Task));
            Assert.That(sut.Instance.Difficulty, Is.EqualTo(Difficulty));
        });
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public async Task RenderDialog_ShowsMultipleChoiceQuestionForm([Values] bool hasQuestionToEdit)
    {
        var question = Substitute.For<IMultipleChoiceQuestionViewModel>();
        question.Difficulty.Returns(Difficulty);
        if (hasQuestionToEdit)
        {
            Task.Questions.Returns(new List<IAdaptivityQuestionViewModel> { question });
        }

        await GetDialogAsync();
        var formComponent = DialogProvider.FindComponent<Stub<MultipleChoiceQuestionForm>>();
        Assert.Multiple(() =>
        {
            Assert.That(formComponent.Instance.Parameters.Get(x => x.Task), Is.EqualTo(Task));
            Assert.That(formComponent.Instance.Parameters.Get(x => x.Difficulty), Is.EqualTo(Difficulty));

            Assert.That(formComponent.Instance.Parameters.Get(x => x.QuestionToEdit),
                hasQuestionToEdit
                    ? Is.EqualTo(question)
                    : Is.Null);
        });
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public async Task RenderDialog_ShowsAdaptivityQuestionPreviewOfOtherQuestionsInTask([Values] bool hasOtherQuestions)
    {
        var questionEasy = Substitute.For<IAdaptivityQuestionViewModel>();
        questionEasy.Difficulty.Returns(QuestionDifficulty.Easy);
        var questionHard = Substitute.For<IAdaptivityQuestionViewModel>();
        questionHard.Difficulty.Returns(QuestionDifficulty.Hard);
        if (hasOtherQuestions)
        {
            Task.Questions.Returns(new List<IAdaptivityQuestionViewModel> { questionEasy, questionHard });
        }

        await GetDialogAsync();
        var sidePanelToggle = DialogProvider.FindComponent<MudToggleIconButton>();
        var textComponents = DialogProvider.FindComponents<MudText>();
        var previewComponents = DialogProvider.FindComponents<Stub<AdaptivityQuestionPreview>>();

        Assert.That(textComponents, Has.Count.EqualTo(1));
        Assert.That(textComponents.First().Find("h6").InnerHtml, Is.EqualTo("title"));
        Assert.That(previewComponents, Has.Count.EqualTo(0));

        sidePanelToggle.Find("button").Click();

        textComponents = DialogProvider.FindComponents<MudText>();
        previewComponents = DialogProvider.FindComponents<Stub<AdaptivityQuestionPreview>>();

        Assert.That(textComponents, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(textComponents[0].Find("h6").InnerHtml, Is.EqualTo("title"));
            Assert.That(textComponents[1].Find("p").InnerHtml,
                Is.EqualTo(hasOtherQuestions
                    ? "AdaptivityQuestionDialog.Sidebar.Header.ExistingQuestions"
                    : "AdaptivityQuestionDialog.Sidebar.Header.NoQuestions"));
            Assert.That(previewComponents, Has.Count.EqualTo(hasOtherQuestions ? 2 : 0));
        });
        if (hasOtherQuestions)
        {
            Assert.Multiple(() =>
            {
                Assert.That(previewComponents[0].Instance.Parameters.Get(x => x.AdaptivityQuestion),
                    Is.EqualTo(questionEasy));
                Assert.That(previewComponents[1].Instance.Parameters.Get(x => x.AdaptivityQuestion),
                    Is.EqualTo(questionHard));
            });
        }
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public async Task RenderDialog_FormSubmitted_ClosesDialog()
    {
        await GetDialogAsync();
        var form = DialogProvider.FindComponent<Stub<MultipleChoiceQuestionForm>>();
        await DialogProvider.InvokeAsync(() => form.Instance.Parameters.Get(x => x.OnSubmitted).InvokeAsync(null));
        Assert.That(Dialog.Result.Result!.Data, Is.EqualTo(true));
    }
}