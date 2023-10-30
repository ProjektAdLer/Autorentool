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
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;
using TestHelpers;

namespace IntegrationTest.Forms.Adaptivity.AdaptivityQuestion;

[TestFixture]
public class MultipleChoiceQuestionFormIt : MudFormTestFixture<MultipleChoiceQuestionForm,
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
        ChoiceValidator = Substitute.For<IValidationWrapper<Choice>>();
        Context.Services.AddSingleton(PresentationLogic);
        Context.Services.AddSingleton(MultipleResponseQuestionValidator);
        Context.Services.AddSingleton(SingleResponseQuestionValidator);
        Context.Services.AddSingleton(ChoiceValidator);
    }

    private IPresentationLogic PresentationLogic { get; set; } = null!;

    private IValidationWrapper<MultipleChoiceMultipleResponseQuestion>
        MultipleResponseQuestionValidator { get; set; } = null!;

    private IValidationWrapper<MultipleChoiceSingleResponseQuestion> SingleResponseQuestionValidator { get; set; } =
        null!;

    private IValidationWrapper<Choice> ChoiceValidator { get; set; } = null!;

    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var onSubmitted = EventCallback.Factory.Create(this, () => { });
        var task = ViewModelProvider.GetAdaptivityTask();
        const QuestionDifficulty difficulty = QuestionDifficulty.Medium;
        IMultipleChoiceQuestionViewModel?
            questionToEdit = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();
        var dialog = Substitute.For<MudDialogInstance>();

        var systemUnderTest = GetRenderedComponent(onSubmitted, task, difficulty, questionToEdit, dialog);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
            Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(FormDataContainer));
            Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(PresentationLogic));
            Assert.That(systemUnderTest.Instance.MudDialog, Is.EqualTo(dialog));
            Assert.That(systemUnderTest.Instance.Task, Is.EqualTo(task));
            Assert.That(systemUnderTest.Instance.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Instance.QuestionToEdit, Is.EqualTo(questionToEdit));
            Assert.That(systemUnderTest.Instance.OnSubmitted, Is.EqualTo(onSubmitted));
            Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
        });
    }

    [Test]
    public void OnParametersSet_CallsMapper()
    {
        IMultipleChoiceQuestionViewModel?
            questionToEdit = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();

        var systemUnderTest = GetRenderedComponent(questionToEdit: questionToEdit);

        Mapper.Received(1).Map(questionToEdit, FormDataContainer.FormModel);
    }


    private IRenderedComponent<MultipleChoiceQuestionForm> GetRenderedComponent(EventCallback? onSubmitted = null,
        IAdaptivityTaskViewModel? task = null, QuestionDifficulty difficulty = QuestionDifficulty.Easy,
        IMultipleChoiceQuestionViewModel? questionToEdit = null, MudDialogInstance? mudDialogInstance = null)
    {
        onSubmitted ??= EventCallback.Empty;
        task ??= ViewModelProvider.GetAdaptivityTask();
        mudDialogInstance ??= Substitute.For<MudDialogInstance>();

        return Context.RenderComponent<MultipleChoiceQuestionForm>(p =>
        {
            p.Add(c => c.OnSubmitted, onSubmitted.Value);
            p.Add(c => c.DebounceInterval, 0);
            p.Add(c => c.Task, task);
            p.Add(c => c.Difficulty, difficulty);
            p.Add(c => c.QuestionToEdit, questionToEdit);
            p.AddCascadingValue(mudDialogInstance);
        });
    }
}