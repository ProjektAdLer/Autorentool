using Bunit;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity.Forms.AdaptivityQuestion;
using Presentation.Components.Adaptivity.Forms.Models;
using Presentation.Components.Forms;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Shared.Adaptivity;
using TestHelpers;

namespace IntegrationTest.Forms.Adaptivity.AdaptivityQuestion;

[TestFixture]
public class CreateAdaptivityQuestionFormIt : MudFormTestFixture<CreateAdaptivityQuestionForm,
    MultipleChoiceQuestionFormModel, IMultipleChoiceQuestion>
{
    [SetUp]
#pragma warning disable CS0108
    public void Setup()
#pragma warning restore CS0108
    {
        PresentationLogic = Substitute.For<IPresentationLogic>();
        MultipleResponseQuestionValidator =
            Substitute.For<IValidationWrapper<MultipleChoiceMultipleResponseQuestion>>();
        SingleResponseQuestionValidator = Substitute.For<IValidationWrapper<MultipleChoiceSingleResponseQuestion>>();
        Context.Services.AddSingleton(PresentationLogic);
        Context.Services.AddSingleton(MultipleResponseQuestionValidator);
        Context.Services.AddSingleton(SingleResponseQuestionValidator);
    }

    private IPresentationLogic PresentationLogic { get; set; } = null!;

    private IValidationWrapper<MultipleChoiceMultipleResponseQuestion>
        MultipleResponseQuestionValidator { get; set; } = null!;

    private IValidationWrapper<MultipleChoiceSingleResponseQuestion> SingleResponseQuestionValidator { get; set; } =
        null!;

    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var onSubmitted = EventCallback.Factory.Create(this, () => { });
        var task = ViewModelProvider.GetAdaptivityTask();
        const QuestionDifficulty difficulty = QuestionDifficulty.Medium;
        var dialog = Substitute.For<MudDialogInstance>();

        var systemUnderTest = GetRenderedComponent(onSubmitted, task, difficulty, dialog);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
            Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(FormDataContainer));
            Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(PresentationLogic));
            Assert.That(systemUnderTest.Instance.MudDialog, Is.EqualTo(dialog));
            Assert.That(systemUnderTest.Instance.Task, Is.EqualTo(task));
            Assert.That(systemUnderTest.Instance.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Instance.OnSubmitted, Is.EqualTo(onSubmitted));
            Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
        });
    }

    private IRenderedComponent<CreateAdaptivityQuestionForm> GetRenderedComponent(EventCallback? onSubmitted = null,
        IAdaptivityTaskViewModel? task = null, QuestionDifficulty difficulty = QuestionDifficulty.Easy,
        MudDialogInstance? mudDialogInstance = null)
    {
        onSubmitted ??= EventCallback.Empty;
        task ??= ViewModelProvider.GetAdaptivityTask();
        mudDialogInstance ??= Substitute.For<MudDialogInstance>();

        return Context.RenderComponent<CreateAdaptivityQuestionForm>(p =>
        {
            p.Add(c => c.OnSubmitted, onSubmitted.Value);
            p.Add(c => c.DebounceInterval, 0);
            p.Add(c => c.Task, task);
            p.Add(c => c.Difficulty, difficulty);
            p.AddCascadingValue(mudDialogInstance);
        });
    }
}